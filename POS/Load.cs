using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace POS
{
    public partial class Load : Form
    {
        public Load()
        {
            InitializeComponent();
        }

        private void Load_Load(object sender, EventArgs e)
        {
            load_lbl.BackColor = System.Drawing.Color.Transparent;
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            loading_pgrsbar.Value = loading_pgrsbar.Value + 1;
            if (loading_pgrsbar.Value >= 100)
            {
                Login lg = new Login();
                lg.Show();
                this.Hide();
                timer1.Enabled = true;
                loading_pgrsbar.Enabled = false;
                timer1.Enabled = false;

            }
        }
    }
}
