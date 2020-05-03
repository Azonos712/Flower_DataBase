using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace FlowerClient
{
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

        private DataView ShowUsersByRole(string role)
        {
            Mediator.instance.SQL = "select * from show_users_by_role('" + role + "')";
            var temp = Mediator.instance.ConvertQueryToTable().DefaultView;
            return temp;
        }

        void UpdateUserTable()
        {
            try
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
            catch (Exception ex)
            {
                new MsgBox(ex.Message, "Ошибка").ShowDialog();
            }
        }

        private void Cbx_roles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateUserTable();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (new AddUser().ShowDialog() == true)
            {
                UpdateUserTable();
            }
        }
        #region DELETE USER
        void DeleteUser()
        {
            try
            {
                DataRowView temp = (DataRowView)users_table.SelectedItem;
                if (temp == null)
                    throw new Exception("Ничего не выбрано! Выберите из таблицы кого хотите удалить!");

                if (new DlgBox("Вы точно хотите удалить этого пользователя?", "Удаление", "Да", "Нет").ShowDialog() == true)
                {
                    Mediator.instance.SQL = "select drop_user('" + temp.Row.ItemArray[0] + "')";
                    Mediator.instance.Execute();

                    new MsgBox("Удаление пользователя прошло успешно!", "Информация").ShowDialog();
                    UpdateUserTable();
                }
            }
            catch (Exception ex)
            {
                new MsgBox(ex.Message, "Ошибка").ShowDialog();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DeleteUser();
        }
        #endregion
        private void Btn_pic_path_Click(object sender, RoutedEventArgs e)
        {
            new PathWindow().ShowDialog();
        }
    }
}