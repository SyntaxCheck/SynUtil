using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynUtil.Database.MSSQL
{
    public class SqlServer
    {
        public SqlServer()
        {
        }

        //TODO Finish filling out the connection Object
        public SqlConnection GetConnection(SqlServerConInfo conInfo)
        {
            SqlConnection connection = new SqlConnection();

            connection.ConnectionString = conInfo.ConnectionString;

            return connection;
        }
    }
}
