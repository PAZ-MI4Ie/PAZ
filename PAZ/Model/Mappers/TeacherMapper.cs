using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using PAZ.Model;

namespace PAZMySQL
{
    public class TeacherMapper : UserMapper
    {
        public TeacherMapper(MysqlDb db)
            : base(db)
        {

        }

        private Teacher ProcessRow(Teacher teacher, MySqlDataReader Reader)
        {
            base.ProcessRow(teacher, Reader);
            //teacher data
            teacher.Session_spread = (Teacher.session_spread) Enum.Parse(typeof(Teacher.session_spread), Reader.GetString(8), true);
            return teacher;
        }

        public Teacher Find(int id)
        {
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT id, username, firstname, surname, email, user_type, status, was_changed, session_spread FROM user, teacher WHERE user.id = teacher.user_id AND id = ?id";
            command.Parameters.Add(new MySqlParameter("?id", MySqlDbType.Int32)).Value = id;
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            Reader.Read();//Only 1 row
            this._db.CloseConnection();
            return this.ProcessRow(new Teacher(), Reader);
        }

        public List<Teacher> FindAll()
        {
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT id, username, firstname, surname, email, user_type, status, was_changed, session_spread FROM user, teacher WHERE user.id = teacher.user_id";
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            List<Teacher> result = new List<Teacher>();
            while (Reader.Read())
            {
                result.Add(this.ProcessRow(new Teacher(), Reader));
            }
            this._db.CloseConnection();
            return result;
        }

        public void Save(Teacher teacher)
        {
			//TODO: blocked days

            Boolean insert = true;
            if (teacher.Id != 0)
            {
                insert = false;
            }
			//Base
            base.Save(teacher);
            
			//Save teacher
			this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            if (insert)
            {
                command.CommandText = "INSERT INTO teacher (user_id, session_spread) VALUES " +
                "(?user_id, ?session_spread)";
            }
            else
            {
                command.CommandText = "UPDATE student (session_spread) VALUES " +
                "(?session_spread) WHERE user_id = ?user_id";
            }
            command.Parameters.Add(new MySqlParameter("?user_id", MySqlDbType.Int32)).Value = teacher.Id;
            command.Parameters.Add(new MySqlParameter("?session_spread", MySqlDbType.String)).Value = teacher.Session_spread.ToString();
			this._db.ExecuteCommand(command);
			this._db.CloseConnection();
        }
    }
}