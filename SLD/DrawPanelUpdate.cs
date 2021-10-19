using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Globalization;
using static SLD.Parameters;
using static SLD.Extensions;
using Autodesk.Revit.Attributes;

namespace SLD
{
    [Transaction(TransactionMode.Manual)]
    class DrawPanelUpdate
    {

        Document doc;
        Panel p;
        string panelName;
        const double powerError = 0.0005;
        const double currentError = 0.05;
        const double voltageError = 0.5;
        const double factorsError = 0.001;
        const double voltageDropError = 0.05;
        const double lengthError = 0.5;
        string Error { get; }



        public DrawPanelUpdate(Document doc, Panel p)
        {
            this.doc = doc;
            this.p = p;
            panelName = p.name;

            using (Transaction tx = new Transaction(doc))
            {
                tx.Start("Обновление однолинейной схемы");


                TotalLoadUpdate();
                PhaseUpdate();
                FeederUpdate();

                foreach (Circuit c in p.circuits)
                {
                    CircuitUpdate(c);
                }
                tx.Commit();

            }
        }

        public void TotalLoadUpdate()
        {
            List<Element> elements = getElements(p.uid);
            foreach (Element element in elements)
            {
                UpdatePhases(element);
            }

        }

        public void PhaseUpdate()
        {
            List<Element> elements = getElements(p.uid);
            foreach (Element element in elements)
            {
                UpdatePhases(element);
            }

        }       

        public void FeederUpdate()
        {
            List<Element> elements = getElements(p.uid, BIND_TYPE_LINEIN);

            foreach (Element element in elements)
            {
                UpdateLineIn(element);
            }

        }

        public void CircuitUpdate(Circuit c)
        {
            List<Element> elements = getElements(c.uid, BIND_TYPE_LINEOUT);

            foreach (Element element in elements)
            {
                UpdateLineOut(element, c);
            }

        }



        public void UpdateLineOut(Element e, Circuit c)
        {
            ParameterSet parameters = e.Parameters;
            foreach (Parameter param in parameters)
            {
                switch (param.Definition.Name)
                {
                    case Parameters.RVT_LINEOUT_IS3PH:
                        if (c.phase == "3Ф")
                        {
                            param.TrySetWithCheck(1, factorsError);
                        }
                        else
                        {
                            param.TrySetWithCheck(0, factorsError);
                        }
                        break;

                    case Parameters.RVT_LINEOUT_NUMBER: param.TrySetWithCheck(c.number); break;
                    case Parameters.RVT_LINEOUT_PANELNAME: param.TrySetWithCheck(panelName); break;
                    case Parameters.RVT_LINEOUT_BREAKERANNOTATION: param.TrySetWithCheck(c.breakerNumber); break;
                    case Parameters.RVT_LINEOUT_DESCRIPTION:
                        string descriptionWithRooms =c.description;
                        if (c.rooms != null)
                        {
                            if (c.rooms.Count() > 0)
                            {
                                descriptionWithRooms = descriptionWithRooms + "\n" + "(пом. " + c.rooms + ")";
                            }
                        }
                        param.TrySet(descriptionWithRooms); break;
                    case Parameters.RVT_LINEOUT_PHASE: param.TrySetWithCheck(c.phase); break;
                    case Parameters.RVT_LINEOUT_VOLTAGE: param.TrySetWithCheck(CTDbl(c.voltage), voltageError); break;
                    case Parameters.RVT_LINEOUT_INSTALLEDPOWERPHASEA: param.TrySetWithCheck(c.loadA, powerError); break;
                    case Parameters.RVT_LINEOUT_INSTALLEDPOWERPHASEB: param.TrySetWithCheck(c.loadB, powerError); break;
                    case Parameters.RVT_LINEOUT_INSTALLEDPOWERPHASEC: param.TrySetWithCheck(c.loadC, powerError); break;
                    case Parameters.RVT_LINEOUT_PF: param.TrySetWithCheck(c.powerFactor, factorsError); break;
                    case Parameters.RVT_LINEOUT_KCC: param.TrySetWithCheck(c.demandCircuitFactor, factorsError); break;
                    case Parameters.RVT_LINEOUT_KCP: param.TrySetWithCheck(c.demandPanelFactor, factorsError); break;
                    case Parameters.RVT_LINEOUT_BREAKERNOMINALCURRRENT: param.TrySetWithCheck(CTDbl(c.breakerNominalCurrent), currentError); break;
                    case Parameters.RVT_LINEOUT_BTREAKERRELEASECURRENT: param.TrySetWithCheck(CTDbl(c.breakerReleaseCurrent), currentError); break;

                    case Parameters.RVT_LINEOUT_DIFFPROTECTIONRELEASE:
                        if (c.diffReleaseCurrent != "нет")
                        {
                            double dbl;
                            if (double.TryParse(c.diffReleaseCurrent.OnlyNumbersAndCommas(), out dbl))
                            {
                                param.TrySetWithCheck(dbl, currentError);
                            }
                            else
                            {
                                param.TrySet(0); //?
                            }
                        }
                        else
                        {
                            param.TrySet(0); //?
                        }
                        break;

                    case Parameters.RVT_LINEOUT_CONTACTORANNOTATION: param.TrySetWithCheck("KM" + c.number); break;
                    case Parameters.RVT_LINEOUT_CONTACTORNOMINALCURRENT: param.TrySetWithCheck(CTDbl(c.breakerNominalCurrent), currentError); break;
                    case Parameters.RVT_LINEOUT_CABLETYPE: param.TrySetWithCheck(c.cableType); break;
                    case Parameters.RVT_LINEOUT_MULTIORSINGLECORE: param.TrySetWithCheck(c.coreQuantity); break;
                    case Parameters.RVT_LINEOUT_COREMATERIAL: param.TrySetWithCheck(c.coreMaterial); break;
                    case Parameters.RVT_LINEOUT_INSULATIONMATERIAL: param.TrySetWithCheck(c.insulationMaterial); break;
                    case Parameters.RVT_LINEOUT_COREQUANTITYANDCROSSSECTION: param.TrySetWithCheck(c.coreQuantityAndCrossSection); break;
                    case Parameters.RVT_LINEOUT_RATEDLENGTH: param.TrySetWithCheck(c.ratedLength, lengthError); break;
                    case Parameters.RVT_LINEOUT_TOTALLENGTH: param.TrySetWithCheck(c.totalLength, lengthError); break;
                    case Parameters.RVT_LINEOUT_MAXLENGTH: param.TrySetWithCheck(c.maxLength, lengthError); break;
                    case Parameters.RVT_LINEOUT_STANDART: param.TrySetWithCheck(c.standart); break;
                    case Parameters.RVT_LINEOUT_CABLINGTYPE: param.TrySetWithCheck(c.cablingType); break;
                    case Parameters.RVT_LINEOUT_MAXVOLTAGEDROP: param.TrySetWithCheck(c.maxVoltageDrop, voltageDropError); break;
                    case Parameters.RVT_LINEOUT_VOLTAGEDROP: param.TrySetWithCheck(c.voltageDrop, voltageDropError); break;
                }
            }
        }

        public void UpdateLineIn(Element e)
        {
            ParameterSet parameters = e.Parameters;
            foreach (Parameter param in parameters)
            {
                switch (param.Definition.Name)
                {
                    case RVT_LINEIN_IS3PH:
                        if (p.phase == "3Ф")
                        {
                            param.TrySetWithCheck(1, factorsError);
                        }
                        else
                        {
                            param.TrySetWithCheck(0, factorsError);
                        }
                        break;

                    case RVT_LINEIN_NUMBER: param.TrySetWithCheck(p.number); break;
                    case RVT_LINEIN_PANELNAME: param.TrySetWithCheck(p.ownerPanel); break;
                    case RVT_LINEIN_BREAKERANNOTATION: param.TrySetWithCheck(p.breakerNumber); break;
                    case RVT_LINEIN_DESCRIPTION: param.TrySetWithCheck(p.description); break;
                    case RVT_LINEIN_PHASE: param.TrySetWithCheck(p.phase); break;
                    case RVT_LINEIN_VOLTAGE: param.TrySetWithCheck(CTDbl(p.voltage), voltageError); break;
                    case RVT_LINEIN_INSTALLEDPOWERPHASEA: param.TrySetWithCheck(p.loadA, powerError); break;
                    case RVT_LINEIN_INSTALLEDPOWERPHASEB: param.TrySetWithCheck(p.loadB, powerError); break;
                    case RVT_LINEIN_PF: param.TrySetWithCheck(p.powerFactor, factorsError); break;
                    case RVT_LINEIN_KCC: param.TrySetWithCheck(p.demandCircuitFactor, factorsError); break;
                    case RVT_LINEIN_KCP: param.TrySetWithCheck(p.demandPanelFactor, factorsError); break;

                    case RVT_LINEIN_BREAKERNOMINALCURRRENT: param.TrySetWithCheck(CTDbl(p.breakerNominalCurrent), currentError); break;
                    case RVT_LINEIN_BTREAKERRELEASECURRENT: param.TrySetWithCheck(CTDbl(p.breakerReleaseCurrent), currentError); break;

                    case RVT_LINEIN_CABLETYPE: param.TrySetWithCheck(p.cableType); break;
                    case RVT_LINEIN_MULTIORSINGLECORE: param.TrySetWithCheck(p.coreQuantity); break;
                    case RVT_LINEIN_COREMATERIAL: param.TrySetWithCheck(p.coreMaterial); break;
                    case RVT_LINEIN_INSULATIONMATERIAL: param.TrySetWithCheck(p.insulationMaterial); break;
                    case RVT_LINEIN_COREQUANTITYANDCROSSSECTION: param.TrySetWithCheck(p.coreQuantityAndCrossSection); break;
                    case RVT_LINEIN_RATEDLENGTH: param.TrySetWithCheck(p.ratedLength, lengthError); break;
                    case RVT_LINEIN_TOTALLENGTH: param.TrySetWithCheck(p.totalLength, lengthError); break;
                    case RVT_LINEIN_MAXLENGTH: param.TrySetWithCheck(p.maxLength, lengthError); break;
                    case RVT_LINEIN_STANDART: param.TrySetWithCheck(p.standart); break;
                    case RVT_LINEIN_CABLINGTYPE: param.TrySetWithCheck(p.cablingType); break;
                    case RVT_LINEIN_MAXVOLTAGEDROP: param.TrySetWithCheck(p.maxVoltageDrop, voltageError); break;
                    case RVT_LINEIN_VOLTAGEDROP: param.TrySetWithCheck(p.voltageDrop, voltageError); break;
                    case RVT_LINEIN_SOURCE:
                        if (p.ownerPanel != null)
                        {
                            param.TrySetWithCheck("От щита " + p.ownerPanel + " выключателя " + p.breakerNumber);
                        }
                        break;
                }
            }
        }

        public void UpdateTotalLoads(Element e)
        {
            ParameterSet parameters = e.Parameters;

            foreach (Parameter param in parameters)
            {
                switch (param.Definition.Name)
                {
                    case Parameters.RVT_PANELINFORMATION_PANELNAME: param.TrySetWithCheck(p.name); break;
                    case Parameters.RVT_PANELINFORMATION_PF: param.TrySetWithCheck(p.powerFactor, factorsError); break;
                    case Parameters.RVT_PANELINFORMATION_FULLPOWER: param.TrySetWithCheck(p.fullPower, powerError); break;
                    case Parameters.RVT_PANELINFORMATION_RATEDPOWER: param.TrySetWithCheck(p.ratedLoad, powerError); break;
                    case Parameters.RVT_PANELINFORMATION_RATEDCURRENT: param.TrySetWithCheck(p.current, currentError); break;
                    case Parameters.RVT_PANELINFORMATION_INSTALLEDPOWER: param.TrySetWithCheck(p.load, powerError); break;
                    case Parameters.RVT_PANELINFORMATION_DEMANDFACTOR: param.TrySetWithCheck(p.demandPanelFactor, factorsError); break;
                }
            }
        }



        public void UpdatePhases(Element e)
        {
            ParameterSet parameters = e.Parameters;
            foreach (Parameter param in parameters)
            {
                if (param.Definition.Name.Equals(Parameters.RVT_BUSSTART_PHASE))
                {
                    if (p.phase == "3Ф")
                    {
                        param.TrySetWithCheck("A,B,C");
                    }
                    else
                    {
                        param.TrySetWithCheck(p.phase);
                    }
                }
            }
        }



        public List<Element> getElements(string uid)
        {
            //Get all symbols in model
            IList<Element> symbols = new FilteredElementCollector(doc)
                       .OfClass(typeof(FamilySymbol))
                       .ToArray();

            //Get elements with storage
            List<Element> elementsWithStorage = new List<Element>();

            foreach (Element e in symbols)
            {
                //Check if have storage
                IList<Guid> guids = e.GetEntitySchemaGuids();

                if (guids.Contains(BIND_GUID_2_01))
                {
                    elementsWithStorage.Add(e);
                }

            }

            List<Element> elements = new List<Element>();

            // Get binded element 
            foreach (Element e in elementsWithStorage)
            {
                Binder eb = new Binder(e);
                string uidfs = eb.LinkedItemId();


                if (uidfs == uid)
                {
                    elements.Add(e);
                }
            }
            return elements;
        }

        public List<Element> getElements(string uid, string type)
        {
            //Get all symbols in model
            IList<Element> symbols = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfCategory(BuiltInCategory.OST_GenericAnnotation)
                      // .OfClass(typeof(FamilySymbol))
                   
                       .ToArray();

            //Get elements with storage
            List<Element> elementsWithStorage = new List<Element>();

            foreach (Element e in symbols)
            {
                //Check if have storage
                IList<Guid> guids = e.GetEntitySchemaGuids();

                if (guids.Contains(BIND_GUID_2_01))
                {
                    elementsWithStorage.Add(e);
                }

            }

            List<Element> elements = new List<Element>();

            // Get binded element 
            foreach (Element e in elementsWithStorage)
            {
                Binder eb = new Binder(e);
                string uidfs = eb.LinkedItemId();
                string t = eb.GetType();

                if (uidfs == uid && t == type)
                {
                    elements.Add(e);
                }
            }
            return elements;
        }







        public List<string> getElementsUId(string uid)
        {
            List<Element> elements = getElements( uid);
            List<string> elementsId = new List<string>();
            if (elements.Count() == 0)
            {
                return null;
            }
            else
            {
                foreach (Element element in elements)
                {
                    elementsId.Add(element.UniqueId);
                }
            }
            return elementsId;
        }


        public List<string> getElementsUId(string uid, string type)
        {
            List<Element> elements = getElements(uid, type);
            List<string> elementsId = new List<string>();
            if (elements.Count() == 0)
            {
                return null;
            }
            else
            {
                foreach (Element element in elements)
                {
                    elementsId.Add(element.UniqueId);
                }
            }
            return elementsId;
        }
    }
}
