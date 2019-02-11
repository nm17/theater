using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TheaterTickets
{
    public partial class PlaceChooser : Form
    {
        Button main;
        int row = 0;
        int place = 0;

        void ClickAction(object sender, EventArgs e)
        {
            MessageBox.Show(((List<int>)((Button)sender).Tag)[0].ToString());
        }

        public PlaceChooser(int rows, int places)
        {
            InitializeComponent();
            for (int r = 0, p = 0, i = rows * places; i != 0; i--)
            {
                for (; p != places; p++)
                {
                    Button test = new Button
                    {
                        Tag = new List<int> { r, p }
                    };
                    test.Click += ClickAction;
                    flowLayoutPanel1.Controls.Add(test);
                }
                r++;
            }
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flowLayoutPanel1_Click(object sender, EventArgs e)
        {

        }
    }
}
