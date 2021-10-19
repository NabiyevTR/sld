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
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();

            this.updateRooms.Checked = global::SLD.Properties.Settings.Default.update_Rooms;
            if (global::SLD.Properties.Settings.Default.update_Rooms)
            {
                this.updateRooms.CheckState = System.Windows.Forms.CheckState.Checked;
            }
            else
            {
                this.updateRooms.CheckState = System.Windows.Forms.CheckState.Unchecked;
            }

            this.updateRatedLength.Checked = global::SLD.Properties.Settings.Default.update_RatedLength;
            if (global::SLD.Properties.Settings.Default.update_RatedLength)
            {
                this.updateRatedLength.CheckState = System.Windows.Forms.CheckState.Checked;
            }
            else
            {
                this.updateRatedLength.CheckState = System.Windows.Forms.CheckState.Unchecked;
            }

            this.updateLoad.Checked = global::SLD.Properties.Settings.Default.update_Load;
            if (global::SLD.Properties.Settings.Default.update_Load)
            {
                this.updateLoad.CheckState = System.Windows.Forms.CheckState.Checked;
            }
            else
            {
                this.updateLoad.CheckState = System.Windows.Forms.CheckState.Unchecked;
            }


            this.updateDescription.Checked = global::SLD.Properties.Settings.Default.update_Description;
            if (global::SLD.Properties.Settings.Default.update_Description
                )
            {
                this.updateDescription.CheckState = System.Windows.Forms.CheckState.Checked;
            }
            else
            {
                this.updateDescription.CheckState = System.Windows.Forms.CheckState.Unchecked;
            }

            this.roomsFromLinkedFile.Checked = global::SLD.Properties.Settings.Default.set_roomsFromLink;
            if (global::SLD.Properties.Settings.Default.set_roomsFromLink)
            {
                this.roomsFromLinkedFile.CheckState = System.Windows.Forms.CheckState.Checked;
            }
            else
            {
                this.roomsFromLinkedFile.CheckState = System.Windows.Forms.CheckState.Unchecked;
            }











        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Main
            Properties.Settings.Default.set_roomsFromLink = roomsFromLinkedFile.Checked;

            //Update
            Properties.Settings.Default.update_Load = updateLoad.Checked;
            Properties.Settings.Default.update_RatedLength = updateRatedLength.Checked;
            Properties.Settings.Default.update_MaxLength = updateRatedLength.Checked;
            Properties.Settings.Default.update_TotalLength = updateRatedLength.Checked;
            Properties.Settings.Default.update_Rooms = updateRooms.Checked;
            Properties.Settings.Default.update_Description = updateDescription.Checked;

            Properties.Settings.Default.Save();

        }
    }
}
