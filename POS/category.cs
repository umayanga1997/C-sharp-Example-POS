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
    public partial class category : Form
    {
        int mov;
        int movX;
        int movY;

        public category()
        {
            InitializeComponent();
            autovalueaddedId();
        }
        //get the database connection
        MySqlConnection conn = new MySqlConnection("server=localhost;database=pos;userid=root;password=;");

        private void category_Load(object sender, EventArgs e)
        {
            Grideload();
            this.ActiveControl = id_txt;
        }
        public void autovalueaddedId()
        {
            AutoCompleteStringCollection col = new AutoCompleteStringCollection();

            try
            {

                string selectQuery = "SELECT * FROM category";
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
            id_txt.AutoCompleteCustomSource = col;
            conn.Close();

        }
        private void save_btn_Click(object sender, EventArgs e)
        {
            if (id_txt.Text == "")
            {
                MessageBox.Show("Please enter category id...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (cname_txt.Text == "")
            {
                MessageBox.Show("Please enter category name...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
            {

                try
                {
                    string insertquery = "INSERT INTO category(id,name,description) VALUES (@id,@name,@description)";
                    MySqlCommand cmd = new MySqlCommand(insertquery, conn);

                    cmd.Parameters.AddWithValue("@id", id_txt.Text);
                    cmd.Parameters.AddWithValue("@name", cname_txt.Text);
                    cmd.Parameters.AddWithValue("@description", dec_txt.Text);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record is added...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    Grideload();
                    autovalueaddedId();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            conn.Close();
        }

        private void update_btn_Click(object sender, EventArgs e)
        {
            if (id_txt.Text == "")
            {
                MessageBox.Show("Please enter category id...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (cname_txt.Text == "")
            {
                MessageBox.Show("Please enter category name...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
            {

                try
                {
                    string insertquery = "UPDATE category SET id=@id,name=@name,description=@description WHERE id='" + id_txt.Text + "'";
                    MySqlCommand cmd = new MySqlCommand(insertquery, conn);

                    cmd.Parameters.AddWithValue("@id", id_txt.Text);
                    cmd.Parameters.AddWithValue("@name", cname_txt.Text);
                    cmd.Parameters.AddWithValue("@description", dec_txt.Text);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record is updated...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    Grideload();
                    autovalueaddedId();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            conn.Close();
        }

        private void del_btn_Click(object sender, EventArgs e)
        {
            if (id_txt.Text == "")
            {
                MessageBox.Show("Please enter category id...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {

                try
                {
                    string insertquery = "DELETE FROM category WHERE id='" + id_txt.Text + "'";
                    MySqlCommand cmd = new MySqlCommand(insertquery, conn);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record is deleted...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    Grideload();
                    autovalueaddedId();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            conn.Close();
        }
        public void Grideload()
        {


            dataGridView1.Rows.Clear();
            string sql = "select * from category";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            conn.Open();
            MySqlDataReader reder = cmd.ExecuteReader();


            while (reder.Read())
            {
                dataGridView1.Rows.Add(reder[0], reder[1], reder[2]);


            }
            conn.Close();

        }

        private void id_txt_TextChanged(object sender, EventArgs e)
        {
            if (id_txt.Text == "")
            {

                Grideload();
                cname_txt.Text = "";
                dec_txt.Text = "";

            }
            else
            {

                try
                {
                    dataGridView1.Rows.Clear();
                    string sql = "select * from category where id like '%" + id_txt.Text + "%' ";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    conn.Open();
                    MySqlDataReader reder = cmd.ExecuteReader();


                    while (reder.Read())
                    {
                        dataGridView1.Rows.Add(reder[0], reder[1], reder[2]);
                        cname_txt.Text = (reder["name"].ToString());
                        dec_txt.Text = (reder["description"].ToString());

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

        private void cnclw_btn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you want to exit..", "POS Info", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {

                this.Hide();
            }
        }

        private void category_KeyDown(object sender, KeyEventArgs e)
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
