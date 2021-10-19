using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SLD
{
    public partial class SelectBoard : System.Windows.Forms.Form
    {
        public string panelName { get; set; }
        public ElementId panelId { get; set; }        

        public Dictionary<ElementId, string> pNames { get; set; }

        public SelectBoard(Dictionary<ElementId, string> panelNames)
        {
            InitializeComponent();
            //this.list.DataSource = panelNames;
            pNames = panelNames;
            list.Items.Clear();
            searchText.Text = string.Empty;

            foreach (KeyValuePair<ElementId, string> pName in pNames)
            {
                ListBoxItem lbi = new ListBoxItem();
                lbi.id = pName.Key;
                lbi.name = pName.Value;
                list.Items.Add(lbi);
            }

            if (list.Items.Count < 1)
            {
                button1.Enabled = false;
                list.Enabled = false;
            }
            else
            {
                list.SetSelected(0, true);
                button1.Enabled = true;
                list.Enabled = true;
            }



        }


        private void button1_Click(object sender, EventArgs e)
        {
            ListBoxItem item = (ListBoxItem)list.SelectedItem;
            this.panelName = item.name;
            this.panelId = item.id;
        }

       

        private void searchText_TextChanged(object sender, EventArgs e)
        {
            list.Items.Clear();
            string sText = searchText.Text;

            if (sText == string.Empty)
            {

                foreach (KeyValuePair<ElementId, string> pName in pNames)
                {
                    ListBoxItem lbi = new ListBoxItem();
                    lbi.id = pName.Key;
                    lbi.name = pName.Value;
                    list.Items.Add(lbi);
                }
            }
            else
            {
                foreach (KeyValuePair<ElementId, string> pName in pNames)
                {
                    ListBoxItem lbi = new ListBoxItem();
                    lbi.id = pName.Key;
                    lbi.name = pName.Value;

                    if (pName.Value.IndexOf(sText, StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        list.Items.Add(lbi);
                    }
                }
            }

            if (list.Items.Count < 1)
            {
                button1.Enabled = false;
                list.Enabled = false;
            }
            else
            {
                list.SetSelected(0, true);
                button1.Enabled = true;
                list.Enabled = true;
            }
        }
    }
}
