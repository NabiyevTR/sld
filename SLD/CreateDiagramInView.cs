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
    public class CreateDiagramInView : IExternalCommand
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
            Document doc;

            try
            {
                doc = uidoc.Document;
                App.docG = doc;
            }
            catch
            {
                TaskDialog.Show("Ошибка", "Откройте рабочий файл!");
                return Result.Failed;
            }

            Selection selection = uidoc.Selection;
            ICollection<ElementId> selectedIds = uidoc.Selection.GetElementIds();

            // Board selection

            string panelName;
            ElementId panelId = null;
            bool getRoomNumberFromLink = false;

            if (Util.GetBoardFromSelection(doc, selectedIds) != null)
            {
                // From selection
                Element p = Util.GetBoardFromSelection(doc, selectedIds);
                panelName = p.Name;
                panelId = p.Id;

                            
            }
            else
            {
                // From form
                using (SelectBoard BoardSelectionForm = new SelectBoard(Util.GetPanelIdAndNames(doc)))
                {
                    BoardSelectionForm.ShowDialog();

                    if (BoardSelectionForm.DialogResult == forms.DialogResult.Cancel) return Result.Cancelled;

                    panelName = BoardSelectionForm.panelName;
                    panelId = BoardSelectionForm.panelId;                    
                    BoardSelectionForm.Close();
                }
            }

            getRoomNumberFromLink = Properties.Settings.Default.set_roomsFromLink;

            //getting rooms from RVT link

           /* if (getRoomNumberFromLink)
            {
                roomsInRVTLink = Util.GetRoomsFromRVTLink(app, doc);
            }
            */

            //getting data from model

            Element docPanel = doc.GetElement(panelId);

            Panel panel = new Panel(docPanel, getRoomNumberFromLink);

            using (BoardTable BoardTableForm = new BoardTable(panel, false))
            {
                BoardTableForm.ShowDialog();

                if (BoardTableForm.DialogResult == forms.DialogResult.Cancel) return Result.Cancelled;

                panel.titleBlockName = null;

                BoardTableForm.Close();
            }

            string draftViewName = "Схема щита " + panel.name;

            //Clean draft views            
            Cleaner c = new Cleaner(uiapp, docPanel);
            c.DeleteViews();

            // Creating new drafting view or clear old
            DrawUtil.CreateDraftView(doc, draftViewName, panel.puid);

            try
            {
                uidoc.ActiveView = Util.GetDraftingViewByName(doc, draftViewName);
            }
            catch
            {

            }

            //Drafting

            DrawDiagramInView.DrawPanel(uidoc, doc, panel);

            return Result.Succeeded;
        }
    }
}
