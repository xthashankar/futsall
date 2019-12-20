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
    public partial class View_Event : UserControl
    {
        //initializing the sql connection string
        SqlConnection conn = new SqlConnection("Data Source=DESKTOP-MR6JCUA\\MSSQLSERVERDEV;Initial Catalog=Futsal_Management_System;Persist Security Info=True;User ID=sa;Password=test");


        //calling a instance
        private static View_Event _instance;

        public static View_Event Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new View_Event();
                return _instance;
            }
        }
        public View_Event()
        {
            InitializeComponent();
        }

        private void View_Event_Load(object sender, EventArgs e)
        {
            //making the connection property ready.
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();

            //loading the event records in the database         id,BookingMode,PersonName,Mobile,Time,Date1,Date2,Date3,Date4
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM EventBooking order by id desc";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter ds = new SqlDataAdapter(cmd);
            ds.Fill(dt) ;
            dataGridView1.DataSource = dt;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(textBox1.Text== "Search here...")
            {
                textBox1.Text = "";

                this.ForeColor = Color.Black;
            }
            else if(string.IsNullOrEmpty(textBox1.Text))
            {
                this.ForeColor = Color.Gray;
                textBox1.Text = "Search here...";
            }
        }
    }
}
