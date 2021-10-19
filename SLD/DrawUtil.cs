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
    class DrawUtil
    {
        Document doc;
        public View view { get; set; }
        Panel panel;


        public DrawUtil(
            Document doc,
            View view,
            Panel panel
            )
        {
            this.doc = doc;
            this.view = view;
            this.panel = panel;
        }



        public static void CreateDraftView(Document doc, string draftViewName, string bindUID)
        {


            // Creating new drafting view or clear old
            using (Transaction tx = new Transaction(doc))
            {
                tx.Start("Создание чертежного вида");

                ViewDrafting panelDraft;

                if (Util.IsDraftingViewExsists(doc, draftViewName))
                {
                    ICollection<ElementId> elementsInDraftingView = Util.ElementsIdsInDraftingView(doc, draftViewName);

                    panelDraft = Util.GetDraftingViewByName(doc, draftViewName);
                    panelDraft.Scale = 1;

                    Binder bindDraft = new Binder(panelDraft);
                    bindDraft.Bind(bindUID, Parameters.BIND_TYPE_DRAFTVIEW);


                    foreach (ElementId e in elementsInDraftingView)
                    {
                        try
                        {
                            if (e != panelDraft.Id)
                            {
                                doc.Delete(e);
                            }
                        }
                        catch
                        {

                        }
                    }
                }
                else
                {
                    panelDraft = ViewDrafting.Create(doc, Util.viewFamilyType(doc).Id);
                    panelDraft.Name = draftViewName;
                    panelDraft.Scale = 1;

                    //Bind element
                    Binder bindDraft = new Binder(panelDraft);
                    bindDraft.Bind(bindUID, Parameters.BIND_TYPE_DRAFTVIEW);
                }
                tx.Commit();
            }

        }



        public void Feeder(double xCoord, double yCoord)
        {
            // Вставляем вводной выключатель
            FamilySymbol lineInSymbol = Util.GetSymbolByName(doc, RVT_LINEIN_TYPENAME_BREAKER);
            XYZ lineInCoords = new XYZ(xCoord, yCoord, 0).ToFeets();
            FamilyInstance lineIn = doc.Create.NewFamilyInstance(lineInCoords, lineInSymbol, view);
            ParameterSet parametersLineIn = lineIn.Parameters;

            //Bind element
            Binder li = new Binder(lineIn);
            li.Bind(panel.uid, BIND_TYPE_LINEIN);

            foreach (Parameter param in parametersLineIn)
            {
                switch (param.Definition.Name)
                {
                    case RVT_LINEIN_IS3PH:
                        if (panel.phase == "3Ф")
                        {
                            param.TrySet(1);
                        }
                        else
                        {
                            param.TrySet(0);
                        }
                        break;
                    //case RVT_LINEOUT_ISN: param.TrySet(circuit.breakerNumber); break;
                    //case RVT_LINEOUT_ISDIFF: param.TrySet(circuit.breakerNumber); break;
                    //case RVT_LINEOUT_ISCONTACTOR: param.TrySet(circuit.breakerNumber); break;
                    //case RVT_LINEOUT_SHOWRELEASE: param.TrySet(circuit.breakerNumber); break;
                    case RVT_LINEIN_NUMBER: param.TrySet(panel.number); break;
                    case RVT_LINEIN_PANELNAME: param.TrySet(panel.ownerPanel); break;
                    case RVT_LINEIN_BREAKERANNOTATION: param.TrySet(panel.breakerNumber); break;
                    case RVT_LINEIN_DESCRIPTION: param.TrySet(panel.description); break;
                    case RVT_LINEIN_PHASE: param.TrySet(panel.phase); break;
                    case RVT_LINEIN_VOLTAGE: param.TrySet(CTDbl(panel.voltage)); break;
                    //case RVT_LINEOUT_INSTALLEDPOWER: param.TrySet(circuit.loadA); break;
                    case RVT_LINEIN_INSTALLEDPOWERPHASEA: param.TrySet(panel.loadA); break;
                    case RVT_LINEIN_INSTALLEDPOWERPHASEB: param.TrySet(panel.loadB); break;
                    case RVT_LINEIN_INSTALLEDPOWERPHASEC:
                        try { param.TrySet(panel.loadC); }
                        catch { param.TrySet(0); }
                        break;


                    case RVT_LINEIN_PF: param.TrySet(panel.powerFactor); break;
                    case RVT_LINEIN_KCC: param.TrySet(panel.demandCircuitFactor); break;
                    case RVT_LINEIN_KCP: param.TrySet(panel.demandPanelFactor); break;
                    //case RVT_LINEOUT_RATEDPOWER: param.TrySet(circuit.breakerNumber); break;
                    //case RVT_LINEOUT_RATEDPOWER_PHASEA: param.TrySet(circuit.breakerNumber); break;
                    //case RVT_LINEOUT_RATEDPOWER_PHASEB: param.TrySet(circuit.breakerNumber); break;
                    //case RVT_LINEOUT_RATEDPOWER_PHASEC: param.TrySet(circuit.breakerNumber); break;
                    //case RVT_LINEOUT_RATEDCURRENT_PHASEA: param.TrySet(circuit.breakerNumber); break;
                    //case RVT_LINEOUT_RATEDCURRENT_PHASEB: param.TrySet(circuit.breakerNumber); break;
                    //case RVT_LINEOUT_RATEDCURRENT_PHASEC: param.TrySet(circuit.breakerNumber); break;
                    //case RVT_LINEOUT_RATEDCURRENT: param.TrySet(circuit.breakerNumber); break;
                    //case RVT_LINEOUT_BREAKERMODEL: param.TrySet(circuit.breakerNumber); break;
                    case RVT_LINEIN_BREAKERNOMINALCURRRENT: param.TrySet(CTDbl(panel.breakerNominalCurrent)); break;
                    case RVT_LINEIN_BTREAKERRELEASECURRENT: param.TrySet(CTDbl(panel.breakerReleaseCurrent)); break;
                    //case RVT_LINEOUT_BREAKERTYPE: param.TrySet(circuit.breakerNumber); break;
                    /*case RVT_LINEOUT_DIFFPROTECTIONRELEASE:

                        if (circuit.diffReleaseCurrent != "нет")
                        {
                            double dbl;
                            if (double.TryParse(circuit.diffReleaseCurrent.OnlyNumbersAndCommas(), out dbl))
                            {
                                param.TrySet(dbl);
                            }
                            else
                            {
                                param.TrySet(0);
                            }
                        }
                        else
                        {
                            param.TrySet(0);
                        }
                        break; */
                    //case RVT_LINEOUT_CONTACTORANNOTATION: param.TrySet("KM" + circuit.number); break;
                    //case RVT_LINEOUT_CONTACTORMODEL: param.TrySet(circuit.breakerNumber); break;
                    //case RVT_LINEOUT_CONTACTORNOMINALCURRENT: param.TrySet(CTDbl(circuit.breakerNominalCurrent)); break;
                    //case RVT_LINEOUT_ONPLANNUMBER: param.TrySet(circuit.breakerNumber); break;
                    case RVT_LINEIN_CABLETYPE: param.TrySet(panel.cableType); break;
                    case RVT_LINEIN_MULTIORSINGLECORE: param.TrySet(panel.coreQuantity); break;
                    case RVT_LINEIN_COREMATERIAL: param.TrySet(panel.coreMaterial); break;
                    case RVT_LINEIN_INSULATIONMATERIAL: param.TrySet(panel.insulationMaterial); break;
                    case RVT_LINEIN_COREQUANTITYANDCROSSSECTION: param.TrySet(panel.coreQuantityAndCrossSection); break;
                    case RVT_LINEIN_RATEDLENGTH: param.TrySet(panel.ratedLength); break;
                    case RVT_LINEIN_TOTALLENGTH: param.TrySet(panel.totalLength); break;
                    case RVT_LINEIN_MAXLENGTH: param.TrySet(panel.maxLength); break;
                    //case RVT_LINEOUT_UGO: param.TrySet(circuit.breakerNumber); break;
                    case RVT_LINEIN_STANDART: param.TrySet(panel.standart); break;
                    case RVT_LINEIN_CABLINGTYPE: param.TrySet(panel.cablingType); break;
                    case RVT_LINEIN_MAXVOLTAGEDROP: param.TrySet(panel.maxVoltageDrop); break;
                    case RVT_LINEIN_VOLTAGEDROP: param.TrySet(panel.voltageDrop); break;
                    case RVT_LINEIN_SOURCE:
                        if (panel.ownerPanel != null)
                        {
                            param.TrySet("От щита " + panel.ownerPanel + " выключателя " + panel.breakerNumber);
                        }
                        break;


                }

            }
        }

        public void BoardParameters(
            double x,
            double y
            )
        {

            XYZ panelIformationCoords = new XYZ(x, y, 0).ToFeets();
            FamilySymbol panelIformationSymbol = Util.GetSymbolByName(doc, Parameters.RVT_PANELINFORMATION_FAMILYNAME);
            FamilyInstance panelIformation = doc.Create.NewFamilyInstance(panelIformationCoords, panelIformationSymbol, view);

            //Bind element
            Binder pi = new Binder(panelIformation);
            pi.Bind(panel.uid, BIND_TYPE_PANELINFORMATION);

            ParameterSet parameterspanelIformation = panelIformation.Parameters;
            foreach (Parameter param in parameterspanelIformation)
            {
                switch (param.Definition.Name)
                {
                    case Parameters.RVT_PANELINFORMATION_PANELNAME: param.TrySet(panel.name); break;
                    case Parameters.RVT_PANELINFORMATION_PF: param.TrySet(panel.powerFactor); break;
                    case Parameters.RVT_PANELINFORMATION_FULLPOWER: param.TrySet(panel.fullPower); break;
                    case Parameters.RVT_PANELINFORMATION_RATEDPOWER: param.TrySet(panel.ratedLoad); break;
                    case Parameters.RVT_PANELINFORMATION_RATEDCURRENT: param.TrySet(panel.current); break;
                    case Parameters.RVT_PANELINFORMATION_INSTALLEDPOWER: param.TrySet(panel.load); break;
                    case Parameters.RVT_PANELINFORMATION_DEMANDFACTOR: param.TrySet(panel.demandPanelFactor); break;
                }
            }
        }

        public void StartElement(
            double x,
            double y
            )
        {
            FamilySymbol busStartSymbol = Util.GetSymbolByName(doc, Parameters.RVT_BUSSTART_FAMILYNAME);
            XYZ busStartCoords = new XYZ(x, y, 0).ToFeets();
            FamilyInstance busStart = doc.Create.NewFamilyInstance(busStartCoords, busStartSymbol, view);
            ParameterSet busStartParams = busStart.Parameters;

            //Bind element
            Binder bs = new Binder(busStart);
            bs.Bind(panel.uid, BIND_TYPE_BUSSTART);

            foreach (Parameter param in busStartParams)
            {
                if (param.Definition.Name.Equals(Parameters.RVT_BUSSTART_PHASE))
                {
                    if (panel.phase == "3Ф")
                    {
                        param.TrySet("A,B,C");
                    }
                    else
                    {
                        param.TrySet(panel.phase);
                    }
                }
            }
        }

        public void FinishElement(
            double x,
            double y)
        {
            FamilySymbol busFinishSymbol = Util.GetSymbolByName(doc, Parameters.RVT_BUSSFINISH_FAMILYNAME);
            XYZ busFinishCoords = new XYZ(x, y, 0).ToFeets();
            FamilyInstance busFinish = doc.Create.NewFamilyInstance(busFinishCoords, busFinishSymbol, view);

            //Bind element
            Binder bf = new Binder(busFinish);
            bf.Bind(panel.uid, BIND_TYPE_BUSFINISH);
        }

        public void Table(
            double x,
            double y)
        {
            FamilySymbol tableSymbol = Util.GetSymbolByName(doc, Parameters.RVT_TABLE_FAMILYNAME);
            XYZ tableCoords = new XYZ(x, y, 0).ToFeets();
            FamilyInstance table = doc.Create.NewFamilyInstance(tableCoords, tableSymbol, view);

            //Bind element
            Binder t = new Binder(table);
            t.Bind(panel.puid, BIND_TYPE_TABLE);
        }


        public void LineOut(
            int num,
            double x,
            double y)
        {
            Circuit circuit = panel.circuits[num];
            string typeName = Parameters.RVT_LINEOUT_TYPENAME_BREAKER; ;
            if (circuit.contactor == true && circuit.diffReleaseCurrent == "Нет")
            {
                typeName = Parameters.RVT_LINEOUT_TYPENAME_BREAKERCONTACTOR;
            }
            if (circuit.contactor == true && circuit.diffReleaseCurrent != "Нет")
            {
                typeName = Parameters.RVT_LINEOUT_TYPENAME_BREAKERDIFFCONTACTOR;
            }
            if (circuit.contactor == false && circuit.diffReleaseCurrent != "Нет")
            {
                typeName = Parameters.RVT_LINEOUT_TYPENAME_BREAKERDIFF;
            }

            FamilySymbol lineSymbol = Util.GetSymbolByName(doc, typeName);
            XYZ coords = new XYZ(x, y, 0).ToFeets();
            FamilyInstance line = doc.Create.NewFamilyInstance(coords, lineSymbol, view);
            ParameterSet parametersLineOut = line.Parameters;

            //Bind element
            Binder l = new Binder(line);
            l.Bind(circuit.uid, BIND_TYPE_LINEOUT);
            /*
              Bind to paneladd if nessesary here
            */


            foreach (Parameter param in parametersLineOut)
            {
                switch (param.Definition.Name)
                {
                    case Parameters.RVT_LINEOUT_IS3PH:
                        if (circuit.phase == "3Ф")
                        {
                            param.TrySet(1);
                        }
                        else
                        {
                            param.TrySet(0);
                        }
                        break;
                    //case Parameters.RVT_LINEOUT_ISN: param.TrySet(circuit.breakerNumber); break;
                    //case Parameters.RVT_LINEOUT_ISDIFF: param.TrySet(circuit.breakerNumber); break;
                    //case Parameters.RVT_LINEOUT_ISCONTACTOR: param.TrySet(circuit.breakerNumber); break;
                    //case Parameters.RVT_LINEOUT_SHOWRELEASE: param.TrySet(circuit.breakerNumber); break;
                    case RVT_LINEOUT_NUMBER: param.TrySet(circuit.number); break;
                    case RVT_LINEOUT_PANELNAME: param.TrySet(panel.name); break;
                    case RVT_LINEOUT_BREAKERANNOTATION: param.TrySet(circuit.breakerNumber); break;
                    case RVT_LINEOUT_DESCRIPTION:

                        string descriptionWithRooms = circuit.description;

                        if (circuit.rooms != null)
                        {
                            if (circuit.rooms.Count() > 0)
                            {
                                descriptionWithRooms = descriptionWithRooms + "\n" + "(пом. " + circuit.rooms + ")";
                            }
                        }


                        param.TrySet(descriptionWithRooms); break;




                    case Parameters.RVT_LINEOUT_PHASE: param.TrySet(circuit.phase); break;
                    case Parameters.RVT_LINEOUT_VOLTAGE: param.TrySet(CTDbl(circuit.voltage)); break;
                    //case Parameters.RVT_LINEOUT_INSTALLEDPOWER: param.TrySet(circuit.loadA); break;
                    case Parameters.RVT_LINEOUT_INSTALLEDPOWERPHASEA: param.TrySet(circuit.loadA); break;
                    case Parameters.RVT_LINEOUT_INSTALLEDPOWERPHASEB: param.TrySet(circuit.loadB); break;
                    case Parameters.RVT_LINEOUT_INSTALLEDPOWERPHASEC: param.TrySet(circuit.loadC); break;
                    case Parameters.RVT_LINEOUT_PF: param.TrySet(circuit.powerFactor); break;
                    case Parameters.RVT_LINEOUT_KCC: param.TrySet(circuit.demandCircuitFactor); break;
                    case Parameters.RVT_LINEOUT_KCP: param.TrySet(circuit.demandPanelFactor); break;
                    //case Parameters.RVT_LINEOUT_RATEDPOWER: param.TrySet(circuit.breakerNumber); break;
                    //case Parameters.RVT_LINEOUT_RATEDPOWER_PHASEA: param.TrySet(circuit.breakerNumber); break;
                    //case Parameters.RVT_LINEOUT_RATEDPOWER_PHASEB: param.TrySet(circuit.breakerNumber); break;
                    //case Parameters.RVT_LINEOUT_RATEDPOWER_PHASEC: param.TrySet(circuit.breakerNumber); break;
                    //case Parameters.RVT_LINEOUT_RATEDCURRENT_PHASEA: param.TrySet(circuit.breakerNumber); break;
                    //case Parameters.RVT_LINEOUT_RATEDCURRENT_PHASEB: param.TrySet(circuit.breakerNumber); break;
                    //case Parameters.RVT_LINEOUT_RATEDCURRENT_PHASEC: param.TrySet(circuit.breakerNumber); break;
                    //case Parameters.RVT_LINEOUT_RATEDCURRENT: param.TrySet(circuit.breakerNumber); break;
                    //case Parameters.RVT_LINEOUT_BREAKERMODEL: param.TrySet(circuit.breakerNumber); break;
                    case Parameters.RVT_LINEOUT_BREAKERNOMINALCURRRENT: param.TrySet(CTDbl(circuit.breakerNominalCurrent)); break;
                    case Parameters.RVT_LINEOUT_BTREAKERRELEASECURRENT: param.TrySet(CTDbl(circuit.breakerReleaseCurrent)); break;
                    //case Parameters.RVT_LINEOUT_BREAKERTYPE: param.TrySet(circuit.breakerNumber); break;
                    case Parameters.RVT_LINEOUT_DIFFPROTECTIONRELEASE:

                        if (circuit.diffReleaseCurrent != "нет")
                        {
                            double dbl;
                            if (double.TryParse(circuit.diffReleaseCurrent.OnlyNumbersAndCommas(), out dbl))
                            {
                                param.TrySet(dbl);
                            }
                            else
                            {
                                param.TrySet(0);
                            }
                        }
                        else
                        {
                            param.TrySet(0);
                        }
                        break;
                    case Parameters.RVT_LINEOUT_CONTACTORANNOTATION: param.TrySet("KM" + circuit.number); break;
                    //case Parameters.RVT_LINEOUT_CONTACTORMODEL: param.TrySet(circuit.breakerNumber); break;
                    case Parameters.RVT_LINEOUT_CONTACTORNOMINALCURRENT: param.TrySet(CTDbl(circuit.breakerNominalCurrent)); break;
                    //case Parameters.RVT_LINEOUT_ONPLANNUMBER: param.TrySet(circuit.breakerNumber); break;
                    case Parameters.RVT_LINEOUT_CABLETYPE: param.TrySet(circuit.cableType); break;
                    case Parameters.RVT_LINEOUT_MULTIORSINGLECORE: param.TrySet(circuit.coreQuantity); break;
                    case Parameters.RVT_LINEOUT_COREMATERIAL: param.TrySet(circuit.coreMaterial); break;
                    case Parameters.RVT_LINEOUT_INSULATIONMATERIAL: param.TrySet(circuit.insulationMaterial); break;
                    case Parameters.RVT_LINEOUT_COREQUANTITYANDCROSSSECTION: param.TrySet(circuit.coreQuantityAndCrossSection); break;
                    case Parameters.RVT_LINEOUT_RATEDLENGTH: param.TrySet(circuit.ratedLength); break;
                    case Parameters.RVT_LINEOUT_TOTALLENGTH: param.TrySet(circuit.totalLength); break;
                    case Parameters.RVT_LINEOUT_MAXLENGTH: param.TrySet(circuit.maxLength); break;
                    //case Parameters.RVT_LINEOUT_UGO: param.TrySet(circuit.breakerNumber); break;
                    case Parameters.RVT_LINEOUT_STANDART: param.TrySet(circuit.standart); break;
                    case Parameters.RVT_LINEOUT_CABLINGTYPE: param.TrySet(circuit.cablingType); break;
                    case Parameters.RVT_LINEOUT_MAXVOLTAGEDROP: param.TrySet(circuit.maxVoltageDrop); break;
                    case Parameters.RVT_LINEOUT_VOLTAGEDROP: param.TrySet(circuit.voltageDrop); break;
                }
            }

            // Вставляем УГО

            string UGOtype = Parameters.RVT_LOADSYMBOL_TYPENAME_OUT;
            string loadType = circuit.description.ToLower();

            if (loadType.Contains("освещ") ||
                loadType.Contains("свети") ||
                loadType.Contains("ламп") ||
                loadType.Contains("светодиод") ||
                loadType.Contains("light") ||
                loadType.Contains("luminar") ||
                loadType.Contains("led"))
            {
                UGOtype = Parameters.RVT_LOADSYMBOL_TYPENAME_LF;
            }

            if (loadType.Contains("розет") ||
                loadType.Contains("socket") ||
                loadType.Contains("plug"))
            {
                UGOtype = Parameters.RVT_LOADSYMBOL_TYPENAME_SOCKET;
            }

            FamilySymbol UGOSymbol = Util.GetSymbolByName(doc, UGOtype);
            XYZ UGOCoords = new XYZ(x, y, 0).ToFeets();
            FamilyInstance UGO = doc.Create.NewFamilyInstance(UGOCoords, UGOSymbol, view);
            ParameterSet UGOParams = UGO.Parameters;

            //Bind element
            Binder ugo = new Binder(UGO);
            ugo.Bind(panel.puid, BIND_TYPE_UGO);
        }

        public void Reserve(
            int num,
            double x,
            double y)
        {
            FamilySymbol reserveSymbol = Util.GetSymbolByName(doc, Parameters.RVT_RESERVE_TYPENAME_BREAKER);
            XYZ reserveCoords = new XYZ(x, y, 0).ToFeets();
            FamilyInstance reserve = doc.Create.NewFamilyInstance(reserveCoords, reserveSymbol, view);

            //Bind element
            Binder r = new Binder(reserve);
            r.Bind(panel.puid, BIND_TYPE_RESERVE);

            ParameterSet parametersReserve = reserve.Parameters;
            foreach (Parameter param in parametersReserve)
            {
                switch (param.Definition.Name)
                {
                    case Parameters.RVT_LINEIN_BREAKERANNOTATION:
                        param.TrySet("QF" + (panel.circuits.Count() + num + 1).ToString("F0"));
                        break;
                    case Parameters.RVT_LINEIN_NUMBER:
                        param.TrySet(panel.circuits.Count() + num + 1.ToString());
                        break;
                }


                if (param.Definition.Name.Equals(Parameters.RVT_LINEIN_BREAKERANNOTATION))
                {
                    param.TrySet("QF" + (panel.circuits.Count() + num + 1).ToString("F0"));
                }

            }


        }

        public void CableListHeader(double x, double y)
        {
            FamilySymbol cableListHeader = GetSymbolByFamilyAndName(doc, RVT_CABLELIST_HEADER_FAMILYNAME, RVT_CABLELIST_HEADER_TYPENAME);
            XYZ cableListHeaderCoords = new XYZ(x, y, 0).ToFeets();
            FamilyInstance cableListHeaderFI = doc.Create.NewFamilyInstance(cableListHeaderCoords, cableListHeader, view);

            //Bind element
            //Binder t = new Binder(cableListHeaderFI);
            //t.Bind(panel.puid, BIND_TYPE_CABLELISTHEADER);
        }



        public void CableListRow(int num, double x, double y)
        {
            FamilySymbol cableListRow = GetSymbolByFamilyAndName(doc, RVT_CABLELIST_ROW_FAMILYNAME, RVT_CABLELIST_ROW_TYPENAME);
            XYZ cableListRowCoords = new XYZ(x, y, 0).ToFeets();
            FamilyInstance cableListRowFI = doc.Create.NewFamilyInstance(cableListRowCoords, cableListRow, view);
            ParameterSet parametersCableListRow = cableListRowFI.Parameters;


            foreach (Parameter param in parametersCableListRow)
            {
                switch (param.Definition.Name)
                {
                    case RVT_CABLELIST_ROW_MARK: param.TrySet(panel.circuits[num].number); break;
                    case RVT_CABLELIST_ROW_START: param.TrySet(panel.circuits[num].number); break;
                    case RVT_CABLELIST_ROW_FINISH: param.TrySet(panel.circuits[num].number); break;
                    case RVT_CABLELIST_ROW_PROJECTSECTOR: param.TrySet(panel.circuits[num].number); break;
                    case RVT_CABLELIST_ROW_PROJECTCABLEMARK: param.TrySet(panel.circuits[num].number); break;
                    case RVT_CABLELIST_ROW_PROJECTSECTION: param.TrySet(panel.circuits[num].number); break;
                    case RVT_CABLELIST_ROW_PROJECTLENGTH: param.TrySet(panel.circuits[num].number); break;

                }
            }


























           // Binder l = new Binder(line);
           // l.Bind(circuit.uid, BIND_TYPE_LINEOUT);

            // реализовать
        }








        // util

        static FamilySymbol GetSymbolByName(
          Document doc,
          string targetName)
        {
            try
            {
                return new FilteredElementCollector(doc)
                         .OfClass(typeof(FamilySymbol))
                         .Where(q => q.Name == targetName)
                         .First() as FamilySymbol;
            }
            catch
            {
                return null;
            }
        }

        static FamilySymbol GetSymbolByFamilyAndName(
          Document doc,
          string fn,
          string tn)
        {
            try
            {
                return new FilteredElementCollector(doc)
                .OfClass(typeof(FamilySymbol))
                .Cast<FamilySymbol>()
                .Where(x => x.FamilyName.Equals(fn))
                .FirstOrDefault(x => x.Name == tn);
            }
            catch
            {
                return null;
            }
        }



    }
}



