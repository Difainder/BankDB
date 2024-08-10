using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace _x_cyeta
{

    public partial class Form1 : Form
    {
        public string Name1;
        private SqlConnection connection;
        public void SqlCon()
        {
            dataGridView1.Rows.Clear();
            string ConnectionString = @"Data Source=192.168.112.103;Initial Catalog=db22203;User ID=User003;Password=User003%]40;Integrated Security=False;MultipleActiveResultSets=True;";
            connection = new SqlConnection(ConnectionString);
 
            try
            {
                connection.Open();
                SqlDataReader reader = null;
                SqlCommand command = new SqlCommand("select intMeterId, txtMeterNumber, txtMeterAddres, txtMeterOwner, fltMeterSum" +
                    "                               from tblMeter"
                                                , connection);
                reader = command.ExecuteReader();

            if (reader.HasRows == true)
                while (reader.Read())
                {
                        //Добавляем строку, указывая значения колонок по очереди слева направо
                        dataGridView1.Rows.Add
                        (
                            reader["intMeterId"].ToString(), 
                            reader["txtMeterNumber"].ToString(),
                            reader["txtMeterAddres"].ToString(), 
                            reader["txtMeterOwner"].ToString(),
                            reader["fltMeterSum"].ToString()
                        );
                }

                dataGridView1.Columns[0].Visible = false;
                dataGridView1.AutoResizeColumns();
                reader.Close();
            }
 
            catch (Exception a)
            {
                Console.WriteLine(a.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        
        public Form1()
        {
            InitializeComponent();
            SqlCon();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var f2 = new Form2();
            f2.Show();
            this.Close();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2 && dataGridView1.Rows.Count > 0 && e.RowIndex >= 0)
            {
                var address = dataGridView1.Rows[e.RowIndex].Cells["Column2"].FormattedValue.ToString();
                var f3 = new Form3(connection, address);
                this.Close();
                f3.Show();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
    }
}