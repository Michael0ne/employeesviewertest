using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmployeesViewer
{
    public partial class EmplEditForm : Form
    {
        private readonly uint MinimumAllowedAge = 18;
        private readonly uint MaximumAllowedAge = 100;
        private readonly bool CheckAgeRestrictions = true;
        private readonly bool CheckErrors = true;
        private readonly Dictionary<Int32, string> ResponseCodes = new Dictionary<int, string>
        {
            { -100, "Данной должности не существует." },
            { -200, "Данного отдела не существует." },
            { -300, "Не удалось добавить сотрудника." },
            { -400, "Такого сотрудника не существует." }
        };

        private Dictionary<int, string> PositionsList;
        private Dictionary<int, string> DepartmentsList;
        private int EmployeeDatabaseId;

        public EmplEditForm()
        {
            InitializeComponent();
        }

        private void EditInfoClicked(object sender, EventArgs e)
        {
            DBManager.Request requestApplyData = new DBManager.Request("UpdateEmployee");
            requestApplyData.AddParameter("employeeId", DBManager.eRequestParameterType.ARGUMENT_INPUT, EmployeeDatabaseId, MySql.Data.MySqlClient.MySqlDbType.Int32);
            requestApplyData.AddParameter("newName", DBManager.eRequestParameterType.ARGUMENT_INPUT, EditEmployeeName.Text, MySql.Data.MySqlClient.MySqlDbType.TinyText);
            requestApplyData.AddParameter("newBirthday", DBManager.eRequestParameterType.ARGUMENT_INPUT, DateEmployee.Value, MySql.Data.MySqlClient.MySqlDbType.Date);
            requestApplyData.AddParameter("newPosition", DBManager.eRequestParameterType.ARGUMENT_INPUT, SelectEmployeePosition.SelectedValue, MySql.Data.MySqlClient.MySqlDbType.Int32);
            requestApplyData.AddParameter("newPhoneNumber", DBManager.eRequestParameterType.ARGUMENT_INPUT, EditEmployeePhoneNumber.Text, MySql.Data.MySqlClient.MySqlDbType.Int64);
            requestApplyData.AddParameter("newDepartment", DBManager.eRequestParameterType.ARGUMENT_INPUT, SelectEmployeeDepartment.SelectedValue, MySql.Data.MySqlClient.MySqlDbType.Int32);
            requestApplyData.AddParameter("newAddress", DBManager.eRequestParameterType.ARGUMENT_INPUT, EditEmployeePostAddress.Text, MySql.Data.MySqlClient.MySqlDbType.Text);
            requestApplyData.AddParameter("newIsSupervisor", DBManager.eRequestParameterType.ARGUMENT_INPUT, CheckIsSupervisor.Checked ? 1 : 0, MySql.Data.MySqlClient.MySqlDbType.Byte);
            requestApplyData.AddParameter(null, DBManager.eRequestParameterType.ARGUMENT_OUTPUT, null, MySql.Data.MySqlClient.MySqlDbType.Int32);

            if (!requestApplyData.Execute(out object result))
            {
                MessageBox.Show("При обновлении возникла ошибка! Ошибка:\n" + requestApplyData.GetLastError());
                return;
            }

            Int32.TryParse(result.ToString(), out int responseCode);
            if (responseCode < 0)
                MessageBox.Show("При обновлении возникла ошибка:\n" + ResponseCodes[responseCode]);
            else
                MessageBox.Show("Информация обновлена успешно!");

            EmplViewForm.SelectedEmployeeId = EmployeeDatabaseId;
            DialogResult = DialogResult.OK;
        }

        private void PropagatePositionsList(Dictionary<int, List<object>> positions)
        {
            PositionsList = new Dictionary<int, string>();

            foreach (KeyValuePair<int, List<object>> position in positions)
                PositionsList.Add((int)position.Value[0], (string)position.Value[1]);

            SelectEmployeePosition.DataSource = new BindingSource(PositionsList, null);
            SelectEmployeePosition.DisplayMember = "Value";
            SelectEmployeePosition.ValueMember = "Key";
        }

        private void PropagateDepartmentsList(Dictionary<int, List<object>> departments)
        {
            DepartmentsList = new Dictionary<int, string>();

            foreach (KeyValuePair<int, List<object>> department in departments)
                DepartmentsList.Add((int)department.Value[0], (string)department.Value[1]);

            SelectEmployeeDepartment.DataSource = new BindingSource(DepartmentsList, null);
            SelectEmployeeDepartment.DisplayMember = "Value";
            SelectEmployeeDepartment.ValueMember = "Key";
        }

        private void LoadData()
        {
            DBManager.Request requestGetData = new DBManager.Request("GetEmployee");
            requestGetData.AddParameter("employeeId", DBManager.eRequestParameterType.ARGUMENT_INPUT, EmplViewForm.SelectedEmployeeId, MySql.Data.MySqlClient.MySqlDbType.Int32);

            if (!requestGetData.Execute() || requestGetData.GetLastError() != null || requestGetData.GetResultsRows() == null)
            {
                MessageBox.Show("Не удалось загрузить информацию о сотруднике! Ошибка:\n" + requestGetData.GetLastError());
                return;
            }

            var resultRows = requestGetData.GetResultsRows();
            EmployeeDatabaseId = (int)resultRows[0][0];

            EditEmployeeName.Text = (string)resultRows[0][1];
            DateEmployee.Value = (DateTime)resultRows[0][2];

            SelectEmployeePosition.SelectedItem = ((int)resultRows[0][3]) - 1;
            SelectEmployeePosition.SelectedIndex = ((int)resultRows[0][3]) - 1;
            SelectEmployeePosition.SelectedValue = (int)resultRows[0][3];

            Int64 phoneNumber = (Int64)resultRows[0][4];
            EditEmployeePhoneNumber.Text = phoneNumber.ToString();

            SelectEmployeeDepartment.SelectedItem = ((int)resultRows[0][5]) - 1;
            SelectEmployeeDepartment.SelectedIndex = ((int)resultRows[0][5]) - 1;
            SelectEmployeeDepartment.SelectedValue = (int)resultRows[0][5];

            CheckIsSupervisor.Checked = DBNull.Value == resultRows[0][6];
            EditEmployeePostAddress.Text = (string)resultRows[0][7];
        }

        private void FormShown(object sender, EventArgs e)
        {
            Left = Screen.PrimaryScreen.WorkingArea.Width / 2 - (Width / 2);
            Top = Screen.PrimaryScreen.WorkingArea.Height / 2 - (Height / 2);

            ButtonSaveInfo.Left = Width / 2 - (ButtonSaveInfo.Width / 2);

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

            DBManager.Request requestDepartments = new DBManager.Request("GetDepartments");
            if (!requestDepartments.Execute() && requestDepartments.GetLastError() != null)
            {
                MessageBox.Show("Не удалось получить список отделов! Ошибка:\n" + requestPositions.GetLastError());
                return;
            }

            if (requestDepartments.GetResultsRows() == null)
                MessageBox.Show("Внимание!\nНе найдено ни одного отдела.");
            else
                PropagateDepartmentsList(requestDepartments.GetResultsRows());

            LoadData();
        }
    }
}