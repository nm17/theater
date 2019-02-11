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
    public partial class AdminForm : Form
    {
        private readonly TheaterAPI api;

        public AdminForm(TheaterAPI api_)
        {
            InitializeComponent();
            api = api_;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            api.AddTicket((int) numericUpDown1.Value, (int) numericUpDown2.Value, (int) numericUpDown3.Value);
        }
    }
}
