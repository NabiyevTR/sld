using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLD
{
    [Transaction(TransactionMode.Manual)]
    public class Cleaner
    {
        Document doc;
        Binder b;
        int error;
        View activeView;
        UIApplication uiapp;
        /*
         * 0 - no errors
         * 1 - cannot delete
         */

        public Cleaner(Element e)
        {
            this.doc = e.Document;
            Binder b = new Binder(e);
            this.b = b;
            this.error = 0;
            this.activeView = null;
            this.uiapp = null;

        }

        public Cleaner(UIApplication uiapp, Element e)
        {
            this.doc = e.Document;
            Binder b = new Binder(e);
            this.b = b;
            this.error = 0;
            this.activeView = uiapp.ActiveUIDocument.ActiveView;
            this.uiapp = uiapp;


        }

        public int Error()
        {
            return error;
        }


        public void DeleteViews()
        {
            // List<View> views = b.GetViews();
            List<ElementId> viewIds = b.GetViewIds();

            if (viewIds == null) return;

            if (viewIds.Count() == 0) return;

            if (activeView == null) return;

            if (viewIds.Contains(activeView.Id))
            {
                IList<UIView> openViews = uiapp.ActiveUIDocument.GetOpenUIViews();

                foreach (UIView openView in openViews)
                {
                    View openViewAsView = doc.GetElement(openView.ViewId) as View;

                    if (!viewIds.Contains(openViewAsView.Id))
                    {
                        uiapp.ActiveUIDocument.ActiveView = openViewAsView;
                        activeView = openViewAsView;
                        break;
                    }
                }
            }

            if (viewIds.Contains(activeView.Id))
            {
                IList<ElementId> allViewIds = new FilteredElementCollector(doc)
                    .OfClass(typeof(View))
                    .WhereElementIsNotElementType()
                    .ToElementIds()
                    .ToArray();


                foreach (ElementId vid in allViewIds)
                {
                    if (vid != activeView.Id)
                    {
                        try
                        {
                            uiapp.ActiveUIDocument.ActiveView = doc.GetElement(vid) as View;
                            activeView = doc.GetElement(vid) as View;
                            break;
                        }
                        catch
                        {

                        }
                    }
                }
            }


            foreach (ElementId viewId in viewIds)
            {
                try
                {
                    using (Transaction tx = new Transaction(doc))
                    {
                        tx.Start("Удаление листов и видов");
                        doc.Delete(viewId);
                        tx.Commit();
                    }
                }

                catch (Exception ex)
                {
                    TaskDialog.Show("Ошибка", ex.Message);
                    error = 1;
                }
            }
        }


        public static void deleteViewSheet(Document doc, string uid, out bool status)
        {
            status = false;

            ICollection<Element> views = new FilteredElementCollector(doc)
                         .OfClass(typeof(ViewSheet))
                         .WhereElementIsNotElementType()
                         .Where(e => e.UniqueId.Equals(uid)).ToArray();

            if (views.Count < 1)
            {
                return;
            }

            foreach (Element view in views)
            {
                try
                {
                    doc.Delete(view.Id);
                }
                catch (Exception ex)
                {
                    return;
                }
            }
            status = true;
        }

        public static void deleteSheetView(Document doc, string uid, out bool status)
        {
            status = false;

            ICollection<Element> sheetViews = new FilteredElementCollector(doc)
                         .OfClass(typeof(ViewSheet))
                         .WhereElementIsNotElementType()
                         .Where(e => e.UniqueId.Equals(uid)).ToArray();

            if (sheetViews.Count < 1)
            {
                return;
            }

            foreach (Element sheetView in sheetViews)
            {
                try
                {
                    doc.Delete(sheetView.Id);
                }
                catch (Exception ex)
                {
                    return;
                }


            }
            status = true;
        }

        void SetAnotherActiveView(Document doc, ElementId eid)
        {
            ICollection<Element> views = new FilteredElementCollector(doc)
                        .OfClass(typeof(View))
                        .WhereElementIsNotElementType()
                        .ToArray();

            foreach (Element view in views)
            {
                if (!view.Id.Equals(eid))
                {
                    //Transaction
                    using (Transaction tr = new Transaction(doc, "Setting active view"))
                    {
                        tr.Start();
                        View activeView = doc.ActiveView;

                        // doc. //Cannot change active view from here. Try to delete bind data from list and leave it alone
                        tr.Commit();
                    }
                }
            }
        }
    }
}
