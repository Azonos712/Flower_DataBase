using System.Windows;

namespace FlowerClient
{
    public partial class MsgBox : Window
    {
        public MsgBox(string info, string title)
        {
            InitializeComponent();
            this.Title = title;
            txt_info.Text = info;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
