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
using static SLD.Extensions;
#endregion

namespace SLD
{

    [Transaction(TransactionMode.Manual)]
    public class CableList : IExternalCommand
    {

        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            Dictionary<ElementId, string> boards;
            string tbName;

            List<string> tbNames = Util.getTitleBlockNames(doc);

            using (CableListForm clf = new CableListForm(Util.GetBindPanelIdAndNames(doc), tbNames))
            {
                clf.ShowDialog();

                if (clf.DialogResult == forms.DialogResult.Cancel) return Result.Cancelled;
                clf.Close();

                if (clf.Error == 1) return Result.Failed;

                boards = clf.Boards;
                tbName = clf.TitleBlockName;

                if (boards == null || tbName == null) return Result.Failed;

                if (boards.Count < 1) return Result.Failed;
            }


            //Get data from storage

            List<Panel> panelFromSorageList = new List<Panel>() { };
                        
            foreach (KeyValuePair<ElementId, string> board in boards)
            {
                Element e = doc.GetElement(board.Key);
                if (e == null) continue;

                Panel panelFromStorage = new Panel();
                Storage s = new Storage(e);
                panelFromStorage = s.Read();

                if (panelFromStorage == null) continue;

                panelFromSorageList.Add(panelFromStorage);
            }

            if (panelFromSorageList == null) return Result.Failed;

            if (panelFromSorageList.Count < 1) return Result.Failed;



                //Draw

                //insert tb

                //loop here

                //add tb if necessary 






                return Result.Succeeded;
        }
    }
}