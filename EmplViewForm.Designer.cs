﻿namespace EmployeesViewer
{
    partial class EmplViewForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmplViewForm));
            this.employeesGroup = new System.Windows.Forms.GroupBox();
            this.ListEmployees = new System.Windows.Forms.ListView();
            this.filtersGroup = new System.Windows.Forms.GroupBox();
            this.SelectSupervisor = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SelectDepartment = new System.Windows.Forms.ComboBox();
            this.ButtonAddEmployee = new System.Windows.Forms.Button();
            this.ButtonRefresh = new System.Windows.Forms.Button();
            this.employeesGroup.SuspendLayout();
            this.filtersGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // employeesGroup
            // 
            this.employeesGroup.Controls.Add(this.ListEmployees);
            this.employeesGroup.Location = new System.Drawing.Point(12, 82);
            this.employeesGroup.Name = "employeesGroup";
            this.employeesGroup.Size = new System.Drawing.Size(760, 334);
            this.employeesGroup.TabIndex = 0;
            this.employeesGroup.TabStop = false;
            this.employeesGroup.Text = "Сотрудники";
            // 
            // ListEmployees
            // 
            this.ListEmployees.GridLines = true;
            this.ListEmployees.HideSelection = false;
            this.ListEmployees.Location = new System.Drawing.Point(6, 19);
            this.ListEmployees.Name = "ListEmployees";
            this.ListEmployees.Size = new System.Drawing.Size(748, 308);
            this.ListEmployees.TabIndex = 0;
            this.ListEmployees.UseCompatibleStateImageBehavior = false;
            this.ListEmployees.View = System.Windows.Forms.View.Details;
            // 
            // filtersGroup
            // 
            this.filtersGroup.Controls.Add(this.SelectSupervisor);
            this.filtersGroup.Controls.Add(this.label2);
            this.filtersGroup.Controls.Add(this.label1);
            this.filtersGroup.Controls.Add(this.SelectDepartment);
            this.filtersGroup.Location = new System.Drawing.Point(12, 12);
            this.filtersGroup.Name = "filtersGroup";
            this.filtersGroup.Size = new System.Drawing.Size(276, 64);
            this.filtersGroup.TabIndex = 1;
            this.filtersGroup.TabStop = false;
            this.filtersGroup.Text = "Фильтры";
            // 
            // SelectSupervisor
            // 
            this.SelectSupervisor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SelectSupervisor.FormattingEnabled = true;
            this.SelectSupervisor.Location = new System.Drawing.Point(141, 32);
            this.SelectSupervisor.Name = "SelectSupervisor";
            this.SelectSupervisor.Size = new System.Drawing.Size(121, 21);
            this.SelectSupervisor.TabIndex = 3;
            this.SelectSupervisor.SelectionChangeCommitted += new System.EventHandler(this.SupervisorFilterChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(138, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Руководитель";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Отдел";
            // 
            // SelectDepartment
            // 
            this.SelectDepartment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SelectDepartment.FormattingEnabled = true;
            this.SelectDepartment.Location = new System.Drawing.Point(9, 32);
            this.SelectDepartment.Name = "SelectDepartment";
            this.SelectDepartment.Size = new System.Drawing.Size(121, 21);
            this.SelectDepartment.TabIndex = 0;
            this.SelectDepartment.SelectionChangeCommitted += new System.EventHandler(this.DepartmentFilterChanged);
            // 
            // ButtonAddEmployee
            // 
            this.ButtonAddEmployee.Image = ((System.Drawing.Image)(resources.GetObject("ButtonAddEmployee.Image")));
            this.ButtonAddEmployee.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ButtonAddEmployee.Location = new System.Drawing.Point(603, 28);
            this.ButtonAddEmployee.Name = "ButtonAddEmployee";
            this.ButtonAddEmployee.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.ButtonAddEmployee.Size = new System.Drawing.Size(169, 37);
            this.ButtonAddEmployee.TabIndex = 2;
            this.ButtonAddEmployee.Text = "Добавить сотрудника...";
            this.ButtonAddEmployee.UseVisualStyleBackColor = true;
            this.ButtonAddEmployee.Click += new System.EventHandler(this.AddEmployeeClicked);
            // 
            // ButtonRefresh
            // 
            this.ButtonRefresh.Image = ((System.Drawing.Image)(resources.GetObject("ButtonRefresh.Image")));
            this.ButtonRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ButtonRefresh.Location = new System.Drawing.Point(496, 28);
            this.ButtonRefresh.Name = "ButtonRefresh";
            this.ButtonRefresh.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.ButtonRefresh.Size = new System.Drawing.Size(101, 37);
            this.ButtonRefresh.TabIndex = 3;
            this.ButtonRefresh.Text = "Обновить";
            this.ButtonRefresh.UseVisualStyleBackColor = true;
            this.ButtonRefresh.Click += new System.EventHandler(this.RefreshClicked);
            // 
            // EmplViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 421);
            this.Controls.Add(this.ButtonRefresh);
            this.Controls.Add(this.ButtonAddEmployee);
            this.Controls.Add(this.filtersGroup);
            this.Controls.Add(this.employeesGroup);
            this.MaximizeBox = false;
            this.Name = "EmplViewForm";
            this.Text = "Справочник сотрудников";
            this.Load += new System.EventHandler(this.EmplViewForm_Load);
            this.employeesGroup.ResumeLayout(false);
            this.filtersGroup.ResumeLayout(false);
            this.filtersGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox employeesGroup;
        private System.Windows.Forms.GroupBox filtersGroup;
        private System.Windows.Forms.ListView ListEmployees;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox SelectDepartment;
        private System.Windows.Forms.ComboBox SelectSupervisor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ButtonAddEmployee;
        private System.Windows.Forms.Button ButtonRefresh;
    }
}
