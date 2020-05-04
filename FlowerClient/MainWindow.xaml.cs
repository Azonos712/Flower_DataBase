using ImageMagick;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FlowerClient
{
    public partial class MainWindow : Window
    {
        private List<MaterialDesignThemes.Wpf.Card> cards = new List<MaterialDesignThemes.Wpf.Card>(6);
        private List<Card> gallery = new List<Card>(6);
        private DataTable results;
        private int currentPage;
        private string currentSQL;
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

            //loadRecords(1);
            currentPage = 1;
            createSearchQuery(currentPage);

            UpdateGallery();
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
                //createSearchQuery(currentPage);
                loadRecords(currentPage);
                ClearGallery();
                UpdateGallery();
            }
        }

        private void Search_btn_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;
            createSearchQuery(currentPage);
            ClearGallery();
            UpdateGallery();
        }

        private void reset_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void nextPage_Click(object sender, RoutedEventArgs e)
        {
            loadPage(true);
        }

        private void prevPage_Click(object sender, RoutedEventArgs e)
        {
            loadPage(false);
        }

        private void loadRecords(int page)
        {
            if (results != null)
            {
                results.Clear();
            }
            Mediator.instance.SQL = currentSQL + " limit 6 offset " + ((page - 1) * 6).ToString();
            results = Mediator.instance.ConvertQueryToTable();
        }

        private void loadPage(bool direction)
        {
            if (direction)
            {
                //createSearchQuery(currentPage + 1);
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

                //createSearchQuery(currentPage);
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
                //createSearchQuery(currentPage);
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
            //createSearchQuery(currentPage);
            loadRecords(currentPage);
            ClearGallery();
            LoadComboxBoxs();
            UpdateGallery();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button temp = sender as Button;
                StackPanel s = temp.Parent as StackPanel;
                ComboBox cbx = null;

                foreach (var c in s.Children)
                {
                    if (c is ComboBox)
                    {
                        cbx = c as ComboBox;
                    }
                }

                if(cbx.SelectedItem == null)
                    throw new Exception("Выберете категорию для добавления!");

                Tag tmp = new Tag();
                tmp.categoryP = cbx.Name;
                tmp.valP = cbx.SelectedItem.ToString();

                for (int i = 0; i < tagPanel.Items.Count; i++)
                {
                    if ((tagPanel.Items[i] as Tag).valP == tmp.valP)
                        throw new Exception("Такой фильтр уже используется!");
                }

                tagPanel.Items.Add(tmp);

            }
            catch (Exception ex)
            {
                new MsgBox(ex.Message, "Ошибка").ShowDialog();
            }
        }

        private void Chip_DeleteClick(object sender, RoutedEventArgs e)
        {
            tagPanel.Items.Remove((sender as FrameworkElement).DataContext);
        }
        
        private void createSearchQuery(int page)
        {
            Mediator.instance.SQL = "select * from plants_all_view";
            if (tagPanel.Items.Count != 0)
            {
                if (results != null)
                {
                    results.Clear();
                }

                string result = string.Empty;

                for (int i = 0; i < tagPanel.Items.Count; i++)
                {
                    
                    if (result.Contains((tagPanel.Items[i] as Tag).categoryP))
                    {
                        result = result.Insert(result.IndexOf(")", result.IndexOf((tagPanel.Items[i] as Tag).categoryP)), " or \"" + (tagPanel.Items[i] as Tag).categoryP + "\" = '" + (tagPanel.Items[i] as Tag).valP + "'");
                    }
                    else
                    {
                        result += "\"" + (tagPanel.Items[i] as Tag).categoryP + "\" = '" + (tagPanel.Items[i] as Tag).valP + "')";
                        result += " and (";
                    }

                }
                if (result.Last() == '(')
                {
                    result = result.Remove(result.Length - 6, 6);
                }

                Mediator.instance.SQL += " where (" + result;
            }
            currentSQL = Mediator.instance.SQL;

            Mediator.instance.SQL += " limit 6 offset " + ((page - 1) * 6).ToString();
            results = Mediator.instance.ConvertQueryToTable();
        }
    }
}
