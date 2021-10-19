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
using System.Linq;



#endregion

namespace SLD
{

    [Transaction(TransactionMode.Manual)]
    public class EditPanel : IExternalCommand
    {
        string panelName;
        ElementId panelId;
        bool getRoomNumberFromLink;


        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;
            App.docG = doc;


            //Select panel to edit
            using (SelectBoard BoardSelectionForm = new SelectBoard(Util.GetBindPanelIdAndNames(doc)))
            {
                BoardSelectionForm.ShowDialog();

                if (BoardSelectionForm.DialogResult == forms.DialogResult.Cancel)
                {
                    return Result.Cancelled;
                }
                panelName = BoardSelectionForm.panelName;
                panelId = BoardSelectionForm.panelId;                
                BoardSelectionForm.Close();
            }

            getRoomNumberFromLink = Properties.Settings.Default.set_roomsFromLink;

            Element e = doc.GetElement(panelId);

            if (e == null)
            {
                return Result.Failed;

                //Show message REvit
            }

            string puid = e.UniqueId;

            Storage s = new Storage(e);
            Panel p = s.Read();
// WARNING CHECK IF P NOT NULL
            bool onSheet = false;

            if (p.titleBlockName == null)
            {
                onSheet = false;
            }
            else
            {
                onSheet = true;
            }

            //Get titleblocks from model
            List<string> tbNames = new List<string>();

            if (onSheet)
            {
                tbNames = Util.getTitleBlockNames(doc);
            }           
                 
            
            using (BoardTable BoardTableForm = new BoardTable(p, onSheet, tbNames))
            {
                BoardTableForm.ShowDialog();

                if (BoardTableForm.DialogResult == forms.DialogResult.Cancel) return Result.Cancelled;
                BoardTableForm.Close();
            }

            string draftViewName = "Схема электрическая однолинейная " + p.name;
            
            //Get all views binded to the panel
            Binder b = new Binder(e);
            List<View> bviews = b.GetViews();

            
            string tbName = null;

            if (bviews != null)
            {


                foreach (View view in bviews)
                {
                    tbName = Util.getViewTitleBlockName(view);

                    if (tbName != null)
                    {
                        p.titleBlockName = tbName;
                        break;
                    }
                }

                

            }

            Cleaner c = new Cleaner(uiapp, e);
            c.DeleteViews();


            if (tbName == null)
            {
                DrawUtil.CreateDraftView(doc, draftViewName, p.uid);

                try
                {
                    uidoc.ActiveView = Util.GetDraftingViewByName(doc, draftViewName);
                }
                catch
                {

                }
         
                DrawDiagramInView.DrawPanel(uidoc, doc, p);

            }
            else
            {
                DrawDiagramOnSheet.DrawPanel(uidoc, doc, p);
            }

            return Result.Succeeded;
        }
    }
}
