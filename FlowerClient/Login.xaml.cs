using Npgsql;
using System;
using System.Text;
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
                if (string.IsNullOrWhiteSpace(txt_login.Text) || string.IsNullOrWhiteSpace(txt_password.Password) || string.IsNullOrWhiteSpace(txt_adress.Text))
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
                if (ex.Message.Contains("28P01"))
                {
                    new MsgBox("Неправильный логин или пароль!", "Ошибка").ShowDialog();
                }
                else
                {
                    new MsgBox(ex.Message, "Ошибка").ShowDialog();
                }
            }
        }

        void TryConnection()
        {
            try
            {
                string connectionString = "Server=" + txt_adress.Text.Trim() + ";Port=5432;User Id=" + txt_login.Text.Trim().ToLower()
            + ";Password=" + txt_password.Password.Trim() + ";Database=flower;";
            
                Mediator.instance.Connection = new NpgsqlConnection(connectionString);
                //npgsql throws nullreferenceexception
                Mediator.instance.Connection.Open();

                Mediator.instance.Login = txt_login.Text.Trim().ToLower();

                Mediator.instance.SQL = "select show_role('" + Mediator.instance.Login + "');";
                Mediator.instance.Role = Mediator.instance.ConvertQueryToValue().ToString();
                //Переключаемся на групповую роль с правами
                Mediator.instance.SQL = "set role \"" + Mediator.instance.Role + "\";";
                Mediator.instance.Execute();

                Mediator.instance.SQL = ("select * from picture_path_view;");
                Mediator.instance.Path = Mediator.instance.ConvertQueryToValue().ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void RoleAlert()
        {
            string temp_str;
            switch (Mediator.instance.Role)
            {
                case "Flower_Admin":
                    temp_str = "Администратор";
                    break;
                case "Flower_Employee":
                    temp_str = "Сотрудник";
                    break;
                case "Flower_SuperAdmin":
                    temp_str = "Супер администратор";
                    break;
                default:
                    temp_str = "Не определён";
                    break;
            }
            new MsgBox("Вы вошли в систему!(" + temp_str + ")", "Оповещение").ShowDialog();
        }
    }
}
