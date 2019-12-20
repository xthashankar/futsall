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
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.IO;

namespace football
{
    public partial class main_form : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=DESKTOP-MR6JCUA\\MSSQLSERVERDEV;Initial Catalog=Futsal_Management_System;Persist Security Info=True;User ID=sa;Password=test");


        public main_form()
        {
            InitializeComponent();

            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();
        }


        private void header_Paint(object sender, PaintEventArgs e)
        {
            System.Timers.Timer timer = new System.Timers.Timer();          //used timer
            timer.Interval = 1000;                                           //time interval
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Invoke(new MethodInvoker(delegate ()
            {
                lblStatus.Text = DateTime.Now.ToString("T");             //for time
                lblDate.Text = DateTime.Now.ToString("d");               //for Date
                lblDay.Text = DateTime.Now.ToString("dddd");            //for Day
            }));
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            lblStatus.Text = DateTime.Now.ToString("T");                   //for time
            lblDate.Text = DateTime.Now.ToString("d");                     //for Date
            lblDay.Text = DateTime.Now.ToString("dddd");                   //for Day

        }

        //Timer stopwatch
        int timeSec, timeMin,timehr;
        bool isactive;

        private void main_form_Load_1(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'futsal_Management_SystemDataSet2.CoachDetails' table. You can move, or remove it, as needed.
            this.coachDetailsTableAdapter.Fill(this.futsal_Management_SystemDataSet2.CoachDetails);
            // TODO: This line of code loads data into the 'futsal_Management_SystemDataSet.TeamEntry' table. You can move, or remove it, as needed.
            this.teamEntryTableAdapter.Fill(this.futsal_Management_SystemDataSet.TeamEntry);

            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();

            ResetTime();

            isactive = false;

            panel27.Visible = true;

            displaydata1();
            displaydata2();
            displaybooked_info_for_payment();
            displayGamestatus();
            displaydata4();
            displaydata5();
            displaydata6();
            AutoComplete_Text();
            autofilltext();
            autofilltext1();
            autofillatext1();
            diss();
            ddddd();//expenses
            ddddda();//investment
            disssps();
            dispas();
            dispas1();
            dssss();//displaying the Count of the same name
            teamdis();//Team display
            AutoCompletea_Text();
            displayCredentials();//Key Amount Display

            ID newid = new ID();
            string aaa = newid.creatid("TeamEntry", "id", "FTM", 4);
            //MessageBox.Show(aaa);
            TeamID.Text = aaa;

        }

        //Starting the Stopwatch
        private void StartBtn_Click(object sender, EventArgs e)
        {
            timer1.Start();
            System.Timers.Timer timer = new System.Timers.Timer();          //used timer
            timer.Interval = 1000;                                           //time interval
            timer.Elapsed += Timer_Elapsed;
            isactive = true;
        }

        //Stopping the Current running Stop watch
        private void StopBtn_Click(object sender, EventArgs e)
        {
            isactive = false;
        }

        //Reset Button of the stopwatch
        private void ResetBtn_Click(object sender, EventArgs e)
        {
            isactive = false;

            ResetTime();
        }

        //Function that reset the stop watch Time
        private void ResetTime()
        {
            timeSec = 0;
            timeMin = 0;
            timehr = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (isactive)
            {
              if(timeSec > 59)
               {
                    timeMin++;
                    timeSec = 0;
                    if(timeMin > 59)
                    {
                        timehr++;
                        timeMin = 0;
                    }
               }
                
            }
            DrawTime();
        }

        private void DrawTime()
        {
            lblsec.Text = string.Format("{0:00}", timeSec);
            lblmin.Text = string.Format("{0:00}", timeMin);
            lblhr.Text = string.Format("{0:00}", timehr);
        }

        //Dashboard
        private void MenuBtn_Click(object sender, EventArgs e)
        {
            NBooking.SelectedTab = Home;
            panel25.Height = MenuBtn.Height;
            panel25.Top = MenuBtn.Top;
            panel27.Visible = true;
        }

        //close
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Booking
        private void button4_Click(object sender, EventArgs e)
        {
            NBooking.SelectedTab = BHome;
            panel25.Height = button4.Height;
            panel25.Top = button4.Top;
            panel27.Visible = false;
        }

        //payment
        private void button5_Click(object sender, EventArgs e)
        {
            NBooking.SelectedTab = payment;
            panel25.Height = button5.Height;
            panel25.Top = button5.Top;
            panel27.Visible = false;
        }

        //Games 
        private void button6_Click(object sender, EventArgs e)
        {
            NBooking.SelectedTab = games;
            panel25.Height = button6.Height;
            panel25.Top = button6.Top;
            panel27.Visible = false;
        }

        //Expenses and investment
        private void button2_Click(object sender, EventArgs e)
        {
            NBooking.SelectedTab = Expenses;
            panel25.Height = button2.Height;
            panel25.Top = button2.Top;
            panel27.Visible = false;
        }

        //Customer
        private void button7_Click(object sender, EventArgs e)
        {
            NBooking.SelectedTab = Customer;
            panel25.Height = button7.Height;
            panel25.Top = button7.Top;
            panel27.Visible = false;
        }

        //Edit customer
        private void button8_Click(object sender, EventArgs e)
        {
            NBooking.SelectedTab = edit_customer;
        }

        //View Customer
        private void viewplayer_Click(object sender, EventArgs e)
        {
            NBooking.SelectedTab = edit_customer;
        }

        //Payment info
        private void button11_Click(object sender, EventArgs e)
        {
            NBooking.SelectedTab = payment_detail;
        }

        //coach info
        private void AddCoach_Click(object sender, EventArgs e)
        {
            NBooking.SelectedTab = coach;
            panel25.Height = AddCoach.Height;
            panel25.Top = AddCoach.Top;
            panel27.Visible = false;
        }

        //refree info
        private void Addref_Click(object sender, EventArgs e)
        {
            NBooking.SelectedTab = refree;
            panel25.Height = Addref.Height;
            panel25.Top = Addref.Top;
            panel27.Visible = false;
        }

        //change the picture
        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "jpeg files (*.jpg)|*.jpg|png files (*.png)|*.png|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            // Insert code to read the stream here.
                            pictureBox1.Image = new Bitmap(openFileDialog1.FileName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        // Backup and Restore database
        private void BckupRestBtn_Click(object sender, EventArgs e)
        {
            NBooking.SelectedTab = setting;
            panel25.Height = BckupRestBtn.Height;
            panel25.Top = BckupRestBtn.Top;
            panel27.Visible=false;
        }

        //Displaying the Name of the Person That contains the 10 times of the Period
        void dssss()
        {
            SqlCommand cmdabs = conn.CreateCommand();
            cmdabs.CommandType = CommandType.Text;
            cmdabs.CommandText = " Select PersonName, count(PersonName) as Times from AdBookingDetails group by PersonName order by count(PersonName) desc ";
            cmdabs.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter ds = new SqlDataAdapter(cmdabs);
            ds.Fill(dt);
            dataGridView12.DataSource = dt;

            foreach(DataRow dr in dt.Rows)
            {
                if (Convert.ToInt32(dr["Times"].ToString()) % 10 == 0)
                {
                    label70.Text = dr["PersonName"].ToString();
                    label145.Text = dr["Times"].ToString(); 
                }
            }
        }

        //Displayihng the Upcomming games in the Home Menu
        void dispas()
        {
            SqlCommand cmdbs = conn.CreateCommand();
            cmdbs.CommandType = CommandType.Text;
            cmdbs.CommandText = " Select PersonName,Time,GroundName from AdBookingDetails where Date > '" + DateTime.Today.ToString() + "'";
            cmdbs.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter ds = new SqlDataAdapter(cmdbs);
            ds.Fill(dt);
            dataGridView10.DataSource = dt;
        }

        //displaying the games on today in the Home menu
        void dispas1()
        {
            SqlCommand cmdbs = conn.CreateCommand();
            cmdbs.CommandType = CommandType.Text;
            cmdbs.CommandText = " Select PersonName,Time,GroundName from AdBookingDetails where Date = '" + DateTime.Today.ToString() + "'";
            cmdbs.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter ds = new SqlDataAdapter(cmdbs);
            ds.Fill(dt);
            dataGridView11.DataSource = dt;
        }




        /// <summary>
        /// Booking Informations
        /// </summary>
        /// 



        //NormalBooking
        private void NormalBooking_Click(object sender, EventArgs e)
        {
            NBooking.SelectedTab = Booking;
        }

        //AdvanceBooking
        private void AdvanceBooking_Click(object sender, EventArgs e)
        {
            NBooking.SelectedTab = AdvBooking;
        }

        //AcademyBooking
        private void EventBooking_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Under Development...");
            //NBooking.SelectedTab = EBooking;
        }

        //EventBooking
        private void EvenBooking_Click(object sender, EventArgs e)
        {
            (new EventBoking()).ShowDialog();
        }


        //String to get the booking Mode.
        String BookingMode;

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            BookingMode = "Phone/Mobile";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            BookingMode = "On the Spot";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            BookingMode = "Facebook Message";
        }

        //Making a hint in the remark section
        private void txtRemark_Enter(object sender, EventArgs e)
        {
            if (txtRemark.Text == "Comment")
            {
                txtRemark.Text = "";

                txtRemark.ForeColor = Color.Black;
            }
        }

        //text Auto fill
        public void autofilltext1()
        {

            txtname.Clear();

            txtname.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtname.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection coll2 = new AutoCompleteStringCollection();

            //conn.Open();
            SqlCommand cmd14 = conn.CreateCommand();
            cmd14.CommandType = CommandType.Text;
            cmd14.CommandText = "select * from AdBookingDetails";
            cmd14.ExecuteNonQuery();
            DataTable dt14 = new DataTable();
            SqlDataAdapter ds14 = new SqlDataAdapter(cmd14);
            ds14.Fill(dt14);
            foreach (DataRow dr14 in dt14.Rows)
            {
                string nams1 = dr14["PersonName"].ToString(); //taking all database PlayerName column values and adding them as an item
                coll2.Add(nams1);
            }

            txtname.AutoCompleteCustomSource = coll2;
        }

        //Auto fill the corresponding details of the entered name
        private void txtname_TextChanged(object sender, EventArgs e)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select * from AdBookingDetails where PersonName ='" + txtname.Text + "'";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter ds = new SqlDataAdapter(cmd);
            ds.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                //autofilling the information who has already booked the game
                txtmobile.Text = dr["Mobile"].ToString();
                txtaddress.Text = dr["Address"].ToString();
                txtCompany.Text = dr["Company"].ToString();

            }
        }

        //text Auto fill
        public void autofillatext1()
        {

            AdvNm.Clear();

            AdvNm.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            AdvNm.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection coll2 = new AutoCompleteStringCollection();

            //conn.Open();
            SqlCommand cmd14 = conn.CreateCommand();
            cmd14.CommandType = CommandType.Text;
            cmd14.CommandText = "select * from AdBookingDetails";
            cmd14.ExecuteNonQuery();
            DataTable dt14 = new DataTable();
            SqlDataAdapter ds14 = new SqlDataAdapter(cmd14);
            ds14.Fill(dt14);
            foreach (DataRow dr14 in dt14.Rows)
            {
                string nams1 = dr14["PersonName"].ToString(); //taking all database PlayerName column values and adding them as an item
                coll2.Add(nams1);
            }

            AdvNm.AutoCompleteCustomSource = coll2;
        }

        private void AdvNm_TextChanged(object sender, EventArgs e)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select * from AdBookingDetails where PersonName ='" + AdvNm.Text + "'";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter ds = new SqlDataAdapter(cmd);
            ds.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                //autofilling the information who has already booked the game
                AdvMob.Text = dr["Mobile"].ToString();
                AdvComp.Text = dr["Company"].ToString();
                AdvAdd.Text = dr["Address"].ToString();
            }
        }

        //Button Book
        private void butBook_Click(object sender, EventArgs e)
        {
            int count = 0;
            int count1 = 0;
            int count2 = 0;
            ///////
            ID newid = new ID();
            string aaa = newid.creatid("AdBookingDetails", "id", "FUT", 4 );
            //MessageBox.Show(aaa);

            if (txtname.Text == "" && txtmobile.Text == "" && txtaddress.Text == "" && timetext.Text == "" )
            {
                MessageBox.Show("Empty fields here..");
            }

            else if(radioButton1.Checked!=true && radioButton2.Checked!=true && radioButton3.Checked!=true)
            {
                MessageBox.Show("Empty fields here..");
            }

            else
            {
                if (MessageBox.Show("Do you want to Book this data?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (comboboxGr.Text != "7A side")
                    {
                        //Making if the 7a side is booked in that time no booking will be done
                        SqlCommand cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "Select * from AdBookingDetails where Date='" + DateTime.Today + "' and Time ='" + timetext.Text + "' and GroundName = '7A side'";
                        cmd.ExecuteNonQuery();
                        DataTable dt = new DataTable();
                        SqlDataAdapter ds = new SqlDataAdapter(cmd);
                        ds.Fill(dt);
                        count = Convert.ToInt32(dt.Rows.Count.ToString());


                        SqlCommand cmda = conn.CreateCommand();
                        cmda.CommandType = CommandType.Text;
                        cmda.CommandText = "Select * from EventBooking where Date1='" + DateTime.Today + "' or Date2='" + DateTime.Today + "' or Date2='" + DateTime.Today + "' or Date4='" + DateTime.Today + "' or Date5 = '" + DateTime.Today + "' or Date6 = '" + DateTime.Today + "' or Date7 = '" + DateTime.Today + "' or Date8 = '" + DateTime.Today + "' and Time = '" + timetext.Text + "' and GroundName = '7A side'";
                        cmda.ExecuteNonQuery();
                        DataTable dt1a = new DataTable();
                        SqlDataAdapter ds1a = new SqlDataAdapter(cmda);
                        ds1a.Fill(dt1a);
                        count1 = Convert.ToInt32(dt1a.Rows.Count.ToString());

                        if (count == 0 && count1 == 0)
                        {

                            if (comboboxGr.Text != "Ground A(5A side)")
                            {
                                SqlCommand cmdas = conn.CreateCommand();
                                cmdas.CommandType = CommandType.Text;
                                cmdas.CommandText = "Select * from AdBookingDetails where Date='" + DateTime.Today + "' and Time ='" + timetext.Text + "' and GroundName != 'Ground A(5A side)'";
                                cmdas.ExecuteNonQuery();
                                DataTable dt1as = new DataTable();
                                SqlDataAdapter ds1as = new SqlDataAdapter(cmdas);
                                ds1as.Fill(dt1as);
                                count2 = Convert.ToInt32(dt1as.Rows.Count.ToString());

                                if (count2 == 0)
                                {

                                    SqlCommand cmd1 = conn.CreateCommand();
                                    cmd1.CommandType = CommandType.Text;//BookingMode,(BookingMode,PersonName,Company, Mobile, Address, Date,Time,GroundName,Remarks,RefrencePerson,Feedback)
                                    cmd1.CommandText = "INSERT into AdBookingDetails Values('" + aaa + "','" + BookingMode.ToString() + "','" + txtname.Text + "','" + txtCompany.Text + "', '" + txtmobile.Text + "', '" + txtaddress.Text + "', '" + DateTime.Today + "','" + timetext.Text + "', '" + comboboxGr.Text + "','" + txtRemark.Text + "','" + txtRefPrsn.Text + "','" + txtFeedback.Text + "')";
                                    cmd1.ExecuteNonQuery();
                                    MessageBox.Show("Booked...!!");


                                    SqlCommand cmd2 = conn.CreateCommand();
                                    cmd2.CommandType = CommandType.Text;
                                    cmd2.CommandText = "INSERT into PaymentInfo(BookId,status,Date) Values('" + aaa + "','UNPAID','" + DateTime.Today + "')";
                                    cmd2.ExecuteNonQuery();


                                    //clearing the fields after saving the datas
                                    txtname.Clear();
                                    txtCompany.Clear();
                                    txtmobile.Clear();
                                    txtaddress.Clear();
                                    comboboxGr.SelectedIndex = -1;
                                    txtRemark.Clear();
                                    txtRefPrsn.Clear();
                                    txtFeedback.Clear();
                                }
                                else
                                {
                                    MessageBox.Show("Ground A(5A side) Game already Booked in this Time");
                                }
                            }

                            else       //if (comboboxGr.Text == "Ground B(5A side)")
                            {
                                SqlCommand cmdas = conn.CreateCommand();
                                cmdas.CommandType = CommandType.Text;
                                cmdas.CommandText = "Select * from AdBookingDetails where Date='" + DateTime.Today + "' and Time ='" + timetext.Text + "' and GroundName != 'Ground B(5A side)'";
                                cmdas.ExecuteNonQuery();
                                DataTable dt1as = new DataTable();
                                SqlDataAdapter ds1as = new SqlDataAdapter(cmdas);
                                ds1as.Fill(dt1as);
                                count2 = Convert.ToInt32(dt1as.Rows.Count.ToString());

                                if (count2 == 0)
                                {
                                    SqlCommand cmd1 = conn.CreateCommand();
                                    cmd1.CommandType = CommandType.Text;//BookingMode,(BookingMode,PersonName,Company, Mobile, Address, Date,Time,GroundName,Remarks,RefrencePerson,Feedback)
                                    cmd1.CommandText = "INSERT into AdBookingDetails Values('" + aaa + "','" + BookingMode.ToString() + "','" + txtname.Text + "','" + txtCompany.Text + "', '" + txtmobile.Text + "', '" + txtaddress.Text + "', '" + DateTime.Today + "','" + timetext.Text + "', '" + comboboxGr.Text + "','" + txtRemark.Text + "','" + txtRefPrsn.Text + "','" + txtFeedback.Text + "')";
                                    cmd1.ExecuteNonQuery();
                                    MessageBox.Show("Booked...!!");


                                    SqlCommand cmd2 = conn.CreateCommand();
                                    cmd2.CommandType = CommandType.Text;
                                    cmd2.CommandText = "INSERT into PaymentInfo(BookId,status,Date) Values('" + aaa + "','UNPAID','" + DateTime.Today + "')";
                                    cmd2.ExecuteNonQuery();

                                    //clearing the fields after saving the datas
                                    txtname.Clear();
                                    txtCompany.Clear();
                                    txtmobile.Clear();
                                    txtaddress.Clear();
                                    comboboxGr.SelectedIndex = -1;
                                    txtRemark.Clear();
                                    txtRefPrsn.Clear();
                                    txtFeedback.Clear();
                                }
                                else
                                {
                                    MessageBox.Show("Ground A(5A side) Game already Booked in this Time");
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("7A side Game already Booked in this Time");
                        }
                    }
                    else
                    {
                        ///
                        //Making if the 5A side is booked in that time no 7A booking will be done
                        ///
                        SqlCommand cmd3 = conn.CreateCommand();
                        cmd3.CommandType = CommandType.Text;
                        cmd3.CommandText = "Select * from AdBookingDetails where Date='" + DateTime.Today + "' and Time ='" + timetext.Text + "' and GroundName != '7A side'";
                        cmd3.ExecuteNonQuery();
                        DataTable dt1 = new DataTable();
                        SqlDataAdapter ds1 = new SqlDataAdapter(cmd3);
                        ds1.Fill(dt1);
                        count = Convert.ToInt32(dt1.Rows.Count.ToString());


                        SqlCommand cmdb = conn.CreateCommand();
                        cmdb.CommandType = CommandType.Text;
                        cmdb.CommandText = "Select * from EventBooking where Date1='" + DateTime.Today + "' or Date2='" + DateTime.Today + "' or Date2='" + DateTime.Today + "' or Date4='" + DateTime.Today + "' or Date5 = '" + DateTime.Today + "' or Date6 = '" + DateTime.Today + "' or Date7 = '" + DateTime.Today + "' or Date8 = '" + DateTime.Today + "' and Time = '" + timetext.Text + "' and GroundName != '7A side'";
                        cmdb.ExecuteNonQuery();
                        DataTable dt1b = new DataTable();
                        SqlDataAdapter ds1b = new SqlDataAdapter(cmdb);
                        ds1b.Fill(dt1b);
                        count1 = Convert.ToInt32(dt1b.Rows.Count.ToString());

                        if (count == 0 && count1 == 0)
                        {
                            SqlCommand cmd3a = conn.CreateCommand();
                            cmd3a.CommandType = CommandType.Text;
                            cmd3a.CommandText = "Select * from AdBookingDetails where Date='" + DateTime.Today + "' and Time ='" + timetext.Text + "' and GroundName = '7A side'";
                            cmd3a.ExecuteNonQuery();
                            DataTable dta1 = new DataTable();
                            SqlDataAdapter dsa1 = new SqlDataAdapter(cmd3a);
                            dsa1.Fill(dta1);
                            count2 = Convert.ToInt32(dta1.Rows.Count.ToString());
                            if (count2 == 0)
                            {
                                SqlCommand cmd4 = conn.CreateCommand();
                                cmd4.CommandType = CommandType.Text;//BookingMode,(BookingMode,PersonName,Company, Mobile, Address, Date,Time,GroundName,Remarks,RefrencePerson,Feedback)
                                cmd4.CommandText = "INSERT into AdBookingDetails Values('" + aaa + "','" + BookingMode.ToString() + "','" + txtname.Text + "','" + txtCompany.Text + "', '" + txtmobile.Text + "', '" + txtaddress.Text + "', '" + DateTime.Today + "','" + timetext.Text + "', '" + comboboxGr.Text + "','" + txtRemark.Text + "','" + txtRefPrsn.Text + "','" + txtFeedback.Text + "')";
                                cmd4.ExecuteNonQuery();
                                MessageBox.Show("Booked...!!");


                                SqlCommand cmd5 = conn.CreateCommand();
                                cmd5.CommandType = CommandType.Text;
                                cmd5.CommandText = "INSERT into PaymentInfo(BookId,status,Date) Values('" + aaa + "','UNPAID','" + DateTime.Today + "')";
                                cmd5.ExecuteNonQuery();

                                //clearing the fields after saving the datas
                                txtname.Clear();
                                txtCompany.Clear();
                                txtmobile.Clear();
                                txtaddress.Clear();
                                comboboxGr.SelectedIndex = -1;
                                txtRemark.Clear();
                                txtRefPrsn.Clear();
                                txtFeedback.Clear();
                            }
                            else
                            {
                                MessageBox.Show(" 7A Side Game already Booked in this Time");
                            }
                        }
                    }

                }
            }

            displaydata2();
            displaybooked_info_for_payment();
            displayGamestatus();
            dispas();
            dispas1();
            displaydata1();
            dssss();    
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            //clearing all fields
            txtname.Clear();
            txtCompany.Clear();
            txtmobile.Clear();
            txtaddress.Clear();
            comboboxGr.SelectedIndex = -1;
            txtRemark.Text="Comment...";
            txtRefPrsn.Clear();
            txtFeedback.Clear();
        }


        // viewing the booked info 
        private void viewBtn_Click(object sender, EventArgs e)
        {
            NBooking.SelectedTab = BookedRecords;
        }

        private void txtSchBook_TextChanged(object sender, EventArgs e)
        {
            (BookviewDatagrid.DataSource as DataTable).DefaultView.RowFilter = string.Format("GroundName LIKE '{0}%' OR PersonName LIKE '{0}%' OR BookingMode LIKE '{0}%' OR Address LIKE '{0}%'", txtSchBook.Text);
        }

        private void txtSchBook_KeyDown(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(txtSchBook.Text))
            {
                txtRemark.Text = "Type Here...";

                txtRemark.ForeColor = Color.Gray;
            }
            else if (txtRemark.Text == "Type Here...")
            {
                txtRemark.Text = "";

                txtRemark.ForeColor = Color.Black;
            }
        }



        /// <summary>
        /// Advance Booking
        /// </summary>


        private void AdvBookingBtn_Click(object sender, EventArgs e)
        {
            int count = 0;
            int count1 = 0;
            int count2 = 0;
            ///////
            ID newid = new ID();
            string aaa = newid.creatid("AdBookingDetails", "id", "FUT", 4);
            //MessageBox.Show(aaa);

            if (AdvNm   .Text == "" && AdvMob.Text == "" && AdvAdd.Text == "" && AdvTime.Text == "")
            {
                MessageBox.Show("Empty fields here..");
            }

            else if (radioButton9.Checked != true && radioButton8.Checked != true && radioButton7.Checked != true)
            {
                MessageBox.Show("Empty fields here..");
            }

            else
            {
                if (MessageBox.Show("Do you want to Book this data?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (comboboxGr.Text != "7A side")
                    {
                        //Making if the 7a side is booked in that time no booking will be done
                        SqlCommand cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "Select * from AdBookingDetails where Date='" + AdvDate.Value.ToString() + "' and Time ='" + AdvTime.Text + "' and GroundName = '7A side'";
                        cmd.ExecuteNonQuery();
                        DataTable dt = new DataTable();
                        SqlDataAdapter ds = new SqlDataAdapter(cmd);
                        ds.Fill(dt);
                        count = Convert.ToInt32(dt.Rows.Count.ToString());


                        SqlCommand cmda = conn.CreateCommand();
                        cmda.CommandType = CommandType.Text;
                        cmda.CommandText = "Select * from EventBooking where Date1='" + AdvDate.Value.ToString() + "' or Date2='" + AdvDate.Value.ToString() + "' or Date2='" + AdvDate.Value.ToString() + "' or Date4='" + AdvDate.Value.ToString() + "' or Date5 = '" + AdvDate.Value.ToString() + "' or Date6 = '" + AdvDate.Value.ToString() + "' or Date7 = '" + AdvDate.Value.ToString() + "' or Date8 = '" + AdvDate.Value.ToString() + "' and Time = '" + AdvTime.Text + "' and GroundName = '7A side'";
                        cmda.ExecuteNonQuery();
                        DataTable dt1a = new DataTable();
                        SqlDataAdapter ds1a = new SqlDataAdapter(cmda);
                        ds1a.Fill(dt1a);
                        count1 = Convert.ToInt32(dt1a.Rows.Count.ToString());

                        if (count == 0 && count1 == 0)
                        {

                            if (comboboxGr.Text != "Ground A(5A side)")
                            {
                                SqlCommand cmdas = conn.CreateCommand();
                                cmdas.CommandType = CommandType.Text;
                                cmdas.CommandText = "Select * from AdBookingDetails where Date='" + AdvDate.Value.ToString() + "' and Time ='" + AdvTime.Text + "' and GroundName != 'Ground A(5A side)'";
                                cmdas.ExecuteNonQuery();
                                DataTable dt1as = new DataTable();
                                SqlDataAdapter ds1as = new SqlDataAdapter(cmdas);
                                ds1as.Fill(dt1as);
                                count2 = Convert.ToInt32(dt1as.Rows.Count.ToString());

                                if (count2 == 0)
                                {

                                    SqlCommand cmd1 = conn.CreateCommand();
                                    cmd1.CommandType = CommandType.Text;//BookingMode,
                                    cmd1.CommandText = "INSERT into AdBookingDetails Values('" + aaa + "','" + BookingMode.ToString() + "','" + AdvNm.Text + "','" + AdvComp.Text + "', '" + AdvMob.Text + "', '" + AdvAdd.Text + "', '" + AdvDate.Value.ToString() + "','" + AdvTime.Text + "', '" + AdvGround.Text + "','" + AdvRemark.Text + "','" + AdvRefPrsn.Text + "','" + AdvFed.Text + "')";
                                    cmd1.ExecuteNonQuery();
                                    MessageBox.Show("Booked...!!");


                                    SqlCommand cmd2 = conn.CreateCommand();
                                    cmd2.CommandType = CommandType.Text;
                                    cmd2.CommandText = "INSERT into PaymentInfo(BookId,status,Date) Values('" + aaa + "','UNPAID''" + AdvDate.Value.ToString() + "')";
                                    cmd2.ExecuteNonQuery();


                                    //clearing the fields after saving the datas
                                    AdvNm.Clear();
                                    AdvMob.Clear();
                                    AdvAdd.Clear();
                                    AdvComp.Clear();
                                    AdvGround.SelectedIndex = -1;
                                    AdvRemark.Text = "Comment...";
                                    AdvRefPrsn.Clear();
                                    AdvFed.Clear();
                                }
                                else
                                {
                                    MessageBox.Show("Ground A(5A side) Game already Booked in this Time");
                                }
                            }

                            else       //if (comboboxGr.Text == "Ground B(5A side)")
                            {
                                SqlCommand cmdas = conn.CreateCommand();
                                cmdas.CommandType = CommandType.Text;
                                cmdas.CommandText = "Select * from AdBookingDetails where Date='" + AdvDate.Value.ToString() + "' and Time ='" + AdvTime.Text + "' and GroundName != 'Ground B(5A side)'";
                                cmdas.ExecuteNonQuery();
                                DataTable dt1as = new DataTable();
                                SqlDataAdapter ds1as = new SqlDataAdapter(cmdas);
                                ds1as.Fill(dt1as);
                                count2 = Convert.ToInt32(dt1as.Rows.Count.ToString());

                                if (count2 == 0)
                                {
                                    SqlCommand cmd1 = conn.CreateCommand();
                                    cmd1.CommandType = CommandType.Text;//BookingMode,
                                    cmd1.CommandText = "INSERT into AdBookingDetails Values('" + aaa + "','" + BookingMode.ToString() + "','" + AdvNm.Text + "','" + AdvComp.Text + "', '" + AdvMob.Text + "', '" + AdvAdd.Text + "', '" + AdvDate.Value.ToString() + "','" + AdvTime.Text + "', '" + AdvGround.Text + "','" + AdvRemark.Text + "','" + AdvRefPrsn.Text + "','" + AdvFed.Text + "')";
                                    cmd1.ExecuteNonQuery();
                                    MessageBox.Show("Booked...!!");


                                    SqlCommand cmd2 = conn.CreateCommand();
                                    cmd2.CommandType = CommandType.Text;
                                    cmd2.CommandText = "INSERT into PaymentInfo(BookId,status,Date) Values('" + aaa + "','UNPAID','" + AdvDate.Value.ToString() + "')";
                                    cmd2.ExecuteNonQuery();

                                    //clearing the fields after saving the datas
                                    AdvNm.Clear();
                                    AdvMob.Clear();
                                    AdvAdd.Clear();
                                    AdvComp.Clear();
                                    AdvGround.SelectedIndex = -1;
                                    AdvRemark.Text = "Comment...";
                                    AdvRefPrsn.Clear();
                                    AdvFed.Clear();
                                }
                                else
                                {
                                    MessageBox.Show("Ground A(5A side) Game already Booked in this Time");
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("7A side Game already Booked in this Time");
                        }
                    }
                    else
                    {
                        ///
                        //Making if the 5A side is booked in that time no 7A booking will be done
                        ///
                        SqlCommand cmd3 = conn.CreateCommand();
                        cmd3.CommandType = CommandType.Text;
                        cmd3.CommandText = "Select * from AdBookingDetails where Date ='" + AdvDate.Value.ToString() + "' and Time ='" + AdvTime.Text + "' and GroundName != '7A side'";
                        cmd3.ExecuteNonQuery();
                        DataTable dt1 = new DataTable();
                        SqlDataAdapter ds1 = new SqlDataAdapter(cmd3);
                        ds1.Fill(dt1);
                        count = Convert.ToInt32(dt1.Rows.Count.ToString());


                        SqlCommand cmdb = conn.CreateCommand();
                        cmdb.CommandType = CommandType.Text;
                        cmdb.CommandText = "Select * from EventBooking where Date1 = '" + AdvDate.Value.ToString() + "' or Date2 = '" + AdvDate.Value.ToString() + "' or Date2 = '" + AdvDate.Value.ToString() + "' or Date4 = '" + AdvDate.Value.ToString() + "' or Date5 = '" + AdvDate.Value.ToString() + "' or Date6 = '" + AdvDate.Value.ToString() + "' or Date7 = '" + AdvDate.Value.ToString() + "' or Date8 = '" + AdvDate.Value.ToString() + "' and Time = '" + AdvTime.Text + "' and GroundName != '7A side'";
                        cmdb.ExecuteNonQuery();
                        DataTable dt1b = new DataTable();
                        SqlDataAdapter ds1b = new SqlDataAdapter(cmdb);
                        ds1b.Fill(dt1b);
                        count1 = Convert.ToInt32(dt1b.Rows.Count.ToString());

                        if (count == 0 && count1 == 0)
                        {
                            SqlCommand cmd3a = conn.CreateCommand();
                            cmd3a.CommandType = CommandType.Text;
                            cmd3a.CommandText = "Select * from AdBookingDetails where Date='" + AdvDate.Value.ToString() + "' and Time ='" + AdvTime.Text + "' and GroundName = '7A side'";
                            cmd3a.ExecuteNonQuery();
                            DataTable dta1 = new DataTable();
                            SqlDataAdapter dsa1 = new SqlDataAdapter(cmd3a);
                            dsa1.Fill(dta1);
                            count2 = Convert.ToInt32(dta1.Rows.Count.ToString());
                            if (count2 == 0)
                            {
                                SqlCommand cmd1 = conn.CreateCommand();
                                cmd1.CommandType = CommandType.Text;//BookingMode,
                                cmd1.CommandText = "INSERT into AdBookingDetails Values('" + aaa + "','" + BookingMode.ToString() + "','" + AdvNm.Text + "','" + AdvComp.Text + "', '" + AdvMob.Text + "', '" + AdvAdd.Text + "', '" + AdvDate.Value.ToString() + "','" + AdvTime.Text + "', '" + AdvGround.Text + "','" + AdvRemark.Text + "','" + AdvRefPrsn.Text + "','" + AdvFed.Text + "')";
                                cmd1.ExecuteNonQuery();
                                MessageBox.Show("Booked...!!");


                                SqlCommand cmd2 = conn.CreateCommand();
                                cmd2.CommandType = CommandType.Text;
                                cmd2.CommandText = "INSERT into PaymentInfo(BookId,status,Date) Values('" + aaa + "','status','" + AdvDate.Value.ToString() + "')";
                                cmd2.ExecuteNonQuery();

                                //clearing the fields after saving the datas
                                AdvNm.Clear();
                                AdvMob.Clear();
                                AdvAdd.Clear();
                                AdvComp.Clear();
                                AdvGround.SelectedIndex = -1;
                                AdvRemark.Text = "Comment...";
                                AdvRefPrsn.Clear();
                                AdvFed.Clear();
                            }
                            else
                            {
                                MessageBox.Show(" 7A Side Game already Booked in this Time");
                            }
                        }
                    }

                }

            }

            displaydata2();
            displaybooked_info_for_payment();
            displayGamestatus();
            dispas();
            dispas1();
            displaydata1();
            dssss();
        }

        //Displaying the Advance Booking.
        public void displaydata2()
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select id as BookId,Date,BookingMode,PersonName,Mobile,Address,Time,GroundName from AdBookingDetails";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter ds = new SqlDataAdapter(cmd);
            ds.Fill(dt);
            BookviewDatagrid.DataSource = dt;
        }

        //back to booking menu button click
        private void BackBook_Click(object sender, EventArgs e)
        {
            NBooking.SelectedTab = BHome;
        }



        /// <summary>
        /// Payment Informations
        /// </summary>



        //payment view
        private void displaybooked_info_for_payment()
        {
            //this is displaying the booked customer details where the customer is remain to paid
            SqlCommand cmdd = conn.CreateCommand();
            cmdd.CommandType = CommandType.Text;
            cmdd.CommandText = "Select p.id,p.Bookid,B.PersonName, p.Paymentinfo,p.Date,p.PaymentDate,P.descripiton as Description from PaymentInfo as p inner join AdBookingDetails as B on p.Bookid = B.id order by Date desc";
            cmdd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter ds = new SqlDataAdapter(cmdd);
            ds.Fill(dt);
            dataGridView5.DataSource = dt;
        }
        
        private void SearchTextboxPayment_TextChanged(object sender, EventArgs e)
        {
            (dataGridView5.DataSource as DataTable).DefaultView.RowFilter = string.Format("PersonName LIKE '{0}%' OR Paymentinfo LIKE '{0}%' OR description LIKE '{0}%'", SearchTextboxPayment.Text);
        }

        //This function is for sugesstion of the custoomer name shile you type the list of  name 
        void AutoComplete_Text()
        {

            displaybooked_info_for_payment();
            SearchTextboxPayment.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            SearchTextboxPayment.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection coll1 = new AutoCompleteStringCollection();


            SqlCommand cmd14 = conn.CreateCommand();
            cmd14.CommandType = CommandType.Text;
            cmd14.CommandText = "select * from PaymentInfo where paymentinfo is NULL ";
            cmd14.ExecuteNonQuery();
            DataTable dt14 = new DataTable();
            SqlDataAdapter ds14 = new SqlDataAdapter(cmd14);
            ds14.Fill(dt14);
            foreach (DataRow dr14 in dt14.Rows)
            {
                string nams1 = dr14["BookId"].ToString(); //taking all database venname colun values and adding them as an item
                coll1.Add(nams1);
            }

            SearchTextboxPayment.AutoCompleteCustomSource = coll1;
            
        }


        private void SearchTextboxPayment_KeyDown(object sender, KeyEventArgs e)
        {
            //this is displaying the booked customer details where the customer is remain to paid
            (dataGridView5.DataSource as DataTable).DefaultView.RowFilter = string.Format("Bookid LIKE '{0}%' OR PersonName LIKE '{0}%'", SearchTextboxPayment.Text);
        }

        //payment done for Normal and Advance Booking
        private void Btnpayment_Click(object sender, EventArgs e)
        {
            if (txtbox3.Text != "" &  txtamount1.Text != "")
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText= "Update PaymentInfo set paymentinfo = '" + txtamount1.Text + "', descripiton = '" + richTextBox1.Text + "', status='PAID' , PaymentDate = '" + DateTime.Today + "' where BookId = '" + txtbox3.Text + "'"; //and GroundName = '" + comboBox0.Text + "'and Timeplay = '" + txtmbnumber1.Text + "" + comboBox1Tmms.SelectedItem.ToString() + "'
                //cmd.CommandText = "Select PersonName from AdBookingDetails where id = '" + txtbox3.Text + "'";
                cmd.ExecuteNonQuery();

                MessageBox.Show("Message", "Paid...!!!");

                txtbox3.Clear();
                txtamount1.Clear();
                richTextBox1.Clear();
            }
            else
            {
                MessageBox.Show("Empty payment!!");
            }
            displaybooked_info_for_payment();
            displaydata4();
        }

        private void dataGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //creating the variable in (int) that is for the comparision of the database Id column
            int id = Convert.ToInt32(dataGridView5.SelectedCells[0].Value.ToString());

            SqlCommand cmd1 = conn.CreateCommand();
            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = "select * from PaymentInfo where id =" + id + " and paymentinfo = null";
            cmd1.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter ds = new SqlDataAdapter(cmd1);
            ds.Fill(dt);
            foreach (DataRow dr2 in dt.Rows)
            {
                //passing the value of Item details datagrid to the corresponding textbox
                txtbox3.Text = dr2["BookId"].ToString();
            }

            }

        //Making the row color vary in not paid bookings
        private void dataGridView5_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView5.Rows)
            {
               if (row.Cells[2].Value == null)
                {
                    row.DefaultCellStyle.BackColor = Color.Red;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                }
            }
        }
    
        //printing all the Records
        private void PrintPayment_Click(object sender, EventArgs e)
        {
            DGVPrinter printer = new DGVPrinter();
            printer.Title = "TechnoPrime Company";//header
            printer.SubTitle = string.Format("Date:{0}", DateTime.Now.Date);
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = "We Learn We Play";//footer ///////////////////////////////////////////
            printer.FooterSpacing = 15;
            printer.PrintDataGridView(dataGridView5);
        }





        /// <summary>
        /// Daily Expenses
        /// </summary>
        /// 



        //Displaying the Expenses Details
        private void AddExpenses_Click(object sender, EventArgs e)
        {
            //inserting the Daily Expenses information in the database
            SqlCommand cmd1 = conn.CreateCommand();
            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = "INSERT into ExpensesDetails(Date,Name, Amount, Catagory, Vendor, Description) Values('" + DateTime.Today + "', '" + ExpName.Text + "', '" + ExpAmt.Text + "', '" + ExpCat.Text + "','" + ExpVen.Text + "', '" + ExpDec.Text + "')";
            cmd1.ExecuteNonQuery();
            MessageBox.Show("Saved...!!");
            ddddd();
        }

        //Displaying the Expenses Details
        void ddddd()
        {
            SqlCommand cmdb = conn.CreateCommand();
            cmdb.CommandType = CommandType.Text;
            cmdb.CommandText = " Select * from ExpensesDetails order by id desc";
            cmdb.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter ds = new SqlDataAdapter(cmdb);
            ds.Fill(dt);
            dataGridView14.DataSource = dt;
        }

        //Displaying the Investment Details
        void ddddda()
        {
            SqlCommand cmdb = conn.CreateCommand();
            cmdb.CommandType = CommandType.Text;
            cmdb.CommandText = " Select * from InvestmentExp order by id desc";
            cmdb.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter ds = new SqlDataAdapter(cmdb);
            ds.Fill(dt);
            dataGridView13.DataSource = dt;
        }

        //Adding all te Investment details
        private void AddInvestments_Click(object sender, EventArgs e)
        {
            //inserting the Daily Expenses information in the database
            SqlCommand cmd1 = conn.CreateCommand();
            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = "INSERT into InvestmentExp(Date,Name, Amount, Description) Values('" + DateTime.Today + "', '" + InvName.Text + "', '" + InvAmount.Text + "','" + InvDec.Text + "')";
            cmd1.ExecuteNonQuery();
            MessageBox.Show("Saved...!!");
            ddddda();
        }

        //Printing the investment details
        private void bunifuFlatButton6_Click(object sender, EventArgs e)
        {
            DGVPrinter printer = new DGVPrinter();
            printer.Title = "TechnoPrime Company";//header
            printer.SubTitle = string.Format("Date:{0}", DateTime.Now.Date);
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = "We Learn We Play";//footer ///////////////////////////////////////////
            printer.FooterSpacing = 15;
            printer.PrintDataGridView(dataGridView13);
        }

        //Printing the Expenses Details
        private void bunifuFlatButton7_Click(object sender, EventArgs e)
        {
            DGVPrinter printer = new DGVPrinter();
            printer.Title = "TechnoPrime Company";//header
            printer.SubTitle = string.Format("Date:{0}", DateTime.Now.Date);
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = "We Learn We Play";//footer ///////////////////////////////////////////
            printer.FooterSpacing = 15;
            printer.PrintDataGridView(dataGridView14);
        }


        /// <summary>
        /// Game Details
        /// </summary>


        //Displaying the Game on Today
        public void displaydata1()
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select PersonName,Time, GroundName from AdBookingDetails where Date = '" + DateTime.Today + "'";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter ds = new SqlDataAdapter(cmd);
            ds.Fill(dt);
            dataGridView1.DataSource = dt;
            int count = dataGridView1.Rows.Count;
            if(count % 2==0)
            {
                dataGridView1.DefaultCellStyle.BackColor = Color.LightCyan;
                dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            }

        }

        //Displaying the Upcomming games
        public void displayGamestatus()
        {
            SqlCommand cmda = conn.CreateCommand();
            cmda.CommandType = CommandType.Text;
            cmda.CommandText = "select Date,PersonName,Time, GroundName from AdBookingDetails where Date > '"+DateTime.Today+"'";
            cmda.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter ds = new SqlDataAdapter(cmda);
            ds.Fill(dt);
            dataGridView2.DataSource = dt;
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = string.Format("PersonName LIKE '{0}%'", textBox8.Text);
        }

        private void comboBox5_TextChanged(object sender, EventArgs e)
        {
            (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = string.Format("GroundName LIKE '{0}%'", comboBox5.Text);
        }




        /// <summary>
        /// Add new Players 
        /// </summary>



        //adding a player with the view of pannel having corresponding textfields
        private void AddUserBtn_Click(object sender, EventArgs e)
        {
            panel9.Visible = true;
            panel12.Visible = false;
        }

        //Add a team 
        private void CreateOwnteam_Click(object sender, EventArgs e)
        {
            panel9.Visible = false;
            panel12.Visible = true;
        }

        private void AddTeamBtn_Click(object sender, EventArgs e)
        {
            (new Team_Registration()).ShowDialog();
        }

        private void AsgTeam_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlCommand cmd2 = conn.CreateCommand();
            cmd2.CommandType = CommandType.Text;
            cmd2.CommandText = "Select * from TeamEntry where id= '"+ AsgTeam.Text +"'";
            cmd2.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter ds = new SqlDataAdapter(cmd2);
            ds.Fill(dt);
            foreach(DataRow dr in dt.Rows)
            {
                asgteamname.Text = dr["TeamName"].ToString();
                textBox32.Text = dr["TeamName"].ToString();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlCommand cmd2 = conn.CreateCommand();
            cmd2.CommandType = CommandType.Text;
            cmd2.CommandText = "Select * from CoachDetails where id= '" + comboBox2.Text + "'";
            cmd2.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter ds = new SqlDataAdapter(cmd2);
            ds.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                asgcoachname.Text = dr["CName"].ToString();
                textBox31.Text = dr["CName"].ToString();
            }
        }

        private void bunifuFlatButton1_Click_1(object sender, EventArgs e)
        {
            panel6.Visible = false;

            ///////
            ID newid = new ID();
            string aaa = newid.creatid("PlayersInfo", "id", "P", 2);
          
            if (txtpersonAg.Text != "" && tstplayerName.Text != "" && txtpersonCt.Text != "" && txtpersonPos.Text != "")
            {
                if (MaleRadio.Checked == false && radioButton12.Checked == false)
                {
                    MessageBox.Show("Select the Gender..");
                    MaleRadio.Focus();
                }
                else
                {
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT into PlayersInfo Values ('" + aaa + "','" + tstplayerName.Text + "','" + textAddress.Text + "','" + txtpersonCt.Text + "','" + txtpersomJn.Text + "','" + txtpersonAg.Text + "','" + txtpersonPos.SelectedItem.ToString() + "','" + txtpersonfee.Text + "','" + gender.ToString() + "','" + AsgTeam.Text + "','" + comboBox2.Text + "')";

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Player Info added successfully...!!!");
                    
                    //Clearing the Fields
                    txtpersonAg.Clear();
                    tstplayerName.Clear();
                    txtpersonCt.Clear();
                    txtpersomJn.Clear();
                    txtpersonfee.Clear();
                    txtpersonPos.SelectedIndex = -1;
                    AsgTeam.SelectedIndex = -1;
                    comboBox2.SelectedIndex = -1;
                }
            }
            else
            {
                MessageBox.Show("Fill the text fields");
            }
            label77.Text = Convert.ToString(Convert.ToInt32(label77.Text) + 1);
            displaydata5();
        }

        private void AddTeamBnam_Click(object sender, EventArgs e)
        {
            ///////
            ID newid = new ID();
            string aaa = newid.creatid("TeamEntry", "id", "FTM", 4);
            //MessageBox.Show(aaa);

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Insert into TeamEntry values('" + TeamID.Text + "','" + TeamName.Text + "')";
            cmd.ExecuteNonQuery();
            MessageBox.Show("Team Saved");

            teamdis();
        }

        //displaying the players info in a dataridview
        public void displaydata5()
        {
            SqlCommand cmd2 = conn.CreateCommand();
            cmd2.CommandType = CommandType.Text;
            cmd2.CommandText = "Select * from PlayersInfo";
            cmd2.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter ds = new SqlDataAdapter(cmd2);
            ds.Fill(dt);
            dataGridView3.DataSource = dt;
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SqlCommand cmds = conn.CreateCommand();
            cmds.CommandType = CommandType.Text;
            cmds.CommandText = " Select * from PlayersInfo ";
            cmds.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter ds = new SqlDataAdapter(cmds);
            ds.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                textBox30.Text = dr["id"].ToString();
                textBox13.Text = dr["PName"].ToString();
                textBox14.Text = dr["PAddress"].ToString();
                textBox15.Text = dr["PContact"].ToString();
                textBox1.Text = dr["JerseyNo"].ToString();
                textBox20.Text = dr["Age"].ToString();
                textBox21.Text = dr["Position"].ToString();
                textBox27.Text = dr["Fees"].ToString();
                textBox34.Text = dr["AssignedTeam"].ToString();
                textBox33.Text = dr["AssignedCoach"].ToString();

            }
        }

        //Displaying the Team 
        void teamdis()
        {
            SqlCommand cmda2 = conn.CreateCommand();
            cmda2.CommandType = CommandType.Text;
            cmda2.CommandText = "Select * from TeamEntry";
            cmda2.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter ds = new SqlDataAdapter(cmda2);
            ds.Fill(dt);
            dataGridView9.DataSource = dt;
        }

        //Displayinng the Team info in the datagridview
        public void displaydata6()
        {
            dataGridView6.Rows.Clear();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select id,Tname,Coach,AgeGradeDivision,boysORgirls,P1Fullname,P1Dob,P1Email,P1Phone,P2Fullname,P2Dob,P2Email,P2Phone,P3Fullname,P3Dob,P3Email,P3Phone,P4Fullname,P4Dob,P4Email,P4Phone,P5Fullname,P5Dob,P5Email,P5Phone,P6Fullname,P6Dob,P6Email,P6Phone,P7Fullname,P7Dob,P7Email,P7Phone,P8Fullname,P8Dob,P8Email,P8Phone,P9Fullname,P9Dob,P9Email,P9Phone,P10Fullname,P10Dob,P10Email,P10Phone from team";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter ds = new SqlDataAdapter(cmd);
            ds.Fill(dt);
            dataGridView6.DataSource = dt;
        }



        /// <summary>
        ///  Editing the Players Info
        /// </summary>



        public void autofilltext()
        {

            textBoxmobSearch.Clear();

            textBoxmobSearch.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBoxmobSearch.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection coll2 = new AutoCompleteStringCollection();

            //conn.Open();
            SqlCommand cmd14 = conn.CreateCommand();
            cmd14.CommandType = CommandType.Text;
            cmd14.CommandText = "select * from Playersinfo";
            cmd14.ExecuteNonQuery();
            DataTable dt14 = new DataTable();
            SqlDataAdapter ds14 = new SqlDataAdapter(cmd14);
            ds14.Fill(dt14);
            foreach (DataRow dr14 in dt14.Rows)
            {
                string nams1 = dr14["PName"].ToString(); //taking all database PlayerName column values and adding them as an item
                coll2.Add(nams1);
            }

            textBoxmobSearch.AutoCompleteCustomSource = coll2;
        }

        //search btn is for the search of the player name in database and retriving them in the current detail field
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            if (textBoxmobSearch.Text != "")
            { 
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Select * from PlayersInfo where PName OR PAddress OR PContact %like'" + textBoxmobSearch.Text + "'";
                cmd.ExecuteNonQuery();
                DataTable dt = new DataTable();
                SqlDataAdapter ds = new SqlDataAdapter(cmd);
                ds.Fill(dt);
                dataGridView3.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Empty Field here..");
            }

        }

        //modifying the data of customer we have recorder in PlayerInfo table
        private void button13_Click(object sender, EventArgs e)
        {
            int check = 0;
            if (textBoxmobSearch.Text != "")
            {
                if (MessageBox.Show("Do you want to Save this data", "Remove Row", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SqlCommand cmdd = conn.CreateCommand();
                    cmdd.CommandType = CommandType.Text;
                    cmdd.CommandText = "Select *  from PlayerInfo where PName = '" + textBox13.Text + "'";
                    cmdd.ExecuteNonQuery();
                    DataTable dt12 = new DataTable();
                    SqlDataAdapter ds12 = new SqlDataAdapter(cmdd);
                    ds12.Fill(dt12);
                    check = Convert.ToInt32(dt12.Rows.Count.ToString());
                    if (check == 0)
                    {
                        SqlCommand cmd16 = conn.CreateCommand();
                        cmd16.CommandText = "update PlayerInfo set PName = '" + textBox16.Text + "', PAddress = '" + textBox17.Text + "', PContact ='" + textBox18.Text + "' JerseyNo='" + textBox22.Text + "',Age='" + textBox23.Text + "',Position='" + textBox24.Text + "' AssignedTeam = '" + comboBox4.Text + "',AssignedCoach='" + comboBox3.Text + "' where PName = '" + textBox13.Text + "' and PlayersAddress = '" + textBox14.Text + "' and PlayerContact = '" + textBox15.Text + "' ";
                        cmd16.CommandType = CommandType.Text;
                        cmd16.ExecuteNonQuery();

                        MessageBox.Show("Edit successfull");
                    }
                }
            }
            else
            {
                MessageBox.Show("Empty Fields here");
            }
        }




        /// <summary>
        /// Payment details
        /// </summary>




        private void payment_detail_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from PaymentInfo where Date >= '" + dateTimePicker3.Value + "' and Date <= '" + dateTimePicker4.Value + "' and paymentinfo is Not Null";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter ds = new SqlDataAdapter(cmd);
            ds.Fill(dt);
            dataGridView4.DataSource = dt;
        }

        private void SearchNpBtn_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from PaymentInfo where Date >= '" + dateTimePicker3.Value + "' and Date <= '" + dateTimePicker4.Value + "' and paymentinfo is Null";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter ds = new SqlDataAdapter(cmd);
            ds.Fill(dt);
            dataGridView4.DataSource = dt;
        }

        //Displaying all the booked and paid unpaid results 
        public void displaydata4()
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from PaymentInfo where status='UNPAID'";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter ds = new SqlDataAdapter(cmd);
            ds.Fill(dt);
            dataGridView4.DataSource = dt;
        }

        //Print the Resulting values
        private void btnprintall_Click(object sender, EventArgs e)
        {
            DGVPrinter printer = new DGVPrinter();
            printer.Title = "TechnoPrime Company";//header
            printer.SubTitle = string.Format("Date:{0}", DateTime.Now.Date);
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = "We Learn We Play";//footer ///////////////////////////////////////////
            printer.FooterSpacing = 15;
            printer.PrintDataGridView(dataGridView4);
        }




        /// <summary>
        /// Coach Details
        /// </summary>



        //Inserting the Coach Info
        
        private void BtnSaveCoach_Click(object sender, EventArgs e)
        {
            int check = 0;
            ///////
            ID newid = new ID();
            string aaa = newid.creatid("CoachDetails", "id", "COH", 4);
            //MessageBox.Show(aaa);

            if (textBox2.Text != "" && textBox3.Text != "" && textBox7.Text != "" && textBox9.Text != "")
            {
                if (MessageBox.Show("Do you want to Save this data", "Remove Row", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SqlCommand cmdd = conn.CreateCommand();
                    cmdd.CommandType = CommandType.Text;
                    cmdd.CommandText = "Select *  from CoachDetails where CName = '" + textBox2.Text + "' and Contact = '" + textBox3.Text + "' and Email='" + textBox9.Text + "'";
                    cmdd.ExecuteNonQuery();
                    DataTable dt12 = new DataTable();
                    SqlDataAdapter ds12 = new SqlDataAdapter(cmdd);
                    ds12.Fill(dt12);
                    check = Convert.ToInt32(dt12.Rows.Count.ToString());
                    if (check == 0)
                    {
                        SqlCommand cmdq = conn.CreateCommand();
                        cmdq.CommandType = CommandType.Text;
                        cmdq.CommandText = "Insert into CoachDetails Values('" + aaa + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox7.Text + "','" + textBox9.Text + "','" + gender + "')";
                        cmdq.ExecuteNonQuery();
                    }
                    else
                    {
                        MessageBox.Show("This ID already exists");
                    }

                }
            }
            else
            {
                MessageBox.Show("Empty Fields Here...");
            }
            textBox2.Clear();
            textBox3.Clear();
            textBox7.Clear();
            textBox9.Clear();
            diss();
        }

        public void diss()
        {
            SqlCommand cmdb = conn.CreateCommand();
            cmdb.CommandType = CommandType.Text;
            cmdb.CommandText = " Select * from CoachDetails order by id desc";
            cmdb.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter ds = new SqlDataAdapter(cmdb);
            ds.Fill(dt);
            dataGridView7.DataSource = dt;
        }

        private void dataGridView7_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            SqlCommand cmds = conn.CreateCommand();
            cmds.CommandType = CommandType.Text;
            cmds.CommandText = " Select * from CoachDetails";
            cmds.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter ds = new SqlDataAdapter(cmds);
            ds.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                textBox2.Text = dr["CName"].ToString();
                textBox3.Text = dr["Address"].ToString();
                textBox7.Text = dr["Contact"].ToString();
                textBox9.Text = dr["Email"].ToString();
            }

         }


        //Delete the Coach Info
        private void DeleteCoach_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dataGridView7.SelectedCells[0].Value.ToString());

            if (MessageBox.Show("Do you want to DELETE  this data", "Remove Row", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "delete from CoachDetails where CName = '" + textBox2.Text + "' and Contact = '" + textBox3.Text + "' and Address = '" + textBox7.Text + "' OR id = '" + id + "'";
                cmd.ExecuteNonQuery();
            }
            diss();
        }

        //Validatng the Email address Coach Field
        private void textBox9_Validating(object sender, CancelEventArgs e)
        {
            System.Text.RegularExpressions.Regex rEmail = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z][\w\.-]{2,28}[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-z][a-zA-Z\.]*[a-zA-Z]$");
            if (textBox9.Text.Length > 0)
            {
                if (!rEmail.IsMatch(textBox9.Text))
                {
                    MessageBox.Show("invalid Email Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox9.SelectAll();
                    e.Cancel = true;
                }
            }
        }

        string gender;
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            gender = "Male";
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            gender = "Female";
        }



        /// <summary>
        /// Refree Details
        /// </summary>




        public void disssps()
        {
            SqlCommand cmdb = conn.CreateCommand();
            cmdb.CommandType = CommandType.Text;
            cmdb.CommandText = " Select * from RefreeDetails order by id desc";
            cmdb.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter ds = new SqlDataAdapter(cmdb);
            ds.Fill(dt);
            dataGridView8.DataSource = dt;
        }

        private void SAvRefBn_Click(object sender, EventArgs e)
        {
            int check = 0;
            ///////
            ID newid = new ID();
            string aaa = newid.creatid("RefreeDetails", "id", "REF", 4);
            //MessageBox.Show(aaa);

            if (textBox19.Text != "" && textBox11.Text != "" && textBox12.Text != "" && textBox10.Text != "")
            {
                if (MessageBox.Show("Do you want to Save this data", "Remove Row", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SqlCommand cmdd = conn.CreateCommand();
                    cmdd.CommandType = CommandType.Text;
                    cmdd.CommandText = "Select *  from RefreeDetails where Name = '" + textBox19.Text + "' and Contact = '" + textBox11.Text + "' and Email='" + textBox10.Text + "'";
                    cmdd.ExecuteNonQuery();
                    DataTable dt12 = new DataTable();
                    SqlDataAdapter ds12 = new SqlDataAdapter(cmdd);
                    ds12.Fill(dt12);
                    check = Convert.ToInt32(dt12.Rows.Count.ToString());
                    if (check == 0)
                    {
                        SqlCommand cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "Insert into RefreeDetails Values('" + aaa + "','" + textBox19.Text + "','" + textBox11.Text + "','" + textBox12.Text + "','" + textBox10.Text + "','" + refgender + "')";
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        MessageBox.Show("This ID is already exists.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Empty Fields here...");
            }
            textBox19.Clear();
            textBox11.Clear();
            textBox12.Clear();
            textBox10.Clear();
            disssps();
        }


        private void DelRefBtn_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dataGridView7.SelectedCells[0].Value.ToString());

            if (MessageBox.Show("Do you want to DELETE  this data", "Remove Row", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "delete from RefreeDetails where Name = '"+ textBox19.Text +"' and Contact = '"+textBox11.Text+"' and Address = '"+textBox12.Text+"' or id = '" + id + "'";
                cmd.ExecuteNonQuery();
            }
            MessageBox.Show("Deleted Succesfuly..");
            disssps();
        }

        private void dataGridView8_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SqlCommand cmds = conn.CreateCommand();
            cmds.CommandType = CommandType.Text;
            cmds.CommandText = " Select * from RefreeDetails ";
            cmds.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter ds = new SqlDataAdapter(cmds);
            ds.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                textBox19.Text = dr["Name"].ToString();
                textBox11.Text = dr["Address"].ToString();
                textBox12.Text = dr["Contact"].ToString();
                textBox10.Text = dr["Email"].ToString();
            }
        }

        //Validating the email address in the  refree field
        private void textBox10_Validating(object sender, CancelEventArgs e)
        {
            System.Text.RegularExpressions.Regex rEmail = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z][\w\.-]{2,28}[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-z][a-zA-Z\.]*[a-zA-Z]$");
            if (textBox10.Text.Length > 0)
            {
                if (!rEmail.IsMatch(textBox10.Text))
                {
                    MessageBox.Show("invalid Email Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox10.SelectAll();
                    e.Cancel = true;
                }
            }
        }

        string refgender;
        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            refgender = "Male";
        }

        private void refree_Click(object sender, EventArgs e)
        {
            refgender = "Female";
        }






        /// <summary>
        /// ///
        /// </summary>
        

        private void txtmobile_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if(!char.IsDigit(ch) & ch!=8 & ch!=46)
            {
                e.Handled = true;
            }
        }

        private void txtname_Leave(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtname.Text))
            {
                txtname.FindForm();
                errorProvider1.SetError(txtname, "Please Enter the Name");
            }
            else
            {
                errorProvider1.SetError(txtname, null);
            }
        }

        private void txtbox3_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtbox3.Text))
            {
                txtbox3.FindForm();
                errorProvider1.SetError(txtbox3, "Please Enter the Name");
            }
            else
            {
                errorProvider1.SetError(txtbox3, null);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
           if(this.WindowState== FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }


        private void AdvMob_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsDigit(ch) & ch != 8 & ch != 46)
            {
                e.Handled = true;
            }
        }
        private void HidepanCus_Click(object sender, EventArgs e)
        {
            panel9.Visible = false;
            panel12.Visible = false;
        }

        private void textBox11_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsDigit(ch) & ch != 8 & ch != 46)
            {
                e.Handled = true;
            }
        }

        private void txtamount1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsDigit(ch) & ch != 8 & ch != 46)
            {
                e.Handled = true;
            }
        }

        private void txtpersomJn_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsLetter(ch) & !char.IsDigit(ch) & ch != 8 & ch != 46)
            {
                e.Handled = true;
            }
        }

        private void txtpersonCt_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsDigit(ch) & ch != 8 & ch != 46)
            {
                e.Handled = true;
            }
        }

        private void tstplayerName_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsLetter(ch) & !char.IsWhiteSpace(ch) & ch != 8 & ch != 46)
            {
                e.Handled = true;
            }
        }

        private void txtbox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsLetter(ch) & !char.IsDigit(ch)& !char.IsWhiteSpace(ch) & ch != 8 & ch != 46)
            {
                e.Handled = true;
            }
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsLetter(ch) & ch != 8 & ch != 46)
            {
                e.Handled = true;
            }
        }

        private void textBoxmobSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsLetter(ch) & ch != 8 & ch != 46)
            {
                e.Handled = true;
            }
        }

        private void textBox18_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsDigit(ch) & ch != 8 & ch != 46)
            {
                e.Handled = true;
            }
        }

        private void textBox16_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsLetter(ch) & ch != 8 & ch != 46)
            {
                e.Handled = true;
            }
        }




        /// <summary>
        /// Backup Database
        /// </summary>
        

        
        private void button12_Click(object sender, EventArgs e)
        {
            //Database Backup Frst You need to add resources 

            //using Microsoft.SqlServer.Management.Smo;
            //using Microsoft.SqlServer.Management.Common;
            // from Resources Extention field
            progressBar1.Value = 0;
            try
            {
                Server dbServer = new Server(new ServerConnection(txtserver.Text, "sa", "test"));
                Backup dbBackup = new Backup { Action = BackupActionType.Database, Database = "Futsal_Management_System" };
                dbBackup.Devices.AddDevice(@"E:\Database\Futsal_Management_System.bak", DeviceType.File);
                dbBackup.Initialize = true;
                dbBackup.PercentComplete += DbBackup_PercentComplete;
                dbBackup.Complete += DbBackup_Complete;
                dbBackup.SqlBackupAsync(dbServer);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DbBackup_Complete(object sender, ServerMessageEventArgs e)
        {
            if(e.Error!=null)
            {
                lblStatuss.Invoke((MethodInvoker)delegate
                {
                    lblStatuss.Text = e.Error.Message;
                });
            }
        }

        private void DbBackup_PercentComplete(object sender, PercentCompleteEventArgs e)
        {
            progressBar1.Invoke((MethodInvoker)delegate
            {
                progressBar1.Value = e.Percent;
                progressBar1.Update();
            });
            lblPercent.Invoke((MethodInvoker)delegate
            {
                lblPercent.Text = $"{e.Percent}%";
            });
        }

        private void button14_Click(object sender, EventArgs e)
        {
            progressBar2.Value = 0;
            try
            {
                Server dbserver = new Server(new ServerConnection(txtserverName.Text, "sa", "test"));
                Restore dbRestore = new Restore() { Database = "Futsal_Management_System", Action = RestoreActionType.Database, ReplaceDatabase = true, NoRecovery = false };
                dbRestore.Devices.AddDevice(@"E:\Database\Futsal_Management_System.bak", DeviceType.File);
                dbRestore.PercentComplete += DbRestore_PercentageComplete;
                dbRestore.Complete += DbRestore_Complete;
                dbRestore.SqlRestoreAsync(dbserver);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"Message",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void DbRestore_Complete(object sender, ServerMessageEventArgs e)
        {
            if (e.Error != null)
            {
                lblStatuss.Invoke((MethodInvoker)delegate
                {
                    lblStatuss.Text = e.Error.Message;
                });
            }
            }

        private void DbRestore_PercentageComplete(object sender, PercentCompleteEventArgs e)
        {
            progressBar2.Invoke((MethodInvoker)delegate
            {
                progressBar2.Value = e.Percent;
                progressBar2.Update();
            });
            LblPercentt.Invoke((MethodInvoker)delegate
            {
                LblPercentt.Text = $"{e.Percent}%";
            });
        }

        //making the normal booking textfields like name reference prerson remark and feedback only excepts the 
        //string Characters
        private void AdvNm_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsLetter(ch) & !char.IsWhiteSpace(ch) & ch != 8 & ch != 46)
            {
                e.Handled = true;
            }
        }

        //making the advance booking textfields like name reference prerson remark and feedback only excepts the 
        //string Characters
        private void txtname_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsLetter(ch) & !char.IsWhiteSpace(ch) & ch != 8 & ch != 46)
            {
                e.Handled = true;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            (new Keys()).ShowDialog();
        }

        private void Refreshlink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            displayCredentials();
        }

        void displayCredentials()
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT TOP(1) id,fiveAMorning,fiveAAfterNoon,fiveAEvening,SevenAMorning,SevenAAfterNoon,SevenAEvening,SaturdayFiveAside,SaturdaySevenAside FROM KeyAmount ORDER BY 1 DESC";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter ds = new SqlDataAdapter(cmd);
            ds.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                label71.Text = dr["fiveAMorning"].ToString();
                label132.Text = dr["fiveAAfterNoon"].ToString();
                label129.Text = dr["fiveAEvening"].ToString();
                label126.Text = dr["SevenAMorning"].ToString();
                label122.Text = dr["SevenAAfterNoon"].ToString();
                label78.Text = dr["SevenAEvening"].ToString();
                label121.Text = dr["SaturdayFiveAside"].ToString();
                label115.Text = dr["SaturdaySevenAside"].ToString();
            }
        }

        private void bunifuImageButton5_Click(object sender, EventArgs e)
        {
            NBooking.SelectedTab = BHome;
        }

        private void bunifuImageButton4_Click(object sender, EventArgs e)
        {
            NBooking.SelectedTab = Customer;
        }

        void AutoCompletea_Text()
        {
            textBox25.Clear();

            textBox25.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBox25.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection coll2 = new AutoCompleteStringCollection();

            //conn.Open();
            SqlCommand cmd14 = conn.CreateCommand();
            cmd14.CommandType = CommandType.Text;
            cmd14.CommandText = "select * from AdBookingDetails";
            cmd14.ExecuteNonQuery();
            DataTable dt14 = new DataTable();
            SqlDataAdapter ds14 = new SqlDataAdapter(cmd14);
            ds14.Fill(dt14);
            foreach (DataRow dr14 in dt14.Rows)
            {
                string nams1 = dr14["PersonName"].ToString(); //taking all database PlayerName column values and adding them as an item
                coll2.Add(nams1);
            }

            textBox25.AutoCompleteCustomSource = coll2;
        }

        private void textBox25_TextChanged(object sender, EventArgs e)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select * from AdBookingDetails where PersonName ='" + textBox25.Text + "'";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter ds = new SqlDataAdapter(cmd);
            ds.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                //autofilling the information who has already booked the game
                txtbox3.Text = dr["id"].ToString();
            }
        }
    }
}