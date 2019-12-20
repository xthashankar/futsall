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


namespace football
{
    public partial class Login : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=DESKTOP-MR6JCUA\\MSSQLSERVERDEV;Initial Catalog=Futsal_Management_System;Persist Security Info=True;User ID=sa;Password=test");

        public Login()
        {
            InitializeComponent();        
        }

        private void Login_Load(object sender, EventArgs e)
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();
            

        }

        private void Checkbox1_OnChange(object sender, EventArgs e)
        {
            if (Checkbox1.Checked)
            {
                txtpass.isPassword = false;
            }
            else
            {
                txtpass.isPassword = true;
            }
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            if (txtuser.Text == "admin")
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select * from AdminTable where AdminName='" + txtuser.Text + "' and Password= '" + txtpass.Text + "' ";
                cmd.CommandType = CommandType.Text;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    { 
                        this.Hide();
                        (new main_form()).ShowDialog();
                        this.Close();
                    }
                    else { MessageBox.Show("invalid username or password"); }

                }
            }
            else
            {
                SqlCommand cmd1 = conn.CreateCommand();
                cmd1.CommandText = "select * from UserTable where Username='" + txtuser.Text + "' and Password= '" + txtpass.Text + "' ";
                cmd1.CommandType = CommandType.Text;
                using (SqlDataReader reader = cmd1.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        this.Hide();
                        (new main_form()).ShowDialog();
                        this.Close();
                        //Form1 frm = new Form1();
                        //frm.ShowDialog();
                    }
                    else { MessageBox.Show("invalid username or password"); }

                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Login_Paint(object sender, PaintEventArgs e)
        {
            txtpass.isPassword = true;
        }
    }
}

           