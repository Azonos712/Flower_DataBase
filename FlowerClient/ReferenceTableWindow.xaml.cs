using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
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

        void OnControls()
        {
            btn_add.IsEnabled = true;
            btn_del.IsEnabled = true;
            cbx_category.IsEnabled = true;
            prgrss_br.Visibility = Visibility.Hidden;
        }

        void OffControls()
        {
            prgrss_br.Visibility = Visibility.Visible;
            btn_add.IsEnabled = false;
            btn_del.IsEnabled = false;
            cbx_category.IsEnabled = false;
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

        async void CallEditForm(bool mode, string value = "")
        {
            try
            {
                ComboBoxItem selectedItem = (ComboBoxItem)cbx_category.SelectedItem;
                string temp = StringToNameTable(selectedItem.Content.ToString());

                if (new ReferenceEditWindow(mode, temp, value).ShowDialog() == true)
                {
                    OffControls();
                    reference_table.DataContext = await Task.Run(() => ShowReference(temp + "_view"));
                    OnControls();
                }
            }
            catch (Exception ex)
            {
                OnControls();
                new MsgBox(ex.Message, "Ошибка").ShowDialog();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CallEditForm(true);
            }
            catch (Exception ex)
            {
                new MsgBox(ex.Message, "Ошибка").ShowDialog();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView temp = (DataRowView)reference_table.SelectedItem;
                if (temp == null)
                    throw new Exception("Ничего не выбрано! Выберите из таблицы что хотите изменить!");

                CallEditForm(false, temp.Row.ItemArray[0].ToString());
            }
            catch (Exception ex)
            {
                new MsgBox(ex.Message, "Ошибка").ShowDialog();
            }
        }

        async void Cbx_category_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                OffControls();
                ComboBoxItem selectedItem = (ComboBoxItem)cbx_category.SelectedItem;
                string temp = StringToNameTable(selectedItem.Content.ToString());
                reference_table.DataContext = await Task.Run(() => ShowReference(temp + "_view"));
                OnControls();
            }
            catch (Exception ex)
            {
                OnControls();
                new MsgBox(ex.Message, "Ошибка").ShowDialog();
            }
        }
    }
}
