using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using static SLD.Parameters;

namespace SLD
{
    public class DrawDiagramInView
    {

        public static void DrawPanel(
            UIDocument uidoc,
            Document doc,
            Panel panel)
        {
            using (Transaction tx = new Transaction(doc))
            {
                tx.Start("Создание однолинейной схемы");
                double xCoord = 0;

                DrawUtil draw = new DrawUtil(doc, uidoc.ActiveView, panel);
                
                // Вставляем рамку
                draw.Table(xCoord, 0);
                
                xCoord = xCoord + DRAW_TABLE_WIDTH + DRAW_TABLE_MARGIN_RIGHT;

                // Вставляем вводной выключатель
                draw.Feeder(xCoord, 0);

                // Вставляем стартовый элемент
                draw.StartElement(xCoord, 0);

                // Вставляем отходящие линии
                for (int i = 0; i < panel.circuits.Count(); i++)
                {
                    draw.LineOut(i, xCoord, 0);
                    xCoord = xCoord + DRAW_LINEOUT_WIDTH;
                }

                // Вставляем резервные линии
                for (int i = 0; i < panel.reserve; i++)
                {
                    draw.Reserve(i, xCoord, 0);
                    xCoord = xCoord + DRAW_LINEOUT_WIDTH;
                }

                // Вставляем завершающий элемент   
                draw.FinishElement(xCoord, 0);

                // Вставляем параметры щита
                draw.BoardParameters(150, 150);               


                tx.Commit();
            }
        }

    }
}
