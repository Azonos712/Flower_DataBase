using Npgsql;
using System;
using System.Windows;

namespace FlowerClient
{
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
                if (String.IsNullOrWhiteSpace(txt_login.Text) || String.IsNullOrWhiteSpace(txt_password.Password) || String.IsNullOrWhiteSpace(txt_adress.Text))
                {
                    throw new Exception("Вы заполнили не все поля!");
                }

                TryConnection();
                RoleAlert();
                new MainWindow().Show();
                this.Close();
            }
            catch (Exception ex)
            {
                new MsgBox(ex.Message, "Ошибка").ShowDialog();
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
        }

        void RoleAlert()
        {
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
            new MsgBox("Вы вошли в систему!(" + temp_str + ")", "Оповещение").ShowDialog();
        }
    }
}
