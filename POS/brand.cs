using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace POS
{
    public partial class brand : Form
    {

        int mov;
        int movX;
        int movY;
        public brand()
        {
            InitializeComponent();
            autovalueaddedId();
        }
        //get the database connection
        MySqlConnection conn = new MySqlConnection("server=localhost;database=pos;userid=root;password=;");

        private void bsave_btn_Click(object sender, EventArgs e)
        {
            

            if (bid_txt.Text == "")
            {
                MessageBox.Show("Please enter brand id...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (bname_txt.Text == "")
            {
                MessageBox.Show("Please enter brand name...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
            {

                try
                {
                    MemoryStream ms = new MemoryStream();
                    pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                    byte[] img = ms.ToArray();
                    ms.Close();

                    string insertquery = "INSERT INTO brand(id,name,category,description,image) VALUES (@id,@name,@category,@description,@image)";
                    MySqlCommand cmd = new MySqlCommand(insertquery, conn);

                    cmd.Parameters.AddWithValue("@id", bid_txt.Text);
                    cmd.Parameters.AddWithValue("@name", bname_txt.Text);
                    cmd.Parameters.AddWithValue("@category", cat_combo.SelectedItem);
                    cmd.Parameters.AddWithValue("@description", bdec_txt.Text);
                    cmd.Parameters.AddWithValue("@image",img);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record is added...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    Grideloadbrand();
                    autovalueaddedId();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            conn.Close();
        }

        private void Brand_Load(object sender, EventArgs e)
        {
            Grideloadbrand();
            catcompbload();
        }
        public void autovalueaddedId()
        {
            AutoCompleteStringCollection col = new AutoCompleteStringCollection();

            try
            {

                string selectQuery = "SELECT * FROM brand";
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
            bid_txt.AutoCompleteCustomSource = col;
            conn.Close();

        }

        private void bupdate_btn_Click(object sender, EventArgs e)
        {
            

            if (bid_txt.Text == "")
            {
                MessageBox.Show("Please enter category id...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (bname_txt.Text == "")
            {
                MessageBox.Show("Please enter category name...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
            {

                try
                {
                    MemoryStream ms = new MemoryStream();
                    pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                    byte[] img = ms.ToArray();
                    ms.Close();

                    string insertquery = "UPDATE brand SET id=@id,name=@name,category=@category, description=@description, image=@image WHERE id='" + bid_txt.Text + "'";
                    MySqlCommand cmd = new MySqlCommand(insertquery, conn);

                    cmd.Parameters.AddWithValue("@id", bid_txt.Text);
                    cmd.Parameters.AddWithValue("@name", bname_txt.Text);
                    cmd.Parameters.AddWithValue("@category", cat_combo.SelectedItem);
                    cmd.Parameters.AddWithValue("@description", bdec_txt.Text);
                    cmd.Parameters.AddWithValue("@image",img);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record is updated...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    Grideloadbrand();
                    autovalueaddedId();


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            conn.Close();
        }

        private void bdelete_btn_Click(object sender, EventArgs e)
        {
            if (bid_txt.Text == "")
            {
                MessageBox.Show("Please enter category id...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {

                try
                {
                    string insertquery = "DELETE FROM brand WHERE id='" + bid_txt.Text + "'";
                    MySqlCommand cmd = new MySqlCommand(insertquery, conn);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record is deleted...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    Grideloadbrand();
                    autovalueaddedId();


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            conn.Close();
        }
        public void Grideloadbrand()
        {

            dataGridView2.Rows.Clear();
            string sql = "select * from brand";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            conn.Open();
            MySqlDataReader reder = cmd.ExecuteReader();


            while (reder.Read())
            {
                dataGridView2.Rows.Add(reder[0], reder[1], reder[2], reder[3]);


            }
            conn.Close();

        }
        public void catcompbload()
        {
            cat_combo.Items.Clear();
            try
            {

                string selectQuery = "SELECT * FROM category";
                conn.Open();
                MySqlCommand command = new MySqlCommand(selectQuery, conn);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    cat_combo.Items.Add(reader.GetString("name"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            conn.Close();
        }

        private void bid_txt_TextChanged(object sender, EventArgs e)
        {
            if (bid_txt.Text == "")
            {

                Grideloadbrand();
                bname_txt.Text = "";
                bdec_txt.Text = "";
                cat_combo.Text = "";
                pictureBox1.Image = null;

            }
            else
            {

                try
                {
                    dataGridView2.Rows.Clear();
                    string sql = "select * from brand where id ='" + bid_txt.Text + "' ";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    conn.Open();
                    MySqlDataReader reder = cmd.ExecuteReader();


                    while (reder.Read())
                    {
                        dataGridView2.Rows.Add(reder[0], reder[1], reder[2], reder[3]);
                        bname_txt.Text = (reder["name"].ToString());
                        cat_combo.Text = (reder["category"].ToString());
                        bdec_txt.Text = (reder["description"].ToString());

                        byte[] arrimg = (byte[])reder["image"];
                        if (arrimg != null)
                        {
                            MemoryStream stream = new MemoryStream(arrimg);
                            pictureBox1.Image = Image.FromStream(stream);
                        }

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

        private void brand_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.End)
            {
                cnclw_btn.PerformClick();

            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog opd = new OpenFileDialog();
            opd.Filter = "Choose Image(*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif";
            if (opd.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(opd.FileName);
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
