using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using PAZ.Model;

namespace PAZMySQL
{
    class DaytimeMapper : Mapper
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
            return daytime;
        }

        public Daytime Find(int id)
        {
            // TO DO?
            //this._db.OpenConnection();
            //MySqlCommand command = this._db.CreateCommand();
            //command.CommandText = "SELECT id, username, firstname, surname, email, user_type, status FROM user WHERE id = ?id";
            //command.Parameters.Add(new MySqlParameter("?id", MySqlDbType.Int32)).Value = id;
            //MySqlDataReader Reader = this._db.ExecuteCommand(command);
            //Reader.Read();//Only 1 row
            //this._db.CloseConnection();
            //return this.ProcessRow(new Classroom(), Reader);

            return null;
        }

        public List<Classroom> FindAll()
        {
            // TO DO?

            //this._db.OpenConnection();
            //MySqlCommand command = this._db.CreateCommand();
            //command.CommandText = "SELECT id, username, firstname, surname, email, user_type, status FROM user";
            //MySqlDataReader Reader = this._db.ExecuteCommand(command);
            //List<User> result = new List<User>();
            //while (Reader.Read())
            //{
            //    result.Add(this.ProcessRow(new Classroom(), Reader));
            //}
            //this._db.CloseConnection();
            //return result;

            return null;
        }

        public void Save(Classroom classroom)
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