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
                var abc = new PlaceChooser(api);
                abc.ShowDialog();
                api.Book(abc.row, abc.place);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Войдите под своим логином");
                return;
            }
            catch (PlaceAlreadyBookedException)
            {
                MessageBox.Show("Уже забронированно");
                return;
            }
            MessageBox.Show("Вы успешно купили билет!");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }
    }
}