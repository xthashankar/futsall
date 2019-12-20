using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace football
{
    public partial class Keys : Form
    {

        //initializing the sql connection string
        SqlConnection conn = new SqlConnection("Data Source=DESKTOP-MR6JCUA\\MSSQLSERVERDEV;Initial Catalog=Futsal_Management_System;Persist Security Info=True;User ID=sa;Password=test");

        public Keys()
        {
            InitializeComponent();
        }

        //Save Btn
        private void button1_Click(object sender, EventArgs e)
        {
            ///////
            ID newid = new ID();
            string aaa = newid.creatid("KeyAmount", "id", "AMT", 4);
            //MessageBox.Show(aaa);

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "INSERT INTO KeyAmount(id,fiveAMorning,[fiveAAfterNoon],[fiveAEvening],[SevenAMorning],[SevenAAfterNoon],[SevenAEvening],[SaturdayFiveAside],[SaturdaySevenAside]) values ('" + aaa + "','" + textBox1.Text + "','"+ textBox2.Text + "','"+ textBox3.Text + "','" + textBox4.Text + "','" + textBox5.Text + "','" + textBox6.Text + "','" + textBox7.Text + "','" + textBox8.Text + "')";
            cmd.ExecuteNonQuery();

            MessageBox.Show("Saved..");
        }

        //Close Btn
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Keys_Load(object sender, EventArgs e)
        {
            if(conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();
        }
    }
}
