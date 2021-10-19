namespace SLD
{
    partial class CableListForm
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
            this.boardsToCableList = new System.Windows.Forms.ListBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.searchText = new System.Windows.Forms.TextBox();
            this.tbLabel = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.ComboBox();
            this.boardsFromModel = new System.Windows.Forms.ListBox();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // boardsToCableList
            // 
            this.boardsToCableList.FormattingEnabled = true;
            this.boardsToCableList.Location = new System.Drawing.Point(298, 47);
            this.boardsToCableList.Name = "boardsToCableList";
            this.boardsToCableList.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.boardsToCableList.Size = new System.Drawing.Size(178, 342);
            this.boardsToCableList.TabIndex = 1;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(207, 174);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = ">";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(207, 212);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "<";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(298, 456);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(84, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnCreate
            // 
            this.btnCreate.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnCreate.Location = new System.Drawing.Point(388, 456);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(88, 23);
            this.btnCreate.TabIndex = 5;
            this.btnCreate.Text = "Создать";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Поиск:";
            // 
            // searchText
            // 
            this.searchText.Location = new System.Drawing.Point(61, 12);
            this.searchText.Name = "searchText";
            this.searchText.Size = new System.Drawing.Size(129, 20);
            this.searchText.TabIndex = 7;
            this.searchText.TextChanged += new System.EventHandler(this.searchText_TextChanged);
            // 
            // tbLabel
            // 
            this.tbLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbLabel.AutoSize = true;
            this.tbLabel.Location = new System.Drawing.Point(13, 403);
            this.tbLabel.Name = "tbLabel";
            this.tbLabel.Size = new System.Drawing.Size(105, 13);
            this.tbLabel.TabIndex = 56;
            this.tbLabel.Text = "Основная надпись:";
            // 
            // tbName
            // 
            this.tbName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tbName.FormattingEnabled = true;
            this.tbName.Location = new System.Drawing.Point(12, 423);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(464, 21);
            this.tbName.TabIndex = 55;
            // 
            // boardsFromModel
            // 
            this.boardsFromModel.FormattingEnabled = true;
            this.boardsFromModel.Location = new System.Drawing.Point(12, 47);
            this.boardsFromModel.Name = "boardsFromModel";
            this.boardsFromModel.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.boardsFromModel.Size = new System.Drawing.Size(178, 342);
            this.boardsFromModel.Sorted = true;
            this.boardsFromModel.TabIndex = 57;
            // 
            // btnUp
            // 
            this.btnUp.Location = new System.Drawing.Point(207, 312);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(75, 23);
            this.btnUp.TabIndex = 58;
            this.btnUp.Text = "Вверх";
            this.btnUp.UseVisualStyleBackColor = true;
            // 
            // btnDown
            // 
            this.btnDown.Location = new System.Drawing.Point(207, 350);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(75, 23);
            this.btnDown.TabIndex = 59;
            this.btnDown.Text = "Вниз";
            this.btnDown.UseVisualStyleBackColor = true;
            // 
            // CableListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 492);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.boardsFromModel);
            this.Controls.Add(this.tbLabel);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.searchText);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.boardsToCableList);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CableListForm";
            this.ShowIcon = false;
            this.Text = "Кабельный журнал";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

       
        private System.Windows.Forms.ListBox boardsToCableList;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox searchText;
        private System.Windows.Forms.Label tbLabel;
        private System.Windows.Forms.ComboBox tbName;
        private System.Windows.Forms.ListBox boardsFromModel;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
    }
}