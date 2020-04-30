using Npgsql;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlowerClient
{
    public partial class MainWindow : Window
    {
        private List<Card> gallery = new List<Card>(6);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            new AdminWindow().ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Mediator.instance.SQL = "select * from plants_all_view";
            DataTable results = Mediator.instance.ExecuteQuery();

            if(results.Rows.Count <= 6)
            {
                foreach(DataRow row in results.Rows)
                {
                    gallery.Add(new Card(row));
                    gallery.Last().captionP = "Lorem ipsum dolor sit amet, consectetur adipiscing elit";
                    gallery.Last().imageP = @"..\img\1.jpg";
                }
            }

            //Card c = new Card(results.Rows[0]);
            //c.captionP = "Lorem ipsum dolor sit amet, consectetur adipiscing elit";
            ///*c.nameP = "Flower1";*/

            //c.imageP = @"..\img\1.jpg";

            //gallery.Add(c);

            //Card c2 = new Card(results.Rows[1]);
            //c2.captionP = "Lorem ipsum dolor sit amet, consectetur adipiscing elit";
            ////c2.nameP = "Flower2";
            //c2.imageP = @"..\img\2.jpg";
            //gallery.Add(c2);

            //Card c3 = new Card(results.Rows[2]);
            //c3.captionP = "Lorem ipsum dolor sit amet, consectetur adipiscing elit";
            ////c3.nameP = "Flower3";
            //c3.imageP = @"..\img\3.jpg";
            //gallery.Add(c3);

            //Card c4 = new Card();
            //c4.captionP = "Lorem ipsum dolor sit amet, consectetur adipiscing elit";
            //c4.nameP = "Flower4";
            //c4.imageP = @"..\img\4.jpg";
            //gallery.Add(c4);

            //Card c5 = new Card();
            //c5.captionP = "Lorem ipsum dolor sit amet, consectetur adipiscing elit";
            //c5.nameP = "Flower5";
            //c5.imageP = @"..\img\5.jpg";
            //gallery.Add(c5);

            //Card c6 = new Card();
            ///*c6.captionP = "Lorem ipsum dolor sit amet, consectetur adipiscing elit";
            //c6.nameP = "Flower6";
            //c6.imageP = @"..\img\6.jpg";*/
            //gallery.Add(c6);
            
            
            if(gallery[0] != null)
            {
                card1.DataContext = gallery[0];
                card1.Visibility = Visibility.Visible;
            }
            
            if (gallery[1] != null)
            {
                card2.DataContext = gallery[1];
                card2.Visibility = Visibility.Visible;
            }
            
            if (gallery[2] != null)
            {
                card3.DataContext = gallery[2];
                card3.Visibility = Visibility.Visible;
            }
            
            /*if (gallery[3] != null)
            {
                card4.DataContext = gallery[3];
                card4.Visibility = Visibility.Visible;
            }
            
            if (gallery[4] != null)
            {
                card5.DataContext = gallery[4];
                card5.Visibility = Visibility.Visible;
            }
            
            if (gallery[5] != null)
            {
                card6.DataContext = gallery[5];
                card6.Visibility = Visibility.Visible;
            }*/
        }

        public void cardActivate(object sender)
        {
            MaterialDesignThemes.Wpf.Card temp = sender as MaterialDesignThemes.Wpf.Card;
            metadata.DataContext = (Card)temp.DataContext;
            metadata.Visibility = Visibility.Visible;
        }

        private void card1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            cardActivate(sender);
        }

        private void card6_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            cardActivate(sender);
        }

        private void card5_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            cardActivate(sender);
        }

        private void card4_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            cardActivate(sender);
        }

        private void card3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            cardActivate(sender);
        }

        private void card2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            cardActivate(sender);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button temp = sender as Button;
            StackPanel tempst = temp.Parent as StackPanel;
            DetailedDesc d = new DetailedDesc();
            d.DataContext = tempst.DataContext;
            d.Show();
        }
    }
}
