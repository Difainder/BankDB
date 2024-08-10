using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Diagnostics;

namespace _x_cyeta
{
    public partial class Form8 : Form
    {
        public Form8()
        {
            InitializeComponent();
            SqlCon();
        }

        public void SqlCon()
        {
            SqlConnection connection = new SqlConnection(@"Data Source=192.168.112.103;Initial Catalog=db22203;Persist Security Info=True;User ID=User003;Password=User003%]40");

            try
            {
                connection.Open();
                SqlDataReader reader = null;
                SqlCommand command = new SqlCommand("select txtInspectorName from tblInspector ORDER BY txtInspectorName ASC", connection);

                reader = command.ExecuteReader();

                while (reader.Read())
                    comboBox1.Items.Add(reader[0].ToString());
                reader.Close();
            }

            catch (Exception a)
            {
                MessageBox.Show(a.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public void SqlCon2()
        {
            string ConnectionString = @"Data Source=192.168.112.103;Initial Catalog=db22203;User ID=User003;Password=User003%]40;Integrated Security=False;MultipleActiveResultSets=True;";
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            sqlConnection.Open();

            string textLine = comboBox1.Text;

            /* Для указанного контролёра */
            var command = new SqlCommand
            (
                "select txtInspectorName,txtInspectorPost,intInspectorId " +
                "from tblInspector " +
                "where (txtInspectorName = '" + textLine + "')"
                , sqlConnection
            );
            var reader = command.ExecuteReader();

            StreamWriter file = new StreamWriter(@"C:\SQLReport\Штрафы.html");
            file.WriteLine("<h1 style='text-align: center; font-size: 3em;'> Штрафы </h1>");

            while (reader.Read())
            {
                file.WriteLine("<h1 style='margin-left: 20px; font-size: 1.15em;'> ФИО контролёра:  " + reader[0] + " </h1>");
                file.WriteLine("<p style='margin-left: 20px; font-size: 1.15em;'> Должность:  " + reader[1] + " </p>");
                file.WriteLine("<h2 style='margin-left: 20px; font-size: 1.15em; text-align: center'> Оштрафованы </h2>");
                file.WriteLine("<hr width = 95% size = 2.5 style = 'background-color: #000000;'></hr>");

                /* Информация о счётчике */
                var command1 = new SqlCommand
                (
                    "select DISTINCT tblMeter.txtMeterOwner, tblMeter.txtMeterAddres, tblMeter.txtMeterNumber " +
                    "from tblInspector, tblPenalty, tblControl, tblMeter " +
                    "where (tblInspector.intInspectorId = tblControl.intInspectorId) and " +
                    "      (tblPenalty.intControlId = tblControl.intControlId) and " +
                    "      (tblMeter.intMeterId = tblControl.intMeterId) and " +
                    "      (tblInspector.intInspectorId = '" + reader[2] + "')"
                    , sqlConnection
                );
                var reader1 = command1.ExecuteReader();

                while (reader1.Read())
                {
                    int billsSum = 0;
                    int paidBillsCount = 0;
                    int notPaidBillsCount = 0;

                    file.WriteLine("<p style='margin-left: 60px;'> Номер счетчика:  " + reader1[2] + " </p>");
                    file.WriteLine("<p style='margin-left: 60px;'> ФИО владельца:  " + reader1[0] + " </p>");
                    file.WriteLine("<p style='margin-left: 60px;'> Адрес:  " + reader1[1] + " </p>");

                    /* Данные в таблицу штрафов */
                    var command2 = new SqlCommand
                    (
                        "select tblControl.datControlDate, tblPenalty.fltPenaltySum,tblPenalty.blnPenaltyPaid " +
                        "from tblInspector, tblControl, tblPenalty, tblMeter	 " +
                        "where (tblInspector.intInspectorId = tblControl.intInspectorId) and " +
                        "      (tblControl.intControlId = tblPenalty.intControlId) and " +
                        "      (tblMeter.intMeterId = tblControl.intMeterId) and " +
                        "      (tblMeter.txtMeterNumber= '" + reader1[2] + "') " +
                        "order by tblControl.datControlDate ASC"
                        , sqlConnection
                    );

                    var tableFiller = command2.ExecuteReader();
                    file.WriteLine
                    (
                        "<table align = 'center' border = '1' width = 80%>  " +
                        "   <tr> " +
                        "       <th> Дата </th> " +
                        "       <th> Сумма </th>  " +
                        "       <th> Информация об уплате </th> " +
                        "   </tr>"
                    );

                    while (tableFiller.Read())
                    {
                        var tr = (bool)tableFiller[2];
                        file.WriteLine("<th>" + (Convert.ToDateTime(tableFiller[0])).ToString("dd.MM.yyyy") + " </th>");
                        file.WriteLine("<th>" + tableFiller[1] + " </th>");
                        if (tr)
                            file.WriteLine("<th> Да </th> </tr>");
                        else
                        {
                            file.WriteLine("<th> Нет </th> </tr>");
                            paidBillsCount++;
                            notPaidBillsCount += Convert.ToInt32(tableFiller[1]);              
                        }
                        billsSum += Convert.ToInt32(tableFiller[1]);
                    }
                    file.WriteLine("</table> <br>");

                    file.WriteLine("<p style='margin-left: 60px; font-size: 1.15em;'> Сумма всех штрафов:  " + billsSum + " </p>");
                    file.WriteLine("<p style='margin-left: 60px;font-size: 1.15em;'> Число не оплаченных штрафов:  " + paidBillsCount + " </p>");
                    file.WriteLine("<p style='margin-left: 60px;font-size: 1.15em;'> Сумма не оплаченных штрафов:  " + notPaidBillsCount + " </p>");
                    file.WriteLine("<hr width = 87% style = 'margin-left: 50px'></hr>");
                    file.WriteLine("</table> <br>");

                    tableFiller.Close();

                }
                reader1.Close();
                file.WriteLine("<hr size = 5 style = 'background-color: #000000;'></hr>");
            }
            reader.Close();

            file.Close();
            this.Close();

            var f8 = new Form8();
            f8.Show();
            
            Process.Start(@"C:\SQLReport\Штрафы.html");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCon2();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
