using Npgsql;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlowerClient
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string connectionString = "Server=" + txt_adress.Text + ";Port=5432;User Id=" + txt_login.Text
                + ";Password=" + txt_password.Password + ";Database=flower;";
                Mediator.instance.SQL = "select * from test";
                Mediator.instance.Connection = new NpgsqlConnection(connectionString);
                Mediator.instance.Connection.Open();
                Mediator.instance.Execute();
                Mediator.instance.Connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
