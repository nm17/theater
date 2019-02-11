using System;
using System.Windows.Forms;

namespace TheaterTickets
{
    public partial class BuyTicketsForm : Form
    {
        TheaterAPI api = new TheaterAPI();

        public BuyTicketsForm()
        {
            InitializeComponent();
        }

        private void ValueChanged()
        {
            int row = (int) PlaceNumeretic1.Value;
            int place = (int) PlaceNumeretic2.Value;
            Place info = api.GetInfo(row, place);
            BuyTicket.Enabled = info.free;
            Price.Text = string.Format("Цена: {0}", info.price.ToString());
        }

        private void BuyTicket_Click(object sender, EventArgs e)
        {
            try
            {
                api.Book((int) PlaceNumeretic1.Value, (int) PlaceNumeretic2.Value);
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось купить билет");
                return;
            }
            MessageBox.Show("Вы успешно купили билет!");
        }

        private void PlaceNumeretic1_ValueChanged(object sender, EventArgs e)
        {
            ValueChanged();
        }

        private void PlaceNumeretic2_ValueChanged(object sender, EventArgs e)
        {
            ValueChanged();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            api.Login(UsernameTextBox.Text, PasswordText.Text);
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            api.Register(UsernameTextBox.Text, PasswordText.Text);
        }
    }
}