using System;
using System.Data.SqlClient; 
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using WpfApp6.Services;

namespace WpfApp6.pages
{
    public partial class Autho : Page
    {
        private int click;
        private string captchaText;

        private string connectionString = @"Server=localhost\SQLEXPRESS;Database=BakeryDB_1;Trusted_Connection=True;";

        public Autho()
        {
            InitializeComponent();
            click = 0;
        }

        private void btnEnterGuest_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Client(null, "Гость"));
        }

        private void GenerateCaptcha()
        {
            tblCaptcha.Visibility = Visibility.Visible;
            tbCaptcha.Visibility = Visibility.Visible;

            captchaText = CaptchaGenerator.GenerateCaptchaText(6);
            tblCaptcha.Text = captchaText;
            tblCaptcha.TextDecorations = TextDecorations.Strikethrough;
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            click += 1;
            string login = tbLogin.Text.Trim();
            string password = tbPassword.Text.Trim();

            bool userIsValid = CheckUserInDatabase(login, password);

            if (click == 1)
            {
                if (userIsValid)
                {
                    MessageBox.Show($"Вы вошли под: {login}");
                    LoadPage("Клиент");
                }
                else
                {
                    MessageBox.Show("Вы ввели логин или пароль неверно!");
                    tbPassword.Clear();
                    GenerateCaptcha();
                }
            }
            else if (click > 1)
            {
                if (userIsValid && tbCaptcha.Text == captchaText)
                {
                    MessageBox.Show($"Вы вошли под: {login}");
                    LoadPage("Клиент");
                }
                else
                {
                    MessageBox.Show("Введите данные заново!");
                    tbPassword.Clear();
                    tbCaptcha.Clear();
                    click = 0;
                    tblCaptcha.Visibility = Visibility.Collapsed;
                    tbCaptcha.Visibility = Visibility.Collapsed;
                }
            }
        }

        private bool CheckUserInDatabase(string login, string password)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM Users WHERE Login = @Login AND PasswordHash = @Password";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Login", login);
                        command.Parameters.AddWithValue("@Password", password);

                        int count = (int)command.ExecuteScalar();
                        return count > 0; 
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения к БД: " + ex.Message);
                return false;
            }
        }

        private void LoadPage(string role)
        {
            click = 0;
            NavigationService.Navigate(new Client(null, role));
        }
    }
}