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
    public partial class EmplAddForm : Form
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

        public EmplAddForm()
        {
            InitializeComponent();
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

        private void FormShown(object sender, EventArgs e)
        {
            Left = Screen.PrimaryScreen.WorkingArea.Width / 2 - (Width / 2);
            Top = Screen.PrimaryScreen.WorkingArea.Height / 2 - (Height / 2);

            ButtonAddEmployee.Left = Width / 2 - (ButtonAddEmployee.Width / 2);

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
        }

        private void EmployeeAddClicked(object sender, EventArgs e)
        {
            string errorString = null;
            byte errorsFound = 0;

            if (String.IsNullOrEmpty(EditEmployeeName.Text))
            {
                errorString += "Поле \"ФИО\" должно быть заполнено.\n";
                errorsFound++;
            }

            if (String.IsNullOrEmpty(DateEmployee.Text))
            {
                errorString += "Поле \"Дата рождения\" должно быть заполнено.\n";
                errorsFound++;
            }

            if (CheckAgeRestrictions)
            {
                DateTime parsedDate = DateEmployee.Value;
                DateTime currentDate = DateTime.Now;
                if (currentDate.Subtract(parsedDate).TotalDays <= (365 * MinimumAllowedAge) ||
                    currentDate.Subtract(parsedDate).TotalDays >= (365 * MaximumAllowedAge))
                {
                    errorString += "Указан недопустимый возраст сотрудника.\n";
                    errorsFound++;
                }
            }

            if (SelectEmployeePosition.Text == null || SelectEmployeePosition.SelectedIndex < 0)
            {
                errorString += "Поле \"Должность\" должно быть заполнено.\n";
                errorsFound++;
            }

            if (String.IsNullOrEmpty(EditEmployeePhoneNumber.Text))
            {
                errorString += "Поле \"Номер телефона\" должно быть заполнено.\n";
                errorsFound++;
            }

            if (SelectEmployeeDepartment.Text == null || SelectEmployeeDepartment.SelectedIndex < 0)
            {
                errorString += "Поле \"Отдел\" должно быть заполнено.\n";
                errorsFound++;
            }

            if (String.IsNullOrEmpty(EditEmployeePostAddress.Text))
            {
                errorString += "Поле \"Почтовый адрес\" должно быть заполнено.\n";
                errorsFound++;
            }

            if (CheckErrors && errorsFound > 0)
            {
                MessageBox.Show(errorString);
                return;
            }

            DBManager.Request dbrequest = new DBManager.Request("AddEmployee");

            dbrequest.AddParameter("name", DBManager.eRequestParameterType.ARGUMENT_INPUT, EditEmployeeName.Text, MySql.Data.MySqlClient.MySqlDbType.TinyText);
            dbrequest.AddParameter("birthday", DBManager.eRequestParameterType.ARGUMENT_INPUT, DateEmployee.Value, MySql.Data.MySqlClient.MySqlDbType.Date);
            dbrequest.AddParameter("position", DBManager.eRequestParameterType.ARGUMENT_INPUT, (int)SelectEmployeePosition.SelectedValue, MySql.Data.MySqlClient.MySqlDbType.Int32);
            dbrequest.AddParameter("phonenumber", DBManager.eRequestParameterType.ARGUMENT_INPUT, EditEmployeePhoneNumber.Text, MySql.Data.MySqlClient.MySqlDbType.Int64);
            dbrequest.AddParameter("department", DBManager.eRequestParameterType.ARGUMENT_INPUT, (int)SelectEmployeeDepartment.SelectedValue, MySql.Data.MySqlClient.MySqlDbType.Int32);
            dbrequest.AddParameter("address", DBManager.eRequestParameterType.ARGUMENT_INPUT, EditEmployeePostAddress.Text, MySql.Data.MySqlClient.MySqlDbType.Text);
            dbrequest.AddParameter("IsSupervisor", DBManager.eRequestParameterType.ARGUMENT_INPUT, CheckIsSupervisor.Checked ? 1 : 0, MySql.Data.MySqlClient.MySqlDbType.Byte);
            dbrequest.AddParameter(null, DBManager.eRequestParameterType.ARGUMENT_OUTPUT, null, MySql.Data.MySqlClient.MySqlDbType.Int32);

            if (!dbrequest.Execute(out object result))
            {
                MessageBox.Show("При добавлении возникла ошибка! Ошибка:\n" + dbrequest.GetLastError());
                return;
            }

            Int32.TryParse(result.ToString(), out Int32 responseCode);
            if (responseCode < 0)
                MessageBox.Show("При добавлении возникла ошибка:\n" + ResponseCodes[responseCode]);
            else
                MessageBox.Show("Добавление сотрудника успешно! ID: " + responseCode);

            EmplViewForm.AddedEmployeeId = responseCode;
            DialogResult = DialogResult.OK;
        }
    }
}