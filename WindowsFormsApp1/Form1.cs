using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private MySqlConnection con;
        private string server;
        private string database;
        private string uid;
        private string password;

        public Form1()
        {
            server = "localhost";
            database = "devlogin";

            uid = "";
            password = "";

            string conString;
            conString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";

            con = new MySqlConnection(conString);

            InitializeComponent();
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            string user = txt_user.Text;
            string pass = txt_pass.Text;

            if (IsLogin(user, pass))
            {
                label1.ForeColor = Color.Green;
                label1.Text = $"Welcome {user}";
            }
            else
            {
                label1.ForeColor = Color.Red;
                label1.Text = $"{user} doesn't exist";
            }
        }

        private bool IsLogin(string user, string pass)
        {
            string query = $"SELECT * FROM users WHERE username='{user}' AND password='{pass}';";

            try
            {
                if (OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, con);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        reader.Close();
                        con.Close();
                        return true;
                    }
                    else
                    {
                        reader.Close();
                        con.Close();
                        return false;
                    }
                }
                else
                {
                    con.Close();
                    return false;
                }
            }

            catch (Exception ex)
            {
                con.Close();
                return false;
            }
        }

        private bool CanRegister(string user, string pass)
        {
            string query = $"INSERT INTO `users` (`id`, `username`, `password`) VALUES (NULL, '{user}', '{pass}');";
            string checkuserquery = $"SELECT * FROM users WHERE username='{user}' AND password='{pass}';";

            try
            {
                if (OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    reader.Close();

                    MySqlCommand cmdcheck = new MySqlCommand(checkuserquery, con);
                    MySqlDataReader readercheck = cmdcheck.ExecuteReader();

                    if (readercheck.Read())
                    {
                        con.Close();
                        readercheck.Close();
                        return true;
                    }
                    else
                    {
                        con.Close();
                        readercheck.Close();
                        return false;
                    }
                }

                else
                {
                    con.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                con.Close();
                return false;
            }
        }

        private bool OpenConnection()
        {
            try
            {
                con.Open();
                return true;
            }

            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        label1.ForeColor = Color.Red;
                        label1.Text = $"Connection to server Failed";
                        break;

                    case 1:
                        label1.ForeColor = Color.Red;
                        label1.Text = $"Incorrect username or password for SQL Database";
                        break;
                }
                return false;
            }
        }

        private void btn_register_Click(object sender, EventArgs e)
        {
            string user = txt_user.Text;
            string pass = txt_pass.Text;

            if (CanRegister(user, pass) == true)
            {
                label1.ForeColor = Color.Green;
                label1.Text = $"{user} registed";
            }
            else
            {
                label1.ForeColor = Color.Red;
                label1.Text = $"{user} not registered";
            }
        }
    }
}