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
    public partial class Main_Menu : Form
    {

        int mov;
        int movX;
        int movY;
        int itemcount = 0;
        public Main_Menu()
        {
            InitializeComponent();
            timer1.Start();
            autovalueadded();
            autovalueaddedID();
        }

        //get connecttion
        MySqlConnection conn = new MySqlConnection("server=localhost;database=pos;userid=root;password=;");
        private void Main_Menu_Load(object sender, EventArgs e)
        {
            this.ActiveControl = itemno_txt;
            Transparency();
            autoID();

        }

        //automatically id inserting method
        public void autoID()
        {

            String invo_ID;
            String sql = "select inv_id from sales order by inv_id desc";
            conn.Open();

            MySqlCommand commandDatabase = new MySqlCommand(sql, conn);
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader = commandDatabase.ExecuteReader();

            if (reader.Read())
            {

                int inv_id = int.Parse(reader[0].ToString()) + 1;
                invo_ID = inv_id.ToString("00000");

            }
            else if (Convert.IsDBNull(reader))
            {

                invo_ID = "00001";

            }
            else
            {

                invo_ID = "00001";

            }

            invoceno_lbl.Text = invo_ID.ToString();
            conn.Close();

        }
        public void Transparency()
        {
            category_btn_img.BackColor = System.Drawing.Color.Transparent;
            brand_btn_img.BackColor = System.Drawing.Color.Transparent;
            item_btn_imag.BackColor = System.Drawing.Color.Transparent;
            sale_btn_img.BackColor = System.Drawing.Color.Transparent;
            rerort_btn_img.BackColor = System.Drawing.Color.Transparent;
            custom_btn_img.BackColor = System.Drawing.Color.Transparent;
            label1.BackColor = System.Drawing.Color.Transparent;
            time_lbl.BackColor = System.Drawing.Color.Transparent;
            groupBox1.BackColor = System.Drawing.Color.Transparent;
            groupBox2.BackColor = System.Drawing.Color.Transparent;
           
            bank_btn_img.BackColor = System.Drawing.Color.Transparent;
            pl_report_btn_img.BackColor = System.Drawing.Color.Transparent;

        }


        private void category_btn_img_MouseLeave(object sender, EventArgs e)
        {
            category_btn_img.Size = new Size(50, 50);

        }

        private void category_btn_img_MouseEnter(object sender, EventArgs e)
        {
            category_btn_img.Size = new Size(60, 60);

        }

        private void Main_Menu_KeyDown(object sender, KeyEventArgs e)
        {
            // go to the category form
            if (e.KeyCode == Keys.F1)
            {
                sendcategory();

            }
            // form exit
            else if (e.KeyCode == Keys.End)
            {
                cnclw_btn.PerformClick();

            }
            // go to the brand form
            else if (e.KeyCode == Keys.F2)
            {
                sendBrand();

            }
            // go to the item store form
            else if (e.KeyCode == Keys.F3)
            {
                Item_StoreSend();

            }
            // go to the sales details form
            else if (e.KeyCode == Keys.F4)
            {
                Sales sl = new Sales();
                sl.Show();
            }
            // go to the customer details form
            else if (e.KeyCode == Keys.F5)
            {
                Customer cm = new Customer();
                cm.Show();
            }
            // go to the report form
            else if (e.KeyCode == Keys.F6)
            {
                Report rp = new Report();
                rp.Show();
            }
            // payment calculation focus 
            else if (e.Control && e.KeyCode == Keys.M)
            {
                itemno_txt.Text = "";
                qt_txt.Text = "";
                this.payment_txt.Focus();
            }
            // item enter focus
            else if (e.Control && e.KeyCode == Keys.I)
            {
                this.itemno_txt.Focus();
                this.payment_txt.Text = "";
                this.qt_txt.Text = "";

            }
            // added item remove
            else if (e.Control && e.KeyCode == Keys.D || e.Control && e.KeyCode == Keys.Delete)
            {

                int oldQTY = 0;
                int index;

                try
                {
                    //get current row field number
                    index = sales_gride.CurrentRow.Index;
                    
                    //database searching
                    string sql = "select * from items where item_id like '" + sales_gride.Rows[index].Cells[1].Value + "' || barcode_id like '" + sales_gride.Rows[index].Cells[2].Value + "' ";
                    MySqlCommand cmd1 = new MySqlCommand(sql, conn);
                    conn.Open();
                    MySqlDataReader reder = cmd1.ExecuteReader();

                    while (reder.Read())
                    {
                        // get and set value
                        oldQTY = int.Parse(reder["qty"].ToString());

                    }
                    conn.Close();

                    // devition item count
                    //set number of items                       
                    itemcount = itemcount - int.Parse(sales_gride.Rows[index].Cells[5].Value.ToString());
                    itemQty_lbl.Text = itemcount.ToString();

                    // update database
                    int x = oldQTY + int.Parse(sales_gride.Rows[index].Cells[5].Value.ToString());
                    string insertquery = "UPDATE items SET qty=@qty WHERE  item_id like '" + sales_gride.Rows[index].Cells[1].Value + "' || barcode_id like '" + sales_gride.Rows[index].Cells[2].Value + "'";
                    MySqlCommand cmd = new MySqlCommand(insertquery, conn);
                    string passX = x.ToString();
                    conn.Open();

                    cmd.Parameters.AddWithValue("@qty", passX);
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    // remove row in datagride view
                    sales_gride.Rows.RemoveAt(index);

                    //make total  
                    int total = 0;

                    for (int i = 0; i < sales_gride.Rows.Count; i++)
                    {
                        int xy = int.Parse(sales_gride.Rows[i].Cells[6].Value.ToString());
                        total = total + xy;
                    }
                    //set total
                    total_lbl.Text = total.ToString();

                    // price calculation
                    int payment = int.Parse(payment_lbl.Text.Trim());
                    int payble = payment - int.Parse(total_lbl.Text.ToString());
                    balance_lbl.Text = payble.ToString();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                conn.Close();

            }
            // go to the print form
            else if (e.Control && e.KeyCode == Keys.P)
            {
               
                if (pay_methd_lbl.Text == "Borrower")
                {
                    if (cus_id_txt.Text == "")
                    {
                        MessageBox.Show("Please enter customer id...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        saveCustomerBill();
                        sendPrintDetails();
                        this.itemno_txt.Focus();
                    }
                }
                else
                {
                    sendPrintDetails();
                    this.itemno_txt.Focus();
                }

            }
            // select the data gride view
            else if (e.Control && e.KeyCode == Keys.Down)
            {

                sales_gride.Select();
            }

        }
        public void Item_StoreSend()
        {

            Item_Store iss = new Item_Store();
            iss.Show();

        }
        public void sendcategory()
        {
            category cg = new category();
            cg.Show();

        }


        private void cnclw_btn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you want to exit..", "POS Info", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void brand_btn_img_MouseEnter(object sender, EventArgs e)
        {
            brand_btn_img.Size = new Size(60, 60);
        }

        private void brand_btn_img_MouseLeave(object sender, EventArgs e)
        {
            brand_btn_img.Size = new Size(50, 50);
        }

        private void item_btn_imag_MouseEnter(object sender, EventArgs e)
        {
            item_btn_imag.Size = new Size(60, 60);
        }

        private void item_btn_imag_MouseLeave(object sender, EventArgs e)
        {
            item_btn_imag.Size = new Size(50, 50);
        }

        private void item_btn_imag_Click(object sender, EventArgs e)
        {
            Item_StoreSend();
        }

        private void brand_btn_img_Click(object sender, EventArgs e)
        {
            sendBrand();
        }
        public void sendBrand()
        {
            brand br = new brand();
            br.Show();

        }

        private void category_btn_img_Click_1(object sender, EventArgs e)
        {
            sendcategory();
        }

        private void sale_btn_img_MouseEnter(object sender, EventArgs e)
        {
            sale_btn_img.Size = new Size(60, 60);
        }

        private void sale_btn_img_MouseLeave(object sender, EventArgs e)
        {
            sale_btn_img.Size = new Size(50, 50);
        }

        private void rerort_btn_img_MouseEnter(object sender, EventArgs e)
        {
            rerort_btn_img.Size = new Size(60, 60);
        }

        private void rerort_btn_img_MouseLeave(object sender, EventArgs e)
        {
            rerort_btn_img.Size = new Size(50, 50);
        }

        private void custom_btn_img_MouseLeave(object sender, EventArgs e)
        {
            custom_btn_img.Size = new Size(50, 50);
        }

        private void custom_btn_img_MouseEnter(object sender, EventArgs e)
        {
            custom_btn_img.Size = new Size(60, 60);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            time_lbl.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
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

        private void bank_btn_img_MouseLeave(object sender, EventArgs e)
        {
            bank_btn_img.Size = new Size(50, 50);
        }

        private void bank_btn_img_MouseEnter(object sender, EventArgs e)
        {
            bank_btn_img.Size = new Size(60, 60);
        }

        private void pl_report_btn_img_MouseLeave(object sender, EventArgs e)
        {
            pl_report_btn_img.Size = new Size(50, 50);
        }

        private void pl_report_btn_img_MouseEnter(object sender, EventArgs e)
        {
            pl_report_btn_img.Size = new Size(60, 60);
        }

        private void itemno_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                this.qt_txt.Focus();

            }
        }

        private void qt_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (qt_txt.Text == "")
            {
                if (e.KeyCode == Keys.Back)
                {
                    this.itemno_txt.Focus();

                }
            }
            else
            {

                if (e.KeyCode == Keys.Enter)
                {

                    try
                    {
                        //get qty
                        int oldQTY = 0;

                        //select item in database
                        string sql = "select * from items where item_id like '" + itemno_txt.Text + "' || barcode_id like '" + itemno_txt.Text + "' ";
                        MySqlCommand cmd1 = new MySqlCommand(sql, conn);
                        conn.Open();
                        MySqlDataReader reder = cmd1.ExecuteReader();

                        while (reder.Read())
                        {

                            oldQTY = int.Parse(reder["qty"].ToString());

                        }
                        conn.Close();

                        if (oldQTY == 0)
                        {

                            MessageBox.Show("Item lot is end...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                        else if (oldQTY < int.Parse(qt_txt.Text.ToString()))
                        {
                            MessageBox.Show("Your Item Qty is low...having items qty is : " + oldQTY, "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            //find item details in database
                            string querySearching = "select * from items where item_id ='" + itemno_txt.Text + "'|| barcode_id = '" + itemno_txt.Text + "'";
                            MySqlCommand cmd = new MySqlCommand(querySearching, conn);
                            conn.Open();

                            MySqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                //calculate prise
                                int price = int.Parse(qt_txt.Text.Trim()) * int.Parse(reader[7].ToString());

                                //set values in datagride view 
                                sales_gride.Rows.Add(sales_gride.RowCount, reader[1], reader[2], reader[3], reader[7], qt_txt.Text.Trim(), price);

                            }
                            conn.Close();

                            //make total  
                            int total = 0;

                            for (int i = 0; i < sales_gride.Rows.Count; i++)
                            {
                                int xy = int.Parse(sales_gride.Rows[i].Cells[6].Value.ToString());
                                total = total + xy;
                            }
                            //set total
                            total_lbl.Text = total.ToString();

                            //set number of items                       
                            itemcount += int.Parse(qt_txt.Text);
                            itemQty_lbl.Text = itemcount.ToString();


                            // make new sale item count
                            int x = oldQTY - int.Parse(qt_txt.Text);

                            // database update
                            string insertquery = "UPDATE items SET qty=@qty WHERE item_id ='" + itemno_txt.Text + "'|| barcode_id = '" + itemno_txt.Text + "'";
                            MySqlCommand cmd2 = new MySqlCommand(insertquery, conn);
                            string passX = x.ToString();

                            conn.Open();

                            cmd2.Parameters.AddWithValue("@qty", passX);

                            cmd2.ExecuteNonQuery();
                            conn.Close();
                        }


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);

                    }
                    conn.Close();

                    //text process
                    this.itemno_txt.Focus();
                    qt_txt.Text = "";
                    itemno_txt.Text = "";

                }

            }

        }

        private void payment_txt_TextChanged(object sender, EventArgs e)
        {
            // text process
            if (payment_txt.Text == "")
            {

                payment_lbl.Text = "0";
                balance_lbl.Text = "0";

            }
            else
            {
                // calculate payment process
                try
                {
                    // pass payment label to payment text value
                    payment_lbl.Text = payment_txt.Text;

                    // calculating the payment 
                    int payment = int.Parse(payment_lbl.Text.Trim());
                    int payble = payment - int.Parse(total_lbl.Text.ToString());

                    // set calculate over value in the balance label
                    balance_lbl.Text = payble.ToString();

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }

            }


        }

        private void payment_txt_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {

                this.pay_method_cmbo.Focus();

            }
        }


        public void sendPrintDetails()
        {
            string time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            try
            {
                if (pay_methd_lbl.Text != ".......................")
                {
                    string insertquery = "INSERT INTO sales(inv_id,numberOfItems,pay_method,total,payment,balance) VALUES (@inv_id,@numberOfItems,@pay_method,@total,@payment,@balance)";
                    MySqlCommand cmd = new MySqlCommand(insertquery, conn);

                    cmd.Parameters.AddWithValue("@inv_id", invoceno_lbl.Text);
                    cmd.Parameters.AddWithValue("@numberOfItems", itemQty_lbl.Text);
                    cmd.Parameters.AddWithValue("@pay_method", pay_methd_lbl.Text);
                    cmd.Parameters.AddWithValue("@total", total_lbl.Text);
                    cmd.Parameters.AddWithValue("@payment", payment_lbl.Text);
                    cmd.Parameters.AddWithValue("@balance", balance_lbl.Text);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Bill is processing...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();

                    BillPrint pr = new BillPrint();
                    pr.name_lblb.Text = "Company Name" + "\n" + "Address Line 1" + "\n" + "Address Line 2" + "\n" + "Phone Number";
                    pr.invo_no_lbl.Text = "Invoice No : " + invoceno_lbl.Text;

                    //make line,
                    pr.AddNewLabel("",0,0, 90, pr.A = pr.A +1).TextAlign = ContentAlignment.MiddleCenter;
                    pr.AddNewLabel("____________________________________", 0, 316, 90, pr.B = pr.B + 1).TextAlign = ContentAlignment.MiddleCenter;// Anchor = (AnchorStyles.Left & AnchorStyles.Right); 
                    pr.AddNewLabel("", 0, 0, 90, pr.C = pr.C + 1).TextAlign = ContentAlignment.MiddleCenter;

                    /////string text, int left, int width, int top, int x

                    pr.AddNewLabel("Item", 8, 90, pr.A * 20, pr.A = pr.A + 1).TextAlign = ContentAlignment.MiddleLeft;
                    pr.AddNewLabel("P x Q", 110, 90, pr.B * 20, pr.B = pr.B + 1).TextAlign = ContentAlignment.MiddleCenter;
                    pr.AddNewLabel("Rs.", 210, 90, pr.C * 20, pr.C = pr.C + 1).TextAlign = ContentAlignment.MiddleRight;

                    //make line,
                    pr.AddNewLabel("", 0, 0, pr.A * 20, pr.A = pr.A + 1).TextAlign = ContentAlignment.MiddleCenter;
                    pr.AddNewLabel("____________________________________", 0, 316, pr.B * 20, pr.B = pr.B + 1).TextAlign = ContentAlignment.MiddleCenter;// Anchor = (AnchorStyles.Left & AnchorStyles.Right); 
                    pr.AddNewLabel("", 0, 0, pr.C * 20, pr.C = pr.C + 1).TextAlign = ContentAlignment.MiddleCenter;

                    for (int i = 0; i < sales_gride.RowCount; i++)
                    {

                        pr.AddNewLabel(sales_gride.Rows[i].Cells[3].Value.ToString(), 8, 90, pr.A * 20, pr.A = pr.A + 1).TextAlign = ContentAlignment.MiddleLeft;
                        pr.AddNewLabel(sales_gride.Rows[i].Cells[4].Value.ToString() + " x " + sales_gride.Rows[i].Cells[5].Value.ToString(), 110, 90, pr.B * 20, pr.B = pr.B + 1).TextAlign = ContentAlignment.MiddleCenter; 
                        pr.AddNewLabel(sales_gride.Rows[i].Cells[6].Value.ToString(), 210, 90, pr.C * 20, pr.C = pr.C + 1).TextAlign = ContentAlignment.MiddleRight;

                    }

                    //for space
                    /*pr.AddNewLabel("", 8);
                    pr.AddNewLabel2("", 100, 120);
                    pr.AddNewLabel1("", 200, 120);
                    */

                    //make line,
                    pr.AddNewLabel("", 0, 0, pr.A * 20, pr.A = pr.A + 1).TextAlign = ContentAlignment.MiddleCenter;
                    pr.AddNewLabel("____________________________________", 0, 316, pr.B * 20, pr.B = pr.B + 1).TextAlign = ContentAlignment.MiddleCenter;// Anchor = (AnchorStyles.Left & AnchorStyles.Right); 
                    pr.AddNewLabel("", 0, 0, pr.C * 20, pr.C = pr.C + 1).TextAlign = ContentAlignment.MiddleCenter;

                    /////string text, int left, int width, int top, int x
                    //set name
                    pr.AddNewLabel("No of Items", 8, 90, pr.A * 20, pr.A = pr.A + 1).TextAlign = ContentAlignment.MiddleLeft;
                    pr.AddNewLabel("Pay Method", 8, 90, pr.A * 20, pr.A = pr.A + 1).TextAlign = ContentAlignment.MiddleLeft;;
                    pr.AddNewLabel("Total", 8, 90, pr.A * 20, pr.A = pr.A + 1).TextAlign = ContentAlignment.MiddleLeft; ;
                    pr.AddNewLabel("Payment", 8, 90, pr.A * 20, pr.A = pr.A + 1).TextAlign = ContentAlignment.MiddleLeft; ;
                    pr.AddNewLabel("Balance", 8, 90, pr.A * 20, pr.A = pr.A + 1).TextAlign = ContentAlignment.MiddleLeft; ;

                    //set value
                    pr.AddNewLabel(itemQty_lbl.Text, 210, 90, pr.B * 20, pr.B = pr.B + 1).TextAlign = ContentAlignment.MiddleRight;
                    pr.AddNewLabel(pay_methd_lbl.Text, 210, 90, pr.B * 20, pr.B = pr.B + 1).TextAlign = ContentAlignment.MiddleRight; 
                    pr.AddNewLabel(total_lbl.Text, 210, 90, pr.B * 20, pr.B = pr.B + 1).TextAlign = ContentAlignment.MiddleRight;
                    pr.AddNewLabel(payment_lbl.Text, 210, 90, pr.B * 20, pr.B = pr.B + 1).TextAlign = ContentAlignment.MiddleRight;
                    pr.AddNewLabel(balance_lbl.Text, 210, 90, pr.B * 20, pr.B = pr.B + 1).TextAlign = ContentAlignment.MiddleRight;

                    //set text  
                    pr.AddNewLabel("===========++++++++++++++==========", 0, 316, pr.A * 20, pr.A = pr.A + 1).TextAlign = ContentAlignment.MiddleCenter;
                    pr.AddNewLabel(time, 0, 316, pr.A * 20, pr.A = pr.A + 1).TextAlign = ContentAlignment.MiddleCenter;
                    pr.AddNewLabel("Thank You Come Again...", 0, 316, pr.A * 20, pr.A = pr.A + 1).TextAlign = ContentAlignment.MiddleCenter;

                    //set space
                    pr.panel1.Height = pr.panel1.Height + 40;
                    pr.Height = pr.panel1.Height + 20;

                    // show print form
                    pr.Show();
                    pr.printDocument1.Print();
                    autoID();
                    allclear();

                }
                else
                {
                    MessageBox.Show("Please select payment method...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            conn.Close();

        }

        public void allclear()
        {
            itemno_txt.Text = "";
            qt_txt.Text = "";
            payment_txt.Text = "";
            pay_method_cmbo.Text = "";

            itemQty_lbl.Text = "0";
            pay_methd_lbl.Text = ".......................";
            total_lbl.Text = "0";
            payment_lbl.Text = "0";
            balance_lbl.Text = "0";
            itemcount = 0;
            sales_gride.Rows.Clear();
        }

        private void done_btn_Click_1(object sender, EventArgs e)
        {
            if (pay_methd_lbl.Text == "Borrower")
            {
                if (cus_id_txt.Text == "")
                {
                    MessageBox.Show("Please enter customer id...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    saveCustomerBill();
                    sendPrintDetails();
                    this.itemno_txt.Focus();
                }
            }else
            {
               sendPrintDetails();
                this.itemno_txt.Focus();
            }


        }
        public void saveCustomerBill()
        {
            
                 try
                    {

                        string insertquery = "INSERT INTO customer_invoice(inv_id, cus_id, total, payment, balance) VALUES (@inv_id, @cus_id, @total, @payment, @balance)";
                        MySqlCommand cmd = new MySqlCommand(insertquery, conn);

                        cmd.Parameters.AddWithValue("@inv_id", invoceno_lbl.Text);
                        cmd.Parameters.AddWithValue("@cus_id", cus_id_txt.Text);
                        cmd.Parameters.AddWithValue("@total", total_lbl.Text);
                        cmd.Parameters.AddWithValue("@payment", payment_lbl.Text);
                        cmd.Parameters.AddWithValue("@balance", balance_lbl.Text);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        //MessageBox.Show("Record is added...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        conn.Close();

                        string insertquery1 = "INSERT INTO  bill_details(inv_id, item_id,barcode_id,price, qty, total) VALUES (@inv_id,@item_id,	@barcode_id @price, @qty, @total)";
                        MySqlCommand cmd1 = new MySqlCommand(insertquery1, conn);

                        for (int i = 0; i < sales_gride.Rows.Count; i++)
                        {

                            cmd1.Parameters.AddWithValue("@inv_id", invoceno_lbl.Text);
                            cmd1.Parameters.AddWithValue("@item_id", sales_gride.Rows[i].Cells[1].Value);
                            cmd1.Parameters.AddWithValue("@barcode_id", sales_gride.Rows[i].Cells[2].Value);
                            cmd1.Parameters.AddWithValue("@price", sales_gride.Rows[i].Cells[4].Value);
                            cmd1.Parameters.AddWithValue("@qty", sales_gride.Rows[i].Cells[5].Value);
                            cmd1.Parameters.AddWithValue("@total", sales_gride.Rows[i].Cells[6].Value);
                            conn.Open();
                            cmd1.ExecuteNonQuery();
                            //MessageBox.Show("Record is added...", "POS Info", MessageBoxButtons.OK, MessageBoxIcon.Information);  
                            conn.Close();
                            cmd1.Parameters.Clear();
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    conn.Close();
                
            

        }
        private void pay_method_cmbo_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Down)
            {
                pay_method_cmbo.DroppedDown = true;
            }


        }

        private void pay_method_cmbo_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (pay_method_cmbo.Text == "")
            {

                pay_methd_lbl.Text = ".......................";
                cus_id_txt.Enabled = false;
            }
            else
            {

                pay_methd_lbl.Text = pay_method_cmbo.Text;

            }

            if (pay_methd_lbl.Text == "Borrower")
            {

               
                cus_id_txt.Enabled = true;
                autovalueadded();
                cus_id_txt.Focus();
            }
            else
            {
                
                cus_id_txt.Enabled = false;
            }

        }

        private void sale_btn_img_Click(object sender, EventArgs e)
        {
            Sales sl = new Sales();
            sl.Show();
        }

        private void custom_btn_img_Click(object sender, EventArgs e)
        {
            Customer cm = new Customer();
            cm.Show();
        }

        public void autovalueadded()
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
            cus_id_txt.AutoCompleteCustomSource = col;
            conn.Close();
        }
        public void autovalueaddedID()
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
            itemno_txt.AutoCompleteCustomSource = col;
            conn.Close();

        }

        private void rerort_btn_img_Click(object sender, EventArgs e)
        {
            Report rp = new Report();
            rp.Show();
        }

        private void cus_id_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Cus_Searching cb = new Cus_Searching();
                cb.Show();
                cb.cid_txt.Focus();
                cb.cid_txt.Text = cus_id_txt.Text;
            }
        }
   
    }
    
}
