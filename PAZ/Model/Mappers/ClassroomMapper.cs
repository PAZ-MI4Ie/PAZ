using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using PAZ.Model;

namespace PAZMySQL
{
    public class ClassroomMapper : Mapper
    {
        public ClassroomMapper(MysqlDb db) : base(db)
        {
            
        }

        public Classroom ProcessRow(Classroom classroom, MySqlDataReader Reader)
        {
            return this.ProcessRow(classroom, Reader, 0);
        }

        public Classroom ProcessRow(Classroom classroom, MySqlDataReader Reader, int offset)
        {
            classroom.Id = Reader.GetInt32(0 + offset);
            classroom.Room_number = Reader.GetString(1 + offset);
            return classroom;
        }

        public Classroom Find(int id)
        {
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT id, room_number FROM classroom WHERE id = ?id";
            command.Parameters.Add(new MySqlParameter("?id", MySqlDbType.Int32)).Value = id;
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            Reader.Read();//Only 1 row
            this._db.CloseConnection();
            return this.ProcessRow(new Classroom(), Reader);
        }

        public List<Classroom> FindAll()
        {
            // TO DO?

            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT id, room_number FROM classroom";
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            List<Classroom> result = new List<Classroom>();
            while (Reader.Read())
            {
                result.Add(this.ProcessRow(new Classroom(), Reader));
            }
            this._db.CloseConnection();
            return result;
        }

        public void Save(Classroom classroom)
        {
            // TO DO?

			Boolean insert = true;
			if (classroom.Id != 0)
			{
				insert = false;
			}
			this._db.OpenConnection();
			MySqlCommand command = this._db.CreateCommand();
			if (insert)
			{
				command.CommandText = "INSERT INTO classroom (id, room_number) VALUES " +
				"(?id, ?room_number)";
			}
			else
			{
				command.CommandText = "UPDATE student (room_number) VALUES " +
				"(?id) WHERE user_id = ?id";
			}
			command.Parameters.Add(new MySqlParameter("?id", MySqlDbType.Int32)).Value = classroom.Id;
			command.Parameters.Add(new MySqlParameter("?room_number", MySqlDbType.String)).Value = classroom.Room_number.ToString();
			this._db.ExecuteCommand(command);
			this._db.CloseConnection();
        }

        /**
         * Dit is een functieverzoek aan Teun: Verwijder alle entries in de tabel classroom (DELETE FROM classroom)
         * Return: true als geslaagd, anders false(kan dat wel? misschien false als lokaal tabel leeg is?)
         */
        public bool Delete()
        {
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "DELETE FROM classroom WHERE 1=1";
            this._db.ExecuteCommand(command);
            this._db.CloseConnection();
            return true;
        }

        public bool Delete(Classroom classroom)
        {
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "DELETE FROM classroom WHERE id=?id";
            command.Parameters.Add(new MySqlParameter("?id", MySqlDbType.Int32)).Value = classroom.Id;
            this._db.ExecuteCommand(command);
            this._db.CloseConnection();
            return true;
        }

        public bool Delete(List<Classroom> classrooms)
        {
            foreach (Classroom classroom in classrooms)
            {
                this.Delete(classroom);
            }
            return true;
        }
    }
}