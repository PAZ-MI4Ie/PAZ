using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using PAZ.Model;

namespace PAZMySQL
{
    class TeacherMapper : Mapper
    {
        public TeacherMapper(MysqlDb db)
            : base(db)
        {

        }

        private Teacher ProcessRow(MySqlDataReader Reader)
        {
            Teacher teacher = new Teacher();

            //user data
            teacher.Id = Reader.GetInt32(0);
            teacher.Username = Reader.GetString(1);
            teacher.Firstname = Reader.GetString(2);
            teacher.Surname = Reader.GetString(3);
            teacher.Email = Reader.GetString(4);
            teacher.User_type = Reader.GetString(5);
            teacher.Status = Reader.GetString(6);
            //teacher data
            teacher.Session_spread = (Teacher.session_spread) Enum.Parse(typeof(Teacher.session_spread), Reader.GetString(7), true);
            return teacher;
        }

        public Teacher Find(int id)
        {
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT id, username, firstname, surname, email, user_type, status, session_spread FROM user, teacher WHERE user.id = teacher.user_id AND id = ?id";
            command.Parameters.Add(new MySqlParameter("?id", MySqlDbType.Int32)).Value = 1;
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            Reader.Read();//Only 1 row
            return this.ProcessRow(Reader);
        }

        public Teacher[] FindAll()
        {
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT id, username, firstname, surname, email, user_type, status, session_spread FROM user, teacher WHERE user.id = teacher.user_id";
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            Teacher[] result = new Teacher[170];//Temporary 170, have to find a way to make size dynamic in F*cking C#
            int i = 0;
            while (Reader.Read())
            {
                result[i++] = this.ProcessRow(Reader);
            }
            return result;
        }
    }
}
