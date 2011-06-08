using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace PAZMySQL
{
    public class MysqlDb
    {
        private String _host;
        private String _username;
        private String _password;
        private String _database;
        private MySqlConnection _connection;
        private static MysqlDb _db;

        public static MysqlDb GetInstance()
        {
            if (MysqlDb._db == null)
            {
                MysqlDb._db = new MysqlDb("student.aii.avans.nl", "MI4Ie", "4DRcUrzV", "MI4Ie_db");
            }
            return MysqlDb._db;
        }

        public MysqlDb(String host, String username, String password, String database)
        {
            this._host = host;
            this._username = username;
            this._password = password;
            this._database = database;
            string MyConString = "SERVER="+this._host+";" +
                "DATABASE="+this._database+";" +
                "UID="+this._username+";" +
                "PASSWORD="+this._password+";";
            this._connection = new MySqlConnection(MyConString);
        }

        public void OpenConnection()
        {
            if (this._connection.State != System.Data.ConnectionState.Open)
            {
                this._connection.Open();
            }
        }

        public void CloseConnection()
        {
            this._connection.Close();
        }

        public MySqlCommand CreateCommand()
        {
            return this._connection.CreateCommand();
        }

        public MySqlDataReader ExecuteCommand(MySqlCommand command)
        {
            //command.ExecuteNonQuery();
            return command.ExecuteReader();
        }
    }
}
