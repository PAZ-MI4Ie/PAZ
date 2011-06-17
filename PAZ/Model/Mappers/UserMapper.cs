using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using PAZ.Model;

namespace PAZMySQL
{
    public class UserMapper : Mapper
    {
        public UserMapper(MysqlDb db) : base(db)
        {
            
        }

        public User ProcessRow(User user, MySqlDataReader Reader)
        {
            return this.ProcessRow(user, Reader, 0);
        }

        public User ProcessRow(User user, MySqlDataReader Reader, int offset)
        {
            user.Id = Reader.GetInt32(0 + offset);
            user.Username = Reader.GetString(1 + offset);
            user.Firstname = Reader.GetString(2 + offset);
            user.Surname = Reader.GetString(3 + offset);
            user.Email = Reader.GetString(4 + offset);
            user.User_type = Reader.GetString(5 + offset);
            user.Status = Reader.GetString(6 + offset);
            user.WasChanged = Convert.ToBoolean(Reader.GetInt32(7 + offset));
            return user;
        }

        public User Find(int id)
        {
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT id, username, firstname, surname, email, user_type, status, was_changed FROM user WHERE id = ?id";
            command.Parameters.Add(new MySqlParameter("?id", MySqlDbType.Int32)).Value = id;
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            Reader.Read();//Only 1 row
            this._db.CloseConnection();
            return this.ProcessRow(new User(), Reader);
        }

		public Boolean FindWithDuplicateCheck(int studentnumber, string email)
		{
			this._db.OpenConnection();
			MySqlCommand command = this._db.CreateCommand();
			command.CommandText = "SELECT email, studentnumber FROM user, student WHERE student.studentnumber = ?studentnumber OR user.email = ?email";
			command.Parameters.Add(new MySqlParameter("?studentnumber", MySqlDbType.Int32)).Value = studentnumber;
			command.Parameters.Add(new MySqlParameter("?email", MySqlDbType.String)).Value = email;
			MySqlDataReader Reader = this._db.ExecuteCommand(command);
			Reader.Read();//Only 1 row
			Reader.Close();
			String strResult = String.Empty;
			strResult = (String)command.ExecuteScalar();

			Boolean found = false;
			if (strResult.Length != 0)
			{
				found = true;
			}

			this._db.CloseConnection();
			return found;
		}

        public List<User> FindAll()
        {
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();

            // Verzoekje van Yorg aan Teun, deze sorteren op naam, gewoon een extra-tje
            command.CommandText = "SELECT id, username, firstname, surname, email, user_type, status, was_changed FROM user";
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            List<User> result = new List<User>();
            while (Reader.Read())
            {
                result.Add(this.ProcessRow(new User(), Reader));
            }
            this._db.CloseConnection();
            return result;
        }

        public void Save(User user)
        {
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            if (user.Id != 0)
            {
                command.CommandText = "UPDATE user SET username=?username, firstname=?firstname, surname=?surname, email=?email, user_type=?user_type, status=?status, was_changed=?was_changed WHERE id = ?id";
                command.Parameters.Add(new MySqlParameter("?id", MySqlDbType.Int32)).Value = user.Id;
            }
            else
            {
                command.CommandText = "INSERT INTO user (username, firstname, surname, email, user_type, status, was_changed) VALUES "+
                "(?username, ?firstname, ?surname, ?email, ?user_type, ?status, ?was_changed)";
            }
            command.Parameters.Add(new MySqlParameter("?username", MySqlDbType.String)).Value = user.Username;
            command.Parameters.Add(new MySqlParameter("?firstname", MySqlDbType.String)).Value = user.Firstname;
            command.Parameters.Add(new MySqlParameter("?surname", MySqlDbType.String)).Value = user.Surname;
            command.Parameters.Add(new MySqlParameter("?email", MySqlDbType.String)).Value = user.Email;
            command.Parameters.Add(new MySqlParameter("?user_type", MySqlDbType.String)).Value = user.User_type;
            command.Parameters.Add(new MySqlParameter("?status", MySqlDbType.String)).Value = user.Status;
            command.Parameters.Add(new MySqlParameter("?was_changed", MySqlDbType.Int32)).Value = user.WasChanged;
            this._db.ExecuteCommand(command);
            this._db.CloseConnection();
            this._db.OpenConnection();
            MySqlCommand command2 = this._db.CreateCommand();
            command2.CommandText = "SELECT LAST_INSERT_ID()";
            MySqlDataReader Reader = this._db.ExecuteCommand(command2);
            Reader.Read();
            this._db.CloseConnection();
            user.Id = Reader.GetInt32(0);
        }

        /**
         * Dit is een functieverzoek aan Teun: Verwijder alle entries in de tabel user(die geen admin zijn) (DELETE FROM user WHERE user_type != 'admin')
         * Return: true als geslaagd, anders false(kan dat wel? misschien false als er geen non admin users zijn om te verwijderen?)
         */
        public bool Delete()
        {
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "DELETE FROM user WHERE user_type != ?usertype";
            command.Parameters.Add(new MySqlParameter("?usertype", MySqlDbType.String)).Value = "admin";
            this._db.ExecuteCommand(command);
            this._db.CloseConnection();
            return true;
        }

        public bool Delete(User user)
        {
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "DELETE FROM user WHERE id = ?id";
            command.Parameters.Add(new MySqlParameter("?id", MySqlDbType.Int32)).Value = user.Id;
            this._db.ExecuteCommand(command);
            this._db.CloseConnection();
            return true;
        }

        public bool Delete(List<User> users)
        {
            foreach (User user in users) {
                this.Delete(user);
            }
            return true;
        }
    }
}