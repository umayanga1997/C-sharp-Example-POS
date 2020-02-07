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
    public partial class Customer : Form
    {
        int mov;
        int movX;
        int movY;

        public Customer()
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

                this.Hide();

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
            id_txt.AutoCompleteCustomSource = col;
            conn.Close();
        }
        private void save_btn_Click(object sender, EventArgs e)
        {

            if (id_txt.Text == "")
            {
                MessageBox.Show("Please enter customer id...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (name_txt.Text == "")
            {
                MessageBox.Show("Please enter customer name...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else if (phone_txt.Text == "")
            {
                MessageBox.Show("Please enter customer phone Number...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else if (adress_txt.Text == "")
            {
                MessageBox.Show("Please enter customer Address...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
            {


                try
                {

                    MemoryStream ms = new MemoryStream();
                    pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                    byte[] img = ms.ToArray();
                    ms.Close();

                    string insertquery = "INSERT INTO customer(id,name,phone,address,image) VALUES (@id,@name,@phone,@address,@image)";
                    MySqlCommand cmd = new MySqlCommand(insertquery, conn);

                    cmd.Parameters.AddWithValue("@id", id_txt.Text);
                    cmd.Parameters.AddWithValue("@name", name_txt.Text);
                    cmd.Parameters.AddWithValue("@phone", phone_txt.Text);
                    cmd.Parameters.AddWithValue("@address", adress_txt.Text);
                    cmd.Parameters.AddWithValue("@image", img);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record is added...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    Grideloaditem();
                    autovalueaddedId();
                    Main_Menu mn = new Main_Menu();
                    mn.autovalueadded();
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                conn.Close();
            }

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
                dataGridView1.Rows.Add(reder[0], reder[1], reder[2], reder[3]);


            }
            conn.Close();

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

        private void update_btn_Click(object sender, EventArgs e)
        {
            if (id_txt.Text == "")
            {
                MessageBox.Show("Please enter customer id...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (name_txt.Text == "")
            {
                MessageBox.Show("Please enter customer name...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else if (phone_txt.Text == "")
            {
                MessageBox.Show("Please enter customer phone Number...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else if (adress_txt.Text == "")
            {
                MessageBox.Show("Please enter customer Address...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
            {


                try
                {


                    MemoryStream ms = new MemoryStream();
                    pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                    byte[] img = ms.ToArray();
                    ms.Close();

                    string insertquery = "UPDATE customer SET name=@name,phone=@phone, address=@address,image = @image WHERE id='" + id_txt.Text + "'";
                    MySqlCommand cmd = new MySqlCommand(insertquery, conn);


                    cmd.Parameters.AddWithValue("@name", name_txt.Text);
                    cmd.Parameters.AddWithValue("@phone", phone_txt.Text);
                    cmd.Parameters.AddWithValue("@address", adress_txt.Text);
                    cmd.Parameters.AddWithValue("@image", img);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record is updated...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    Grideloaditem();
                    autovalueaddedId();
                    Main_Menu mn = new Main_Menu();
                    mn.autovalueadded();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                conn.Close();
            }

            conn.Close();
        }

        private void delete_btn_Click(object sender, EventArgs e)
        {
            if (id_txt.Text == "")
            {
                MessageBox.Show("Please enter customer id...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {

                try
                {
                    string insertquery = "DELETE FROM customer WHERE id='" + id_txt.Text + "'";
                    MySqlCommand cmd = new MySqlCommand(insertquery, conn);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record is deleted...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    Grideloaditem();
                    autovalueaddedId();
                    Main_Menu mn = new Main_Menu();
                    mn.autovalueadded();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            conn.Close();
        }


        private void Customer_Load(object sender, EventArgs e)
        {
            Grideloaditem();
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Selected)
            {
                Bill_Details bd = new Bill_Details();
                // cb.label1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                bd.Cus_ID = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                bd.Show();
            }
        }


        private void id_txt_TextChanged(object sender, EventArgs e)
        {
            if (id_txt.Text == "")
            {

                Grideloaditem();
                id_txt.Text = "";
                name_txt.Text = "";
                phone_txt.Text = "";
                adress_txt.Text = "";
                pictureBox1.Image = null;

            }
            else
            {

                try
                {
                    dataGridView1.Rows.Clear();
                    string sql = "select * from customer where id = '" + id_txt.Text + "' ";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    conn.Open();
                    MySqlDataReader reder = cmd.ExecuteReader();

                    while (reder.Read())
                    {
                        dataGridView1.Rows.Add(reder[0], reder[1], reder[2], reder[3]);
                        id_txt.Text = (reder["id"].ToString());
                        name_txt.Text = (reder["name"].ToString());
                        phone_txt.Text = (reder["phone"].ToString());
                        adress_txt.Text = (reder["address"].ToString());

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
