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
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
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
                string connectionString = "Server="+txt_adress.Text+";Port=5432;User Id=" + txt_login.Text
                        + ";Password=" + txt_password.Password + ";Database=flower;";
                //Например: "Server=127.0.0.1;Port=5432;User Id=postgres;Password=mypass;Database=mybase;"
                string sql = "select * from test";
                NpgsqlConnection conn = new NpgsqlConnection(connectionString);
                NpgsqlCommand comm = new NpgsqlCommand(sql, conn);
                conn.Open(); //Открываем соединение.
                var result = comm.ExecuteScalar().ToString(); //Выполняем нашу команду.
                conn.Close(); //Закрываем соединение.
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
