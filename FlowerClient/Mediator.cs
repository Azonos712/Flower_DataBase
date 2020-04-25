using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerClient
{
    class Mediator
    {
        public static Mediator instance;

        public NpgsqlConnection Connection { get; set; }
        public string SQL { get; set; }
        public NpgsqlCommand Command { get; set; }
    }
}
