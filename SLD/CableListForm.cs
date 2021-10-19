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
    public partial class CableListForm : System.Windows.Forms.Form
    {
        List<string> tbnames;
        Dictionary<ElementId, string> boards;

        //outputs
        int error;

        public int Error
        {
            get
            {
                return error;
            }
        }

        public Dictionary<ElementId, string> Boards
        {
            get
            {
                Dictionary<ElementId, string> boards = new Dictionary<ElementId, string>();

                foreach (ListBoxItem lbi in boardsToCableList.Items)
                {
                    boards.Add(lbi.id, lbi.name);
                }
                return boards;
            }
        }

        public string TitleBlockName
        {
            get
            {
                return tbName.Text;
            }
        }

        // Local
        List<ListBoxItem> bs;


        public CableListForm(Dictionary<ElementId, string> boards, List<string> tbnames)
        {
            this.boards = boards;
            this.tbnames = tbnames;
            this.error = 0;

            InitializeComponent();

            bs = new List<ListBoxItem>();

            btnCreate.Enabled = false;

            tbName.DataSource = tbnames;
            searchText.Text = string.Empty;

            if (boards == null || tbnames == null)
            {
                error = 1;
            }
            else
            {
                foreach (KeyValuePair<ElementId, string> board in boards)
                {
                    ListBoxItem lbi = new ListBoxItem();
                    lbi.id = board.Key;
                    lbi.name = board.Value;
                    bs.Add(lbi);
                }

                boardsFromModel.Items.Clear();
                foreach (ListBoxItem b in bs)
                {
                    boardsFromModel.Items.Add(b);
                }

                if (tbnames.Contains(Properties.Settings.Default.set_lastTitleBlockForCableList))
                {
                    tbName.Text = Properties.Settings.Default.set_lastTitleBlockForCableList;
                }
                else
                {
                    tbName.Text = tbnames[0];
                }
            }
        }

        private void searchText_TextChanged(object sender, EventArgs e)
        {
            boardsFromModel.Items.Clear();
            string sText = searchText.Text;

            if (sText == string.Empty)
            {
                boardsFromModel.Items.Clear();
                foreach (ListBoxItem b in bs)
                {
                    boardsFromModel.Items.Add(b);
                }
            }
            else
            {
                foreach (ListBoxItem b in bs)
                {
                    if (b.name.IndexOf(sText, StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        boardsFromModel.Items.Add(b);
                    }
                }
            }


        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            List<ListBoxItem> lbis = new List<ListBoxItem>();

            foreach (object item in boardsFromModel.SelectedItems)
            {
                ListBoxItem lbi = new ListBoxItem();
                lbi = (ListBoxItem)item;
                boardsToCableList.Items.Add(lbi);
            }

            for (int i = boardsFromModel.SelectedIndices.Count - 1; i >= 0; i--)
            {
                ListBoxItem lbi = new ListBoxItem();
                lbi = (ListBoxItem)boardsFromModel.Items[boardsFromModel.SelectedIndices[i]];
                bs.Remove(bs.Find(x => x.id.Equals(lbi.id)));
                boardsFromModel.Items.RemoveAt(boardsFromModel.SelectedIndices[i]);
            }

            if (boardsToCableList.Items.Count > 0)
            {
                btnCreate.Enabled = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            List<ListBoxItem> lbis = new List<ListBoxItem>();

            foreach (object item in boardsToCableList.SelectedItems)
            {
                ListBoxItem lbi = new ListBoxItem();
                lbi = (ListBoxItem)item;
                bs.Add(lbi);
            }

            boardsFromModel.Items.Clear();
            foreach (ListBoxItem b in bs)
            {
                boardsFromModel.Items.Add(b);
            }

            for (int i = boardsToCableList.SelectedIndices.Count - 1; i >= 0; i--)
            {
                boardsToCableList.Items.RemoveAt(boardsToCableList.SelectedIndices[i]);
            }

            if (boardsToCableList.Items.Count == 0)
            {
                btnCreate.Enabled = false;
            }

            string strTemp = searchText.Text;
            searchText.Text = "";
            searchText.Text = strTemp;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {

        }
    }
}
