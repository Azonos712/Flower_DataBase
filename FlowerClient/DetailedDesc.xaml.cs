using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ComboBox = System.Windows.Controls.ComboBox;
using System.IO;

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
            try
            {
                Card context = DataContext as Card;

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

                if (context == null)
                {
                    btn_foto.Content = "Добавить фото";
                    btn_save.Content = "Добавить";
                    btn_save_foto.Visibility = Visibility.Collapsed;
                }
                else
                {
                    //Загружаем исходную картинку, что бы можно было скачать исходный размер
                    photo.Source = Mediator.instance.NonBlockingLoad(Mediator.instance.Path + context.idP + ".jpg");

                    author.SelectedIndex = author.Items.IndexOf(context.authorP);
                    exposition.SelectedIndex = exposition.Items.IndexOf(context.expositionP);
                    life_form.SelectedIndex = life_form.Items.IndexOf(context.lifeFormP);
                    species_name.SelectedIndex = species_name.Items.IndexOf(context.speciesNameP);
                    group.SelectedIndex = group.Items.IndexOf(context.groupP);
                    econ_group.SelectedIndex = econ_group.Items.IndexOf(context.economicGroupP);
                    people.SelectedIndex = people.Items.IndexOf(context.peopleP);
                    history.SelectedIndex = history.Items.IndexOf(context.historyP);
                    buildings.SelectedIndex = buildings.Items.IndexOf(context.buildingsP);
                    category.SelectedIndex = category.Items.IndexOf(context.categoryP);
                    year.SelectedIndex = year.Items.IndexOf(Convert.ToInt32(context.yearP));
                    season.SelectedIndex = season.Items.IndexOf(context.seasonP);
                }
            }
            catch (Exception ex)
            {
                new MsgBox(ex.Message, "Ошибка").ShowDialog();
            }
        }

        private void fillYearsSeasons()
        {
            List<int> years = new List<int>();
            List<string> seasons = new List<string>();

            for (int i = 2017; i < 2031; i++)
            {
                years.Add(i);
            }
            seasons.Add("Весна");
            seasons.Add("Лето");
            seasons.Add("Осень");
            seasons.Add("Зима");

            season.ItemsSource = seasons;
            year.ItemsSource = years;
        }
        private void loadReference(string refName)
        {
            Mediator.instance.SQL = "select * from " + refName + "_view";
            ComboBox c = FindName(refName) as ComboBox;
            c.ItemsSource = Mediator.instance.ConvertQueryToComboBox();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private string itemToString(string name)
        {
            ComboBox c = FindName(name) as ComboBox;
            //ComboBoxItem item = (ComboBoxItem)c.SelectedItem;
            if (c.SelectedItem == null)
                throw new Exception("Выберите все поля!");

            
            return c.SelectedItem.ToString();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Card temp = (Card)this.DataContext;

                int fotoID = -1;

                if (temp != null)
                {
                    Mediator.instance.SQL = "select * from update_plant('" + itemToString("author") + "','" +
                        itemToString("exposition") + "','" + itemToString("species_name") + "','" +
                        itemToString("life_form") + "','" + itemToString("group") +
                        "','" + itemToString("econ_group") + "','" + itemToString("people") +
                        "','" + itemToString("history") + "','" + itemToString("buildings") +
                        "','" + itemToString("category") + "'," + itemToString("year") +
                        ",'" + itemToString("season") + "'," + temp.idP + ");";

                    Mediator.instance.Execute();

                    fotoID = temp.idP;
                }
                else
                {
                    Mediator.instance.SQL = "select * from create_plant('" + itemToString("author") + "','" +
                        itemToString("exposition") + "','" + itemToString("species_name") + "','" +
                        itemToString("life_form") + "','" + itemToString("group") +
                        "','" + itemToString("econ_group") + "','" + itemToString("people") +
                        "','" + itemToString("history") + "','" + itemToString("buildings") +
                        "','" + itemToString("category") + "'," + itemToString("year") +
                        ",'" + itemToString("season") + "');";

                    fotoID = Convert.ToInt32(Mediator.instance.ConvertQueryToValue());
                }

                //Сохранение или замена старого фото на новое фото
                JpegBitmapEncoder jpegBitmapEncoder = new JpegBitmapEncoder();
                jpegBitmapEncoder.Frames.Add(BitmapFrame.Create(photo.Source as BitmapSource));
                using (FileStream fileStream = new FileStream(Mediator.instance.Path + fotoID + ".jpg", FileMode.Create))
                    jpegBitmapEncoder.Save(fileStream);

                new MsgBox("Действие завершенно успешно!", "Информация").ShowDialog();
                DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                new MsgBox(ex.Message, "Ошибка").ShowDialog();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            LoadNewPhoto();
        }

        private void LoadNewPhoto()
        {
            try
            {
                photo.Source = null;
                Card card = (Card)this.DataContext;
                OpenFileDialog op = new OpenFileDialog();

                op.Filter = "";

                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                op.Filter = string.Format("{0}| All image files ({1})|{1}|All files|*",
                    string.Join("|", codecs.Select(codec =>
                    string.Format("{0} ({1})|{1}", codec.CodecName, codec.FilenameExtension)).ToArray()),
                    string.Join(";", codecs.Select(codec => codec.FilenameExtension).ToArray()));
                op.FilterIndex = 6;

                if (op.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                    return;
                string filename = op.FileName;

                photo.Source = Mediator.instance.NonBlockingLoad(filename);

                op.Dispose();
            }
            catch (Exception ex)
            {
                new MsgBox(ex.Message, "Ошибка").ShowDialog();
            }
        }

        private void btn_save_foto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (photo.Source != null)
                {
                    Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                    dlg.FileName = "newFlowerPhoto"; // Default file name
                    dlg.DefaultExt = ".jpg"; // Default file extension
                    dlg.Filter = "JPG Files (*.jpg)|*.jpg"; // Filter files by extension

                    // Show save file dialog box
                    Nullable<bool> result = dlg.ShowDialog();

                    // Process save file dialog box results
                    if (result == true)
                    {
                        // Save document
                        string filename = dlg.FileName;

                        JpegBitmapEncoder jpegBitmapEncoder = new JpegBitmapEncoder();
                        jpegBitmapEncoder.Frames.Add(BitmapFrame.Create(photo.Source as BitmapSource));
                        using (FileStream fileStream = new FileStream(filename, FileMode.Create))
                            jpegBitmapEncoder.Save(fileStream);

                        new MsgBox("Фото сохранено!", "Информация").ShowDialog();
                    }
                }
                else
                {
                    throw new Exception("Нечего сохранять!");
                }
            }
            catch (Exception ex)
            {
                new MsgBox(ex.Message, "Ошибка").ShowDialog();
            }
        }
    }
}
