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
using System.Windows.Shapes;

namespace FlowerClient
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TryConnection();
            new MainWindow().Show();
            this.Close();
        }

        void TryConnection()
        {
            try
            {
                string connectionString = "Server=" + txt_adress.Text + ";Port=5432;User Id=" + txt_login.Text
                + ";Password=" + txt_password.Password + ";Database=flower;";
                Mediator.instance.SQL = "select * from test";
                Mediator.instance.Connection = new NpgsqlConnection(connectionString);
                Mediator.instance.Connection.Open();
                Mediator.instance.Execute();
                //Mediator.instance.Connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
