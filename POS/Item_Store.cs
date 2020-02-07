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
    public partial class Item_Store : Form
    {
        int mov;
        int movX;
        int movY;

        public Item_Store()
        {
            InitializeComponent();
            autovalueadded();
            autovalueaddedBar();
        }
        //get the database connection
        MySqlConnection conn = new MySqlConnection("server=localhost;database=pos;userid=root;password=;");
        
        //close btn
        private void cnclw_btn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you want to exit..", "POS Info", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {

                this.Hide();

            }      
            
        }

        private void Item_Store_Load(object sender, EventArgs e)
        {        
    
            Grideloaditem();
            brandcompbload();

        }
        public void autovalueadded()
        {
            AutoCompleteStringCollection col = new AutoCompleteStringCollection();

            try
            {

                string selectQuery = "SELECT * FROM items";
                conn.Open();
                MySqlCommand command = new MySqlCommand(selectQuery, conn);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string ids = reader.GetString("item_id");
                    col.Add(ids);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            iid_txt.AutoCompleteCustomSource = col;
            conn.Close();
        }
        public void autovalueaddedBar()
        {
            AutoCompleteStringCollection col = new AutoCompleteStringCollection();

            try
            {

                string selectQuery = "SELECT * FROM items";
                conn.Open();
                MySqlCommand command = new MySqlCommand(selectQuery, conn);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string ids = reader.GetString("barcode_id");
                    col.Add(ids);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            barcodeid_txt.AutoCompleteCustomSource = col;
            conn.Close();
        }
        //item details

        public void Grideloaditem()
        {

            dataGridView3.Rows.Clear();
            string sql = "select * from items";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            conn.Open();
            MySqlDataReader reder = cmd.ExecuteReader();


            while (reder.Read())
            {
                dataGridView3.Rows.Add(reder[1], reder[2], reder[3], reder[4], reder[5], reder[6], reder[7], reder[8], reder[9]);

            }
            conn.Close();

        }

        public void brandcompbload()
        {
            bname_combo.Items.Clear();
            try
            {

                string selectQuery = "SELECT * FROM brand";
                conn.Open();
                MySqlCommand command = new MySqlCommand(selectQuery, conn);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    bname_combo.Items.Add(reader.GetString("name"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            conn.Close();
        }

        private void Item_Store_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.End)
            {
                cnclw_btn.PerformClick();

            }
        }

        private void iid_txt_TextChanged_1(object sender, EventArgs e)
        {
            if (iid_txt.Focus())
            {
                if (iid_txt.Text == "")
                {
                    Grideloaditem();
                    iname_txt.Text = "";
                    barcodeid_txt.Text = "";
                    idec_txt.Text = "";
                    bname_combo.Text = "";
                    qty_txt.Text = "";
                    saleprice_txt.Text = "";
                    realprice_txt.Text = "";
                    pictureBox1.Image = null;
                }
                else
                {

                    try
                    {
                        dataGridView3.Rows.Clear();
                        string sql = "select * from items where item_id ='" + iid_txt.Text + "' ";
                        MySqlCommand cmd = new MySqlCommand(sql, conn);
                        conn.Open();
                        MySqlDataReader reder = cmd.ExecuteReader();

                        while (reder.Read())
                        {
                            dataGridView3.Rows.Add(reder[1], reder[2], reder[3], reder[4], reder[5], reder[6], reder[7], reder[8], reder[9]);

                            iname_txt.Text = (reder["name"].ToString());
                            barcodeid_txt.Text = (reder["barcode_id"].ToString());
                            bname_combo.Text = (reder["brand"].ToString());
                            qty_txt.Text = (reder["qty"].ToString());
                            realprice_txt.Text = (reder["price_real"].ToString());
                            saleprice_txt.Text = (reder["price_sale"].ToString());
                            idec_txt.Text = (reder["description"].ToString());

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
        }

        private void isave_btn_Click_1(object sender, EventArgs e)
        {

            if (iid_txt.Text == "")
            {
                MessageBox.Show("Please enter item id...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (barcodeid_txt.Text == "")
            {
                MessageBox.Show("Please enter Barcode id...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (iname_txt.Text == "")
            {
                MessageBox.Show("Please enter item name...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else if (qty_txt.Text == "")
            {
                MessageBox.Show("Please enter item quentity...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else if (realprice_txt.Text == "")
            {
                MessageBox.Show("Please enter item Real price...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else if (saleprice_txt.Text == "")
            {
                MessageBox.Show("Please enter item Sale price...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            
            else
            {

                try
                {

                    MemoryStream ms = new MemoryStream();
                    pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                    byte[] img = ms.ToArray();
                    ms.Close();

                    string insertquery = "INSERT INTO items(item_id,barcode_id,name,brand,qty,price_real,price_sale,description,image) VALUES (@item_id,@barcode_id,@name,@brand,@qty,@price_real,@price_sale,@description,@image)";
                    MySqlCommand cmd = new MySqlCommand(insertquery, conn);

                    cmd.Parameters.AddWithValue("@item_id", iid_txt.Text);
                    cmd.Parameters.AddWithValue("@barcode_id", barcodeid_txt.Text);
                    cmd.Parameters.AddWithValue("@name", iname_txt.Text);
                    cmd.Parameters.AddWithValue("@brand", bname_combo.SelectedItem);
                    cmd.Parameters.AddWithValue("@qty", qty_txt.Text);
                    cmd.Parameters.AddWithValue("@price_real", realprice_txt.Text);
                    cmd.Parameters.AddWithValue("@price_sale", saleprice_txt.Text);
                    cmd.Parameters.AddWithValue("@description", idec_txt.Text);
                    cmd.Parameters.AddWithValue("@image", img);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record is added...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    Grideloaditem();
                    autovalueadded();
                    autovalueaddedBar();
                    Main_Menu mn = new Main_Menu();
                    mn.autovalueaddedID();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            conn.Close();
        }

        private void iupdate_btn_Click_1(object sender, EventArgs e)
        {

            if (iid_txt.Text == "")
            {
                MessageBox.Show("Please enter item id...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (barcodeid_txt.Text == "")
            {
                MessageBox.Show("Please enter Barcode id...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (iname_txt.Text == "")
            {
                MessageBox.Show("Please enter item name...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else if (qty_txt.Text == "")
            {
                MessageBox.Show("Please enter item quentity...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else if (realprice_txt.Text == "")
            {
                MessageBox.Show("Please enter item Real price...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else if (saleprice_txt.Text == "")
            {
                MessageBox.Show("Please enter item Sale price...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
            {

                try
                {
                    MemoryStream ms = new MemoryStream();
                    pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                    byte[] img = ms.ToArray();
                    ms.Close();

                    string insertquery = "UPDATE items SET item_id=@item_id,barcode_id=@barcode_id,name=@name,brand=@brand, qty=@qty,price_real=@price_real,price_sale=@price_sale,description=@description,image=@image WHERE id='" + iid_txt.Text + "'";
                    MySqlCommand cmd = new MySqlCommand(insertquery, conn);

                    cmd.Parameters.AddWithValue("@item_id", iid_txt.Text);
                    cmd.Parameters.AddWithValue("@barcode_id", barcodeid_txt.Text);
                    cmd.Parameters.AddWithValue("@name", iname_txt.Text);
                    cmd.Parameters.AddWithValue("@brand", bname_combo.SelectedItem);
                    cmd.Parameters.AddWithValue("@qty", qty_txt.Text);
                    cmd.Parameters.AddWithValue("@price_real", realprice_txt.Text);
                    cmd.Parameters.AddWithValue("@price_sale", saleprice_txt.Text);
                    cmd.Parameters.AddWithValue("@description", idec_txt.Text);
                    cmd.Parameters.AddWithValue("@image", img);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record is updated...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    Grideloaditem();
                    autovalueadded();
                    autovalueaddedBar();
                    Main_Menu mn = new Main_Menu();
                    mn.autovalueaddedID();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            conn.Close();
        }

        private void idelete_btn_Click_1(object sender, EventArgs e)
        {
            if (iid_txt.Text == "")
            {
                MessageBox.Show("Please enter item id...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {

                try
                {
                    string insertquery = "DELETE FROM items WHERE id='" + iid_txt.Text + "'";
                    MySqlCommand cmd = new MySqlCommand(insertquery, conn);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record is deleted...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    Grideloaditem();
                    autovalueadded();
                    autovalueaddedBar();
                    Main_Menu mn = new Main_Menu();
                    mn.autovalueaddedID();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            conn.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog opd = new OpenFileDialog();
            opd.Filter = "Choose Image(*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif";
            if (opd.ShowDialog()== DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(opd.FileName);
            }


        }

        private void label1_Click(object sender, EventArgs e)
        {

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

        private void barcodeid_txt_TextChanged(object sender, EventArgs e)
        {
            /*
            if (barcodeid_txt.Focus())
            {
                if (barcodeid_txt.Text == "")
                {

                    Grideloaditem();
                    iname_txt.Text = "";
                    iid_txt.Text = "";
                    idec_txt.Text = "";
                    bname_combo.Text = "";
                    qty_txt.Text = "";
                    saleprice_txt.Text = "";
                    realprice_txt.Text = "";
                    pictureBox1.Image = null;
                }
                else
                {

                    try
                    {
                        dataGridView3.Rows.Clear();
                        string sql = "select * from items where barcode_id ='" + barcodeid_txt.Text + "' ";
                        MySqlCommand cmd = new MySqlCommand(sql, conn);
                        conn.Open();
                        MySqlDataReader reder = cmd.ExecuteReader();

                        while (reder.Read())
                        {
                            dataGridView3.Rows.Add(reder[1], reder[2], reder[3], reder[4], reder[5], reder[6], reder[7], reder[8], reder[9]);

                            iname_txt.Text = (reder["name"].ToString());
                            iid_txt.Text = (reder["item_id"].ToString());
                            bname_combo.Text = (reder["brand"].ToString());
                            qty_txt.Text = (reder["qty"].ToString());
                            realprice_txt.Text = (reder["price_real"].ToString());
                            saleprice_txt.Text = (reder["price_sale"].ToString());
                            idec_txt.Text = (reder["description"].ToString());

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
            }*/
        }
        /*
private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
{
if (e.RowIndex >= 0)
{
//gets a collection that contains all the rows
DataGridViewRow row = this.dataGridView3.Rows[e.RowIndex];
//populate the textbox from specific value of the coordinates of column and row.
iid_txt.Text = row.Cells[0].Value.ToString();
iname_txt.Text = row.Cells[1].Value.ToString();
bname_combo.Text = row.Cells[2].Value.ToString();
qty_txt.Text = row.Cells[3].Value.ToString();
price_txt.Text = row.Cells[4].Value.ToString();
idec_txt.Text = row.Cells[5].Value.ToString();
}
}*/
    }
}
