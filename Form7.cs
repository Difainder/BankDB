using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using System.Diagnostics;

namespace _x_cyeta
{
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
        }

        public void SqlCon()
        {
            string ConnectionString = @"Data Source=192.168.112.103;Initial Catalog=db22203;User ID=User003;Password=User003%]40;Integrated Security=False;MultipleActiveResultSets=True;";
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            sqlConnection.Open();

            var meterCount = 0;
            decimal profit = 0;

            StreamWriter file = new StreamWriter(@"C:\SQLReport\Квитанции.html");
            file.WriteLine("<h1 style='text-align: center; font-size: 3em;'> Квитанции </h1>");

            /* Получаем информацию по счётчикам */
            SqlCommand command = new SqlCommand
            (
                "select txtMeterOwner, datMeterBegin, txtMeterAddres, txtMeterBeginValue, intMeterId, txtMeterNumber " +
                "from tblMeter"
                , sqlConnection
            );

            SqlDataReader reader = command.ExecuteReader();
            SqlCommand command2;
            SqlDataReader reader2;

            while (reader.Read())
            {
                file.WriteLine("<p style='margin-left: 20px; font-size: 1.15em;'> Номер счетчика:  " + reader[5] + " </p>");
                file.WriteLine("<p style='margin-left: 20px; font-size: 1.15em;'> ФИО владельца:  " + reader[0] + " </p>");
                file.WriteLine("<p style='margin-left: 20px; font-size: 1.15em;'> Дата установки:  " + (Convert.ToDateTime(reader[1])).ToString("dd.MM.yyyy") + " </p>");
                file.WriteLine("<p style='margin-left: 20px; font-size: 1.15em;'> Адрес:  " + reader[2] + " </p>");
                file.WriteLine("<p style='margin-left: 20px; font-size: 1.15em;'> Начальные показания:  " + reader[3] + " </p>");
                var meterNumber = reader[5];
                var beginVal = Int32.Parse(reader[3].ToString());
                var id = reader[4]; //РК
                decimal paidSum = 0;
                int checkPaidCount = 0;
                meterCount++;

                file.WriteLine
                ("<table align = 'center' border = '1' width = 80%> " +
                    "<tr> " +
                    "   <th> Показания счетчика </th> " +
                    "   <th> Дата оплаты </th>  " +
                    "   <th> Сумма </th> " +
                    "</tr>"
                );

                /* Получаем данные для таблицы по каждому счётчику */
                command2 = new SqlCommand
                (
                    "select tblCheck.txtCheckMeterValue, tblCheck.datCheckPaid, tblCheck.fltCheckSum " +
                    "from tblCheck,tblMeter " +
                    "where " +
                    "   (tblMeter.intMeterId = tblCheck.intMeterId) and " +
                    "   (tblCheck.intMeterId = '" + id + "') " +
                    "ORDER BY tblCheck.datCheckPaid ASC",
                    sqlConnection
                );
                reader2 = command2.ExecuteReader();

                while (reader2.Read())
                {
                    file.WriteLine("<tr> <th>" + reader2[0] + " </th>");
                    file.WriteLine("<th>" + Convert.ToDateTime(reader2[1]).ToString("dd.MM.yyyy") + " </th>");
                    file.WriteLine("<th>" + reader2[2] + " </th>");
                    paidSum += reader2.GetDecimal(2);
                    profit += reader2.GetDecimal(2);
                    checkPaidCount++;
                }
                file.WriteLine("</table> <br>");
                reader2.Close();

                int start = 0;
                
                command2 = new SqlCommand
                (
                    "select txtMeterBeginValue " +
                    "from tblMeter " +
                    "where (txtMeterNumber = '" + meterNumber + "')", 
                    sqlConnection
                );
                reader2 = command2.ExecuteReader();

                while (reader2.Read())
                {
                    start = Int32.Parse(reader2[0].ToString());
                }
                reader2.Close();

                /* Последнее значение на счётчике */
                command2 = new SqlCommand
                (
                    "SELECT top 1 txtCheckMeterValue " +
                    "FROM tblCheck,tblMeter " +
                    "WHERE " +
                    "   (tblMeter.intMeterId = tblCheck.intMeterId) and " +
                    "   (tblMeter.txtMeterNumber = '" + meterNumber + "') " +
                    "ORDER BY CAST(txtCheckMeterValue AS int) DESC", 
                    sqlConnection
                );
                reader2 = command2.ExecuteReader();

                int finish = 0;
                while (reader2.Read())
                {
                    finish = Int32.Parse(reader2[0].ToString());
                }

                reader2.Close();

                var diff = finish - start;
                if (finish > 0)
                {                 
                    file.WriteLine("<p style='margin-left: 30px; font-size: 1.15em;'>Количество выплат по квитанциям:  " + checkPaidCount + " </p>");
                    file.WriteLine("<p style='margin-left: 30px; font-size: 1.15em;'>Сумма выплат по квитанциям:  " + paidSum + " </p>");
                    file.WriteLine("<p style='margin-left: 30px; font-size: 1.15em;'>Число использованных КВт (разница):  " + diff + " </p>");
                    file.WriteLine("<p style='margin-left: 30px; font-size: 1.15em;'>Стоимость 1 КВт:  " + (paidSum / finish).ToString("F2") + " </p>");
                }
                file.WriteLine("<hr width = 100% size = 5 style = 'background-color: #000000;'></hr>");
            }
            reader.Close();

            file.WriteLine("<hr width = 100%></hr>");
            file.WriteLine("<p style='margin-left: 60px; font-size: 1.5em'>КОЛИЧЕСТВО СЧЁТЧИКОВ:  " + meterCount + " </p>");
            file.WriteLine("<p style='margin-left: 60px; font-size: 1.5em'>ОБЩАЯ ВЫРУЧКА:  " + profit + " </p>");
            file.WriteLine("<hr width = 100%></hr>");
            file.Close();
            this.Close();

            Process.Start(@"C:\SQLReport\Квитанции.html");
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
