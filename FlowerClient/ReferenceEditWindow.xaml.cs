using System;
using System.Windows;
using System.Windows.Documents;

namespace FlowerClient
{
    public partial class ReferenceEditWindow : Window
    {
        bool addMode = true;
        string table;
        string old_value;
        public ReferenceEditWindow(bool mode, string t, string value = "")
        {
            InitializeComponent();
            addMode = mode;
            table = t;
            txt_info.Text = old_value = value;
        }

        void AddNewReference()
        {
            Mediator.instance.SQL = "select add_reference_value('" + table + "','" + txt_info.Text.Trim() + "');";
            Mediator.instance.Execute();
        }

        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txt_info.Text.Trim()))
                {
                    throw new Exception("Заполните поле!");
                }

                if (new DlgBox("Вы точно хотите совершить это действие?", "Предупреждение", "Да", "Нет").ShowDialog() == true)
                {
                    if (addMode == true)
                    {
                        AddNewReference();
                    }
                    else
                    {

                    }

                    DialogResult = true;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("23505"))
                {
                    new MsgBox("Такое значение уже существует!", "Ошибка").ShowDialog();
                }
                else
                {
                    new MsgBox(ex.Message, "Ошибка").ShowDialog();
                }
            }
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}