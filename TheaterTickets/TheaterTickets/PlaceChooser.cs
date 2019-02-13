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
        readonly int amount_row;
        readonly int amount_place;
        readonly int button_size = 25;
        public int row = 0;
        public int place = 0;
        readonly TheaterAPI api;


        void ClickAction(object sender, EventArgs e)
        {
            var button_tag = (List<int>)(((Button) sender).Tag);
            row = button_tag[0];
            place = button_tag[0];
            Close();
        }

        public PlaceChooser(TheaterAPI api)
        {
            InitializeComponent();
            this.api = api;
            var result = api.GetScheme();
            amount_row = result[0];
            amount_place = result[1];
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flowLayoutPanel1_Click(object sender, EventArgs e)
        {

        }

        private void PlaceChooser_Load(object sender, EventArgs e)
        {
            for (int r = 0; r != amount_row; r++)
            {
                for (int p = 0; p != amount_place; p++)
                {
                    var color = Color.Brown;
                    Place place = api.GetInfo(r, p);
                    if (place.free)
                    {
                        color = Color.Green;
                    }
                    Button test = new Button
                    {
                        Tag = new List<int> { r, p },
                        Size = new Size(button_size, button_size),
                        Location = new Point(button_size * r, button_size * p),
                        Text = place.price.ToString(),
                        BackColor = color
                    };
                    test.Click += ClickAction;
                    this.Controls.Add(test);
                }
            }
            this.Size = new Size((amount_row + 1) * button_size, (amount_place + 2) * button_size);
        }
    }
}
