using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using PAZ.Model;

namespace PAZMySQL
{
    class ClassroomMapper : Mapper
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

        public void Save(User user)
        {
            // TO DO?

            //this._db.OpenConnection();
            //MySqlCommand command = this._db.CreateCommand();
            //if (user.Id != 0)
            //{
            //    command.CommandText = "UPDATE user (username, firstname, surname, email, user_type, status) VALUES " +
            //    "(?username, ?firstname, ?surname, ?email, ?user_type, ?status) WHERE id = ?id";
            //    command.Parameters.Add(new MySqlParameter("?id", MySqlDbType.Int32)).Value = user.Id;
            //}
            //else
            //{
            //    command.CommandText = "INSERT INTO user (username, firstname, surname, email, user_type, status) VALUES "+
            //    "(?username, ?firstname, ?surname, ?email, ?user_type, ?status)";
            //}
            //command.Parameters.Add(new MySqlParameter("?username", MySqlDbType.String)).Value = user.Username;
            //command.Parameters.Add(new MySqlParameter("?firstname", MySqlDbType.String)).Value = user.Firstname;
            //command.Parameters.Add(new MySqlParameter("?surname", MySqlDbType.String)).Value = user.Surname;
            //command.Parameters.Add(new MySqlParameter("?email", MySqlDbType.String)).Value = user.Email;
            //command.Parameters.Add(new MySqlParameter("?user_type", MySqlDbType.String)).Value = user.User_type;
            //command.Parameters.Add(new MySqlParameter("?status", MySqlDbType.String)).Value = user.Status;
            //this._db.ExecuteCommand(command);
            //this._db.CloseConnection();
            //this._db.OpenConnection();
            //MySqlCommand command2 = this._db.CreateCommand();
            //command2.CommandText = "SELECT LAST_INSERT_ID()";
            //MySqlDataReader Reader = this._db.ExecuteCommand(command2);
            //Reader.Read();
            //this._db.CloseConnection();
            //user.Id = Reader.GetInt32(0);
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