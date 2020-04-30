﻿using Npgsql;
using System.Data;

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

        public void Execute()
        {
            instance.Command = new NpgsqlCommand(instance.SQL, instance.Connection);
            instance.Command.ExecuteNonQuery();
        }
    }
}
