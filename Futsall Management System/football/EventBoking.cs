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
    public partial class EventBoking : Form
    {
        //initializing the sql connection string
        SqlConnection conn = new SqlConnection("Data Source=DESKTOP-MR6JCUA\\MSSQLSERVERDEV;Initial Catalog=Futsal_Management_System;Persist Security Info=True;User ID=sa;Password=test");

        public EventBoking()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //closing the current form
            this.Close();
        }

        private void EventBoking_Load(object sender, EventArgs e)
        {
            //making the connection property ready.
           if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();

            //Displaying the booked info
            if (!panel3.Controls.Contains(Event.Instance))
            {
                panel3.Controls.Add(Event.Instance);
                Event.Instance.Dock = DockStyle.Fill;
                Event.Instance.BringToFront();
            }
            else
            {
                Event.Instance.BringToFront();
            }
            button3.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#007acc");

        }

       

        private void button2_Click(object sender, EventArgs e)
        {
            //view button click
            button2.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#007acc");
            if (!panel3.Controls.Contains(View_Event.Instance))
            {
                panel3.Controls.Add(View_Event.Instance);
                View_Event.Instance.Dock = DockStyle.Fill;
                View_Event.Instance.BringToFront();
            }
            else
            {
                View_Event.Instance.BringToFront();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //search buttonn click

            button1.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#007acc");
            if (!panel3.Controls.Contains(View_Event.Instance))
            {
                panel3.Controls.Add(View_Event.Instance);
                View_Event.Instance.Dock = DockStyle.Fill;
                View_Event.Instance.BringToFront();
            }
            else
            {
                View_Event.Instance.BringToFront();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //book button click

            button3.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#007acc");
            if (!panel3.Controls.Contains(Event.Instance))
            {
                panel3.Controls.Add(Event.Instance);
                Event.Instance.Dock = DockStyle.Fill;
                Event.Instance.BringToFront();
            }
            else
            {
                Event.Instance.BringToFront();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //book button click
            button5.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#007acc");
            if (!panel3.Controls.Contains(EventPayment.Instance))
            {
                panel3.Controls.Add(EventPayment.Instance);
                EventPayment.Instance.Dock = DockStyle.Fill;
                EventPayment.Instance.BringToFront();
            }
            else
            {
                EventPayment.Instance.BringToFront();
            }
        }

        private void button3_MouseHover(object sender, EventArgs e)
        {
            button3.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#007acc");
        }
    }
}
