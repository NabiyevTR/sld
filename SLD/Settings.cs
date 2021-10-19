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
    public class Settings : IExternalCommand
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

            using (SettingsForm settings = new SettingsForm())
            {
                settings.ShowDialog();
                if (settings.DialogResult == forms.DialogResult.Cancel) return Result.Cancelled;
                settings.Close();
            }

            return Result.Succeeded;
        }
    }
}
