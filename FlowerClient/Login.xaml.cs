using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FlowerClient
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TryConnection();
                new MainWindow().Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void TryConnection()
        {
            string connectionString = "Server=" + txt_adress.Text.Trim() + ";Port=5432;User Id=" + txt_login.Text.Trim()
            + ";Password=" + txt_password.Password.Trim() + ";Database=flower;";
            Mediator.instance.Connection = new NpgsqlConnection(connectionString);
            Mediator.instance.Connection.Open();

            Mediator.instance.Login = txt_login.Text.Trim();
            Mediator.instance.SQL = "select show_role('" + Mediator.instance.Login + "');";
            Mediator.instance.Role = (string)Mediator.instance.ConvertQueryToValue();

            string temp_str = String.Empty;
            switch (Mediator.instance.Role)
            {
                case "Flower_Admin":
                    temp_str = "Администратор";
                    break;
                case "Flower_Employee":
                    temp_str = "Сотрудник";
                    break;
                default:
                    temp_str = "Не определён";
                    break;
            }
            new MsgBox("Вы вошли в систему!(" + temp_str + ")","Оповещение").ShowDialog();
            //MessageBox.Show("Вы вошли в систему!(" + temp_str + ")");
        }
    }
}
