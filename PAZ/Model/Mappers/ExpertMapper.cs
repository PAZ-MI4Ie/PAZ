using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using PAZ.Model;

namespace PAZMySQL
{
    public class ExpertMapper : UserMapper
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

		public void Save(Expert expert)
		{
			Boolean insert = true;
			if (expert.Id != 0)
			{
				insert = false;
			}
			base.Save(expert);
			this._db.OpenConnection();
			MySqlCommand command = this._db.CreateCommand();
			if (insert)
			{
				command.CommandText = "INSERT INTO expert (user_id, company, address, postcode, telephone, city) VALUES " +
				"(?user_id, ?company, ?address, ?postcode, ?telephone, ?city)";
			}
			else
			{
				command.CommandText = "UPDATE student (company, address, postcode, telephone, city) VALUES " +
				"(?company, ?address, ?postcode, ?telephone, ?city) WHERE user_id = ?user_id";
			}
			command.Parameters.Add(new MySqlParameter("?user_id", MySqlDbType.Int32)).Value = expert.Id;
			command.Parameters.Add(new MySqlParameter("?company", MySqlDbType.String)).Value = expert.Company;
			command.Parameters.Add(new MySqlParameter("?address", MySqlDbType.String)).Value = expert.Address;
			command.Parameters.Add(new MySqlParameter("?postcode", MySqlDbType.String)).Value = expert.Postcode;
			command.Parameters.Add(new MySqlParameter("?telephone", MySqlDbType.String)).Value = expert.Telephone;
			command.Parameters.Add(new MySqlParameter("?city", MySqlDbType.String)).Value = expert.City;
			this._db.ExecuteCommand(command);
			this._db.CloseConnection();
		}
    }
}
