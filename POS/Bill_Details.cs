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
    public partial class Bill_Details : Form
    {
        int mov;
        int movX;
        int movY;

        public Bill_Details()
        {
            InitializeComponent();

            
        }
        //get the database connection
        MySqlConnection conn = new MySqlConnection("server=localhost;database=pos;userid=root;password=;");

        private string id;

        public string Cus_ID
        {
            get { return id; }
            set { id = value; }
        }


        private void cnclw_btn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you want to exit..", "POS Info", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {

                this.Hide();

            }
        }

        private void Bill_Details_Load(object sender, EventArgs e)
        {
            string cid = Cus_ID;
            invoiceDetailsdatagrideView(cid);
        }
        public void invoiceDetailsdatagrideView(string cid)
        {

            try
            {
                invoice_gride.Rows.Clear();
                invoicelessgride.Rows.Clear();
                string sql = "select * from customer_invoice where cus_id ='" + cid + "' ";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                conn.Open();
                MySqlDataReader reder = cmd.ExecuteReader();

                while (reder.Read())
                {
                    invoice_gride.Rows.Add(reder[1], reder[0], reder[2], reder[3], reder[4], reder[5]);

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            conn.Close();

        }

        private void invoice_gride_Click(object sender, EventArgs e)
        {
            try
            {
                
                invoicelessgride.Rows.Clear();
                string sql = "select * from bill_details where inv_id ='" + invoice_gride.CurrentRow.Cells[1].Value.ToString() + "' ";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                conn.Open();
                MySqlDataReader reder = cmd.ExecuteReader();

                while (reder.Read())
                {
                    invoicelessgride.Rows.Add(reder[1], reder[2], reder[4], reder[5], reder[6]);

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            conn.Close();
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
