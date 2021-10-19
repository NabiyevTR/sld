#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;

using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Linq;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Architecture;

#endregion

namespace SLD
{
    class Util
    {
        //Elements

        public static Element GetElementByCategoryAndName(
            Document doc,
            BuiltInCategory cat,
            string targetName)
        {
            return new FilteredElementCollector(doc)
              .OfCategory(cat)
              .FirstOrDefault<Element>(
                e => e.Name.Equals(targetName));
        }

        public static ICollection<Element> GetElectricalCircuitsByBoardName(
            Document doc,
            string targetName)
        {
            ParameterValueProvider provider = new ParameterValueProvider(
                new ElementId((int)BuiltInParameter.RBS_ELEC_CIRCUIT_PANEL_PARAM)
                               );

            FilterStringRule rule = new FilterStringRule(provider, new FilterStringEquals(), targetName, true);

            ElementParameterFilter filter = new ElementParameterFilter(rule);

            return
                (new FilteredElementCollector(doc))
                .OfCategory(BuiltInCategory.OST_ElectricalCircuit)
                .WherePasses(filter)
                .ToArray();
        }


        // FOR TEST!!!

        /*
                public static Element GetCircuitByConnectedElement (
                    Document doc,
                    ElementId id)
                {
                    ICollection<Element> elems = new FilteredElementCollector(doc)
                        .OfCategory(BuiltInCategory.OST_ElectricalCircuit).
                        ToArray();

                    ElectricalSystem sys;
                    ConnectorSet connectors = sys.ConnectorManager.Connectors;



                    return elems;

                }
        */


        //FOR TEST !!!!
        public static void testingConnectors(
            Document doc,
            Element board
            )
        {
            FilteredElementCollector c = new FilteredElementCollector(doc);
            IList<Element> systems = c.OfClass(typeof(ElectricalSystem)).ToElements();
            ElementId bid = board.Id;

            foreach (ElectricalSystem system in systems)
            {
                ElementSet elems = system.Elements;
                foreach (Element e in elems)
                {
                    if (e.Id.Equals(bid)
                        )
                    {
                        ConnectorSet cs = system.ConnectorManager.Connectors;
                        foreach (Connector co in cs)
                        {
                            Element el = co.Owner;
                            //string name = el.get_Parameter(BuiltInParameter.RBS_ELEC_CIRCUIT_PANEL_PARAM)?.AsValueString();
                            Parameter p = el.get_Parameter(BuiltInParameter.RBS_ELEC_CIRCUIT_PANEL_PARAM);
                            string name = el.get_Parameter(BuiltInParameter.RBS_ELEC_CIRCUIT_PANEL_PARAM)?.AsString();
                            if (name != null)
                            {
                                TaskDialog.Show("Revit", name);
                            }

                        }
                    }
                }
            }
        }


        public static Element getSystemByElemntId(
            Document doc,
            ElementId eId)
        {
            FilteredElementCollector c = new FilteredElementCollector(doc);
            IList<Element> systems = c.OfClass(typeof(ElectricalSystem)).ToElements();

            foreach (ElectricalSystem system in systems)
            {
                ElementSet systemElements = system.Elements;
                foreach (Element e in systemElements)
                {
                    if (e.Id.Equals(eId)
                        )
                    {
                        ConnectorSet cs = system.ConnectorManager.Connectors;
                        foreach (Connector co in cs)
                        {
                            Element owner = co.Owner;
                            Parameter p = owner.get_Parameter(BuiltInParameter.RBS_ELEC_CIRCUIT_PANEL_PARAM);
                            if (p != null)
                            {
                                return owner;
                            }

                        }
                    }
                }
            }
            return null;
        }

        public static Element getElementSystem(
            Element e)
        {
            try
            {
                return getSystemByElemntId(e.Document, e.Id);
            }
            catch
            {
                return null;
            }

        }

        public static List<Element> getSystemsByPanel(Element e)
        {
            ElectricalSystemSet esys = ((FamilyInstance)e).MEPModel.AssignedElectricalSystems;

            List<Element> systems = new List<Element>();

            foreach (ElectricalSystem sys in esys)
            {
                systems.Add(sys as Element);
            }
            return systems;
        }





        /*   public static List<string> GetLoadTypeByCiruit(
               Document doc,
               Element e)
            {
                ParameterValueProvider provider = new ParameterValueProvider(
                    new ElementId((int)BuiltInParameter.RBS_ELEC_LOAD_CLASSIFICATION)
                                   );

                FilterStringRule rule = new FilterStringRule(provider, new FilterStringEquals(), targetName, true);

                ElementParameterFilter filter = new ElementParameterFilter(rule);

                return
                    (new FilteredElementCollector(doc))
                    .OfCategory(BuiltInCategory.OST_ElectricalCircuit)
                    .WherePasses(filter)
                    .ToArray();
            }

    */


        /*
         !!! ДОДЕЛАТЬ !!! Элементы могут быть не только Aluminium или Copper
         */

        public static string GetCoreMaterialByCableType(
           Document doc,
           string cableType)
        {
            Element wireType = new FilteredElementCollector(doc)
                .OfClass(typeof(WireType))
                .FirstOrDefault<Element>(
                e => e.Name.Equals(cableType));

            if (wireType == null)
            {
                return "медь";
            }

            string coreMaterial = wireType.get_Parameter(BuiltInParameter.RBS_WIRE_MATERIAL_PARAM).AsValueString();

            if (coreMaterial == "Aluminum")
            {
                return "алюминий";
            }
            else
            {
                return "медь";
            }
        }

        public static string GetNeutralSizeByCableType(
            Document doc,
            string cableType)
        {
            Element wireType = new FilteredElementCollector(doc)
                .OfClass(typeof(WireType))
                .FirstOrDefault<Element>(
                e => e.Name.Equals(cableType));

            if (wireType == null)
            {
                return "равный фазному";
            }

            string neutralSize = wireType.get_Parameter(BuiltInParameter.RBS_WIRE_NEUTRAL_MODE_PARAM).AsValueString();

            if (neutralSize == "Разные диаметры")
            {
                return "половина фазного";
            }
            else
            {
                return "равный фазному";
            }
        }


        public static FilteredElementCollector GetElementsByCategory(
            Document doc,
            BuiltInCategory cat)
        {
            return new FilteredElementCollector(doc)
                .OfCategory(cat);
        }


        public static Element GetBoardFromSelection(Document doc, ICollection<ElementId> selectedElemntsIds)
        {

            ICollection<Element> boards;

            try
            {
                boards = new FilteredElementCollector(doc, selectedElemntsIds)
                .OfCategory(BuiltInCategory.OST_ElectricalEquipment)
                .ToArray();
            }
            catch
            {
                return null;
            }

            if (boards.Count == 1)
            {
                return boards.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        public static Element GetBoardByName(
            Document doc,
            string targetName)
        {
            return new FilteredElementCollector(doc)
              .OfCategory(BuiltInCategory.OST_ElectricalEquipment)
              .FirstOrDefault<Element>(
                e => e.Name.Equals(targetName));
        }

        public static FamilySymbol GetSymbolByName(
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

        public static List<Room> GetRoomsFromRVTLink(Application app, Document doc)
        {

            FilteredElementCollector links = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_RvtLinks);

            List<Room> rooms = new List<Room>();

            foreach (Document d in app.Documents)
            {

                //Check if file is a link
                if (d.IsLinked)
                {
                    ICollection<Element> roomsInLink = new FilteredElementCollector(d)
                          .OfCategory(BuiltInCategory.OST_Rooms)
                          .ToArray();
                    //Adding rooms in general room list
                    foreach (Element room in roomsInLink)
                    {
                        rooms.Add(room as Room);
                    }
                }
            }
            return rooms;
        }

        /* public static string GetElementRoomNumberFromRVTLink (List<Element> rooms, List<Element> e)
         {
             foreach (Element room in rooms )
             {
                 BoundingBoxXYZ bb = room.get_BoundingBox(null);

                 Outline outline = new Outline(bb.Min, bb.Max);

                 BoundingBoxIntersectsFilter filter
                   = new BoundingBoxIntersectsFilter(outline);

                 BoundingBoxIsInsideFilter filter2 = new BoundingBoxIsInsideFilter(outline);

                 FilteredElementCollector collector = new FilteredElementCollector(document);
                 IList<Element> elements = collector.WherePasses(filter).ToElements();


             }

         }
         */

        //Very slow function
        public static List<string> GetCircuitRoomNumbersFromRVTLink(Document doc, string panelName, string circuitName)
        {
            List<Room> roomsInRVTLink = CreateDiagramInView.roomsInRVTLink;
            List<string> circuitRoomNumbers = new List<string>();


            foreach (Room room in roomsInRVTLink)
            {
                IList<Element> elementsEE =
                    new FilteredElementCollector(doc)
                    .WhereElementIsNotElementType()
                    .WhereElementIsViewIndependent()
                    .OfCategory(BuiltInCategory.OST_ElectricalEquipment)
                    .ToArray();

                IList<Element> elementsEF =
                    new FilteredElementCollector(doc)
                    .WhereElementIsNotElementType()
                    .WhereElementIsViewIndependent()
                    .OfCategory(BuiltInCategory.OST_ElectricalFixtures)
                    .ToArray();

                IList<Element> elementsLF =
                    new FilteredElementCollector(doc)
                    .WhereElementIsNotElementType()
                    .WhereElementIsViewIndependent()
                    .OfCategory(BuiltInCategory.OST_LightingFixtures)
                    .ToArray();

                IList<Element> elementsLD =
                    new FilteredElementCollector(doc)
                    .WhereElementIsNotElementType()
                    .WhereElementIsViewIndependent()
                    .OfCategory(BuiltInCategory.OST_LightingDevices)
                    .ToArray();

                List<Element> elements = new List<Element>();
                elements.AddRange(elementsEE);
                elements.AddRange(elementsEF);
                elements.AddRange(elementsLF);
                elements.AddRange(elementsLD);

                foreach (Element element in elements)
                {
                    string pN = element.get_Parameter(BuiltInParameter.RBS_ELEC_CIRCUIT_PANEL_PARAM)?.AsString();
                    string cN = element.get_Parameter(BuiltInParameter.RBS_ELEC_CIRCUIT_NUMBER)?.AsString();

                    XYZ point;
                    LocationPoint lpoint = element.Location as LocationPoint;
                    point = lpoint.Point as XYZ;

                    if (pN == panelName && cN == circuitName && room.IsPointInRoom(point))
                    {
                        circuitRoomNumbers.Add(room.get_Parameter(BuiltInParameter.ROOM_NUMBER)?.AsString());
                    }
                }

            }

            circuitRoomNumbers = circuitRoomNumbers.Distinct().ToList();

            try
            {
                circuitRoomNumbers = circuitRoomNumbers.OrderBy(o => int.Parse(o)).ToList();
            }
            catch
            {
                circuitRoomNumbers.Sort();
            }

            return circuitRoomNumbers.Distinct().ToList();
        }



        public static List<string> GetCircuitRoomsNumberFromLink(Document doc, Dictionary<ElementId, string> elementsInPanel, string circuitName)
        {
            List<string> roomsNumber = new List<string> { };

            foreach (KeyValuePair<ElementId, string> element in elementsInPanel)
            {
                Element el = doc.GetElement(element.Key);
                string cN = el.get_Parameter(BuiltInParameter.RBS_ELEC_CIRCUIT_NUMBER)?.AsString();
                if (cN == circuitName && !roomsNumber.Contains(element.Value))
                {
                    roomsNumber.Add(element.Value);
                }

            }
            roomsNumber = roomsNumber.Distinct().ToList();

            try
            {
                roomsNumber = roomsNumber.OrderBy(o => int.Parse(o)).ToList();
            }
            catch
            {
                roomsNumber.Sort();
            }

            return roomsNumber.Distinct().ToList();

        }


        public static Dictionary<ElementId, string> GetPanelRoomsNumberFromLink(Document doc, List<ElementId> elementIds)
        {
            Dictionary<ElementId, string> rooms = new Dictionary<ElementId, string> { };

            foreach (ElementId elementId in elementIds)
            {
                rooms.Add(elementId, "null");
            }

            List<Room> roomsInRVTLink = Util.GetRoomsFromRVTLink(doc.Application, doc);

            
            foreach (Room room in roomsInRVTLink)
            {
                foreach (ElementId elementId in elementIds)
                {
                    Element element = doc.GetElement(elementId);
                    XYZ point;
                    LocationPoint lpoint = element.Location as LocationPoint;
                    point = lpoint.Point as XYZ;

                    if (room.IsPointInRoom(point))
                    {
                        rooms[elementId] = room.get_Parameter(BuiltInParameter.ROOM_NUMBER)?.AsString();
                    }

                }
            }

            return rooms;
        }

        public static List<ElementId> GetPanelElectricalElementIds(Document doc, string panelName)
        {

            List<ElementId> elementIds = new List<ElementId> { };

            IList<Element> elementsEE =
                       new FilteredElementCollector(doc)
                       .WhereElementIsNotElementType()
                       .WhereElementIsViewIndependent()
                       .OfCategory(BuiltInCategory.OST_ElectricalEquipment)
                       .ToArray();

            IList<Element> elementsEF =
                new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .WhereElementIsViewIndependent()
                .OfCategory(BuiltInCategory.OST_ElectricalFixtures)
                .ToArray();

            IList<Element> elementsLF =
                new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .WhereElementIsViewIndependent()
                .OfCategory(BuiltInCategory.OST_LightingFixtures)
                .ToArray();

            IList<Element> elementsLD =
                new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .WhereElementIsViewIndependent()
                .OfCategory(BuiltInCategory.OST_LightingDevices)
                .ToArray();

            List<Element> elements = new List<Element>();
            elements.AddRange(elementsEE);
            elements.AddRange(elementsEF);
            elements.AddRange(elementsLF);
            elements.AddRange(elementsLD);

            foreach (Element element in elements)
            {
                string pN = element.get_Parameter(BuiltInParameter.RBS_ELEC_CIRCUIT_PANEL_PARAM)?.AsString();


                if (pN == panelName)
                {
                    elementIds.Add(element.Id);
                }
            }

            // elementIds = elementIds.Distinct().ToList();

            return elementIds;

        }

        public static List<string> GetCircuitRoomNumbers(Document doc, string panelName, string circuitName)
        {
            List<string> circuitRoomNumbers = new List<string>();

            IList<Element> elementsEE = new FilteredElementCollector(doc)
                           .OfCategory(BuiltInCategory.OST_ElectricalEquipment)
                           .ToArray();

            IList<Element> elementsEF = new FilteredElementCollector(doc)
                           .OfCategory(BuiltInCategory.OST_ElectricalFixtures)
                           .ToArray();

            IList<Element> elementsLF = new FilteredElementCollector(doc)
                           .OfCategory(BuiltInCategory.OST_LightingFixtures)
                           .ToArray();

            IList<Element> elementsLD = new FilteredElementCollector(doc)
                           .OfCategory(BuiltInCategory.OST_LightingDevices)
                           .ToArray();

            List<Element> elements = new List<Element>();
            elements.AddRange(elementsEE);
            elements.AddRange(elementsEF);
            elements.AddRange(elementsLF);
            elements.AddRange(elementsLD);


            foreach (Element element in elements)
            {
                string pN = element.get_Parameter(BuiltInParameter.RBS_ELEC_CIRCUIT_PANEL_PARAM)?.AsString();
                string cN = element.get_Parameter(BuiltInParameter.RBS_ELEC_CIRCUIT_NUMBER)?.AsString();

                if (pN == panelName && cN == circuitName)
                {
                    string roomNumber = element.get_Parameter(BuiltInParameter.ELEM_ROOM_NUMBER)?.AsString();
                    if (roomNumber != "" && roomNumber != null)
                    {
                        circuitRoomNumbers.Add(roomNumber);
                    }

                }
            }


            circuitRoomNumbers = circuitRoomNumbers.Distinct().ToList();

            try
            {
                circuitRoomNumbers = circuitRoomNumbers.OrderBy(o => int.Parse(o)).ToList();

            }
            catch
            {
                circuitRoomNumbers.Sort();
            }


            return circuitRoomNumbers.Distinct().ToList();

        }


        /*public static List<Element> GetElementsInCircuit (Document doc, string panelName, string circuitName)
        {
            FilteredElementCollector elements = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_ElectricalCircuit)
        }
        */


        /*     public static FamilySymbol GetSymbolByFamilyNameAndType(
          Document doc,
          string familyName,
          string typeName)
              {

                  try
                  {
                      ICollection<FamilySymbol> fs = new FilteredElementCollector(doc)
                          .OfClass(typeof(FamilySymbol))
                          .Where(q => q.Name == familyName)
                          .ToArray() as FamilySymbol;

                     foreach (Element e in fs)
                      {
      e.
                      }




                  }
                  catch
                  {
                      return null;
                  }


              }

          */


        public static List<string> GetPanelNames(
            Document doc)
        {
            ICollection<Element> panels = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_ElectricalEquipment)
                .ToArray();

            List<string> panelNames = new List<string>();

            foreach (Element panel in panels)
            {

                if (panel.Location != null)
                {
                    panelNames.Add(panel.Name);
                }

            }

            panelNames.Sort();
            return panelNames;

        }

        public static Dictionary<ElementId, String> GetPanelIdAndNames(Document doc)
        {
            ICollection<Element> panels = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_ElectricalEquipment)
                //.OfType()
                .ToArray();

            Dictionary<ElementId, string> dict = new Dictionary<ElementId, string>();

            foreach (Element panel in panels)
            {
                if (panel.Location != null)
                {
                    dict.Add(panel.Id, panel.Name);
                }
            }
            return dict.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public static Dictionary<ElementId, String> GetBindPanelIdAndNames(Document doc)
        {
            ICollection<Element> panels = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_ElectricalEquipment)
                .ToArray();

            Dictionary<ElementId, string> dict = new Dictionary<ElementId, string>();

            foreach (Element panel in panels)
            {
                if (panel.Location != null)
                {
                    if (Storage.IsValid(panel))
                    {
                        dict.Add(panel.Id, panel.Name);
                    }
                }
            }
            return dict.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
        }






        public static ViewFamilyType viewFamilyType(
            Document doc)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(ViewFamilyType));
            ViewFamilyType viewFamilyType = collector.Cast<ViewFamilyType>().First(vft => vft.ViewFamily == ViewFamily.Drafting);
            return viewFamilyType;
        }

        public static bool IsDraftingViewExsists(
            Document doc,
            string targetName)
        {
            Element draftingView = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewDrafting))
                .FirstOrDefault<Element>(
                    e => e.Name.Equals(targetName));
            if (draftingView == null)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public static ElementId GetDraftingViewIdByName(
          Document doc,
          string targetName)
        {
            Element draftingView = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewDrafting))
                .FirstOrDefault<Element>(
                    e => e.Name.Equals(targetName));

            return draftingView.Id;
        }

        public static ViewDrafting GetDraftingViewByName(
            Document doc,
            string targetName)
        {
            Element draftingView = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewDrafting))
                .FirstOrDefault<Element>(
                    e => e.Name.Equals(targetName));
            return draftingView as ViewDrafting;
        }

        public static Element DraftingViewByName(
            Document doc,
             string targetName)
        {
            Element draftingView = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewDrafting))
                .FirstOrDefault<Element>(
                    e => e.Name.Equals(targetName));

            return draftingView;
        }

        public static ElementId DraftingViewIdByName(
            Document doc,
             string targetName)
        {
            Element draftingView = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewDrafting))
                .FirstOrDefault<Element>(
                    e => e.Name.Equals(targetName));

            return draftingView.Id;
        }

        public static ICollection<ElementId> ElementsIdsInDraftingView(
            Document doc,
            string targetName)
        {

            ElementId vId = GetDraftingViewIdByName(doc, targetName);

            ElementOwnerViewFilter eOVF
                = new ElementOwnerViewFilter(vId);

            FilteredElementCollector vColl
              = new FilteredElementCollector(doc)
                .WherePasses(eOVF)
                .OfClass(typeof(FamilyInstance))
                ;

            return vColl.ToElementIds();
        }

        // public static ICollection<Cir>

        #region TitleBlocks
        public static Element getTitleBlock(
        Document doc,
            string familyName,
            string typeName)
        {
            FamilySymbol fs = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilySymbol))
                .OfCategory(BuiltInCategory.OST_TitleBlocks)
                .Cast<FamilySymbol>()
                .Where(x => x.FamilyName.Equals(familyName))
                .FirstOrDefault(x => x.Name == typeName);
            return fs;
        }

        public static Element getTitleBlock(
            Document doc,
            string name)
        {
            int index;
            try
            {
                index = name.IndexOf(": ");
            }
            catch
            {
                return null;
            }

            // index - 1
            string familyName = name.Substring(0, index);
            string typeName = name.Substring(index + 2);

            if (familyName.Length > 0 && typeName.Length > 0)
            {
                Element e = getTitleBlock(doc, familyName, typeName);
                return e;
            }
            else
            {
                return null;
            }
        }

        public static List<string> getTitleBlockNames(
            Document doc)
        {
            ICollection<FamilySymbol> fsCol = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilySymbol))
                .OfCategory(BuiltInCategory.OST_TitleBlocks)
                .Cast<FamilySymbol>()
                .ToArray();

            List<String> tbNames = new List<string>();

            foreach (FamilySymbol fs in fsCol)
            {
                tbNames.Add(fs.FamilyName + ": " + fs.Name);
            }
            tbNames.Sort();

            return tbNames;
        }

        public static string getViewTitleBlockName(View view)
        {

            Document doc = view.Document;

            Element e = new FilteredElementCollector(doc, view.Id)
            .OfCategory(BuiltInCategory.OST_TitleBlocks)
            .WhereElementIsNotElementType()
            .FirstOrDefault();

            if (e == null) return null;

            FamilyInstance fi = e as FamilyInstance;
            FamilySymbol fs = fi.Symbol;

            return (fs.FamilyName + ": " + fs.Name);
        }

        public static IList<Element> GetAllTitleBlocks(Document doc)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(FamilySymbol));
            collector.OfCategory(BuiltInCategory.OST_TitleBlocks);
            return collector.ToElements();
        }

        public static List<Element> GetAllElementsInView(Document doc,
            View view)
        {
            List<Element> elementsOnSheet = new List<Element>();

            foreach (Element e in new FilteredElementCollector(doc).OwnedByView(view.Id))
            {
                elementsOnSheet.Add(e);
            }
            return elementsOnSheet;

        }


        public static Element GetSheetTitleBlock(Document doc,
            View view)
        {
            List<Element> elementsOnSheet = new List<Element>();
            elementsOnSheet = GetAllElementsInView(doc, view);
            IList<Element> tbs = GetAllTitleBlocks(doc);

            foreach (Element e in elementsOnSheet)
            {
                foreach (FamilySymbol fs in tbs)
                {
                    if (e.GetTypeId().IntegerValue == fs.Id.IntegerValue)
                    {
                        return fs;
                    }
                }
            }
            return null;
        }


        #endregion

        #region Parameters

        public static String GetParameterValueStr(Element e, BuiltInParameter bip)
        {
            try
            {
                Parameter p = e.get_Parameter(bip);
                return p.AsString();
            }
            catch
            {
                return "";
            }
        }

        public static Double GetParameterValueDbl(Element e, BuiltInParameter bip)
        {
            try
            {
                Parameter p = e.get_Parameter(bip);
                return p.AsDouble();
            }
            catch
            {
                return 0;
            }
        }

        public static Double GetPowerInWatts(Element e, BuiltInParameter bip)
        {
            try
            {
                Parameter p = e.get_Parameter(bip);
                return UnitUtils.ConvertFromInternalUnits(p.AsDouble(), DisplayUnitType.DUT_WATTS);
            }
            catch
            {
                return 0;
            }
        }

        public static Double GetPowerInKiloWatts(Element e, BuiltInParameter bip)
        {
            try
            {
                Parameter p = e.get_Parameter(bip);
                return UnitUtils.ConvertFromInternalUnits(p.AsDouble(), DisplayUnitType.DUT_KILOWATTS);
            }
            catch
            {
                return 0;
            }
        }

        public static Double GetLengthInMeters(Element e, BuiltInParameter bip)
        {
            try
            {
                Parameter p = e.get_Parameter(bip);
                return UnitUtils.ConvertFromInternalUnits(p.AsDouble(), DisplayUnitType.DUT_METERS);
            }
            catch
            {
                return 0;
            }
        }

        public static Double GetVoltageInVolts(Element e, BuiltInParameter bip)
        {
            try
            {
                Parameter p = e.get_Parameter(bip);
                return UnitUtils.ConvertFromInternalUnits(p.AsDouble(), DisplayUnitType.DUT_VOLTS);
            }
            catch
            {
                return 0;
            }
        }


        #endregion





    }
}
