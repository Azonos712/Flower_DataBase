using ImageMagick;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
        private List<Tag> tags = new List<Tag>();
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

        //void ResetComboboxBoxs()
        //{
        //    (FindName("surname") as ComboBox).SelectedItem = "Все";
        //    (FindName("exposition") as ComboBox).SelectedItem = "Все";
        //    (FindName("life_form") as ComboBox).SelectedItem = "Все";
        //    (FindName("species_name") as ComboBox).SelectedItem = "Все";
        //    (FindName("group") as ComboBox).SelectedItem = "Все";
        //    (FindName("econ_group") as ComboBox).SelectedItem = "Все";
        //    (FindName("people") as ComboBox).SelectedItem = "Все";
        //    (FindName("history") as ComboBox).SelectedItem = "Все";
        //    (FindName("buildings") as ComboBox).SelectedItem = "Все";
        //    (FindName("category") as ComboBox).SelectedItem = "Все";

        //    season.SelectedItem = "Все";
        //    year.SelectedItem = "Все";
        //}

        void LoadComboxBoxs()
        {
            loadReference("author");
            loadReference("exposition");
            loadReference("life_form");
            loadReference("species_name");
            loadReference("group");
            loadReference("econ_group");
            loadReference("people");
            loadReference("history");
            loadReference("buildings");
            loadReference("category");
            fillYearsSeasons();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cards.Add(card1);
            cards.Add(card2);
            cards.Add(card3);
            cards.Add(card4);
            cards.Add(card5);
            cards.Add(card6);

            LoadComboxBoxs();

            loadRecords(1);
            currentPage = 1;

            UpdateGallery();

            createSearchQuery();
        }

        private void fillYearsSeasons()
        {
            List<string> years = new List<string>();
            List<string> seasons = new List<string>();

            for (int i = 2017; i < 2031; i++)
            {
                years.Add(i.ToString());
            }

            seasons.Add("Весна");
            seasons.Add("Лето");
            seasons.Add("Осень");
            seasons.Add("Зима");

            season.ItemsSource = seasons;
            year.ItemsSource = years;
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

            if (d.ShowDialog() == true)
            {
                loadRecords(currentPage);
                ClearGallery();
                UpdateGallery();
            }
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
            loadPage(false);
        }

        private void loadRecords(int currentPage)
        {
            if (results != null)
            {
                results.Clear();
            }
            Mediator.instance.SQL = "select * from plants_all_view limit 6 offset " + ((currentPage - 1) * 6).ToString();
            results = Mediator.instance.ConvertQueryToTable();
        }

        private void loadPage(bool direction)
        {
            if (direction)
            {
                loadRecords(currentPage + 1);

                if (results.Rows.Count > 0)
                    currentPage++;
                else
                    return;
            }
            else
            {
                if (currentPage != 1)
                    currentPage--;
                else
                    return;

                loadRecords(currentPage);
            }

            ClearGallery();
            UpdateGallery();
        }

        void ClearGallery()
        {
            gallery.Clear();
            foreach (MaterialDesignThemes.Wpf.Card c in cards)
            {
                c.DataContext = null;
                c.Visibility = Visibility.Hidden;
            }
            GC.Collect();
        }

        void UpdateGallery()
        {
            for (int i = 0; i < results.Rows.Count; i++)
            {
                gallery.Add(new Card(results.Rows[i]));
                gallery.Last().captionP = gallery.Last().groupP + " - " + gallery.Last().economicGroupP;
                gallery.Last().ImageS = Mediator.instance.ByteToImage(MinimizeImage(Mediator.instance.Path + gallery.Last().idP + ".jpg"));
                //gallery.Last().ImageS = Mediator.instance.NonBlockingLoad(Mediator.instance.Path + gallery.Last().idP + ".jpg");
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

        byte[] MinimizeImage(string path)
        {
            if (!(File.Exists(path)))
                return null;

            var img = new MagickImage(path);

            img.Quality = 70;

            if (img.Height > img.Width)
                img.Scale(140, 340);
            else
                img.Scale(340, 140);

            return img.ToByteArray();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            if (new DetailedDesc().ShowDialog() == true)
            {
                loadRecords(currentPage);
                ClearGallery();
                UpdateGallery();
            }
        }

        private void loadReference(string refName)
        {
            Mediator.instance.SQL = "select * from " + refName + "_view";
            ComboBox c = FindName(refName) as ComboBox;

            if(c != null)
            {
                var temp = Mediator.instance.ConvertQueryToComboBox();
                c.ItemsSource = temp;
            }
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            new ReferenceTableWindow().ShowDialog();
            loadRecords(currentPage);
            ClearGallery();
            LoadComboxBoxs();
            UpdateGallery();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            tagPanel.Items.Add("Гаф");
            tagPanel.Items.Add("Авававававававав");
            tagPanel.Items.Add("Мяу");
        }

        private void Chip_DeleteClick(object sender, RoutedEventArgs e)
        {
            tagPanel.Items.Remove((sender as FrameworkElement).DataContext);
        }
        
        private void createSearchQuery()
        {
            Mediator.instance.SQL = "select * from plants_all_view";
            if (tags.Count != 0)
            {
                tags.Add(new FlowerClient.Tag { categoryP = "exposition", valP = "Партер" });
                //tags.Add(new FlowerClient.Tag { categoryP = "author", valP = "Коврик" });
                //tags.Add(new FlowerClient.Tag { categoryP = "season", valP = "Лето" });
                //tags.Add(new FlowerClient.Tag { categoryP = "season", valP = "Зима" });
                //tags.Add(new FlowerClient.Tag { categoryP = "year", valP = "2020" });
                //tags.Add(new FlowerClient.Tag { categoryP = "year", valP = "2019" });
                //tags.Add(new FlowerClient.Tag { categoryP = "author", valP = "Бездетный" });
                //tags.Add(new FlowerClient.Tag { categoryP = "exposition", valP = "Оранжереи" });
                //tags.Add(new FlowerClient.Tag { categoryP = "season", valP = "Весна" });

                if (results != null)
                {
                    results.Clear();
                }

                string result = string.Empty;

                for (int i = 0; i < tags.Count; i++)
                {
                    if (result.Contains(tags[i].categoryP))
                    {
                        result = result.Insert(result.IndexOf(")", result.IndexOf(tags[i].categoryP)), " or " + tags[i].categoryP + " = '" + tags[i].valP + "'");
                    }
                    else
                    {
                        result += tags[i].categoryP + " = '" + tags[i].valP + "')";
                        result += " and (";
                    }

                }
                if (result.Last() == '(')
                {
                    result = result.Remove(result.Length - 6, 6);
                }

                Mediator.instance.SQL += " where (" + result;
            }
            Mediator.instance.SQL += " limit 6 offset 0";
            results = Mediator.instance.ConvertQueryToTable();
        }
    }
}
