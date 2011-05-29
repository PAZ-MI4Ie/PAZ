using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using PAZ.Model;

namespace PAZMySQL
{
    class UserMapper : Mapper
    {
        public UserMapper(MysqlDb db) : base(db)
        {
            
        }

        private User ProcessRow(MySqlDataReader Reader)
        {
            User user = new User();
            user.Id = Reader.GetInt32(0);
            user.Username = Reader.GetString(1);
            user.Firstname = Reader.GetString(2);
            user.Surname = Reader.GetString(3);
            user.Email = Reader.GetString(4);
            user.User_type = Reader.GetString(5);
            user.Status = Reader.GetString(6);
            return user;
        }

        public User Find(int id)
        {
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT id, username, firstname, surname, email, user_type, status FROM user WHERE id = ?id";
            command.Parameters.Add(new MySqlParameter("?id", MySqlDbType.Int32)).Value = 1;
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            Reader.Read();//Only 1 row
            return this.ProcessRow(Reader);
        }

        public User[] FindAll()
        {
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT id, username, firstname, surname, email, user_type, status FROM user";
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            User[] result = new User[170];//Temporary 170, have to find a way to make size dynamic in F*cking C#
            int i = 0;
            while (Reader.Read())
            {
                result[i++] = this.ProcessRow(Reader);
            }
            return result;
        }
    }
}
