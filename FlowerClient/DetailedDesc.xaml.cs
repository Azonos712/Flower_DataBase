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
    /// Логика взаимодействия для DetailedDesc.xaml
    /// </summary>
    public partial class DetailedDesc : Window
    {
        public DetailedDesc()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadReference("author");
            
        }

        private void loadReference(string refName)
        {
            Mediator.instance.SQL = "select * from " + refName + "_view";
            DataTable results = Mediator.instance.ExecuteQuery();
            //results.Rows.
            //author.DataContext = results.Rows;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
