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
    public partial class EmplAddPositionForm : Form
    {
        public EmplAddPositionForm()
        {
            InitializeComponent();
        }

        private void FormShown(object sender, EventArgs e)
        {
            Left = Screen.PrimaryScreen.WorkingArea.Width / 2 - (Width / 2);
            Top = Screen.PrimaryScreen.WorkingArea.Height / 2 - (Height / 2);

            ButtonAddPosition.Left = Width / 2 - (ButtonAddPosition.Width / 2);
            EditTitle.Focus();
        }

        private void PositionAddClicked(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(EditTitle.Text))
            {
                MessageBox.Show("Поле \"Название\" должно быть заполнено.");
                return;
            }

            DBManager.Request addPositionRequest = new DBManager.Request("AddPosition");
            addPositionRequest.AddParameter("newTitle", DBManager.eRequestParameterType.ARGUMENT_INPUT, EditTitle.Text.ToString(), MySql.Data.MySqlClient.MySqlDbType.TinyText);
            addPositionRequest.AddParameter(null, DBManager.eRequestParameterType.ARGUMENT_OUTPUT, null, MySql.Data.MySqlClient.MySqlDbType.Int32);

            if (!addPositionRequest.Execute(out object responseCode) || (int)responseCode == 0)
            {
                MessageBox.Show("Не удалось добавить должность. Возможно, она уже существует. Ошибка:\n" + addPositionRequest.GetLastError());
                return;
            }
            else
                MessageBox.Show("Должность успешно добавлена.");

            EmplViewForm.AddedPositionId = (int)responseCode;
            DialogResult = DialogResult.OK;
        }
    }
}