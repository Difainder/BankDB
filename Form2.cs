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
    public partial class Form2 : Form
    {
        private SqlConnection connection;
        public Form2()
        {
            InitializeComponent();
            connection = new SqlConnection(@"Data Source=192.168.112.103;Initial Catalog=db22203;Persist Security Info=True;User ID=User003;Password=User003%]40");
            connection.Open();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox6.Text.ToString().Length != 10)
            {
                MessageBox.Show("В поле 'Начальные показания счетчика' должно быть ровно 10 цифр", "Ошибка");
                connection.Close();
                Close();
                this.Close();
                var f2 = new Form2();
                f2.Show();
            }

            try
            {
                SqlCommand sqlCommand = new SqlCommand
                (
                    "INSERT INTO tblMeter " +
                    "   (txtMeterNumber, txtMeterAddres,  txtMeterOwner, datMeterBegin, txtMeterBeginValue, intMeterControlCount, fltMeterSum) " +
                    "VALUES " +
                    "   (@txtMeterNumber, @txtMeterAddres, @txtMeterOwner, @datMeterBegin, @txtMeterBeginValue, @intMeterControlCount, @fltMeterSum)", 
                    connection
                );
                sqlCommand.Parameters.AddWithValue("txtMeterNumber", textBox7.Text);
                sqlCommand.Parameters.AddWithValue("txtMeterAddres", textBox1.Text);
                sqlCommand.Parameters.AddWithValue("txtMeterOwner", textBox2.Text);
                sqlCommand.Parameters.AddWithValue("datMeterBegin", dateTimePicker2.Value);
                sqlCommand.Parameters.AddWithValue("txtMeterBeginValue", textBox6.Text);
                sqlCommand.Parameters.AddWithValue("intMeterControlCount", int.Parse(textBox3.Text));
                sqlCommand.Parameters.AddWithValue("fltMeterSum", decimal.Parse(textBox4.Text));
                sqlCommand.ExecuteNonQuery();

                connection.Close();
                Close();

                var f1 = new Form1();
                f1.Update();
                f1.ShowDialog();               
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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
