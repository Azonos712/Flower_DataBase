using ImageMagick;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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

            OnControls();
        }

        void OnControls()
        {
            blck_pnl.Visibility = Visibility.Hidden;
            prgrss_br.Visibility = Visibility.Hidden;
            metadata.IsEnabled = true;
            search_panel.IsEnabled = true;
            tagPanel.IsEnabled = true;
            page_panel.IsEnabled = true;
            main_menu.IsEnabled = true;
        }

        void OffControls()
        {
            blck_pnl.Visibility = Visibility.Visible;
            prgrss_br.Visibility = Visibility.Visible;
            metadata.IsEnabled = false;
            search_panel.IsEnabled = false;
            tagPanel.IsEnabled = false;
            page_panel.IsEnabled = false;
            main_menu.IsEnabled = false;
        }

        ComboBox GetComboBoxByName(string refName)
        {
            var c = FindName(refName) as ComboBox;
            return c;
        }

        List<string> LoadReference(string refName)
        {
            Mediator.instance.SQL = "select * from " + refName + "_view";
            var temp = Mediator.instance.ConvertQueryToComboBox();
            return temp;
        }

        List<string> LoadYears()
        {
            List<string> years = new List<string>();
            for (int i = 2017; i < 2031; i++)
            {
                years.Add(i.ToString());
            }
            return years;
        }

        List<string> LoadSeasons()
        {
            List<string> seasons = new List<string>();
            seasons.Add("Весна");
            seasons.Add("Лето");
            seasons.Add("Осень");
            seasons.Add("Зима");
            return seasons;
        }

        async void LoadComboBoxs()
        {
            GetComboBoxByName("author").ItemsSource = await Task.Run(() => LoadReference("author"));
            GetComboBoxByName("exposition").ItemsSource = await Task.Run(() => LoadReference("exposition"));
            GetComboBoxByName("life_form").ItemsSource = await Task.Run(() => LoadReference("life_form"));
            GetComboBoxByName("species_name").ItemsSource = await Task.Run(() => LoadReference("species_name"));
            GetComboBoxByName("group").ItemsSource = await Task.Run(() => LoadReference("group"));
            GetComboBoxByName("econ_group").ItemsSource = await Task.Run(() => LoadReference("econ_group"));
            GetComboBoxByName("people").ItemsSource = await Task.Run(() => LoadReference("people"));
            GetComboBoxByName("history").ItemsSource = await Task.Run(() => LoadReference("history"));
            GetComboBoxByName("buildings").ItemsSource = await Task.Run(() => LoadReference("buildings"));
            GetComboBoxByName("category").ItemsSource = await Task.Run(() => LoadReference("category"));
            GetComboBoxByName("season").ItemsSource = await Task.Run(() => LoadSeasons());
            GetComboBoxByName("year").ItemsSource = await Task.Run(() => LoadYears());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                cards.Add(card1);
                cards.Add(card2);
                cards.Add(card3);
                cards.Add(card4);
                cards.Add(card5);
                cards.Add(card6);

                OffControls();

                LoadComboBoxs();

                //loadRecords(1);
                currentPage = 1;
                createSearchQuery(currentPage);

                UpdateGallery();

                OnControls();
            }
            catch (Exception ex)
            {
                new MsgBox(ex.Message, "Ошибка").ShowDialog();
            }
            
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

        public static T FindChild<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                    if (child != null && child is T)
                    {
                        return (T)child;
                    }

                    T childItem = FindChild<T>(child);
                    if (childItem != null)
                    {
                        return childItem;
                    }
                }
            }
            return null;
        }

        void ConfirmFilterColor()
        {
            foreach (var t in tagPanel.Items)
            {
                var parentObject = tagPanel.ItemContainerGenerator.ContainerFromItem(t);
                Chip c = FindChild<Chip>(parentObject);
                c.Background = System.Windows.Media.Brushes.LightGreen;
            }
        }

        private void Search_btn_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;
            createSearchQuery(currentPage);
            ConfirmFilterColor();
            ClearGallery();
            UpdateGallery();
        }

        private void reset_btn_Click(object sender, RoutedEventArgs e)
        {
            tagPanel.Items.Clear();
            currentPage = 1;
            createSearchQuery(currentPage);
            ClearGallery();
            UpdateGallery();
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

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            new ReferenceTableWindow().ShowDialog();
            //createSearchQuery(currentPage);
            loadRecords(currentPage);
            ClearGallery();
            LoadComboBoxs();
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
