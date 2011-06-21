using System;
using Ini;
using MySql.Data.MySqlClient;
using PAZ.Control;

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
                IniFile ini = PAZController.GetInstance().IniReader;
                MysqlDb._db = new MysqlDb(ini["DATABASESETTINGS"]["db_host"], ini["DATABASESETTINGS"]["db_username"], ini["DATABASESETTINGS"]["db_password"], ini["DATABASESETTINGS"]["db_database"]);
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
            return command.ExecuteReader();
        }
    }
}
