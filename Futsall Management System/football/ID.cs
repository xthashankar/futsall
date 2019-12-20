using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace football
{
    class ID
    {
        //initializing the sql connection string
        SqlConnection conn = new SqlConnection("Data Source=DESKTOP-MR6JCUA\\MSSQLSERVERDEV;Initial Catalog=Futsal_Management_System;Persist Security Info=True;User ID=sa;Password=test");

        /// <summary>
        /// generate unique id
        /// </summary>
        /// <param name="tablename">table name</param>
        /// <param name="colum">field name</param>
        /// <param name="character">character of id you want to include</param>
        ///  /// <param name="number">length of string + 1</param>
        /// <returns>unique id</returns>

        public string creatid(string tablename, string colum, string character, int number)
        {
            //string character = "kist";
            string myid = string.Empty;
            string newid = string.Empty;
            //try
            {
                if(conn.State==ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText="select ISNULL(max(cast(substring(" + colum + "," + number + ", len(" + colum + ")) as int)),0)+1 as newId from " + tablename;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        myid = reader["newid"].ToString();
                    }
                }
                switch (myid.Length)
                {
                    case 1:
                        {
                            newid = character + "000" + myid;
                            break;
                        }
                    case 2:
                        {
                            newid = character + "00" + myid;
                            break;
                        }
                    case 3:
                        {
                            newid = character + "0" + myid;
                            break;
                        }
                    case 4:
                        {
                            newid = character + myid;
                            break;
                        }
                }
            }
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            //finally
            //{
            //    if (conn.State == ConnectionState.Open) conn.Close();
            //}
            return newid;
        }
    }
}
