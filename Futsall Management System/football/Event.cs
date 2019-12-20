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
    public partial class Event : UserControl
    {
        //initializing the sql connection string
        SqlConnection conn = new SqlConnection("Data Source=DESKTOP-MR6JCUA\\MSSQLSERVERDEV;Initial Catalog=Futsal_Management_System;Persist Security Info=True;User ID=sa;Password=test");


        //calling a instance
        private static Event _instance;

        public static Event Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Event();
                return _instance;
            }
        }
        public Event()
        {
            InitializeComponent();
        }

        //making the variable to get the radio button value
        string modeBooking;
        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            modeBooking = "Phone/Mobile";
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            modeBooking = "On The Spot";
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            modeBooking = "Facebook/Message";
        }

        //clearing all text fields
        private void button14_Click(object sender, EventArgs e)
        { 
            EEvNm.Clear();
            ENm.Clear();
            EComp.Clear();
            EMob.Clear();
            EAdd.Clear();
            EGrdNm.SelectedIndex = -1;
            ETm.SelectedIndex = -1;
            Enoofdays.SelectedIndex = -1;
            ErefPrsn.Clear();
            ERem.Clear();
            Efed.Clear();

            dateTimePicker5.Visible = false;
            label145.Visible = false;
            dateTimePicker6.Visible = false;
            label150.Visible = false;
            label151.Visible = false;
            label152.Visible = false;
            dateTimePicker7.Visible = false;
            dateTimePicker8.Visible = false;
            label1.Visible = false;
            dateTimePicker1.Visible = false;
            label2.Visible = false;
            dateTimePicker2.Visible = false;
            label3.Visible = false;
            dateTimePicker3.Visible = false;
        }

        //checking if the selected no of the days of event
        private void Enoofdays_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Enoofdays.SelectedIndex == 0)
            {
                dateTimePicker5.Visible = true;
                label145.Visible = true;
                dateTimePicker6.Visible = false;
                label150.Visible = false;
                label151.Visible = false;
                label152.Visible = false;
                dateTimePicker7.Visible = false;
                dateTimePicker8.Visible = false;
            }
            if (Enoofdays.SelectedIndex == 1)
            {
                dateTimePicker5.Visible = true;
                label145.Visible = true;
                dateTimePicker6.Visible = true;
                label150.Visible = true;
                label151.Visible = false;
                label152.Visible = false;
                dateTimePicker7.Visible = false;
                dateTimePicker8.Visible = false;
            }
            if (Enoofdays.SelectedIndex == 2)
            {
                dateTimePicker5.Visible = true;
                label145.Visible = true;
                dateTimePicker6.Visible = true;
                label150.Visible = true;
                label151.Visible = true;
                label152.Visible = false;
                dateTimePicker7.Visible = true;
                dateTimePicker8.Visible = false;
            }
            if (Enoofdays.SelectedIndex == 3)
            {
                dateTimePicker5.Visible = true;
                label145.Visible = true;
                dateTimePicker6.Visible = true;
                label150.Visible = true;
                label151.Visible = true;
                label152.Visible = true;
                dateTimePicker7.Visible = true;
                dateTimePicker8.Visible = true;
            }
            if (Enoofdays.SelectedIndex == 3)
            {
                dateTimePicker5.Visible = true;
                label145.Visible = true;
                dateTimePicker6.Visible = true;
                label150.Visible = true;
                label151.Visible = true;
                label152.Visible = true;
                dateTimePicker7.Visible = true;
                dateTimePicker8.Visible = true;
            }
            if (Enoofdays.SelectedIndex == 4)
            {
                dateTimePicker5.Visible = true;
                label145.Visible = true;
                dateTimePicker6.Visible = true;
                label150.Visible = true;
                label151.Visible = true;
                label152.Visible = true;
                dateTimePicker7.Visible = true;
                dateTimePicker8.Visible = true;
                label1.Visible = true;
                dateTimePicker1.Visible = true;
            }
            if (Enoofdays.SelectedIndex == 5)
            {
                dateTimePicker5.Visible = true;
                label145.Visible = true;
                dateTimePicker6.Visible = true;
                label150.Visible = true;
                label151.Visible = true;
                label152.Visible = true;
                dateTimePicker7.Visible = true;
                dateTimePicker8.Visible = true;
                label1.Visible = true;
                dateTimePicker1.Visible = true;
                label2.Visible = true;
                dateTimePicker2.Visible = true;
            }
            if (Enoofdays.SelectedIndex == 6)
            {
                dateTimePicker5.Visible = true;
                label145.Visible = true;
                dateTimePicker6.Visible = true;
                label150.Visible = true;
                label151.Visible = true;
                label152.Visible = true;
                dateTimePicker7.Visible = true;
                dateTimePicker8.Visible = true;
                label1.Visible = true;
                dateTimePicker3.Visible = true;
                label3.Visible = true;
                dateTimePicker1.Visible = true;
                label2.Visible = true;
                dateTimePicker2.Visible = true;
            }
            if (Enoofdays.SelectedIndex == 7)
            {
                dateTimePicker5.Visible = true;
                label145.Visible = true;
                dateTimePicker6.Visible = true;
                label150.Visible = true;
                label151.Visible = true;
                label152.Visible = true;
                dateTimePicker7.Visible = true;
                dateTimePicker8.Visible = true;
                label1.Visible = true;
                dateTimePicker3.Visible = true;
                label3.Visible = true;
                dateTimePicker1.Visible = true;
                label2.Visible = true;
                dateTimePicker2.Visible = true;
                label4.Visible = true;
                dateTimePicker4.Visible = true;
            }
        }

        private void EBook_Click(object sender, EventArgs e)
        {
            int count;

            ///////
            ID newid = new ID();
            string aaa = newid.creatid("EventBooking", "id", "EVF", 4);
            //MessageBox.Show(aaa);

            /////
            string aaab = newid.creatid("EventPayment", "id", "EVP", 4);
            //MessageBox.Show(aaa);

            if (ENm.Text == "" || EMob.Text == "" || EAdd.Text == "" || ETm.Text == "" )
            {
                MessageBox.Show("Select the Booking Mode first!!!/Empty fields here..");
            }

            else
            {
                if (MessageBox.Show("Do you want to Book this data?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if(ERem.Text== "Comment...")
                    {
                        ERem.Text = null;
                    }

                    if (dateTimePicker5.Value == DateTime.Today)
                    {
                        dateTimePicker5.Text = null;
                    }

                    else if (dateTimePicker6.Value == DateTime.Today)
                    {
                        dateTimePicker6.Text = null;
                    }
                    else if (dateTimePicker7.Value == DateTime.Today)
                    {
                        dateTimePicker7.Text = null;
                    }
                    else if (dateTimePicker8.Value == DateTime.Today)
                    {
                        dateTimePicker8.Text = null;
                    }
                    else if (dateTimePicker3.Value == DateTime.Today)
                    {
                        dateTimePicker3.Text = null;
                    }
                    else if (dateTimePicker1.Value == DateTime.Today)
                    {
                        dateTimePicker1.Text = null;
                    }
                    else if (dateTimePicker2.Value == DateTime.Today)
                    {
                        dateTimePicker2.Text = null;
                    }
                    else if (dateTimePicker4.Value == DateTime.Today)
                    {
                        dateTimePicker4.Text = null;
                    }
                    

                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Select * from EventBooking where EventName='" + EEvNm.Text + "' and Time ='" + ETm.Text + "' and GroundName = '" + EGrdNm.SelectedItem.ToString() + "'";
                    cmd.ExecuteNonQuery();
                    DataTable dt1 = new DataTable();
                    SqlDataAdapter ds1 = new SqlDataAdapter(cmd);
                    ds1.Fill(dt1);
                    count = Convert.ToInt32(dt1.Rows.Count.ToString());
                    if (count == 0)
                    {
                        SqlCommand cmd1 = conn.CreateCommand();
                        cmd1.CommandType = CommandType.Text;//BookingMode,(BookingMode,PersonName,Company, Mobile, Address, Date,Time,GroundName,Remarks,RefrencePerson,Feedback)
                        cmd1.CommandText = "INSERT into EventBooking Values('" + aaa + "','" + modeBooking.ToString() + "','" + EEvNm.Text + "','" + ENm.Text + "','" + EComp.Text + "', '" + EMob.Text + "', '" + EAdd.Text + "', '" + Enoofdays.Text + "','" + dateTimePicker5.Text + "','" + dateTimePicker6.Text + "','" + dateTimePicker7.Text + "','" + dateTimePicker8.Text + "','" + dateTimePicker1.Text + "','" + dateTimePicker2.Text + "','" + dateTimePicker3.Text + "','" + dateTimePicker4.Text + "','" + ETm.Text + "', '" + EGrdNm.Text + "','" + ERem.Text + "','" + ErefPrsn.Text + "','" + Efed.Text + "','UNPAID')";
                        cmd1.ExecuteNonQuery();
                        MessageBox.Show("Booked...!!");


                        SqlCommand cmd2 = conn.CreateCommand();
                        cmd2.CommandType = CommandType.Text;
                        cmd2.CommandText = "INSERT into EventPayment(id,EventId,EventDate) Values('" + aaab + "','" + ENm.Text + "','" + aaa + "','" + DateTime.Today + "')";
                        cmd2.ExecuteNonQuery();
                    }
                    else
                    {
                        MessageBox.Show("Game already Booked in this Time");
                    }
                }
            }
        }

        private void ERem_KeyDown(object sender, KeyEventArgs e)
        {
            if(ERem.Text== "Comment...")
            {
                ERem.Text = "";

                ERem.ForeColor = Color.Black;
            }
            else if (string.IsNullOrEmpty(ERem.Text))
            {
                ERem.Text = "Comment...";

                ERem.ForeColor = Color.Gray;
            }
        }

        private void EMob_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsDigit(ch) & ch != 8 & ch != 46)
            {
                e.Handled = true;
            }
        }

        private void ENm_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsLetter(ch) & !char.IsWhiteSpace(ch)  & ch != 8 & ch != 46)
            {
                e.Handled = true;
            }
        }

        private void Event_Load(object sender, EventArgs e)
        {
            if(conn.State==ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();
        }
    }
}
