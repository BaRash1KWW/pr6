using System.Windows.Controls;

namespace WpfApp6.pages
{
    public partial class Client : Page
    {
        public Client(object user, string role) 
        {
            InitializeComponent();

            if (user == null)
                tbWelcome.Text = $"Вы вошли как {role}";
            else
                tbWelcome.Text = $"Добро пожаловать! Роль: {role}";
        }
    }
}