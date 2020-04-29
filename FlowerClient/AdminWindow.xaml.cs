using System;
using System.Collections.Generic;
using System.Data;
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

        private void cbx_roles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;

            if (selectedItem.Content.ToString() == "Пользователи")
            {
                users_table.DataContext = ShowUsersByRole("Flower_Employee");
            }
            else
            {
                users_table.DataContext = ShowUsersByRole("Flower_Admin");
            }
        }
    }
}
