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
    public partial class Team_Registration : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=DESKTOP-MR6JCUA\\MSSQLSERVERDEV;Initial Catalog=Futsal_Management_System;Persist Security Info=True;User ID=sa;Password=test");
        public Team_Registration()
        {
            InitializeComponent();
        }

        //String to get the gender.
        String Gender;

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Gender = "Female";
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Gender = "Male";
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Insert into team values('" + bunifuMaterialTextbox1.Text + "','" + bunifuMaterialTextbox2.Text + "','" + bunifuMaterialTextbox3.Text + "','" + bunifuMaterialTextbox4.Text + "','" + Gender.ToString()
                + "','" + bunifuMaterialTextbox7.Text + "','" + bunifuMaterialTextbox8.Text + "','" + bunifuMaterialTextbox9.Text + "','" + bunifuMaterialTextbox10.Text + "','" + bunifuMaterialTextbox11.Text + "','" + bunifuMaterialTextbox12.Text + "','" + bunifuMaterialTextbox13.Text + "','" + bunifuMaterialTextbox14.Text + "','" + bunifuMaterialTextbox15.Text + "','" + bunifuMaterialTextbox16.Text
                + "','" + bunifuMaterialTextbox18.Text + "','" + bunifuMaterialTextbox19.Text + "','" + bunifuMaterialTextbox20.Text + "','" + bunifuMaterialTextbox21.Text + "','" + bunifuMaterialTextbox22.Text + "','" + bunifuMaterialTextbox23.Text + "','" + bunifuMaterialTextbox24.Text + "','" + bunifuMaterialTextbox25.Text + "','" + bunifuMaterialTextbox26.Text + "','" + bunifuMaterialTextbox27.Text
                + "','" + bunifuMaterialTextbox29.Text + "','" + bunifuMaterialTextbox30.Text + "','" + bunifuMaterialTextbox31.Text + "','" + bunifuMaterialTextbox32.Text + "','" + bunifuMaterialTextbox33.Text + "','" + bunifuMaterialTextbox34.Text + "','" + bunifuMaterialTextbox35.Text + "','" + bunifuMaterialTextbox36.Text + "','" + bunifuMaterialTextbox37.Text + "','" + bunifuMaterialTextbox38.Text
                + "','" + bunifuMaterialTextbox40.Text + "','" + bunifuMaterialTextbox41.Text + "','" + bunifuMaterialTextbox42.Text + "','" + bunifuMaterialTextbox43.Text + "','" + bunifuMaterialTextbox44.Text + "','" + bunifuMaterialTextbox45.Text + "','" + bunifuMaterialTextbox46.Text + "','" + bunifuMaterialTextbox47.Text + "','" + bunifuMaterialTextbox48.Text + "','" + bunifuMaterialTextbox49.Text + "')";
            cmd.ExecuteNonQuery();
            MessageBox.Show("Team added successfully..");
        }

        private void Team_Registration_Load(object sender, EventArgs e)
        {
            if(conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();
        }

        private void bunifuMaterialTextbox49_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsDigit(ch) & ch != 8 & ch != 46)
            {
                e.Handled = true;
            }
        }

        private void bunifuMaterialTextbox38_Validating(object sender, CancelEventArgs e)
        {
            
            System.Text.RegularExpressions.Regex rEmail = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z][\w\.-]{2,28}[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-z][a-zA-Z\.]*[a-zA-Z]$");
            if (bunifuMaterialTextbox29.Text.Length > 0)
            {
                if (!rEmail.IsMatch(bunifuMaterialTextbox29.Text))
                {
                    MessageBox.Show("invalid Email Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    bunifuMaterialTextbox29.Select();
                    e.Cancel = true;
                }
            }
            if (bunifuMaterialTextbox30.Text.Length > 0)
            {
                if (!rEmail.IsMatch(bunifuMaterialTextbox30.Text))
                {
                    MessageBox.Show("invalid Email Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    bunifuMaterialTextbox30.Select();
                    e.Cancel = true;
                }
            }
            if (bunifuMaterialTextbox31.Text.Length > 0)
            {
                if (!rEmail.IsMatch(bunifuMaterialTextbox31.Text))
                {
                    MessageBox.Show("invalid Email Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    bunifuMaterialTextbox31.Select();
                    e.Cancel = true;
                }
            }
            if (bunifuMaterialTextbox32.Text.Length > 0)
            {
                if (!rEmail.IsMatch(bunifuMaterialTextbox32.Text))
                {
                    MessageBox.Show("invalid Email Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    bunifuMaterialTextbox32.Select();
                    e.Cancel = true;
                }
            }
            if (bunifuMaterialTextbox32.Text.Length > 0)
            {
                if (!rEmail.IsMatch(bunifuMaterialTextbox32.Text))
                {
                    MessageBox.Show("invalid Email Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    bunifuMaterialTextbox32.Select();
                    e.Cancel = true;
                }
            }
            if (bunifuMaterialTextbox33.Text.Length > 0)
            {
                if (!rEmail.IsMatch(bunifuMaterialTextbox33.Text))
                {
                    MessageBox.Show("invalid Email Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    bunifuMaterialTextbox33.Select();
                    e.Cancel = true;
                }
            }
            if (bunifuMaterialTextbox34.Text.Length > 0)
            {
                if (!rEmail.IsMatch(bunifuMaterialTextbox34.Text))
                {
                    MessageBox.Show("invalid Email Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    bunifuMaterialTextbox34.Select();
                    e.Cancel = true;
                }
            }
            if (bunifuMaterialTextbox35.Text.Length > 0)
            {
                if (!rEmail.IsMatch(bunifuMaterialTextbox35.Text))
                {
                    MessageBox.Show("invalid Email Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    bunifuMaterialTextbox35.Select();
                    e.Cancel = true;
                }
            }
            if (bunifuMaterialTextbox36.Text.Length > 0)
            {
                if (!rEmail.IsMatch(bunifuMaterialTextbox36.Text))
                {
                    MessageBox.Show("invalid Email Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    bunifuMaterialTextbox36.Select();
                    e.Cancel = true;
                }
            }
            if (bunifuMaterialTextbox37.Text.Length > 0)
            {
                if (!rEmail.IsMatch(bunifuMaterialTextbox37.Text))
                {
                    MessageBox.Show("invalid Email Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    bunifuMaterialTextbox37.Select();
                    e.Cancel = true;
                }
            }
            if (bunifuMaterialTextbox38.Text.Length > 0)
            {
                if (!rEmail.IsMatch(bunifuMaterialTextbox38.Text))
                {
                    MessageBox.Show("invalid Email Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    bunifuMaterialTextbox38.Select();
                    e.Cancel = true;
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
