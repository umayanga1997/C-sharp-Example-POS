using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace POS
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            timer1.Start();
            this.ActiveControl = username_txt;

        }
      /*  protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            GraphicsPath grPath = new GraphicsPath();
            grPath.AddEllipse(0, 0, ClientSize.Width, ClientSize.Height);
            this.Region = new System.Drawing.Region(grPath);
            base.OnPaint(e);
        }
        */

        //get the database connection
        MySqlConnection conn = new MySqlConnection("server=localhost;database=pos;userid=root;password=;");
        private void Login_Load(object sender, EventArgs e)
        {
            changeing();
        }
    
        public void changeing()
        {

            label1.BackColor = System.Drawing.Color.Transparent;
            label2.BackColor = System.Drawing.Color.Transparent;
            label3.BackColor = System.Drawing.Color.Transparent;
            login_btn.BackColor = System.Drawing.Color.Transparent;
            groupBox1.BackColor = System.Drawing.Color.Transparent;
            create_accnt_lbl.BackColor = System.Drawing.Color.Transparent;


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label3.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            clickchange();


        }
        public void clickchange()
        {
            if (create_accnt_lbl.Text == "Create Account")
            {
                create_accnt_lbl.Text = "I Have Alredy Account";
                login_btn.Text = "Register";

            }
            else
            {
                create_accnt_lbl.Text = "Create Account";
                login_btn.Text = "Login";
            }
        }

        private void login_btn_Click(object sender, EventArgs e)
        {
            logi_register();


        }

        public void logi_register()
        {

            string username = username_txt.Text;
            string password = password_txt.Text;
            string datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            if (username == "")
            {
                MessageBox.Show("Please enter your username...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else if (password == "")
            {

                MessageBox.Show("Please enter your password...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
            {

                if (login_btn.Text == "Login")
                {

                    string query = "SELECT * FROM register where username ='" + username + "' && password = '" + password + "'";
                    MySqlDataAdapter adapt = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapt.Fill(dt);

                    if (dt.Rows.Count == 1)
                    {
                        string queryadd = "INSERT INTO login(username,password)VALUES(@username,@password)";
                        MySqlCommand cmd = new MySqlCommand(queryadd, conn);
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);
                        //cmd.Parameters.AddWithValue("@datetime", datetime);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Welcome to POS...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Main_Menu mn = new Main_Menu();
                        mn.Show();
                        this.Hide();
                        conn.Close();

                    }
                    else
                    {

                        MessageBox.Show("Please register in the software...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }

                }
                else
                {
                    try
                    {

                        //check duplicate values
                        DataSet ds = new DataSet();
                        string querycheck = "select * from register where username = '" + username + "'";
                        MySqlCommand cmdchech = new MySqlCommand(querycheck, conn);
                        MySqlDataAdapter da = new MySqlDataAdapter(cmdchech);
                        da.Fill(ds);

                        int i = ds.Tables[0].Rows.Count;
                        if (i == 1)
                        {
                            MessageBox.Show("This username is already exists...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            string queryadd = "INSERT INTO register(username,password)VALUES(@username,@password)";
                            MySqlCommand cmd = new MySqlCommand(queryadd, conn);
                            cmd.Parameters.AddWithValue("@username", username);
                            cmd.Parameters.AddWithValue("@password", password);
                            //cmd.Parameters.AddWithValue("@datetime", datetime);
                            conn.Open();
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Registered your Account...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            conn.Close();

                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    conn.Close();
                }

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you want to exit..", "POS Info",MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
                
        }  

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkreport();
        }
        public void linkreport()
        {
            Login_Report lr = new Login_Report();
            lr.Show();
            this.Hide();
        }

        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if(username_txt.Text != "")
            {
                if (e.KeyCode == Keys.Enter)
                {
                    this.password_txt.Focus();

                }
            }
            
            else if(e.KeyCode == Keys.End)
            {
                cnclw_btn.PerformClick();

            }else if (e.Control && e.KeyCode ==Keys.R)
            {
                clickchange();
            }else if(e.KeyCode == Keys.R)
            {
                linkreport();
            }
        }

        private void password_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if(password_txt.Text == "")
            {
                if (e.KeyCode == Keys.Back)
                {
                    this.username_txt.Focus();

                }
                
            }
            if (e.KeyCode == Keys.Enter)
            {

                logi_register();

            }


        }

        private void username_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                
                if (username_txt.Text == "")

                {

                    MessageBox.Show("Please enter your username...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }

                else

                {

                    password_txt.Focus();

                }
            }
        }
    }

}


