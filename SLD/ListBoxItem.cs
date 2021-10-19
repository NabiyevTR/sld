using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLD
{
    public class ListBoxItem
    {
        public virtual string name { get; set; }
        public virtual ElementId id { get; set; }

        public ListBoxItem()
        {
            this.name = string.Empty;
            this.id =null;
        }

        public ListBoxItem(ElementId id, string name)
        {
            this.name = name;
            this.id = id;
        }

        public override string ToString()
        {
            return this.name;
        }


    }
}
