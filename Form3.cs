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

namespace _x_cyeta
{
    public partial class Form3 : Form
    {
        private SqlDataAdapter adapter = null;
        private DataTable table = null;

        private string Address = "";
        private SqlConnection sqlConn = null;
        private string meterId = "";

        public Form3(SqlConnection sqlConnection, string addr)
        {
            
            InitializeComponent();
            sqlConnection.Open();
            sqlConn = sqlConnection;
            this.Address = addr;

            SqlCommand command = new SqlCommand(
                "SELECT * " +
                "FROM tblMeter " +
                "WHERE (txtMeterAddres = '" + addr + "')"
                , sqlConnection);

            SqlDataReader reader;
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                textBox13.Text = reader[1].ToString(); // Номер
                textBox12.Text = reader[2].ToString(); // Адрес
                textBox11.Text = reader[3].ToString(); // Владелец
                textBox1 .Text = reader[4].ToString(); // Дата установки
                textBox14.Text = reader[5].ToString(); // Начальные показания
                textBox10.Text = reader[6].ToString(); // Кол-во проверок
                textBox9 .Text = reader[7].ToString(); // Сумма
            }
            reader.Close();


            table = new DataTable();

            adapter = new SqlDataAdapter
           (
                "SELECT " +
                    "txtMeterControlValue as 'Показания', " +
                    "datControlDate as 'Дата проверки', " +
                    "txtInspectorName as 'Контролёр', " +
                    "txtInspectorPost as 'Должность'" +
                "FROM tblControl, tblInspector, tblMeter " +
                "WHERE (tblControl.intInspectorId = tblInspector.intInspectorId) and " +
                    "(tblMeter.intMeterId = tblControl.intMeterId) and " +
                    "(tblMeter.txtMeterNumber = '" + textBox13.Text + "')"
                , sqlConnection
            );

            adapter.Fill(table);
            dataGridView1.DataSource = table;
            dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            sqlConnection.Close();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
        }
        private void Cancel_Click_1(object sender, EventArgs e)
        {
            this.Close();

            var form = new Form1();
            form.ShowDialog();
        }


        private void button_Click(object sender, EventArgs e)
        {
            table.Clear();

            adapter.Fill(table);
            dataGridView1.DataSource = table;
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        
        private void button1_Click_1(object sender, EventArgs e)
        {
            var number = textBox13.Text.ToString().Split(' ')[0];
            var addr   = textBox12.Text.ToString();
            var owner  = textBox11.Text.ToString();
            var last   = getLastResults(number);
            
            var f4 = new Form4(number, addr, owner, last, meterId);
            f4.Show();
        }

        private string getLastResults(string number)
        {
            sqlConn.Open();
            SqlCommand getMeterId = new SqlCommand
            (
                "select intMeterId " +
                "from tblMeter " +
                "where (txtMeterNumber LIKE '" + number + "');"
                , sqlConn
            );

            var readderMeterId = getMeterId.ExecuteReader();
            var meterId = 0;
            while (readderMeterId.Read())
            {
                meterId = readderMeterId.GetInt32(0);
                break;
            }
            readderMeterId.Close();
            this.meterId = meterId.ToString();
            SqlCommand getLastResult = new SqlCommand
            (
                "Select txtCheckMeterValue " +
                "FROM tblCheck " +
                "WHERE (intMeterId = '" + meterId + "')" +
                "ORDER BY datCheckPaid DESC;"
                , sqlConn
            );

            var readderLastResult = getLastResult.ExecuteReader();
            var value = "";

            while (readderLastResult.Read())
            {
                value = readderLastResult[0].ToString();
                break;
            }
            readderLastResult.Close();

            return value;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            var f1 = new Form1();
            f1.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            sqlConn.Close();
            var f3 = new Form3(sqlConn, Address);
            f3.Show();
        }
    }
}
