using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using PAZ.Model;

namespace PAZMySQL
{
    class TeacherMapper : UserMapper
    {
        public TeacherMapper(MysqlDb db)
            : base(db)
        {

        }

        private Teacher ProcessRow(Teacher teacher, MySqlDataReader Reader)
        {
            base.ProcessRow(teacher, Reader);
            //teacher data
            teacher.Session_spread = (Teacher.session_spread) Enum.Parse(typeof(Teacher.session_spread), Reader.GetString(7), true);
            return teacher;
        }

        public Teacher Find(int id)
        {
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT id, username, firstname, surname, email, user_type, status, session_spread FROM user, teacher WHERE user.id = teacher.user_id AND id = ?id";
            command.Parameters.Add(new MySqlParameter("?id", MySqlDbType.Int32)).Value = id;
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            Reader.Read();//Only 1 row
            return this.ProcessRow(new Teacher(), Reader);
        }

        public List<Teacher> FindAll()
        {
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT id, username, firstname, surname, email, user_type, status, session_spread FROM user, teacher WHERE user.id = teacher.user_id";
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            List<Teacher> result = new List<Teacher>();
            while (Reader.Read())
            {
                result.Add(this.ProcessRow(new Teacher(), Reader));
            }
            return result;
        }
    }
}
