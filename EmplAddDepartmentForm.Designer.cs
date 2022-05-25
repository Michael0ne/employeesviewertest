namespace EmployeesViewer
{
    partial class EmplAddDepartmentForm
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
            this.ButtonAddDepartment = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.EditTitle = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonAddDepartment
            // 
            this.ButtonAddDepartment.Location = new System.Drawing.Point(12, 84);
            this.ButtonAddDepartment.Name = "ButtonAddDepartment";
            this.ButtonAddDepartment.Size = new System.Drawing.Size(110, 25);
            this.ButtonAddDepartment.TabIndex = 0;
            this.ButtonAddDepartment.Text = "Добавить";
            this.ButtonAddDepartment.UseVisualStyleBackColor = true;
            this.ButtonAddDepartment.Click += new System.EventHandler(this.DepartmentAddClicked);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.EditTitle);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(280, 72);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Информация об отделе";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Название";
            // 
            // EditTitle
            // 
            this.EditTitle.Location = new System.Drawing.Point(9, 32);
            this.EditTitle.Name = "EditTitle";
            this.EditTitle.Size = new System.Drawing.Size(265, 20);
            this.EditTitle.TabIndex = 1;
            // 
            // EmplAddDepartmentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 116);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ButtonAddDepartment);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EmplAddDepartmentForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Добавить отдел";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.FormShown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ButtonAddDepartment;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox EditTitle;
    }
}