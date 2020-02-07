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
    public partial class Cus_Searching : Form
    {
        int mov;
        int movX;
        int movY;
        public Cus_Searching()
        {
            InitializeComponent();
            autovalueaddedId();
            
        }
        //get the database connection
        MySqlConnection conn = new MySqlConnection("server=localhost;database=pos;userid=root;password=;");

        private void cnclw_btn_Click(object sender, EventArgs e)
        {

            DialogResult result = MessageBox.Show("Are you want to exit..", "POS Info", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                
                this.Dispose();

            }
        }
        public void autovalueaddedId()
        {
            AutoCompleteStringCollection col = new AutoCompleteStringCollection();

            try
            {

                string selectQuery = "SELECT * FROM customer";
                conn.Open();
                MySqlCommand command = new MySqlCommand(selectQuery, conn);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string ids = reader.GetString("id");
                    col.Add(ids);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            cid_txt.AutoCompleteCustomSource = col;
            conn.Close();
        }
        public void Grideloaditem()
        {

            dataGridView1.Rows.Clear();
            string sql = "select * from customer";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            conn.Open();
            MySqlDataReader reder = cmd.ExecuteReader();


            while (reder.Read())
            {
                dataGridView1.Rows.Add(reder[1], reder[2], reder[3]);


            }
            conn.Close();

        }

        private void customerBill_Load(object sender, EventArgs e)
        {
            Grideloaditem();
            Main_Menu mn = new Main_Menu();
            cid_txt.Text = mn.cus_id_txt.Text;
            this.cid_txt_TextChanged("",e);
        }

        private void cid_txt_TextChanged(object sender, EventArgs e)
        {
            if (cid_txt.Text == "")
            {

                Grideloaditem();
                cid_txt.Text = "";

            }
            else
            {

                try
                {
                    dataGridView1.Rows.Clear();
                    string sql = "select * from customer where id like '%" + cid_txt.Text + "%' ";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    conn.Open();
                    MySqlDataReader reder = cmd.ExecuteReader();

                    while (reder.Read())
                    {
                        dataGridView1.Rows.Add(reder[1], reder[2], reder[3]);

                    }
                    conn.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                conn.Close();
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

        private void cid_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.End)
            {
                this.Dispose();
            }
            {

            }
        }
    }
}
