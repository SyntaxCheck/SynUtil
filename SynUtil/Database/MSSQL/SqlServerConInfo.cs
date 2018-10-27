using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynUtil.Database.MSSQL
{
    //TODO add AD support
    public class SqlServerConInfo
    {
        private string server, database, user, password, dbParm;
        private int timeout;

        public string Database
        {
            get { return database; }
            set { database = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public string Server
        {
            get { return server; }
            set { server = value; }
        }
        public int Timeout
        {
            get { return timeout; }
            set { timeout = value; }
        }
        public string User
        {
            get { return user; }
            set { user = value; }
        }
        public string DbParm
        {
            get { return dbParm; }
            set { dbParm = value; }
        }
        //Compute ConnectionString
        public string ConnectionString
        {
            get
            {
                return "user id=" + user + "; password=" + password + "; server=" + server + "; database=" + database + "; connection timeout=" + timeout.ToString() + "; Trusted_Connection=false; " + dbParm;
            }
        }

        public SqlServerConInfo(string database = "", string server = "", string user = "", string password = "", int timeout = 30)
        {
            Database = database;
            Server = server;
            User = user;
            Password = password;
            Timeout = timeout;
        }
    }
}
