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
        private List<MaterialDesignThemes.Wpf.Card> cards = new List<MaterialDesignThemes.Wpf.Card>(6);
        private List<Card> gallery = new List<Card>(6);
        private DataTable results;
        private int currentPage;
        private int pagesCount;
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
            
            cards.Add(card1);
            cards.Add(card2);
            cards.Add(card3);
            cards.Add(card4);
            cards.Add(card5);
            cards.Add(card6);

            Mediator.instance.SQL = "select * from plants_all_view";
            results = Mediator.instance.ExecuteQuery();

            pagesCount = (int)Math.Ceiling((decimal)results.Rows.Count / 6);
            currentPage = 1;

            if(results.Rows.Count <= 6)
            {
                foreach(DataRow row in results.Rows)
                {
                    gallery.Add(new Card(row));
                    gallery.Last().captionP = "Lorem ipsum dolor sit amet, consectetur adipiscing elit";
                    gallery.Last().imageP = @"..\img\" + (results.Rows.IndexOf(row) + 1).ToString() + ".jpg";
                }
            }
            else
            {
                for(int i = 0; i < 6; i++)
                {
                    gallery.Add(new Card(results.Rows[i]));
                    gallery.Last().captionP = "Lorem ipsum dolor sit amet, consectetur adipiscing elit";
                    gallery.Last().imageP = @"..\img\" + (i + 1).ToString() + ".jpg";
                }
            }
            
            for(int i = 0; i < gallery.Count; i++)
            {
                if(gallery[i] != null)
                {
                    cards[i].DataContext = gallery[i];
                    cards[i].Visibility = Visibility.Visible;
                }
            }

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

        private void nextPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < pagesCount)
            {
                loadPage(true);
            }
        }

        private void prevPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage != 1)
            {
                loadPage(false);
            }
        }

        private void loadPage(bool direction)
        {
            if (direction)
            {
                gallery.Clear();
                for (int i = currentPage*6; i < 11; i++)
                {
                    gallery.Add(new Card(results.Rows[i]));
                    gallery.Last().captionP = "Lorem ipsum dolor sit amet, consectetur adipiscing elit";
                    gallery.Last().imageP = @"..\img\1.jpg";
                }
                for (int i = 0; i < gallery.Count; i++)
                {
                    if (gallery[i] != null)
                    {
                        cards[i].DataContext = gallery[i];
                        cards[i].Visibility = Visibility.Visible;
                    }
                }
                currentPage++;
            }
            else
            {
                currentPage--;
                gallery.Clear();
                for (int i = currentPage * 6; i < 6; i++)
                {
                    gallery.Add(new Card(results.Rows[i]));
                    gallery.Last().captionP = "Lorem ipsum dolor sit amet, consectetur adipiscing elit";
                    gallery.Last().imageP = @"..\img\1.jpg";
                }
                for (int i = 0; i < gallery.Count; i++)
                {
                    if (gallery[i] != null)
                    {
                        cards[i].DataContext = gallery[i];
                        cards[i].Visibility = Visibility.Visible;
                    }
                }
                
            }
        }
    }
}
