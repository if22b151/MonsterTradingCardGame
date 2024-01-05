using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace MTCGNew {
    internal abstract class DBConnection {
        protected const string _connection = "Host=127.0.0.1;Database=MCTG;Username=MCTG;Password=MonstercardTradinggame;";

        protected void AddParameter(IDbCommand command, string name, object value, DbType type) {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            parameter.DbType = type;
            command.Parameters.Add(parameter);
        }
    }
}
