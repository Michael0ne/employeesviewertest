using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace EmployeesViewer
{
    public partial class EmplViewForm : Form
    {
        public bool ConnectionFailed = false;

        static public Int32? AddedEmployeeId;

        private readonly string DataBase = "catalog";
        private readonly string UserName = "root";
        private readonly string UserPassword = "root";
        private readonly string DataBaseHost = "localhost";
        private readonly ushort DataBasePort = 3306;
        private readonly Dictionary<string, string> ColumnsNames = new Dictionary<string, string>
        {
            { "employee_id", "No" },
            { "name", "ФИО" },
            { "birthday", "Дата рождения" },
            { "address", "Почтовый адрес" },
            { "phonenumber", "Телефон" },
            { "position_title", "Должность" },
            { "department_title", "Отдел" },
            { "supervisor_name", "Начальник" }
        };

        private Dictionary<int, string> PositionsList;
        private Dictionary<int, string> SupervisorsList;

        public EmplViewForm()
        {
            try
            {
                DBManager.Instantiate(DataBaseHost, DataBasePort, DataBase, UserName, UserPassword);
                if (!DBManager.ConnectionEstabilished)
                    ConnectionFailed = true;
            }catch(MySql.Data.MySqlClient.MySqlException e)
            {
                MessageBox.Show(e.Message);
                ConnectionFailed = true;
            }catch (System.ArgumentException e)
            {
                MessageBox.Show(e.Message);
                ConnectionFailed = true;
            }
            InitializeComponent();
        }

        private void AddEmployeeClicked(object sender, EventArgs e)
        {
            using (EmplAddForm addForm = new EmplAddForm ())
            {
                DialogResult addFormResult = addForm.ShowDialog();
                if (addFormResult != DialogResult.OK || !AddedEmployeeId.HasValue)
                    return;

                RefreshData();
            }
        }

        private void RefreshData()
        {
            Int32.TryParse(SelectDepartment.SelectedValue.ToString(), out int departmentId);
            Int32.TryParse(SelectSupervisor.SelectedValue.ToString(), out int supervisorId);

            SelectDepartment.Enabled = false;
            SelectSupervisor.Enabled = false;
            ButtonRefresh.Enabled = false;
            ButtonAddEmployee.Enabled = false;
            ListEmployees.Enabled = false;
            ListEmployees.Clear();

            LoadData();
            LoadEmployees(departmentId, supervisorId);

            ListEmployees.Enabled = true;
            ButtonAddEmployee.Enabled = true;
            ButtonRefresh.Enabled = true;
            SelectSupervisor.Enabled = true;
            SelectDepartment.Enabled = true;

            SelectSupervisor.SelectedValue = supervisorId;
            SelectDepartment.SelectedValue = departmentId;
        }

        private void RefreshClicked(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void PropagatePositionsList(Dictionary<int, List<object>> positions)
        {
            PositionsList = new Dictionary<int, string>();

            PositionsList.Add(-1, "<не выбрано>");

            foreach (KeyValuePair<int, List<object>> position in positions)
                PositionsList.Add((int)position.Value[0], (string)position.Value[1]);

            SelectDepartment.DataSource = new BindingSource(PositionsList, null);
            SelectDepartment.DisplayMember = "Value";
            SelectDepartment.ValueMember = "Key";
        }

        private void PropagateSupervisorsList(Dictionary<int, List<object>> supervisors)
        {
            SupervisorsList = new Dictionary<int, string>();

            SupervisorsList.Add(-1, "<не выбрано>");

            foreach (KeyValuePair<int, List<object>> supervisor in supervisors)
                SupervisorsList.Add((int)supervisor.Value[0], (string)supervisor.Value[1]);

            SelectSupervisor.DataSource = new BindingSource(SupervisorsList, null);
            SelectSupervisor.DisplayMember = "Value";
            SelectSupervisor.ValueMember = "Key";
        }

        private void PropagateEmployeesList(List<string> columns, Dictionary<int, List<object>> employees)
        {
            // Добавить к списку сотрудников наименования столбцов из аргумента columns.
            foreach (string column in columns)
            {
                if (ColumnsNames.TryGetValue(column, out string columnValue))
                    ListEmployees.Columns.Add(columnValue);
                else
                    ListEmployees.Columns.Add(column);
            }

            //  Популировать список сотрудников строками из аргумента employees.
            foreach (KeyValuePair<int, List<object>> employee in employees)
            {
                ListViewItem listViewItem = new ListViewItem();
                int index = 0;
                foreach (object valueObject in employee.Value)
                {
                    if (index++ == 0)
                        listViewItem.Text = valueObject.ToString();
                    else
                        listViewItem.SubItems.Add(valueObject.ToString());
                }

                ListEmployees.Items.Add(listViewItem);
            }
        }

        private void LoadData()
        {
            DBManager.Request requestPositions = new DBManager.Request("GetPositions");
            if (!requestPositions.Execute() && requestPositions.GetLastError() != null)
            {
                MessageBox.Show("Не удалось получить список должностей! Ошибка:\n" + requestPositions.GetLastError());
                return;
            }

            if (requestPositions.GetResultsRows() == null)
                MessageBox.Show("Внимание!\nНе найдено ни одной должности.");
            else
                PropagatePositionsList(requestPositions.GetResultsRows());

            DBManager.Request requestSupervisors = new DBManager.Request("GetSupervisors");
            if (!requestSupervisors.Execute() && requestSupervisors.GetLastError() != null)
            {
                MessageBox.Show("Не удалось получить список руководителей! Ошибка:\n" + requestSupervisors.GetLastError());
                return;
            }

            if (requestSupervisors.GetResultsRows() == null)
                MessageBox.Show("Внимание!\nНе найдено ни одного руководителя.");
            else
                PropagateSupervisorsList(requestSupervisors.GetResultsRows());
        }

        private void LoadEmployees(int departmentId = -1, int supervisorId = -1)
        {
            //  TODO: thread!
            DBManager.Request requestEmployees = new DBManager.Request("GetEmployees");
            requestEmployees.AddParameter("department", DBManager.eRequestParameterType.ARGUMENT_INPUT, departmentId, MySql.Data.MySqlClient.MySqlDbType.Int32);
            requestEmployees.AddParameter("supervisor", DBManager.eRequestParameterType.ARGUMENT_INPUT, supervisorId, MySql.Data.MySqlClient.MySqlDbType.Int32);
            if (!requestEmployees.Execute() && requestEmployees.GetLastError() != null)
            {
                MessageBox.Show("Не удалось получить список сотрудников! Ошибка:\n" + requestEmployees.GetLastError());
                return;
            }

            if (requestEmployees.GetResultsRows() == null)
                if (supervisorId != -1)
                    MessageBox.Show("У данного сотрудника нет подчинённых.");
                else
                    MessageBox.Show("Внимание!\nНе найдено ни одного сотрудника.");
            else
                PropagateEmployeesList(requestEmployees.GetResultsColumns(), requestEmployees.GetResultsRows());
        }

        private void EmplViewForm_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadEmployees();
        }

        private void SupervisorFilterChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void DepartmentFilterChanged(object sender, EventArgs e)
        {
            RefreshData();
        }
    }
}