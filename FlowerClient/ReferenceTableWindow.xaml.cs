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

        private void Cbx_category_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        string StringToNameTable(string str)
        {
            string result = string.Empty;
            switch (str)
            {
                case "Автор":
                    result = "author";
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
