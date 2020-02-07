using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS
{
    public partial class BillPrint : Form 
    {
        public BillPrint()
        {
            InitializeComponent();
        }
        int mov;
        int movX;
        int movY;

        public int A = 5;
        public int B = 5;
        public int C = 5;

        public System.Windows.Forms.Label AddNewLabel(string text, int left, int width, int top, int x)
        {

            System.Windows.Forms.Label lbl1 = new System.Windows.Forms.Label();
            lbl1.AutoEllipsis = true;
            lbl1.AutoSize = false;
            lbl1.Width = width;
            lbl1.Height = 14;

            lbl1.Font = new Font("Fake Receipt", 8);
            lbl1.Name = "Label" + x;

            lbl1.Top = top;

            lbl1.ForeColor = Color.Black;
            lbl1.BackColor = Color.Transparent;
            lbl1.Left = left;
            lbl1.Text = text;
            //lbl1.TextAlign = ContentAlignment.MiddleLeft;
            this.panel1.Controls.Add(lbl1);

            return lbl1;

        }

       
        private void cnclw_btn_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (mov == 1)
            {
                this.SetDesktopLocation(MousePosition.X - movX, MousePosition.Y - movY);
            }
        }

        private void panel2_MouseUp(object sender, MouseEventArgs e)
        {
            mov = 0;
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            mov = 1;
            movX = e.X;
            movY = e.Y;
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
           
            
            Bitmap bm = new Bitmap(this.panel1.Width, this.panel1.Height);        
            panel1.DrawToBitmap(bm, new Rectangle(0, 0, this.panel1.Width, this.panel1.Height));
            e.Graphics.DrawImage(bm, 0, 0);
            PageSetupDialog ps = new PageSetupDialog();
            ps.Document = printDocument1;
        }
    }
}
