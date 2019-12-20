using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace football
{
    public partial class EventPayment : UserControl
    {
        //initializing the sql connection string
        SqlConnection conn = new SqlConnection("Data Source=DESKTOP-MR6JCUA\\MSSQLSERVERDEV;Initial Catalog=Futsal_Management_System;Persist Security Info=True;User ID=sa;Password=test");

        //calling a instance
        private static EventPayment _instance;

        public static EventPayment Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new EventPayment();
                return _instance;
            }
        }

        public EventPayment()
        {
            InitializeComponent();
        }

        private void EventPayment_Load(object sender, EventArgs e)
        {
            //making the connection property ready.
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();

            display();  //calling the display function

            fillcombo();//calling the autofill function
        }

        public void display()
        {
            //displaying the event payment info in datagridview
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from EventPayment order by id desc";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter ds = new SqlDataAdapter(cmd);
            ds.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select id,EventNm as EventName,Amount,Date,Description from EventPayment order by id desc";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter ds = new SqlDataAdapter(cmd);
            ds.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                comboBox1.Text = dr["EventId"].ToString();
                textBox1.Text = dr["EventName"].ToString();
                textBox3.Text = dr["Amount"].ToString();
                richTextBox1.Text = dr["Description"].ToString();
                if(string.IsNullOrEmpty(textBox3.Text))
                {
                    label6.Visible = true;
                }
                else
                {
                    label5.Visible = true;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ///////
            ID newid = new ID();
            string aaa = newid.creatid("AdBookingDetails", "id", "FUT", 4);
            //MessageBox.Show(aaa);
            
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Update EventPayment set Amount = '" + textBox3.Text + "', PaymentDate = '" + DateTime.Today + "', Description = '" + richTextBox1.Text + "' where EventId = '" + comboBox1.Text + "'";
            cmd.ExecuteNonQuery();

            display();
        }

        void fillcombo()
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select id from EventBooking order by id desc";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter ds = new SqlDataAdapter(cmd);
            ds.Fill(dt);
            foreach(DataRow dr in dt.Rows)
            {
                comboBox1.Text = dr["id"].ToString();
            }
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from EventBooking where id='"+ comboBox1.Text + "'";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter ds = new SqlDataAdapter(cmd);
            ds.Fill(dt);
            foreach(DataRow dr in dt.Rows)
            {
                textBox1.Text = dr["EventName"].ToString();
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsDigit(ch) & ch != 8 & ch != 46)
            {
                e.Handled = true;
            }
        }
    }
}
