using System;
using System.Windows;
using System.Windows.Forms;

namespace FlowerClient
{
    public partial class PathWindow : Window
    {
        public PathWindow()
        {
            InitializeComponent();
            txt_path.Text = Mediator.instance.Path;
        }

        private void Btn_apply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txt_path.Text))
                {
                    throw new Exception("Вы не указали путь!");
                }

                if (new DlgBox("Вы точно хотите обновить путь?", "Обновление", "Да", "Нет").ShowDialog() == true)
                {
                    Mediator.instance.SQL = ("select set_path('" + txt_path.Text.Trim() + "');");
                    Mediator.instance.Execute();
                    Mediator.instance.Path = txt_path.Text.Trim();

                    new MsgBox("Путь успешно обновлен", "Информация").ShowDialog();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                new MsgBox(ex.Message, "Ошибка").ShowDialog();
            }
        }

        private void Btn_view_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();

            DialogResult result = folderBrowser.ShowDialog();

            if (!string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
            {
                txt_path.Text = folderBrowser.SelectedPath + "\\";
            }
        }
    }
}
