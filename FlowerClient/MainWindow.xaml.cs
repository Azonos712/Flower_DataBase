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
        public MainWindow()
        {
            InitializeComponent();

            if (Mediator.instance.Role == "Flower_Employee")
                admin_menuitem.Visibility = Visibility.Collapsed;
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                Mediator.instance.Connection.Close();
            }
            catch (Exception ex)
            {
                new MsgBox(ex.Message, "Ошибка").ShowDialog();
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            new AdminWindow().ShowDialog();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            new Login().Show();
            this.Close();
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cards.Add(card1);
            cards.Add(card2);
            cards.Add(card3);
            cards.Add(card4);
            cards.Add(card5);
            cards.Add(card6);

            loadRecords(1);
            currentPage = 1;

            for(int i = 0; i < 6; i++)
            {
                gallery.Add(new Card(results.Rows[i]));
                gallery.Last().captionP = gallery.Last().groupP + " - " + gallery.Last().economicGroupP;
                gallery.Last().imageP = Mediator.instance.Path + gallery.Last().idP + ".jpg";
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

        private void card_active_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DetailedDesc d = new DetailedDesc();
            MaterialDesignThemes.Wpf.Card temp = sender as MaterialDesignThemes.Wpf.Card;
            d.DataContext = temp.DataContext;
            d.ShowDialog();
        }

        private void Search_btn_Click(object sender, RoutedEventArgs e)
        {
            /*Button temp = sender as Button;
            StackPanel tempst = temp.Parent as StackPanel;
            DetailedDesc d = new DetailedDesc();
            d.DataContext = tempst.DataContext;
            d.Show();*/
        }

        private void nextPage_Click(object sender, RoutedEventArgs e)
        {
            loadPage(true);
        }

        private void prevPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage != 1)
            {
                loadPage(false);
            }
        }

        private void loadRecords(int currentPage)
        {
            if (results != null)
            {
                results.Clear();
            }
            Mediator.instance.SQL = "select * from plants_all_view limit 6 offset " + ((currentPage - 1) * 6).ToString();
            results = Mediator.instance.ExecuteQuery();
        }
        private void loadPage(bool direction)
        {
            if (direction)
            {
                loadRecords(currentPage + 1);
                if (results.Rows.Count > 0)
                {
                    currentPage++;
                    gallery.Clear();
                    foreach (MaterialDesignThemes.Wpf.Card c in cards)
                    {
                        c.DataContext = null;
                        c.Visibility = Visibility.Hidden;
                    }


                    for (int i = 0; i < results.Rows.Count; i++)
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
            else
            {
                currentPage--;
                gallery.Clear();
                foreach (MaterialDesignThemes.Wpf.Card c in cards)
                {
                    c.DataContext = null;
                    c.Visibility = Visibility.Hidden;
                }
                loadRecords(currentPage);

                for (int i = 0; i < results.Rows.Count; i++)
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
