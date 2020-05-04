using Npgsql;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FlowerClient
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            OnControls();
        }

        void OnControls()
        {
            blck_pnl.Visibility = Visibility.Hidden;
            prgrss_br.Visibility = Visibility.Hidden;

        }

        void OffControls()
        {
            blck_pnl.Visibility = Visibility.Visible;
            prgrss_br.Visibility = Visibility.Visible;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txt_login.Text) || string.IsNullOrWhiteSpace(txt_password.Password) || string.IsNullOrWhiteSpace(txt_adress.Text))
                {
                    throw new Exception("Вы заполнили не все поля!");
                }

                OffControls();

                var s_t = txt_adress.Text.Trim();
                var s_l = txt_login.Text.Trim();
                var s_p = txt_password.Password.Trim();

                await Task.Run(() => TryConnection(s_t, s_l, s_p));

                OnControls();
                RoleAlert();

                new MainWindow().Show();
                this.Close();
            }
            catch (Exception ex)
            {
                OnControls();
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

        void TryConnection(string adr, string log, string pass)
        {
            string connectionString = "Server=" + adr + ";Port=5432;User Id=" + log.ToLower()
        + ";Password=" + pass + ";Database=flower;";

            Mediator.instance.Connection = new NpgsqlConnection(connectionString);
            //npgsql throws nullreferenceexception
            Mediator.instance.Connection.Open();

            Mediator.instance.Login = log.Trim().ToLower();

            Mediator.instance.SQL = "select show_role('" + Mediator.instance.Login + "');";
            Mediator.instance.Role = Mediator.instance.ConvertQueryToValue().ToString();
            //Переключаемся на групповую роль с правами
            Mediator.instance.SQL = "set role \"" + Mediator.instance.Role + "\";";
            Mediator.instance.Execute();

            Mediator.instance.SQL = ("select * from picture_path_view;");
            Mediator.instance.Path = Mediator.instance.ConvertQueryToValue().ToString();
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
