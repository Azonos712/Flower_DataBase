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
    /// Логика взаимодействия для AddUser.xaml
    /// </summary>
    public partial class AddUser : Window
    {
        public AddUser()
        {
            InitializeComponent();

            try
            {
                if (Mediator.instance.Role == "Flower_Admin")
                {
                    radio_panel.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                new MsgBox(ex.Message, "Ошибка").ShowDialog();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txt_login.Text) || string.IsNullOrWhiteSpace(txt_password1.Password) || string.IsNullOrWhiteSpace(txt_password2.Password))
                {
                    throw new Exception("Вы заполнили не все поля!");
                }

                if (txt_password1.Password != txt_password2.Password)
                    throw new Exception("Пароли не совпадают!");

                AddNewUser();

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                new MsgBox(ex.Message, "Ошибка").ShowDialog();
            }
        }

        void AddNewUser()
        {
            //try
            //{
            string tmp_role = radio_usver.IsChecked == true ? "Flower_Employee" : "Flower_Admin";
            Mediator.instance.SQL = "select create_user('" + txt_login.Text.Trim().ToLower() + "','" + txt_password2.Password.Trim() + "','" + tmp_role + "');";

            Mediator.instance.Execute();
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }
    }
}
