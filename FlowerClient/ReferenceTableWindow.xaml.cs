using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace FlowerClient
{
    public partial class ReferenceTableWindow : Window
    {
        public ReferenceTableWindow()
        {
            InitializeComponent();
            cbx_category.SelectedIndex = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new ReferenceEditWindow().ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new ReferenceEditWindow().ShowDialog();
        }

        string StringToNameTable(string str)
        {
            string result = string.Empty;
            switch (str)
            {
                case "Автор":
                    result = "author";
                    break;
                case "Экспозиция":
                    result = "exposition";
                    break;
                case "Название вида":
                    result = "species_name";
                    break;
                case "Жизненная форма":
                    result = "life_form";
                    break;
                case "Группа":
                    result = "group";
                    break;
                case "Группа по хозяйственному назначению":
                    result = "econ_group";
                    break;
                case "Люди":
                    result = "people";
                    break;
                case "История":
                    result = "history";
                    break;
                case "Здания и сооружения":
                    result = "buildings";
                    break;
                case "Категория изображения":
                    result = "category";
                    break;
                default:
                    break;
            }
            return result;
        }

        private DataView ShowReference(string table)
        {
            Mediator.instance.SQL = "select * from get_view_by_name('" + table + "')";
            var temp = Mediator.instance.ConvertQueryToTable().DefaultView;
            return temp;
        }

        void UpdateReferenceTable()
        {
            try
            {
                ComboBoxItem selectedItem = (ComboBoxItem)cbx_category.SelectedItem;
                string temp = StringToNameTable(selectedItem.Content.ToString());
                reference_table.DataContext = ShowReference(temp + "_view");
            }
            catch (Exception ex)
            {
                new MsgBox(ex.Message, "Ошибка").ShowDialog();
            }
        }

        private void Cbx_category_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateReferenceTable();
        }
    }
}
