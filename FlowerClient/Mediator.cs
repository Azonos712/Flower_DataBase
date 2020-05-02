using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace FlowerClient
{
    class Mediator
    {
        public static Mediator instance = new Mediator();

        public NpgsqlConnection Connection { get; set; }
        public NpgsqlCommand Command { get; set; }
        public string SQL { get; set; }
        public string Login { get; set; }
        public string Role { get; set; }
        public string Path { get; set; }

        public object ConvertQueryToValue()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(instance.SQL, instance.Connection);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            return dt.Rows[0].ItemArray[0];
        }

        public DataTable ConvertQueryToTable()
        {
            DataTable dt = new DataTable();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(SQL, Connection);
            da.Fill(dt);
            return dt;
        }

        public List<string> ConvertQueryToComboBox()
        {
            DataTable dt = new DataTable();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(SQL, Connection);
            da.Fill(dt);
            List<string> temp = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                temp.Add(dt.Rows[i].ItemArray[0].ToString());
            }
            return temp;
        }

        public void Execute()
        {
            instance.Command = new NpgsqlCommand(instance.SQL, instance.Connection);
            instance.Command.ExecuteNonQuery();
        }

        public DataTable ExecuteQuery()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(instance.SQL, instance.Connection);
                ds.Reset();
                da.Fill(ds);
                dt = ds.Tables[0];

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return dt;
        }

        public BitmapImage NonBlockingLoad(string path)
        {
            if (!(File.Exists(path)))
                return null;

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            image.UriSource = new Uri(path);
            image.EndInit();
            image.Freeze();
            return image;
        }


    }
}
