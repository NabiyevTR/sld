using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLD
{
    public class Helper
    {

       public static void GetElementParameterInformation(Document document, Element element)
        {
            // Format the prompt information string
            String prompt = "Show parameters in selected Element: \n\r";

            StringBuilder st = new StringBuilder();
            // iterate element's parameters
            foreach (Parameter para in element.Parameters)
            {
                st.AppendLine(GetParameterInformation(para, document));
            }

            // Give the user some information
            TaskDialog.Show("Revit", prompt + st.ToString());
        }

       public static String GetParameterInformation(Parameter para, Document document)
        {
            string defName = para.Definition.Name + "\t : ";
            string defValue = string.Empty;
            // Use different method to get parameter data according to the storage type
            switch (para.StorageType)
            {
                case StorageType.Double:
                    //covert the number into Metric
                    defValue = para.AsValueString();
                    break;
                case StorageType.ElementId:
                    //find out the name of the element
                    Autodesk.Revit.DB.ElementId id = para.AsElementId();
                    if (id.IntegerValue >= 0)
                    {
                        defValue = document.GetElement(id).Name;
                    }
                    else
                    {
                        defValue = id.IntegerValue.ToString();
                    }
                    break;
                case StorageType.Integer:
                    if (ParameterType.YesNo == para.Definition.ParameterType)
                    {
                        if (para.AsInteger() == 0)
                        {
                            defValue = "False";
                        }
                        else
                        {
                            defValue = "True";
                        }
                    }
                    else
                    {
                        defValue = para.AsInteger().ToString();
                    }
                    break;
                case StorageType.String:
                    defValue = para.AsString();
                    break;
                default:
                    defValue = "Unexposed parameter.";
                    break;
            }

            return defName + defValue;
        }
    }
}
