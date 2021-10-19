using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SLD
{
    public static class Extensions
    {
        public static XYZ ToFeets(this XYZ coords)
        {
            coords = coords / 304.8;
            return coords;
        }

        public static XYZ ToMeters(this XYZ coords)
        {
            coords = coords * 304.8;
            return coords;
        }

        public static string ToRoomStr(this List<String> list)
        {
            string div = ", ";
            string bigstr = "";

            foreach (string str in list)
            {
                if (str != "null")
                {
                    bigstr = bigstr + str + div;
                }
            }

            try
            {
                bigstr = bigstr.Substring(0, bigstr.Length - 2);
            }
            catch
            {
                return "";
            }

            return bigstr;
        }













        public static double ToFeets(this double value)
        {
            value = value / 304.8;
            return value;
        }

        public static double ToDoubleWithCulture(this string str)
        {
            double val = 0;

            if (
                Double.TryParse(str.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out val))
            {
                return val;
            }

            return 0;


        }

        public static double ToDbl(this string str)
        {

            if (str == null) { return 0; }

            double val = 0;

            if (
                Double.TryParse(str.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out val)
                )
            { return val; }

            return 0;
        }

        public static decimal ToDec(this string str)
        {

            if (str == null) { return 0; }

            decimal val = 0;

            if (
                Decimal.TryParse(str.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out val)
                )
            { return val; }

            return 0;
        }


        public static double CTDbl(string str)
        {

            if (str == null) { return 0; }

            double val = 0;

            if (
                Double.TryParse(str.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out val)
                )
            { return val; }

            return 0;
        }




        public static string OnlyNumbersAndCommas(this string str)
        {
            if (str == null)
            {
                return null;
            }
            Regex rgx = new Regex("[^0-9 - ,]");
            str = rgx.Replace(str, "");
            return str;



            /*int number;

            if (int.TryParse(str, out number))
            {
                return number;
            }
            else
            {
                return 0;
            } */
        }

        public static void TrySet(this Parameter p, double val)
        {
            if (!p.IsReadOnly)
            {
                try
                {
                    p.Set(val);
                }
                catch
                {
                }
            }
        }


        public static void TrySet(this Parameter p, double newVal, double oldVal, double error)
        {
            if (!p.IsReadOnly && Math.Abs(newVal - oldVal) > error)
            {
                try
                {
                    p.Set(newVal);
                }
                catch
                {
                }
            }
        }

        public static void TrySetWithCheck(this Parameter p, double newVal, double error)
        {
            double oldVal;

            try
            {
                oldVal = p.AsDouble();
            }
            catch
            {
                oldVal = newVal;
            }


            if (!p.IsReadOnly && Math.Abs(newVal - oldVal) > error)
            {
                // try
                //  {
                p.Set(newVal);
                // }
                //  catch
                //  {
                //  }
            }
        }














        public static void TrySet(this Parameter p, string val)
        {
            if (!p.IsReadOnly)
            {
                try
                {
                    p.Set(val);
                }
                catch
                {

                }
            }
        }

        public static void TrySet(this Parameter p, string newVal, string oldVal)
        {
            if (!p.IsReadOnly && newVal != oldVal)
            {
                try
                {
                    p.Set(newVal);
                }
                catch
                {

                }
            }
        }

        public static void TrySetWithCheck(this Parameter p, string newVal)
        {
            string oldVal;

            try
            {
                oldVal = p.AsString();
            }
            catch
            {
                oldVal = newVal;
            }


            if (!p.IsReadOnly && newVal != oldVal)
            {
                try
                {
                    p.Set(newVal);
                }
                catch
                {

                }
            }
        }


    }
}
