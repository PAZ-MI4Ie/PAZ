using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using PAZ.Model;

namespace PAZMySQL
{
    public class DaytimeMapper : Mapper
    {
        public DaytimeMapper(MysqlDb db) : base(db)
        {
            
        }

        public Daytime ProcessRow(Daytime daytime, MySqlDataReader Reader)
        {
            return this.ProcessRow(daytime, Reader, 0);
        }

        public Daytime ProcessRow(Daytime daytime, MySqlDataReader Reader, int offset)
        {
            daytime.Id = Reader.GetInt32(0 + offset);
            daytime.Date = Reader.GetDateTime(1 + offset);
            daytime.Timeslot = Reader.GetInt32(2 + offset);
            return daytime;
        }

        public Daytime Find(int id)
        {
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT id, date, timeslot FROM daytime WHERE id = ?id";
            command.Parameters.Add(new MySqlParameter("?id", MySqlDbType.Int32)).Value = id;
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            Reader.Read();//Only 1 row
            this._db.CloseConnection();
            return this.ProcessRow(new Daytime(), Reader);
        }

		public List<Daytime> FindWithDate(string date)
		{
			string[] d = date.Split('-');
			date = d[2] + "-" + d[1] + "-" + d[0];
			this._db.OpenConnection();
			MySqlCommand command = this._db.CreateCommand();
			command.CommandText = "SELECT id, date, timeslot FROM daytime WHERE date = ?date";
			command.Parameters.Add(new MySqlParameter("?date", MySqlDbType.String)).Value = date;
			MySqlDataReader Reader = this._db.ExecuteCommand(command);
			List<Daytime> result = new List<Daytime>();
			while (Reader.Read())
			{
				result.Add(this.ProcessRow(new Daytime(), Reader));
			}
			this._db.CloseConnection();
			return result;
		}

        public Daytime Find(string date, int timeslot)
        {
            string[] d = date.Split('-');
            date = d[2] + "-" + d[1] + "-" + d[0];
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT id, date, timeslot FROM daytime WHERE date = ?date AND timeslot = ?timeslot";
            command.Parameters.Add(new MySqlParameter("?date", MySqlDbType.String)).Value = date;
            command.Parameters.Add(new MySqlParameter("?timeslot", MySqlDbType.Int32)).Value = timeslot+1;
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            if (Reader.Read())
			{//Only 1 row
				this._db.CloseConnection();
                return this.ProcessRow(new Daytime(), Reader);
			}
			this._db.CloseConnection();
            return null;
        }

        public List<Daytime> FindAll()
        {
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT id, date, timeslot FROM daytime";
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            List<Daytime> result = new List<Daytime>();
            while (Reader.Read())
            {
                result.Add(this.ProcessRow(new Daytime(), Reader));
            }
            this._db.CloseConnection();
            return result;
        }
        
        public void Save(Daytime daytime)
        {
            Boolean insert = true;
            if (daytime.Id != 0)
            {
                insert = false;
            }
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            if (insert)
            {
                command.CommandText = "INSERT INTO daytime (date, timeslot) VALUES " +
                "(?date, ?timeslot)";
            }
            else
            {
                command.CommandText = "UPDATE daytime (date, timeslot) VALUES " +
                "(?date, ?timeslot) WHERE id = ?id";
            }
            command.Parameters.Add(new MySqlParameter("?id", MySqlDbType.Int32)).Value = daytime.Id;
            command.Parameters.Add(new MySqlParameter("?date", MySqlDbType.Datetime)).Value = daytime.Date;
            command.Parameters.Add(new MySqlParameter("?timeslot", MySqlDbType.Int32)).Value = daytime.Timeslot;
            this._db.ExecuteCommand(command);
            this._db.CloseConnection();
        }

        /**
         * Dit is een functieverzoek aan Teun: Verwijder alle entries in de tabel classroom (DELETE FROM classroom)
         * Return: true als geslaagd, anders false(kan dat wel? misschien false als lokaal tabel leeg is?)
         */
        public bool Delete()
        {
            return true;
        }
    }
}