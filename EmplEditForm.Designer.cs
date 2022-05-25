namespace EmployeesViewer
{
    partial class EmplEditForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CheckIsSupervisor = new System.Windows.Forms.CheckBox();
            this.EditEmployeePostAddress = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SelectEmployeeDepartment = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.EditEmployeePhoneNumber = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SelectEmployeePosition = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.DateEmployee = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.EditEmployeeName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ButtonSaveInfo = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CheckIsSupervisor);
            this.groupBox1.Controls.Add(this.EditEmployeePostAddress);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.SelectEmployeeDepartment);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.EditEmployeePhoneNumber);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.SelectEmployeePosition);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.DateEmployee);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.EditEmployeeName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(280, 283);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Информация о сотруднике";
            // 
            // CheckIsSupervisor
            // 
            this.CheckIsSupervisor.AutoSize = true;
            this.CheckIsSupervisor.Location = new System.Drawing.Point(9, 216);
            this.CheckIsSupervisor.Name = "CheckIsSupervisor";
            this.CheckIsSupervisor.Size = new System.Drawing.Size(119, 17);
            this.CheckIsSupervisor.TabIndex = 13;
            this.CheckIsSupervisor.Text = "Начальник отдела";
            this.CheckIsSupervisor.UseVisualStyleBackColor = true;
            // 
            // EditEmployeePostAddress
            // 
            this.EditEmployeePostAddress.Location = new System.Drawing.Point(9, 253);
            this.EditEmployeePostAddress.MaxLength = 1024;
            this.EditEmployeePostAddress.Name = "EditEmployeePostAddress";
            this.EditEmployeePostAddress.Size = new System.Drawing.Size(265, 20);
            this.EditEmployeePostAddress.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 237);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Почтовый адрес";
            // 
            // SelectEmployeeDepartment
            // 
            this.SelectEmployeeDepartment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SelectEmployeeDepartment.FormattingEnabled = true;
            this.SelectEmployeeDepartment.Location = new System.Drawing.Point(9, 189);
            this.SelectEmployeeDepartment.Name = "SelectEmployeeDepartment";
            this.SelectEmployeeDepartment.Size = new System.Drawing.Size(265, 21);
            this.SelectEmployeeDepartment.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 173);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Отдел";
            // 
            // EditEmployeePhoneNumber
            // 
            this.EditEmployeePhoneNumber.Location = new System.Drawing.Point(9, 150);
            this.EditEmployeePhoneNumber.MaxLength = 16;
            this.EditEmployeePhoneNumber.Name = "EditEmployeePhoneNumber";
            this.EditEmployeePhoneNumber.Size = new System.Drawing.Size(265, 20);
            this.EditEmployeePhoneNumber.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 134);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Номер телефона";
            // 
            // SelectEmployeePosition
            // 
            this.SelectEmployeePosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SelectEmployeePosition.FormattingEnabled = true;
            this.SelectEmployeePosition.Location = new System.Drawing.Point(9, 110);
            this.SelectEmployeePosition.Name = "SelectEmployeePosition";
            this.SelectEmployeePosition.Size = new System.Drawing.Size(265, 21);
            this.SelectEmployeePosition.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Должность";
            // 
            // DateEmployee
            // 
            this.DateEmployee.CustomFormat = "";
            this.DateEmployee.Location = new System.Drawing.Point(9, 71);
            this.DateEmployee.Name = "DateEmployee";
            this.DateEmployee.Size = new System.Drawing.Size(265, 20);
            this.DateEmployee.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Дата рождения";
            // 
            // EditEmployeeName
            // 
            this.EditEmployeeName.Location = new System.Drawing.Point(9, 32);
            this.EditEmployeeName.Name = "EditEmployeeName";
            this.EditEmployeeName.Size = new System.Drawing.Size(265, 20);
            this.EditEmployeeName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Фамилия имя отчество";
            // 
            // ButtonSaveInfo
            // 
            this.ButtonSaveInfo.Location = new System.Drawing.Point(12, 301);
            this.ButtonSaveInfo.Name = "ButtonSaveInfo";
            this.ButtonSaveInfo.Size = new System.Drawing.Size(110, 25);
            this.ButtonSaveInfo.TabIndex = 1;
            this.ButtonSaveInfo.Text = "Сохранить";
            this.ButtonSaveInfo.UseVisualStyleBackColor = true;
            this.ButtonSaveInfo.Click += new System.EventHandler(this.EditInfoClicked);
            // 
            // EmplEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 337);
            this.Controls.Add(this.ButtonSaveInfo);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EmplEditForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Редактировать сотрудника";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.FormShown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button ButtonSaveInfo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox EditEmployeeName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker DateEmployee;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox SelectEmployeePosition;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox EditEmployeePhoneNumber;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox SelectEmployeeDepartment;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox EditEmployeePostAddress;
        private System.Windows.Forms.CheckBox CheckIsSupervisor;
    }
}