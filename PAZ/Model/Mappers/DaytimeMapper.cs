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
            // TO DO?
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