using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using static SLD.Parameters;
using static SLD.Extensions;
using Autodesk.Revit.Attributes;

namespace SLD
{
    [Transaction(TransactionMode.Manual)]
    public class DrawDiagramOnSheet
    {
        public static void DrawPanel(
            UIDocument uidoc,
            Document doc,
            Panel panel)
        {
            ElementId activeViewId = uidoc.ActiveView.Id;
            View activeView = uidoc.ActiveView;

            // Count sheet quantity
            int linesCount = panel.circuits.Count() + panel.reserve;
            int circuitCount = panel.circuits.Count();
            int reserveCount = panel.reserve;

            Element tb = Util.getTitleBlock(doc, panel.titleBlockName);
            ElementId tbId = tb.Id;

            if (tb == null)
            {
                DrawDiagramInView.DrawPanel(uidoc, doc, panel);
                return;
            }

            // Create sheet name list
            List<string> sheetNames = new List<string>();

            ViewSheet sheet;

            List<ViewSheet> sheets = new List<ViewSheet>();

            using (Transaction tx = new Transaction(doc))
            {
                tx.Start("Создание однолинейной схемы");
                sheet = ViewSheet.Create(doc, tbId);
                activeView = sheet;
                sheets.Add(sheet);

                //Bind element
                Binder bindSheet = new Binder(sheet);
                bindSheet.Bind(panel.puid, BIND_TYPE_SHEET);

                tx.Commit();
            }


            // Getting all title blocks in model
            ICollection<Element> tbs = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_TitleBlocks)
                .OfClass(typeof(FamilyInstance))
                .ToElements();

            double xMax = 0;
            double xMin = 0;
            double yMax = 100;
            double yMin = 100;

            // Find title block on sheet

            bool tbInModel = false;

            foreach (FamilyInstance e in tbs)
            {
                string tbNumber = e.get_Parameter(BuiltInParameter.SHEET_NUMBER)?.AsString();
                Parameter par = e.get_Parameter(BuiltInParameter.SHEET_NUMBER);
                if (tbNumber == sheet.SheetNumber)
                {
                    double sheetWidth = CTDbl(e.get_Parameter(BuiltInParameter.SHEET_WIDTH).AsValueString());
                    double sheetHigth = CTDbl(e.get_Parameter(BuiltInParameter.SHEET_HEIGHT).AsValueString());
                    BoundingBoxXYZ tbbb = e.get_BoundingBox(sheet);
                    XYZ cMin = tbbb.Min.ToMeters();
                    XYZ cMax = tbbb.Max.ToMeters();

                    if (Math.Abs(xMax - xMin) < Math.Abs(cMax.X - cMin.X))
                    {
                        xMax = cMax.X;
                        xMin = cMin.X;
                    }

                    if (Math.Abs(yMax - yMin) < Math.Abs(cMax.Y - cMin.Y))
                    {
                        yMax = cMax.Y;
                        yMin = cMin.Y;
                    }

                    tbInModel = true;

                    break;
                }
            }

            /*if (!tbInModel)
            {

            }*/



            using (Transaction tx = new Transaction(doc))
            {
                tx.Start("Создание однолинейной схемы");

                double xCoord = xMin + DRAW_MARGIN_LEFT + DRAW_TABLE_MARGIN_LEFT;
                double yCoord = yMin + DRAW_MARGIN_BOTTOM;

                DrawUtil draw = new DrawUtil(doc, sheet, panel);

                // Вставляем таблицу
                draw.Table(xCoord, yCoord);
                xCoord = xCoord + DRAW_TABLE_WIDTH + DRAW_TABLE_MARGIN_RIGHT;

                // Вставляем вводной выключатель
                draw.Feeder(xCoord, yCoord);

                // Вставляем параметры щита                       
                draw.BoardParameters(-DRAW_MARGIN_TOP, DRAW_SHEET_HEIGTH - DRAW_MARGIN_TOP);

                // Вставляем стартовый элемент
                draw.StartElement(xCoord, yCoord);

                for (int iLineCount = 0; iLineCount < circuitCount; iLineCount++)
                {
                    draw.LineOut(iLineCount, xCoord, yCoord);
                    xCoord = xCoord + DRAW_LINEOUT_WIDTH;

                    if (xCoord + DRAW_LINEOUT_WIDTH > xMax - DRAW_MARGIN_RIGHT)
                    {
                        draw.FinishElement(xCoord, yCoord);



                        if (reserveCount == 0 && iLineCount == circuitCount - 1)
                        {

                        }
                        else
                        {
                            xCoord = xMin + DRAW_MARGIN_LEFT + DRAW_TABLE_MARGIN_LEFT;
                            sheet = ViewSheet.Create(doc, tbId);
                            sheets.Add(sheet);
                            draw.view = sheet;

                            //Bind element
                            Binder bindSheet = new Binder(sheet);
                            bindSheet.Bind(panel.uid, BIND_TYPE_SHEET);

                            // Вставляем рамку
                            draw.Table(xCoord, yCoord);

                            // Вставляем стартовый элемент
                            xCoord = xCoord + DRAW_TABLE_WIDTH + DRAW_TABLE_MARGIN_RIGHT;
                            draw.StartElement(xCoord, yCoord);
                        }



                    }

                }


                for (int iReserveCount = 0; iReserveCount < reserveCount; iReserveCount++)
                {
                    draw.Reserve(iReserveCount, xCoord, yCoord);
                    xCoord = xCoord + DRAW_LINEOUT_WIDTH;

                    if (xCoord + DRAW_LINEOUT_WIDTH > xMax - DRAW_MARGIN_RIGHT)
                    {
                        draw.FinishElement(xCoord, yCoord);


                        if (iReserveCount != reserveCount - 1)
                        {

                            xCoord = xMin + DRAW_MARGIN_LEFT + DRAW_TABLE_MARGIN_LEFT;
                            sheet = ViewSheet.Create(doc, tbId);
                            sheets.Add(sheet);
                            draw.view = sheet;

                            //Bind element
                            Binder bindSheet = new Binder(sheet);
                            bindSheet.Bind(panel.puid, BIND_TYPE_SHEET);

                            // Вставляем рамку
                            draw.Table(xCoord, yCoord);

                            // Вставляем стартовый элемент
                            xCoord = xCoord + DRAW_TABLE_WIDTH + DRAW_TABLE_MARGIN_RIGHT;
                            draw.StartElement(xCoord, yCoord);
                        }
                    }

                }
                draw.FinishElement(xCoord, yCoord);

                int sheetCount = sheets.Count();

                if (sheetCount == 1)
                {
                    sheets[0].Name = Parameters.DRAW_SHEET_NAME + " " + panel.name;
                }

                if (sheetCount > 1)
                {
                    sheets[0].Name = DRAW_SHEET_NAME + " " + panel.name + " (Начало)";
                    sheets[sheetCount - 1].Name = DRAW_SHEET_NAME + " " + panel.name + " (Окончание)";

                    for (int i = 1; i < sheetCount - 1; i++)
                    {
                        sheets[i].Name = DRAW_SHEET_NAME + " " + panel.name + " (Продолжение)";
                    }
                }

                tx.Commit();
            }


            try
            {
                uidoc.ActiveView = activeView;
            }
            catch
            {

            }





        }
    }
}
