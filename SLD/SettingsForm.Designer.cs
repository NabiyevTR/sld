namespace SLD
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.updateRooms = new System.Windows.Forms.CheckBox();
            this.updateRatedLength = new System.Windows.Forms.CheckBox();
            this.updateLoad = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.roomsFromLinkedFile = new System.Windows.Forms.CheckBox();
            this.updateDescription = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft PhagsPa", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(341, 336);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.White;
            this.tabPage2.Controls.Add(this.updateDescription);
            this.tabPage2.Controls.Add(this.updateRooms);
            this.tabPage2.Controls.Add(this.updateRatedLength);
            this.tabPage2.Controls.Add(this.updateLoad);
            this.tabPage2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(333, 309);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Обновление";
            // 
            // updateRooms
            // 
            this.updateRooms.AutoSize = true;
            this.updateRooms.Location = new System.Drawing.Point(12, 64);
            this.updateRooms.Name = "updateRooms";
            this.updateRooms.Size = new System.Drawing.Size(185, 18);
            this.updateRooms.TabIndex = 2;
            this.updateRooms.Text = "Обновлять номера помещений";
            this.updateRooms.UseVisualStyleBackColor = true;
            // 
            // updateRatedLength
            // 
            this.updateRatedLength.AutoSize = true;
            this.updateRatedLength.Location = new System.Drawing.Point(12, 40);
            this.updateRatedLength.Name = "updateRatedLength";
            this.updateRatedLength.Size = new System.Drawing.Size(154, 18);
            this.updateRatedLength.TabIndex = 1;
            this.updateRatedLength.Text = "Обновлять длину кабеля";
            this.updateRatedLength.UseVisualStyleBackColor = true;
            // 
            // updateLoad
            // 
            this.updateLoad.AutoSize = true;
            this.updateLoad.Location = new System.Drawing.Point(12, 17);
            this.updateLoad.Name = "updateLoad";
            this.updateLoad.Size = new System.Drawing.Size(138, 18);
            this.updateLoad.TabIndex = 0;
            this.updateLoad.TabStop = false;
            this.updateLoad.Text = "Обновлять мощности";
            this.updateLoad.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(274, 359);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Сохранить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(191, 359);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Отмена";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.roomsFromLinkedFile);
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(333, 309);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Общие";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // roomsFromLinkedFile
            // 
            this.roomsFromLinkedFile.AutoSize = true;
            this.roomsFromLinkedFile.Location = new System.Drawing.Point(10, 14);
            this.roomsFromLinkedFile.Name = "roomsFromLinkedFile";
            this.roomsFromLinkedFile.Size = new System.Drawing.Size(195, 18);
            this.roomsFromLinkedFile.TabIndex = 0;
            this.roomsFromLinkedFile.Text = "Помещения из связанного файла";
            this.roomsFromLinkedFile.UseVisualStyleBackColor = true;
            // 
            // updateDescription
            // 
            this.updateDescription.AutoSize = true;
            this.updateDescription.Location = new System.Drawing.Point(12, 88);
            this.updateDescription.Name = "updateDescription";
            this.updateDescription.Size = new System.Drawing.Size(161, 18);
            this.updateDescription.TabIndex = 3;
            this.updateDescription.Text = "Обновлять наименование";
            this.updateDescription.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 393);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tabControl1);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.ShowIcon = false;
            this.Text = "Настройки";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox updateRooms;
        private System.Windows.Forms.CheckBox updateRatedLength;
        private System.Windows.Forms.CheckBox updateLoad;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.CheckBox roomsFromLinkedFile;
        private System.Windows.Forms.CheckBox updateDescription;
    }
}