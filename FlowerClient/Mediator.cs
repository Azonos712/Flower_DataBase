using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void Execute()
        {
            instance.Command = new NpgsqlCommand(instance.SQL, instance.Connection);
            instance.Command.ExecuteNonQuery();
        }
    }
}
