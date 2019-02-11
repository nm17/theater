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
            try
            {
                Place info = api.GetInfo(row, place);
                BuyTicket.Enabled = info.free;
                Price.Text = string.Format("Цена: {0}", info.price.ToString());
            } catch (ArgumentNullException)
            {
                MessageBox.Show("Сервер не работает");
                Close();
            }
        }

        /*private void BuyTicket_Click(object sender, EventArgs e)
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
        }*/

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
            try
            {
                api.Login(UsernameTextBox.Text, PasswordText.Text);
                if (api.IsAdmin)
                {
                    var admin_form = new AdminForm(api);
                    admin_form.Show();
                }
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Сервер не работает");
                Close();
            } catch (WrongLoginException)
            {
                MessageBox.Show("Не верный логин");
            }
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            try
            {
                api.Register(UsernameTextBox.Text, PasswordText.Text);
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Сервер не работает");
                Close();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void BuyTicket_Click(object sender, EventArgs e)
        {
            try
            {
                api.Book((int) PlaceNumeretic1.Value, (int) PlaceNumeretic2.Value);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Войдите под своим логином");
                return;
            }
            MessageBox.Show("Вы успешно купили билет!");
        }
    }
}