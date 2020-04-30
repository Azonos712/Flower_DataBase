using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace FlowerClient
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();

            try
            {
                cbx_roles.SelectedIndex = 0;
                if (Mediator.instance.Role == "Flower_Admin")
                {
                    cbx_roles.Visibility = Visibility.Collapsed;
                    btn_pic_path.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                new MsgBox(ex.Message, "Ошибка").ShowDialog();
            }
        }

        private void cbx_roles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateUserTable();
        }

        void UpdateUserTable()
        {
            ComboBoxItem selectedItem = (ComboBoxItem)cbx_roles.SelectedItem;
            if (selectedItem.Content.ToString() == "Пользователи")
            {
                users_table.DataContext = ShowUsersByRole("Flower_Employee");
            }
            else
            {
                users_table.DataContext = ShowUsersByRole("Flower_Admin");
            }
        }

        private DataView ShowUsersByRole(string role)
        {
            Mediator.instance.SQL = "select * from show_users_by_role('" + role + "')";
            var temp = Mediator.instance.ConvertQueryToTable().DefaultView;
            return temp;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(new AddUser().ShowDialog() == true)
            {
                UpdateUserTable();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DeleteUser();
        }

        void DeleteUser()
        {

        }
    }
}
