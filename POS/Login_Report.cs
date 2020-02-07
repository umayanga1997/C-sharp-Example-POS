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

namespace POS
{
    public partial class Login_Report : Form
    {
        int mov;
        int movX;
        int movY;

        public Login_Report()
        {
            InitializeComponent();
        }
        //get the database connection
        MySqlConnection conn = new MySqlConnection("server=localhost;database=pos;userid=root;password=;");
        private void Login_Report_Load(object sender, EventArgs e)
        {
            Grideload();
            grideviewcolor();
        }

        public void grideviewcolor()
        {

            
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Transparent;
            dataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.Transparent;
            
        }
        public void Grideload()
        {


            dataGridView1.Rows.Clear();
            string sql = "select * from login";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            conn.Open();
            MySqlDataReader reder = cmd.ExecuteReader();

            
            while (reder.Read())
            {
                dataGridView1.Rows.Add(reder[0], reder[1], reder[3]);
               
            }
            conn.Close();

        }

        private void cnclw_btn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you want to exit..", "POS Info", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Hide();
                Login lg = new Login();
                lg.Show();
            }
              
        }

        private void Login_Report_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.End)
            {
                cnclw_btn.PerformClick();

            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mov = 1;
            movX = e.X;
            movY = e.Y;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mov == 1)
            {
                this.SetDesktopLocation(MousePosition.X - movX, MousePosition.Y - movY);
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mov = 0;
        }
    }
}
