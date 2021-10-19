using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
using System.Globalization;
using System.Windows.Forms;
using static SLD.Extensions;

namespace SLD
{

    public class Circuit
    {
     


        //Temporary
        public string rvtWireSize;
        public string voltage;
        public string polesNumber;
        public string temp;
        public ElementId id;
        public string uid;
        public bool roomsFromLinkedFile = false;


        //Identification
        public string breakerNumber;
        public string number;
        public string fullNumber;
        public string description;
        public List<string> roomsList;
        public string rooms;
        public string type;

        //Load Calculation
        public double loadA;
        public double loadB;
        public double loadC;
        public double load;
        public double ratedLoadA;
        public double ratedLoadB;
        public double ratedLoadC;
        public double ratedLoad;
        public double currentA;
        public double currentB;
        public double currentC;
        public double current;
        public double fullPowerA;
        public double fullPowerB;
        public double fullPowerC;
        public double fullPower;

        public double maxCurrent;
        public List<double> currents;
        public double demandPanelFactor;
        public double demandCircuitFactor;
        public double powerFactor;
        public string phase;

        //Breaker selection
        public string breakerNominalCurrent;
        public string breakerReleaseCurrent;
        public string diffReleaseCurrent;
        public double breakerSafetyFactor;

        // Cable selection
        public string wireInsulation;
        public string insulationMaterial;
        public double ratedLength;
        public double maxLength;
        public double totalLength;
        public string coreQuantity;
        public string cablingType;
        public string coreQuantityAndCrossSection;
        public bool contactor;
        public string neutralWire;
        public double cableDeratingFactor;
        public double cableSafeFactor;
        public int cableQuantity;
        public string cableType;
        public string coreMaterial;
        public string wireType;
        public string standart;
        public string maxCrossSection;

        //VoltageDrop
        public double voltageDrop;
        public double maxVoltageDrop;

        public Circuit()
        {

        }


        public Circuit(Element docCircuit,
                       int circuitNaming,
                       string panelName,
                       string circuitPrefix,
                       string circuitSeparator,
                       bool getRoomNumberFromLink,
                       Dictionary<ElementId, string> roomsInPanel
                       )
        {
            Document doc = docCircuit.Document;

            //Парметры для теста

            id = docCircuit.Id;

            uid = docCircuit.UniqueId;

            number = docCircuit.get_Parameter(BuiltInParameter.RBS_ELEC_CIRCUIT_NUMBER)?.AsString();

            polesNumber = docCircuit.get_Parameter(BuiltInParameter.RBS_ELEC_NUMBER_OF_POLES)?.AsValueString();

            rvtWireSize = docCircuit.get_Parameter(BuiltInParameter.RBS_ELEC_CIRCUIT_WIRE_SIZE_PARAM)?.AsValueString();

            voltage = Util.GetVoltageInVolts(docCircuit, BuiltInParameter.RBS_ELEC_VOLTAGE).ToString();

            //Circuit nubmer with prefix
            fullNumber = docCircuit.get_Parameter(BuiltInParameter.RBS_ELEC_CIRCUIT_NUMBER)?.AsString();

            //rooms in active document

            roomsList = new List<string>();

            if (getRoomNumberFromLink)
            {
                // Getting element room number from linked file
                //rooms = Filters.GetCircuitRoomNumbersFromRVTLink(doc, panelName, fullNumber);
                roomsList = Util.GetCircuitRoomsNumberFromLink(doc, roomsInPanel, fullNumber);
            }
            else
            {
                // Getting element room number from active doc
                roomsList = Util.GetCircuitRoomNumbers(doc, panelName, fullNumber);
            }

            rooms = roomsList.ToRoomStr();

            //description = docCircuit.get_Parameter(BuiltInParameter.RBS_WIRE_CIRCUIT_LOAD_NAME).AsString();
            description = docCircuit.get_Parameter(BuiltInParameter.CIRCUIT_LOAD_CLASSIFICATION_PARAM).AsString();

           /* if (rooms.Count != 0)
            {
                description = description + "\n" + " (пом. ";
                foreach (string room in rooms)
                {
                    if (room != "null")
                    {
                        description = description + room + ", ";
                    }

                }

                description = description.Substring(0, description.Length - 2);
                description = description + ")";
            }
            */


            //Удаляем префиксы

            int del = 1;
            int circuitPrefixLength;
            int circuitSeparatorLength;

            if (circuitPrefix == null)
            {
                circuitPrefixLength = 0;
            }
            else
            {
                circuitPrefixLength = circuitPrefix.Length;
            }

            if (circuitSeparator == null)
            {
                circuitSeparatorLength = 0;
            }
            else
            {
                circuitSeparatorLength = circuitSeparator.Length;
            }

            switch (circuitNaming)
            {
                case 0: del = circuitPrefixLength + circuitSeparatorLength; break;
                case 1: del = 0; break;
                case 2: del = panelName.Length + circuitSeparatorLength; break;
                case 3: del = 1; break;
            }

            try
            {
                number = fullNumber.Substring(del);
            }
            catch
            {

            }

            breakerNumber = "QF" + number;

            loadA = Util.GetPowerInKiloWatts(docCircuit, BuiltInParameter.RBS_ELEC_TRUE_LOAD_PHASEA);
            loadB = Util.GetPowerInKiloWatts(docCircuit, BuiltInParameter.RBS_ELEC_TRUE_LOAD_PHASEB);
            loadC = Util.GetPowerInKiloWatts(docCircuit, BuiltInParameter.RBS_ELEC_TRUE_LOAD_PHASEC);



            phase = PhaseDetection(loadA, loadB, loadC);

            powerFactor = CTDbl(docCircuit.get_Parameter(BuiltInParameter.RBS_ELEC_POWER_FACTOR)?.AsValueString());

            if (powerFactor == 0) { powerFactor = 1; }

            demandPanelFactor = CTDbl(
                docCircuit.get_Parameter(BuiltInParameter.RBS_ELEC_PANEL_TOTAL_DEMAND_FACTOR_PARAM)?.AsValueString()
                );

            if (demandPanelFactor <= 0 || demandPanelFactor > 1) { demandPanelFactor = 1; }

            demandCircuitFactor = CTDbl(
                docCircuit.get_Parameter(BuiltInParameter.RBS_ELEC_DEMANDFACTOR_LOAD_PARAM)?.AsValueString()
                );

            if (demandCircuitFactor <= 0 || demandCircuitFactor > 1) { demandCircuitFactor = 1; }

            currentA = ElectricUtil.pnCurrent(loadA, Parameters.phaseVoltage, powerFactor, demandCircuitFactor);
            currentB = ElectricUtil.pnCurrent(loadB, Parameters.phaseVoltage, powerFactor, demandCircuitFactor);
            currentC = ElectricUtil.pnCurrent(loadC, Parameters.phaseVoltage, powerFactor, demandCircuitFactor);

            currents = new List<double>() { currentA, currentB, currentC };
            maxCurrent = currents.Max();

            breakerSafetyFactor = 1.1;

            breakerNominalCurrent = ElectricUtil.GetBreakerNominalCurrent(maxCurrent, breakerSafetyFactor, 10).ToString();

            breakerReleaseCurrent = breakerNominalCurrent;

            diffReleaseCurrent = "Нет";

            contactor = false;

            insulationMaterial = "ПВХ";

            standart = Parameters.STANDART_GOST50517;

            coreQuantity = "многожильный";

            cablingType = "A1";

            maxCrossSection = "240";

            cableDeratingFactor = 0.75;

            cableQuantity = 1;

            cableType = docCircuit.get_Parameter(BuiltInParameter.RBS_ELEC_CIRCUIT_WIRE_TYPE_PARAM)?.AsValueString();

            coreMaterial = Util.GetCoreMaterialByCableType(doc, cableType);

            neutralWire = Util.GetNeutralSizeByCableType(doc, cableType);

            ratedLength = Util.GetLengthInMeters(docCircuit, BuiltInParameter.RBS_ELEC_CIRCUIT_LENGTH_PARAM);

            maxLength = ratedLength;

            totalLength = ratedLength;

            maxVoltageDrop = 4;

            List<string> data
             = ElectricUtil.GetCable(
                phase,
               CTDbl(breakerReleaseCurrent),
                currentA,
                currentB,
                currentC,
                coreMaterial,
                insulationMaterial,
                standart,
                coreQuantity,
                neutralWire,
                cablingType,
                cableDeratingFactor,
                CTDbl(maxCrossSection),
                maxVoltageDrop,
                ratedLength);

            coreQuantityAndCrossSection = data[0];

            bool is3ph;
            if (phase == "3Ф")
            {
                is3ph = true;
            }
            else
            {
                is3ph = false;
            }



            voltageDrop = ElectricUtil.getDropVoltage(
                                                                                         currentA,
                                                                                         currentB,
                                                                                         currentC,
                                                                                         is3ph,
                                                                                         coreMaterial,
                                                                                         data[1],
                                                                                         data[2],
                                                                                         data[3],
                                                                                         data[4],
                                                                                         ratedLength
                                                                                       );


        }


        public Circuit(DataGridViewRow row)
        {
            breakerNumber = row.Cells[Parameters.DGV_NUMBER_COL].Value?.ToString();
            number = breakerNumber.OnlyNumbersAndCommas();
            description = row.Cells[Parameters.DGV_DESCRIPTION_COL].Value?.ToString();
            rooms = row.Cells[Parameters.DGV_ROOMS_COL].Value?.ToString();
            phase = row.Cells[Parameters.DGV_PHASE_COL].Value?.ToString();
            if (phase == "3Ф")
            {
                voltage = 380.ToString();
            }
            else
            {
                voltage = 220.ToString();
            }

            loadA = CTDbl(row.Cells[Parameters.DGV_LOADA_COL].Value?.ToString());
            loadB = CTDbl(row.Cells[Parameters.DGV_LOADB_COL].Value?.ToString());
            loadC = CTDbl(row.Cells[Parameters.DGV_LOADC_COL].Value?.ToString());
            powerFactor = CTDbl(row.Cells[Parameters.DGV_PF_COL].Value?.ToString());
            demandPanelFactor = CTDbl(row.Cells[Parameters.DGV_KCP_COL].Value?.ToString());
            demandCircuitFactor = CTDbl(row.Cells[Parameters.DGV_KCC_COL].Value?.ToString());
            currentA = CTDbl(row.Cells[Parameters.DGV_CURRENTA_COL].Value?.ToString());
            currentB = CTDbl(row.Cells[Parameters.DGV_CURRENTB_COL].Value?.ToString());
            currentC = CTDbl(row.Cells[Parameters.DGV_CURRENTC_COL].Value?.ToString());
            breakerSafetyFactor = CTDbl(row.Cells[Parameters.DGV_SAFEFACTOR_COL].Value?.ToString());
            breakerNominalCurrent = row.Cells[Parameters.DGV_CBNOMINAL_COL].Value?.ToString();
            breakerReleaseCurrent = row.Cells[Parameters.DGV_CBRELEASE_COL].Value?.ToString();
            diffReleaseCurrent = row.Cells[Parameters.DGV_DIFF_COL].Value?.ToString();
            contactor = bool.Parse(row.Cells[Parameters.DGV_CONTACTOR_COL].Value?.ToString());
            coreMaterial = row.Cells[Parameters.DGV_COREMATERIAL_COL].Value?.ToString();
            insulationMaterial = row.Cells[Parameters.DGV_INSULATIONMATERIAL_COL].Value?.ToString();
            standart = row.Cells[Parameters.DGV_STANDART_COL].Value?.ToString();
            cablingType = row.Cells[Parameters.DGV_CABLINGTYPE_COL].Value?.ToString();
            coreQuantity = row.Cells[Parameters.DGV_SINGLEMULTICORE_COL].Value?.ToString();
            neutralWire = row.Cells[Parameters.DGV_N_COL].Value?.ToString();
            maxCrossSection = row.Cells[Parameters.DGV_MAXCROSSSECTION_COL].Value?.ToString();
            cableDeratingFactor = CTDbl(row.Cells[Parameters.DGV_CABLEDERATINGFACTOR_COL].Value?.ToString());
            cableQuantity = int.Parse(row.Cells[Parameters.DGV_LINESQUANTITY_COL].Value?.ToString());
            cableType = row.Cells[Parameters.DGV_CABLETYPE_COL].Value?.ToString();
            coreQuantityAndCrossSection = row.Cells[Parameters.DGV_COREQUANTITYANDSECTION_COL].Value?.ToString();
            ratedLength = CTDbl(row.Cells[Parameters.DGV_RATEDLENGTH_COL].Value?.ToString());
            maxLength = CTDbl(row.Cells[Parameters.DGV_MAXLENGTH_COL].Value?.ToString());
            totalLength = CTDbl(row.Cells[Parameters.DGV_TOTALLENGT_COL].Value?.ToString());
            maxVoltageDrop = CTDbl(row.Cells[Parameters.DGV_MAXVOLTAGEDROP_COL].Value?.ToString());
            voltageDrop = CTDbl(row.Cells[Parameters.DGV_VOLTAGEDROP_COL].Value?.ToString());
            uid = row.Cells[Parameters.DGV_UID_COL].Value?.ToString();

        }


        public double DeleteDimAndConvertToDouble(string str)
        {
            try
            {
                return CTDbl(str.Substring(0, str.IndexOf(" ")));
            }
            catch
            {
                return 0;
            }
        }

        public string PhaseDetection(double a, double b, double c)
        {
            if (a > 0 & b == 0 & c == 0) return "A";
            if (b > 0 & a == 0 & c == 0) return "B";
            if (c > 0 & b == 0 & a == 0) return "C";
            return "3Ф";
        }


        public double PhaseCurrent(double load, double pf)
        {
            return load / 0.22 / pf;
        }

    }




    public class Panel : Circuit
    {

        public string name;
        public ElementId id { get; set; }
        public string puid;

        public List<Circuit> circuits;
        public int reserve;
        public string circuitPrefix;
        public string circuitSeparator;
        public int circuitNaming;
        public string ownerPanel;
        public string titleBlockName;
        public string panelName;
        public double nonSymmetry;
        public string reservePhases;
        public int cableReserve;
        public double minCircuitBreaker;

        public List<Circuit> GetCircuits()
        {
            return circuits;
        }

        public int Count()
        {
            return circuits.Count();
        }

        public Panel()
        {

        }


        public Panel(Element docPanel, bool getRoomNumberFromLink)
        {
            
            //Getting panel data from Revit document

            id = docPanel.Id;
            Document doc = docPanel.Document;
            
            name = docPanel.Name;
            panelName = name;
            puid = docPanel.UniqueId;
            cableReserve = 0;

            minCircuitBreaker = 10;


            Element panelOwner = Util.getElementSystem(docPanel);

            try
            {
                ownerPanel = panelOwner.get_Parameter(BuiltInParameter.RBS_ELEC_CIRCUIT_PANEL_PARAM)?.AsString();
                fullNumber = panelOwner.get_Parameter(BuiltInParameter.RBS_ELEC_CIRCUIT_NUMBER)?.AsString();
            }
            catch
            {
                panelOwner = null;
            }

            if (fullNumber == "<без имени>" || fullNumber == "<unnamed>" || ownerPanel == "") { panelOwner = null; }


            if (panelOwner != null)
            {
                ratedLength = Util.GetLengthInMeters(panelOwner, BuiltInParameter.RBS_ELEC_CIRCUIT_LENGTH_PARAM);
                ownerPanel = panelOwner.get_Parameter(BuiltInParameter.RBS_ELEC_CIRCUIT_PANEL_PARAM)?.AsString();
                maxLength = ratedLength;
                totalLength = ratedLength;
                cableType = panelOwner.get_Parameter(BuiltInParameter.RBS_ELEC_CIRCUIT_WIRE_TYPE_PARAM)?.AsValueString();
                powerFactor = (panelOwner.get_Parameter(BuiltInParameter.RBS_ELEC_POWER_FACTOR)?.AsValueString()).ToDoubleWithCulture();
                fullNumber = panelOwner.get_Parameter(BuiltInParameter.RBS_ELEC_CIRCUIT_NUMBER)?.AsString();

                if (powerFactor <= 0 || powerFactor > 1) { powerFactor = 1; }

                // доделать
                demandPanelFactor = (panelOwner.get_Parameter(BuiltInParameter.RBS_ELEC_PANEL_TOTAL_DEMAND_FACTOR_PARAM)?.AsValueString()).ToDbl();
                if (demandPanelFactor <= 0 || demandPanelFactor > 1) { demandPanelFactor = 1; }

                //доделать
                demandCircuitFactor = CTDbl(panelOwner.get_Parameter(BuiltInParameter.RBS_ELEC_DEMANDFACTOR_LOAD_PARAM)?.AsValueString());
                if (demandCircuitFactor <= 0 || demandCircuitFactor > 1) { demandCircuitFactor = 1; }
            }
            else
            {
                ownerPanel = "...";
                fullNumber = "...";
                ratedLength = 0;
                maxLength = 0;
                totalLength = 0;
                cableType = "ВВГнг(А)-LS";
                powerFactor = 1;
                demandCircuitFactor = 1;
                demandPanelFactor = 1;

            }

            circuitPrefix = docPanel.get_Parameter(BuiltInParameter.RBS_ELEC_CIRCUIT_PREFIX)?.AsString();
            circuitSeparator = docPanel.get_Parameter(BuiltInParameter.RBS_ELEC_CIRCUIT_PREFIX_SEPARATOR)?.AsString();
            circuitNaming = docPanel.get_Parameter(BuiltInParameter.RBS_ELEC_CIRCUIT_NAMING).AsInteger();

            int del = 1;
            int circuitPrefixLength;
            int circuitSeparatorLength;

            if (circuitPrefix == null)
            {
                circuitPrefixLength = 0;
            }
            else
            {
                circuitPrefixLength = circuitPrefix.Length;
            }

            if (circuitSeparator == null)
            {
                circuitSeparatorLength = 0;
            }
            else
            {
                circuitSeparatorLength = circuitSeparator.Length;
            }

            switch (circuitNaming)
            {
                case 0: del = circuitPrefixLength + circuitSeparatorLength; break;
                case 1: del = 0; break;
                case 2: del = panelName.Length + circuitSeparatorLength; break;
                case 3: del = 1; break;
                case 5: del = 0; break;
            }

            try
            {
                number = fullNumber.Substring(del);
            }
            catch
            {

            }

            breakerNumber = "QF" + number;


            int numPhases = docPanel.get_Parameter(BuiltInParameter.RBS_ELEC_PANEL_NUMPHASES_PARAM).AsInteger();







            // 0 Prefixed
            // 1 Standart
            // 2 Panel
            // 3 Phase

            //Helper.GetElementParameterInformation(doc, docPanel);

            //Getting circuits data from Revit document

            ICollection<Element> docCircuits = Util.GetElectricalCircuitsByBoardName(doc, panelName);

            circuits = new List<Circuit>();

            Dictionary<ElementId, string> roomsInPanel = new Dictionary<ElementId, string> { };

            if (getRoomNumberFromLink)
            {
                List<ElementId> elementIdsInPanel = Util.GetPanelElectricalElementIds(doc, panelName);
                roomsInPanel = Util.GetPanelRoomsNumberFromLink(doc, elementIdsInPanel);
            }

            foreach (Element docCircuit in docCircuits)
            {




                Circuit circuit = new Circuit(docCircuit,
                                              circuitNaming,
                                              panelName,
                                              circuitPrefix,
                                              circuitSeparator,
                                              getRoomNumberFromLink,
                                              roomsInPanel);
                // Helper.GetElementParameterInformation(doc, docCircuit);

                circuits.Add(circuit);
            }

            try
            {
                circuits = circuits.OrderBy(o => int.Parse(o.number)).ToList();
            }
            catch
            {

            }


            // Load distribution board parameters


            loadA = 0;
            loadB = 0;
            loadC = 0;
            ratedLoadA = 0;
            ratedLoadB = 0;
            ratedLoadC = 0;
            double rratedLoadA = 0;
            double rratedLoadB = 0;
            double rratedLoadC = 0;



            foreach (Circuit circuit in circuits)
            {
                loadA = loadA + circuit.loadA;
                ratedLoadA = ratedLoadA + circuit.loadA * demandCircuitFactor;
                rratedLoadA = rratedLoadA + circuit.loadA * demandCircuitFactor * Math.Tan(Math.Acos(powerFactor));

                loadB = loadB + circuit.loadB;
                ratedLoadB = ratedLoadB + circuit.loadB * demandCircuitFactor;
                rratedLoadB = rratedLoadB + circuit.loadB * demandCircuitFactor * Math.Tan(Math.Acos(powerFactor));

                loadC = loadC + circuit.loadC;
                ratedLoadC = ratedLoadC + circuit.loadC * demandCircuitFactor;
                rratedLoadC = rratedLoadC + circuit.loadC * demandCircuitFactor * Math.Tan(Math.Acos(powerFactor));
            }


            load = new List<double> { loadA, loadB, loadC }.Max();

            List<double> ratedLoads = new List<double>() { ratedLoadA, ratedLoadB, ratedLoadC };

            ratedLoad = ratedLoads.Max();
            double rratedLoad = new List<double>() { rratedLoadA, rratedLoadB, rratedLoadC }.Max();

            try
            {
                powerFactor = ratedLoad / Math.Sqrt(ratedLoad * ratedLoad + rratedLoad * rratedLoad);
            }
            catch
            {
                powerFactor = 1;
            }





            if (ratedLoads.Max() == 0 && ratedLoads.Min() == 0)
            {
                nonSymmetry = 0;
            }
            else
            {
                nonSymmetry = (ratedLoads.Max() - ratedLoads.Min()) / ratedLoads.Max() * 100;
            }




            phase = PhaseDetection(loadA, loadB, loadC);

            currentA = ElectricUtil.pnCurrent(loadA, Parameters.phaseVoltage, powerFactor, demandCircuitFactor);
            currentB = ElectricUtil.pnCurrent(loadB, Parameters.phaseVoltage, powerFactor, demandCircuitFactor);
            currentC = ElectricUtil.pnCurrent(loadC, Parameters.phaseVoltage, powerFactor, demandCircuitFactor);

            maxCurrent = new List<double>() { currentA, currentB, currentC }.Max();
            current = maxCurrent;

            fullPowerA = ratedLoadA / powerFactor;
            fullPowerB = ratedLoadB / powerFactor;
            fullPowerC = ratedLoadC / powerFactor;

            fullPower = new List<double>() { fullPowerA, fullPowerB, fullPowerC }.Max();





            breakerSafetyFactor = 1.1;

            breakerNominalCurrent = ElectricUtil.GetBreakerNominalCurrent(maxCurrent, breakerSafetyFactor, 10).ToString();

            breakerReleaseCurrent = breakerNominalCurrent;

            diffReleaseCurrent = "Нет";

            contactor = false;

            insulationMaterial = "ПВХ";

            standart = Parameters.STANDART_GOST50517;

            coreQuantity = "многожильный";

            cablingType = "A1";

            maxCrossSection = "240";

            cableDeratingFactor = 0.75;

            cableQuantity = 1;

            coreMaterial = Util.GetCoreMaterialByCableType(doc, cableType);

            neutralWire = Util.GetNeutralSizeByCableType(doc, cableType);

            maxVoltageDrop = 4;

            List<string> data
             = ElectricUtil.GetCable(
                phase,
               CTDbl(breakerReleaseCurrent),
                currentA,
                currentB,
                currentC,
                coreMaterial,
                insulationMaterial,
                standart,
                coreQuantity,
                neutralWire,
                cablingType,
                cableDeratingFactor,
                CTDbl(maxCrossSection),
                maxVoltageDrop,
                ratedLength);

            coreQuantityAndCrossSection = data[0];

            bool is3ph;
            if (phase == "3Ф")
            {
                is3ph = true;
            }
            else
            {
                is3ph = false;
            }


            if (ratedLength != 0)
            {
                voltageDrop = ElectricUtil.getDropVoltage(
                                                                                        currentA,
                                                                                        currentB,
                                                                                        currentC,
                                                                                        is3ph,
                                                                                        coreMaterial,
                                                                                        data[1],
                                                                                        data[2],
                                                                                        data[3],
                                                                                        data[4],
                                                                                        ratedLength
                                                                                      );
            }
            else
            {
                voltageDrop = 0;
            }           

        }


    }
}
