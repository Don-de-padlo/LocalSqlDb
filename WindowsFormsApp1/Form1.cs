using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection= null;
        private SqlConnection northwndSqlConnection = null;


        private List<string[]> rows;
        public Form1()
        {
            InitializeComponent();
        }
        
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand( $"INSERT INTO [Table](Name , Surname, Login , Password) VALUES ('sdfgsdfg', 'sdfg' , 'fgfg', 'sdfvs') " , sqlConnection);
            MessageBox.Show(command.ExecuteNonQuery().ToString());

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
            sqlConnection.Open();
            northwndSqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["NorthwindDB"].ConnectionString);
            northwndSqlConnection.Open();
            SqlDataAdapter sortDataAdapter = new SqlDataAdapter("SELECT * FROM Products", northwndSqlConnection);
            DataSet db = new DataSet();
            sortDataAdapter.Fill(db);
            dataGridView2.DataSource = db.Tables[0];



            string row = null;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //SqlCommand command = new SqlCommand($"INSERT INTO [Table](Name , Surname, Login , Password ) VALUES ('{textBox1.Text}', '{textBox2.Text}' , '{textBox3.Text}', '{textBox4.Text}') ", sqlConnection);
            SqlCommand command = new SqlCommand($"INSERT INTO [Table](Name , Surname, Login , Password , Phone) VALUES (@Name , @Surname, @Login , @Password , @Phone) ", sqlConnection);
            command.Parameters.AddWithValue("Name", textBox1.Text);
            command.Parameters.AddWithValue("Surname", textBox2.Text);
            command.Parameters.AddWithValue("Login", textBox3.Text);
            command.Parameters.AddWithValue("Password", textBox4.Text);
            command.Parameters.AddWithValue("Phone", textBox5.Text);

            MessageBox.Show(command.ExecuteNonQuery().ToString());
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter(
                textBox7.Text, northwndSqlConnection);
            //"SELECT * FROM Products WHERE UnitPrice > 100", northwndSqlConnection);

            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            dataGridView1.DataSource = dataSet.Tables[0];
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            SqlDataReader dataReader = null;
            try
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT ProductName, QuantityPerUnit, UnitPrice FROM Products" , 
                    northwndSqlConnection);
                dataReader = sqlCommand.ExecuteReader();
                ListViewItem item = null;

                while (dataReader.Read())
                {
                    item = new ListViewItem( new string[] { Convert.ToString(dataReader["ProductName"]) , 
                        Convert.ToString(dataReader["QuantityPerUnit"]), 
                        Convert.ToString(dataReader["UnitPrice"]) });
                    listView1.Items.Add(item);
                    
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { 
                if (dataReader != null && !dataReader.IsClosed)
                {
                    dataReader.Close();
                }
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"ProductName LIKE '%{textBox8.Text}%'";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"UnitsInStock <= 10";
                    break;
                case 1:
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"UnitsInStock >= 10 AND UnitsInStock <= 50" ;
                    break;
                case 2:
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"UnitsInStock >= 50";
                    break;
                case 3:
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = "";
                    break;
            }
        }
    }
}
