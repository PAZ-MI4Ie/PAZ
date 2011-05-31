using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using PAZ.Model;

namespace PAZMySQL
{
    class ExpertMapper : UserMapper
    {
        public ExpertMapper(MysqlDb db)
            : base(db)
        {

        }

        protected Expert ProcessRow(Expert expert, MySqlDataReader Reader)
        {
            base.ProcessRow(expert, Reader);
            //expert data
            expert.Company = Reader.GetString(7);
            expert.Address = Reader.GetString(8);
            expert.Postcode = Reader.GetString(9);
            return expert;
        }

        public Expert Find(int id)
        {
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT id, username, firstname, surname, email, user_type, status, company, address, postcode FROM user, expert WHERE user.id = expert.user_id AND id = ?id";
            command.Parameters.Add(new MySqlParameter("?id", MySqlDbType.Int32)).Value = id;
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            Reader.Read();//Only 1 row
            this._db.CloseConnection();
            return this.ProcessRow(new Expert(), Reader);
        }

        public List<Expert> FindAll()
        {
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT id, username, firstname, surname, email, user_type, status, company, address, postcode FROM user, expert WHERE user.id = expert.user_id";
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            List<Expert> result = new List<Expert>();
            while (Reader.Read())
            {
                result.Add(this.ProcessRow(new Expert(), Reader));
            }
            this._db.CloseConnection();
            return result;
        }
    }
}
