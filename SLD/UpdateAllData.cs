#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Electrical;
using forms = System.Windows.Forms;
using Autodesk.Revit.DB.Architecture;
#endregion

namespace SLD
{

    [Transaction(TransactionMode.Manual)]
    public class UpdateAllData : IExternalCommand
    {
        public static List<Room> roomsInRVTLink;

        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            Dictionary<ElementId, string> panels = Util.GetBindPanelIdAndNames(doc); //WARNING CHECK IF ELEMENT WAS NOT COPIED

            if (panels == null)
            {
                return Result.Succeeded;
            }

            if (panels.Count == 0)
            {
                return Result.Succeeded;
            }

            // Get all binded panels

            Dictionary<ElementId, string> pe = Util.GetBindPanelIdAndNames(doc);

            bool getError = false;
            List<string> boardsWithError = new List<string>();

            foreach (KeyValuePair<ElementId, string> pair in pe)
            {
                Element panel = doc.GetElement(pair.Key);
                PanelUpdate pu = new PanelUpdate(panel);
                if (pu.Status == -1)
                {
                    //Error during Update
                    getError = true;
                    try
                    {
                        boardsWithError.Add(panel.Name);
                    }
                    catch
                    {

                    }
                }
            }


            try
            {
                boardsWithError.Sort();
            }
            catch
            {
               
            }


            if (getError)
            {
                string errorMsg = "";

                if (boardsWithError != null)
                {
                    if (boardsWithError.Count > 0)
                    {
                        if (boardsWithError.Count > 1)
                        {
                            errorMsg = errorMsg + "При обновлении возникла ошибка. Не были обновлены щиты ";
                            string boardsWithErrorInStr = string.Join(", ", boardsWithError.ToArray());
                            errorMsg = errorMsg + boardsWithErrorInStr;
                        }
                        else
                        {
                            errorMsg = errorMsg + "При обновлении возникла ошибка. Не был обновлен щит " + boardsWithError[0];
                        }
                    }
                }
                TaskDialog.Show("Ошибка", errorMsg);
            }
            return Result.Succeeded;
        }
    }
}
