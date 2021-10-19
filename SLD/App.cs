#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Media.Imaging;

#endregion

namespace SLD
{
    class App : IExternalApplication
    {
        public static Document docG;

        static void AddRibbonPanel(UIControlledApplication application)
        {
            try
            {
                String tabName = "SLD 2.02";
                application.CreateRibbonTab(tabName);
                string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

                RibbonPanel ribbonPanelDiagrams = application.CreateRibbonPanel(tabName, "Однолинейные схемы");

                //Diagrams
                PushButtonData b1_1Data = new PushButtonData(
                    "Новая схема",
                    "Новая схема",
                    thisAssemblyPath,
                    "SLD.CreateDiagramInView");
                PushButton pb1_1 = ribbonPanelDiagrams.AddItem(b1_1Data) as PushButton;
                pb1_1.ToolTip = "Создать однолинейную схему на чертежном виде";
                BitmapImage pb1_1Image = new BitmapImage(new Uri("pack://application:,,,/SLD;component/icons/draft.ico"));
                pb1_1.LargeImage = pb1_1Image;

                PushButtonData b1_2Data = new PushButtonData(
                    "Новая схема\nна листе",
                    "Новая схема\nна листе",
                    thisAssemblyPath,
                    "SLD.CreateDiagramOnSheet");
                PushButton pb1_2 = ribbonPanelDiagrams.AddItem(b1_2Data) as PushButton;
                pb1_2.ToolTip = "Создать однолинейную схему на листе с основной надписью";
                BitmapImage pb1_2Image = new BitmapImage(new Uri("pack://application:,,,/SLD;component/icons/sheet.ico"));
                pb1_2.LargeImage = pb1_2Image;

                PushButtonData b1_3Data = new PushButtonData(
                    "Редактировать\nсхему",
                    "Редактировать\nсхему",
                    thisAssemblyPath,
                    "SLD.EditPanel");
                PushButton pb1_3 = ribbonPanelDiagrams.AddItem(b1_3Data) as PushButton;
                pb1_3.ToolTip = "Редактировать схему";
                BitmapImage pb1_3Image = new BitmapImage(new Uri("pack://application:,,,/SLD;component/icons/edit.ico"));
                pb1_3.LargeImage = pb1_3Image;

                PushButtonData b1_4Data = new PushButtonData(
                    "Обновить\nвсе схемы",
                    "Обновить\nвсе схемы",
                    thisAssemblyPath,
                    "SLD.UpdateAllData");
                PushButton pb1_4 = ribbonPanelDiagrams.AddItem(b1_4Data) as PushButton;
                pb1_4.ToolTip = "Обновить все схемы";
                BitmapImage pb1_4Image = new BitmapImage(new Uri("pack://application:,,,/SLD;component/icons/refresh.ico"));
                pb1_4.LargeImage = pb1_4Image;

                //Cable List
                RibbonPanel ribbonPanelCableList = application.CreateRibbonPanel(tabName, "Кабельный журнал");
                PushButtonData b3_1Data = new PushButtonData(
              "Кабельный\nжурнал",
              "Кабельный\nжурнал",
              thisAssemblyPath,
              "SLD.CableList"); 
                PushButton pb3_1 = ribbonPanelCableList.AddItem(b3_1Data) as PushButton;
                pb3_1.ToolTip = "Кабельный журнал";
                BitmapImage pb3_1Image = new BitmapImage(new Uri("pack://application:,,,/SLD;component/icons/settings.ico")); 
                pb3_1.LargeImage = pb3_1Image;


                // Settings
                RibbonPanel ribbonPanelSettings = application.CreateRibbonPanel(tabName, "Настройки");
                PushButtonData b2_1Data = new PushButtonData(
              "Настройки",
              "Настройки",
              thisAssemblyPath,
              "SLD.Settings");
                PushButton pb2_1 = ribbonPanelSettings.AddItem(b2_1Data) as PushButton;
                pb2_1.ToolTip = "Дополнительные настройки для работы с программой";
                BitmapImage pb2_1Image = new BitmapImage(new Uri("pack://application:,,,/SLD;component/icons/settings.ico"));
                pb2_1.LargeImage = pb2_1Image;

            }
            catch
            {

            }

        }
        public Result OnStartup(UIControlledApplication a)
        {
            AddRibbonPanel(a);
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }
    }
}
