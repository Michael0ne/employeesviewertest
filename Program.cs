using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmployeesViewer
{
    internal static class Program
    {
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            EmplViewForm emplViewForm = new EmplViewForm();
            if (emplViewForm.ConnectionFailed)
                return;
            Application.Run(emplViewForm);
        }
    }
}