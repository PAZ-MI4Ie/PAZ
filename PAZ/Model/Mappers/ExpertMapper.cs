using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using PAZ.Model;

namespace PAZMySQL
{
    class ExpertMapper : Mapper
    {
        public ExpertMapper(MysqlDb db)
            : base(db)
        {

        }

        private Expert ProcessRow(MySqlDataReader Reader)
        {
            Expert expert = new Expert();

            //user data
            expert.Id = Reader.GetInt32(0);
            expert.Username = Reader.GetString(1);
            expert.Firstname = Reader.GetString(2);
            expert.Surname = Reader.GetString(3);
            expert.Email = Reader.GetString(4);
            expert.User_type = Reader.GetString(5);
            expert.Status = Reader.GetString(6);
            //expert data
            expert.Company = Reader.GetString(7);
            expert.Address = Reader.GetString(8);
            expert.Postcode = Reader.GetString(9);
            return expert;
        }

        public Expert Find(int id)
        {
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT id, username, firstname, surname, email, user_type, status, company, address, postcode FROM user, expert WHERE user.id = expert.user_id AND id = ?id";
            command.Parameters.Add(new MySqlParameter("?id", MySqlDbType.Int32)).Value = 1;
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            Reader.Read();//Only 1 row
            return this.ProcessRow(Reader);
        }

        public Expert[] FindAll()
        {
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT id, username, firstname, surname, email, user_type, status, company, address, postcode FROM user, expert WHERE user.id = expert.user_id";
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            Expert[] result = new Expert[170];//Temporary 170, have to find a way to make size dynamic in F*cking C#
            int i = 0;
            while (Reader.Read())
            {
                result[i++] = this.ProcessRow(Reader);
            }
            return result;
        }
    }
}
