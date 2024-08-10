using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace _x_cyeta
{
    public partial class Form4 : Form
    {
        private SqlConnection sqlConnection = null;
        private string ID = "";
        public Form4(string meterNumber, string addr, string owner, string last, string meterId)
        {
            InitializeComponent();
            textBox1.Text = meterNumber;
            textBox2.Text = addr;
            textBox3.Text = owner;
            textBox4.Text = last;
            ID = meterId;
            string ConnectionString = @"Data Source=192.168.112.103;Initial Catalog=db22203;Persist Security Info=True;User ID=User003;Password=User003%]40";
            sqlConnection = new SqlConnection(ConnectionString);
            sqlConnection.Open();

            SqlCommand command2 = new SqlCommand("SELECT txtInspectorName From tblInspector", sqlConnection);
            SqlDataReader reader2 = command2.ExecuteReader();
            var sp2 = new List<string>();
            while (reader2.Read())
            {
                sp2.Add(reader2[0].ToString());
            }
            reader2.Close();
            
            comboBox1.DataSource = sp2;
        }
        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (maskedTextBox1.Text.ToString().Length != 10)
            {
                MessageBox.Show("В поле 'Начальные показания счетчика' должно быть ровно 10 цифр", "Ошибка");
                sqlConnection.Close();
                Close();
                this.Close();
                var f2 = new Form2();
                f2.Show();
            }

            SqlCommand getIdInspector = new SqlCommand("Select intInspectorId FROM tblInspector WHERE (txtInspectorName = '" + comboBox1.Text + "')", sqlConnection);
            var readrerInsp = getIdInspector.ExecuteReader();
            var inspId = 0;

            while (readrerInsp.Read())
            {
                inspId = readrerInsp.GetInt32(0);
            }
            readrerInsp.Close();

            SqlCommand getMeterId = new SqlCommand
            (
                "select intMeterId " +
                "from tblMeter " +
                "where (txtMeterNumber = '" + textBox1.Text + "') and " +
                "      (txtMeterAddres   = '" + textBox2.Text + "') and" +
                "      (txtMeterOwner    = '" + textBox3.Text + "')" 
                , sqlConnection
            );

            var readderMeterId = getMeterId.ExecuteReader();
            var meterId = 0;

            while (readderMeterId.Read())
            {
                meterId = readderMeterId.GetInt32(0);
            }
            readderMeterId.Close();

            SqlCommand command = new SqlCommand
            (
                "INSERT INTO tblControl (datControlDate, intInspectorId, intMeterId, txtMeterControlValue)" +
                "VALUES              (@datControlDate, @intInspectorId, @intMeterId, @txtMeterControlValue)"
                , sqlConnection
            );

            DateTime date = DateTime.Parse(dateTimePicker1.Text);
            command.Parameters.AddWithValue("datControlDate", $"{date.Month}/{date.Day}/{date.Year}");
            command.Parameters.AddWithValue("intInspectorId", inspId);
            command.Parameters.AddWithValue("intMeterId", ID);
            command.Parameters.AddWithValue("txtMeterControlValue", textBox4.Text);


            int success = command.ExecuteNonQuery();

            if (success != 0)
                MessageBox.Show("Изменения внесены", "Изменение записи");
            else 
                MessageBox.Show("Не удалось внести изменения", "Изменение записи");

            this.Close();
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
