using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using PAZ.Model;

namespace PAZMySQL
{
    class AdminMapper : UserMapper
    {
        public AdminMapper(MysqlDb db)
            : base(db)
        {

        }

        protected Admin ProcessRow(Admin admin, MySqlDataReader Reader)
        {
            base.ProcessRow(admin, Reader);
            return admin;
        }

        // Waarom fixen jullie dit niet een keer
        // 1. Voeg override toe;
        // 2. Markeer de functie waarvan je override als virtual.
        // 3. Rinse n repeat tot alle warnings weg zijn
        // 4. ???
        // 5. Profit.
        // Ik zou het zelf doen, maar weet niet of jullie een bepaalde reden hebben om de warnings te laten staan.
        public Admin Find(int id)
        {
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT id, username, firstname, surname, email, user_type, status FROM user, admin WHERE user.id = admin.user_id AND id = ?id";
            command.Parameters.Add(new MySqlParameter("?id", MySqlDbType.Int32)).Value = id;
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            Reader.Read();//Only 1 row
            this._db.CloseConnection();
            return this.ProcessRow(new Admin(), Reader);
        }

        public List<Admin> FindAll()
        {
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT id, username, firstname, surname, email, user_type, status FROM user, admin WHERE user.id = admin.user_id";
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            List<Admin> result = new List<Admin>();
            while (Reader.Read())
            {
                result.Add(this.ProcessRow(new Admin(), Reader));
            }
            this._db.CloseConnection();
            return result;
        }

        public Boolean CheckLogin(String username, String password)
        {
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT id FROM user, admin WHERE username = ?username AND user.id = admin.user_id AND passwordhash = ?passwordhash";
            command.Parameters.Add(new MySqlParameter("?username", MySqlDbType.String)).Value = username;

            MD5 md5 = new MD5CryptoServiceProvider();
            Byte[] originalBytes = ASCIIEncoding.Default.GetBytes(password);
            Byte[] encodedBytes = md5.ComputeHash(originalBytes);
            command.Parameters.Add(new MySqlParameter("?passwordhash", MySqlDbType.String)).Value = BitConverter.ToString(encodedBytes);
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            Boolean found = Reader.Read();
            this._db.CloseConnection();
            return found;
        }
    }
}
