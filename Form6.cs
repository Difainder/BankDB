using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Diagnostics;

namespace _x_cyeta
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }

        public void SqlCon()
        {
            string ConnectionString = @"Data Source=192.168.112.103;Initial Catalog=db22203;User ID=User003;Password=User003%]40;Integrated Security=False;MultipleActiveResultSets=True;";
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            sqlConnection.Open();

            StreamWriter file = new StreamWriter(@"C:\SQLReport\Проверки.html");
            file.WriteLine("<h1 style='text-align: center; font-size: 3em'> Проверки </h1>");

            /* Для каждого контролёра... */
            SqlCommand command = new SqlCommand
            (
                "select txtInspectorName,txtInspectorPost, intInspectorId " +
                "from tblInspector"
                , sqlConnection
            );
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int allChecksCount = 0;
                file.WriteLine("<h3 style='margin-left: 20px;'> ФИО контролёра:  " + reader[0] + " </h3>");
                file.WriteLine("<p style='margin-left: 20px;'> Должность:  " + reader[1] + " </p>");
                file.WriteLine("<p align = 'center' style = 'font-size: 1.3em; font-weight: bold;'> Проверенные счётчики </p><br>");

                var command1 = new SqlCommand("select tblMeter.txtMeterNumber, tblMeter.datMeterBegin, tblMeter.txtMeterOwner, tblMeter.txtMeterAddres from tblControl, tblMeter,tblInspector where (tblInspector.intInspectorId = tblControl.intInspectorId) and (tblMeter.intMeterId = tblControl.intMeterId) and (tblControl.intInspectorId = '" + reader[2] + "')", sqlConnection);
                var reader1 = command1.ExecuteReader();

                int currMeterCount = 0;
                while (reader1.Read())
                {              

                    file.WriteLine("<p style='margin-left: 70px;'> Номер счетчика:  " + reader1[0] + " </p>");
                    file.WriteLine("<p style='margin-left: 70px;'> Дата установки:  " + (Convert.ToDateTime(reader1[1])).ToString("dd.MM.yyyy") + " </p>");
                    file.WriteLine("<p style='margin-left: 70px;'>  ФИО владельца:  " + reader1[2] + " </p>");
                    file.WriteLine("<p style='margin-left: 70px;'>  Адрес:  " + reader1[3] + " </p>");

                    /* Выводим информацию по проверкам */
                    var command2 = new SqlCommand
                    (
                        "select tblControl.datControlDate, tblControl.txtMeterControlValue, tblMeter.txtMeterNumber " +
                        "from tblControl, tblMeter, tblInspector " +
                        "where  (tblControl.intInspectorId = tblInspector.intInspectorId) and " +
                        "       (tblControl.intMeterId = tblMeter.intMeterId) and " +
                        "       (tblControl.intInspectorId = '" + reader[2] + "') and " +
                        "       (tblMeter.txtMeterNumber = '" + reader1[0] + "') " +
                        "ORDER BY tblControl.datControlDate ASC"
                        , sqlConnection
                    );
                    var reader2 = command2.ExecuteReader();

                    file.WriteLine
                    (
                        "<table width = 90% align = 'center' border = '1'>  " +
                        "   <tr> " +
                        "       <th> Дата проверки </th> " +
                        "       <th> Показатели </th>"+
                        "   </tr>");

                    while (reader2.Read())
                    {
                        file.WriteLine("<tr> <th>" + (Convert.ToDateTime(reader2[0])).ToString("dd.MM.yyyy") + " </th>");
                        file.WriteLine("<th>" + reader2[1] + " </th>");
                        currMeterCount++;
                    }
                    file.WriteLine("</table> <br>");

                    reader2.Close();
                    file.WriteLine("<p style='margin-left: 1200px;'> Количество проверок счетчика:  " + currMeterCount + " </p>");
                    file.WriteLine("<hr width = 90%></hr>");
                    allChecksCount++;
                }

                if (currMeterCount == 0)
                {
                    file.WriteLine("<p style='margin-left: 70px;'> У данного контролёра проверок нет </p>");
                }
                else
                {
                    file.WriteLine("<p style='margin-left: 60px; font-weight: bold; font-size: 1.1em;'> Количество всех проверок:  " + allChecksCount + " </p>");
                }
                
                file.WriteLine("<hr size = 5 style = 'background-color: #000000;'></hr>");
                reader1.Close();

            }
            reader.Close();
            file.Close();
            this.Close();

            Process.Start(@"C:\SQLReport\Проверки.html");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCon();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
