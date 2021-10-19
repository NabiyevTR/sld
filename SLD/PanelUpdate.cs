using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SLD.Extensions;

namespace SLD
{
    public class PanelUpdate
    {
        //errors
        double loadError = 0.0005;
        double lengthError = 0.5;
        public int Status { get; set; }

        bool panelUpdate = false;
        bool updateLoads;
        bool updateRatedLength;
        bool updateMaxLength;
        bool updateTotalLength;
        bool updateRooms;
        bool updateDescription;
        bool roomsFromLinkedFiles;
        double cableReserve;
        double minCircuitBreaker;

        Dictionary<ElementId, string> roomsInPanel;


        Element docPanel;
        List<Element> docCircuits;
        Document doc;
        Panel panel;
        string panelName;
        string name;
        Panel panelFromStorage;


        public PanelUpdate(Element docPanel)
        {
            // Data from application settings
            updateLoads = Properties.Settings.Default.update_Load;
            updateRatedLength = Properties.Settings.Default.update_RatedLength;
            updateMaxLength = Properties.Settings.Default.update_MaxLength;
            updateTotalLength = Properties.Settings.Default.update_TotalLength;
            updateRooms = Properties.Settings.Default.update_Rooms;
            updateDescription = Properties.Settings.Default.update_Description;

            roomsFromLinkedFiles = Properties.Settings.Default.set_roomsFromLink;

            this.docPanel = docPanel;
            this.docCircuits = GetPanelSystems(docPanel);
            this.doc = docPanel.Document;
            this.panel = GetPanelData();


            Panel panelFromStorage = new Panel();
            panelFromStorage = GetPanelFromStorage();
            this.panelFromStorage = panelFromStorage;

            this.cableReserve = 1 + panelFromStorage.cableSafeFactor / 100;
            this.minCircuitBreaker = panelFromStorage.minCircuitBreaker;

            switch (CheckAll())
            {
                case -1: this.Status = -1; break;
                case 0: this.Status = 0; break;
                case 1:
                    this.Status = 1;
                    Calculate(panelFromStorage);
                    UpdateStorage(panelFromStorage);
                    break;
            }
            DrawPanelUpdate dpu = new DrawPanelUpdate(doc, panelFromStorage);
        }

        Panel GetPanelData()
        {
            Panel panel = new Panel();
            panel.name = docPanel.Name;
            panel.panelName = panel.name;
            this.name = panel.name;
            this.panelName = panel.name;
            Element panelOwner = Util.getElementSystem(docPanel);

            try
            {
                panel.ownerPanel = panelOwner.get_Parameter(BuiltInParameter.RBS_ELEC_CIRCUIT_PANEL_PARAM)?.AsString();
                panel.fullNumber = panelOwner.get_Parameter(BuiltInParameter.RBS_ELEC_CIRCUIT_NUMBER)?.AsString();
            }
            catch
            {
                panelOwner = null;
            }

            if (panel.fullNumber == "<без имени>" || panel.fullNumber == "<unnamed>" || panel.ownerPanel == "") { panelOwner = null; }

            if (panelOwner != null & updateRatedLength)
            {
                panel.ratedLength = Util.GetLengthInMeters(panelOwner, BuiltInParameter.RBS_ELEC_CIRCUIT_LENGTH_PARAM);
            }

            if (panelOwner != null & updateMaxLength)
            {
                panel.maxLength = Util.GetLengthInMeters(panelOwner, BuiltInParameter.RBS_ELEC_CIRCUIT_LENGTH_PARAM);
            }

            if (panelOwner != null & updateTotalLength)
            {
                panel.totalLength = Util.GetLengthInMeters(panelOwner, BuiltInParameter.RBS_ELEC_CIRCUIT_LENGTH_PARAM);
            }

            // panelOwner может быть отсоединен, а данные останутся

            Dictionary<ElementId, string> roomsInPanel = new Dictionary<ElementId, string> { };
            if (updateRooms)
            {
                if (roomsFromLinkedFiles)
                {

                    List<ElementId> elementIdsInPanel = Util.GetPanelElectricalElementIds(doc, panel.panelName);
                    roomsInPanel = Util.GetPanelRoomsNumberFromLink(doc, elementIdsInPanel);
                    this.roomsInPanel = roomsInPanel;
                }
            }


            List<Element> docCiruit = Util.getSystemsByPanel(this.docPanel);

            List<Circuit> circuits = new List<Circuit>();

            foreach (Element docCircuit in docCircuits)
            {
                Circuit circuit = GetCircuitData(docCircuit);
                circuits.Add(circuit);
            }

            panel.circuits = circuits;

            try
            {
                panel.uid = panelOwner.UniqueId;
            }
            catch
            {
                panel.uid = null;
            }

            panel.puid = docPanel.UniqueId;

            return panel;
        }

        Circuit GetCircuitData(Element system)
        {
            Circuit docCircuit = new Circuit();

            docCircuit.uid = system.UniqueId;

            docCircuit.fullNumber = system.get_Parameter(BuiltInParameter.RBS_ELEC_CIRCUIT_NUMBER)?.AsString();

            if (updateLoads)
            {
                docCircuit.loadA = Util.GetPowerInKiloWatts(system, BuiltInParameter.RBS_ELEC_TRUE_LOAD_PHASEA);
                docCircuit.loadB = Util.GetPowerInKiloWatts(system, BuiltInParameter.RBS_ELEC_TRUE_LOAD_PHASEB);
                docCircuit.loadC = Util.GetPowerInKiloWatts(system, BuiltInParameter.RBS_ELEC_TRUE_LOAD_PHASEC);
            }

            if (updateRatedLength)
            {
                docCircuit.ratedLength = Util.GetLengthInMeters(system, BuiltInParameter.RBS_ELEC_CIRCUIT_LENGTH_PARAM);
            }

            if (updateMaxLength)
            {
                docCircuit.maxLength = Util.GetLengthInMeters(system, BuiltInParameter.RBS_ELEC_CIRCUIT_LENGTH_PARAM);
            }

            if (updateTotalLength)
            {
                docCircuit.totalLength = Util.GetLengthInMeters(system, BuiltInParameter.RBS_ELEC_CIRCUIT_LENGTH_PARAM);
            }

            if (updateDescription)
            {
                docCircuit.description = system.get_Parameter(BuiltInParameter.CIRCUIT_LOAD_CLASSIFICATION_PARAM).AsString();
            }

            if (updateRooms)
            {
                docCircuit.roomsList = new List<string>();


                if (roomsFromLinkedFiles)
                {
                    // Getting element room number from linked file                   
                    docCircuit.roomsList = Util.GetCircuitRoomsNumberFromLink(doc, roomsInPanel, docCircuit.fullNumber);
                }
                else
                {
                    // Getting element room number from active doc
                    docCircuit.roomsList = Util.GetCircuitRoomNumbers(doc, panelName, docCircuit.fullNumber);
                }

                docCircuit.rooms = docCircuit.roomsList.ToRoomStr();
            }

            /*
            if (updateRooms)
            {
                if (roomsFromLinkedFiles)
                {
                    // Getting element room number from linked file                    
                    rooms = Util.GetCircuitRoomsNumberFromLink(doc, roomsInPanel, fullNumber);

                }
                else
                {
                    // Getting element room number from active doc
                    rooms = Util.GetCircuitRoomNumbers(doc, panelName, fullNumber);
                }
            }
            */

            // НУЖЕН РАСЧЕТ !!!! ????

            return docCircuit;
        }

        List<Element> GetPanelSystems(Element panel)
        {
            return Util.getSystemsByPanel(panel);
        }

        void UpdateStorage(Panel p)
        {
            Storage ps = new Storage(docPanel);
            ps.UpDate(p);

            foreach (Circuit circuit in p.circuits)
            {
                Element docCircuit = doc.GetElement(circuit.uid);
                Storage circuitStorage = new Storage(docCircuit);
                circuitStorage.Write(circuit);
            }
        }

        Panel GetPanelFromStorage()
        {
            Storage s = new Storage(docPanel);
            return s.Read();
        }

        int CheckAll()
        {
            List<Circuit> circuitsFromModel = panel.circuits;
            List<Circuit> circuitsFromStorage = panelFromStorage.circuits;

            bool error = true;
            bool update = false;


            if (panel == null )
            {
                return -1;
            }

            if (panelFromStorage == null)
            {
                return -1;
            }

            if (panel.circuits == null)
            {
                return -1;
            }

            if (panelFromStorage.circuits == null)
            {
                return -1;
            }

            if (panel.circuits.Count != panelFromStorage.circuits.Count)
            {
                return -1;
            }

            if (circuitsFromStorage == null)
            {
                return -1;
            }

            if (circuitsFromStorage == null)
            {
                return -1;
            }

            /*
            for (int i=0; i<panel.circuits.Count; i++)
            {
                Circuit circuitFromModel = panel.circuits[i];


            }
            */

            foreach (Circuit circuitFromModel in panel.circuits)
            {
                error = true;

                foreach (Circuit circuitFromStorage in panelFromStorage.circuits)
                {

                    if (circuitFromModel.uid == circuitFromStorage.uid)
                    {
                        if (updateLoads)
                        {
                            if (Math.Abs(circuitFromModel.loadA - circuitFromStorage.loadA) > loadError)
                            {
                                circuitFromStorage.loadA = circuitFromModel.loadA;
                                update = true;
                            }
                            if (Math.Abs(circuitFromModel.loadB - circuitFromStorage.loadB) > loadError)
                            {
                                circuitFromStorage.loadB = circuitFromModel.loadB;
                                update = true;
                            }
                            if (Math.Abs(circuitFromModel.loadC - circuitFromStorage.loadC) > loadError)
                            {
                                circuitFromStorage.loadC = circuitFromModel.loadC;
                                update = true;
                            }
                        }

                        if (updateRatedLength)
                        {
                            if (Math.Abs(circuitFromModel.ratedLength - circuitFromStorage.ratedLength / cableReserve) > lengthError)
                            {
                                circuitFromStorage.ratedLength = circuitFromModel.ratedLength * cableReserve;
                                update = true;
                            }
                        }

                        if (updateMaxLength)
                        {
                            if (Math.Abs(circuitFromModel.maxLength - circuitFromStorage.maxLength / cableReserve) > lengthError)
                            {
                                circuitFromStorage.maxLength = circuitFromModel.maxLength * cableReserve;
                                update = true;
                            }
                        }


                        if (updateTotalLength)
                        {
                            if (Math.Abs(circuitFromModel.totalLength - circuitFromStorage.totalLength / cableReserve) > lengthError)
                            {
                                circuitFromStorage.totalLength = circuitFromModel.totalLength * cableReserve;
                                update = true;
                            }
                        }

                        if (updateRooms)
                        {
                            if (circuitFromModel.rooms != circuitFromStorage.rooms) ;
                            circuitFromStorage.rooms = circuitFromModel.rooms;
                            update = true;
                        }

                        if (updateDescription)
                        {
                            if (circuitFromModel.description != circuitFromStorage.description) ;
                            circuitFromStorage.description = circuitFromModel.description;
                            update = true;
                        }
                        error = false;
                    }
                }

                if (error)
                {
                    return -1;
                }
            }

            if (update)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        void Calculate(Panel p)
        {
            List<Circuit> cs = p.circuits;

            foreach (Circuit c in cs)
            {
                c.phase = PhaseDetection(c.loadA, c.loadB, c.loadC);

                c.currentA = ElectricUtil.pnCurrent(c.loadA, Parameters.phaseVoltage, c.powerFactor, c.demandCircuitFactor);
                c.currentB = ElectricUtil.pnCurrent(c.loadB, Parameters.phaseVoltage, c.powerFactor, c.demandCircuitFactor);
                c.currentC = ElectricUtil.pnCurrent(c.loadC, Parameters.phaseVoltage, c.powerFactor, c.demandCircuitFactor);

                c.currents = new List<double>() { c.currentA, c.currentB, c.currentC };
                c.maxCurrent = c.currents.Max();

                c.breakerNominalCurrent = ElectricUtil.GetBreakerNominalCurrent(c.maxCurrent, c.breakerSafetyFactor, minCircuitBreaker).ToString();

                c.breakerReleaseCurrent = c.breakerNominalCurrent;

                List<string> datac
                 = ElectricUtil.GetCable(
                    c.phase,
                    CTDbl(c.breakerReleaseCurrent),
                    c.currentA,
                    c.currentB,
                    c.currentC,
                    c.coreMaterial,
                    c.insulationMaterial,
                    c.standart,
                    c.coreQuantity,
                    c.neutralWire,
                    c.cablingType,
                    c.cableDeratingFactor,
                    CTDbl(c.maxCrossSection),
                    c.maxVoltageDrop,
                    c.ratedLength);

                c.coreQuantityAndCrossSection = datac[0];

                bool is3phc;
                if (c.phase == "3Ф")
                {
                    is3phc = true;
                }
                else
                {
                    is3phc = false;
                }



                c.voltageDrop = ElectricUtil.getDropVoltage(
                                                                                             c.currentA,
                                                                                             c.currentB,
                                                                                             c.currentC,
                                                                                             is3phc,
                                                                                             c.coreMaterial,
                                                                                             datac[1],
                                                                                             datac[2],
                                                                                             datac[3],
                                                                                             datac[4],
                                                                                             c.ratedLength
                                                                                           );







            }


            double loadA = 0;
            double loadB = 0;
            double loadC = 0;
            double ratedLoadA = 0;
            double ratedLoadB = 0;
            double ratedLoadC = 0;
            double rratedLoadA = 0;
            double rratedLoadB = 0;
            double rratedLoadC = 0;




            foreach (Circuit c in cs)
            {
                loadA = loadA + c.loadA;
                ratedLoadA = ratedLoadA + c.loadA * p.demandCircuitFactor;
                rratedLoadA = rratedLoadA + c.loadA * p.demandCircuitFactor * Math.Tan(Math.Acos(p.powerFactor));

                loadB = loadB + c.loadB;
                ratedLoadB = ratedLoadB + c.loadB * p.demandCircuitFactor;
                rratedLoadB = rratedLoadB + c.loadB * p.demandCircuitFactor * Math.Tan(Math.Acos(p.powerFactor));

                loadC = loadC + c.loadC;
                ratedLoadC = ratedLoadC + c.loadC * c.demandCircuitFactor;
                rratedLoadC = rratedLoadC + c.loadC * c.demandCircuitFactor * Math.Tan(Math.Acos(p.powerFactor));
            }

            List<double> ratedLoads = new List<double>() { ratedLoadA, ratedLoadB, ratedLoadC };

            p.loadA = loadA;
            p.loadB = loadB;
            p.loadC = loadC;
            p.load = new List<double> { loadA, loadB, loadC }.Max();
            p.ratedLoadA = ratedLoadA;
            p.ratedLoadB = ratedLoadB;
            p.ratedLoadC = ratedLoadC;
            p.ratedLoad = ratedLoads.Max();


            double rratedLoad = new List<double>() { rratedLoadA, rratedLoadB, rratedLoadC }.Max();

            try
            {
                p.powerFactor = p.ratedLoad / Math.Sqrt(p.ratedLoad * p.ratedLoad + rratedLoad * rratedLoad);
            }
            catch
            {
                p.powerFactor = 1;
            }

            if (ratedLoads.Max() == 0 && ratedLoads.Min() == 0)
            {
                p.nonSymmetry = 0;
            }
            else
            {
                p.nonSymmetry = (ratedLoads.Max() - ratedLoads.Min()) / ratedLoads.Max() * 100;
            }

            p.phase = PhaseDetection(p.loadA, p.loadB, p.loadC);
            p.currentA = ElectricUtil.pnCurrent(loadA, Parameters.phaseVoltage, p.powerFactor, p.demandCircuitFactor);
            p.currentB = ElectricUtil.pnCurrent(loadB, Parameters.phaseVoltage, p.powerFactor, p.demandCircuitFactor);
            p.currentC = ElectricUtil.pnCurrent(loadC, Parameters.phaseVoltage, p.powerFactor, p.demandCircuitFactor);
            p.maxCurrent = new List<double>() { p.currentA, p.currentB, p.currentC }.Max();
            p.current = p.maxCurrent;
            p.fullPowerA = p.ratedLoadA / p.powerFactor;
            p.fullPowerB = p.ratedLoadB / p.powerFactor;
            p.fullPowerC = p.ratedLoadC / p.powerFactor;
            p.fullPower = new List<double>() { p.fullPowerA, p.fullPowerB, p.fullPowerC }.Max();

            //breakerSafetyFactor = 1.1;

            //breakerNominalCurrent = ElectricUtil.GetBreakerNominalCurrent(maxCurrent, breakerSafetyFactor, 10).ToString();

            //breakerReleaseCurrent = breakerNominalCurrent;

            //diffReleaseCurrent = "Нет";

            //contactor = false;

            //insulationMaterial = "ПВХ";

            //standart = Parameters.STANDART_GOST50517;

            //coreQuantity = "многожильный";

            //cablingType = "A1";

            //maxCrossSection = "240";

            //cableDeratingFactor = 0.75;

            //cableQuantity = 1;

            //coreMaterial = Util.GetCoreMaterialByCableType(doc, cableType);

            //neutralWire = Util.GetNeutralSizeByCableType(doc, cableType);

            //maxVoltageDrop = 4;

            List<string> data
             = ElectricUtil.GetCable(
                p.phase,
               CTDbl(p.breakerReleaseCurrent),
                p.currentA,
                p.currentB,
                p.currentC,
                p.coreMaterial,
                p.insulationMaterial,
                p.standart,
                p.coreQuantity,
                p.neutralWire,
                p.cablingType,
                p.cableDeratingFactor,
                CTDbl(p.maxCrossSection),
                p.maxVoltageDrop,
                p.ratedLength);

            p.coreQuantityAndCrossSection = data[0];

            bool is3ph;
            if (p.phase == "3Ф")
            {
                is3ph = true;
            }
            else
            {
                is3ph = false;
            }

            if (p.ratedLength != 0)
            {
                p.voltageDrop = ElectricUtil.getDropVoltage(
                                                                                        p.currentA,
                                                                                        p.currentB,
                                                                                        p.currentC,
                                                                                        is3ph,
                                                                                        p.coreMaterial,
                                                                                        data[1],
                                                                                        data[2],
                                                                                        data[3],
                                                                                        data[4],
                                                                                        p.ratedLength
                                                                                      );
            }
            else
            {
                p.voltageDrop = 0;
            }
        }

        string PhaseDetection(double a, double b, double c)
        {
            if (a > 0 & b == 0 & c == 0) return "A";
            if (b > 0 & a == 0 & c == 0) return "B";
            if (c > 0 & b == 0 & a == 0) return "C";
            return "3Ф";
        }
    }
}
