using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FlowerClient
{
    class Mediator
    {
        public static Mediator instance = new Mediator();

        public NpgsqlConnection Connection { get; set; }
        public string SQL { get; set; }
        public NpgsqlCommand Command { get; set; }

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
    }
}
