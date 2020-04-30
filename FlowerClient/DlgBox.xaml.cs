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
    /// Логика взаимодействия для DlgBox.xaml
    /// </summary>
    public partial class DlgBox : Window
    {
        public DlgBox(string info, string title, string left, string right)
        {
            InitializeComponent();
            this.Title = title;
            txt_info.Text = info;
            btn_lft.Content = left;
            btn_rght.Content = right;
        }

        private void btn_lft_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            Close();
        }

        private void btn_rght_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }
    }
}
