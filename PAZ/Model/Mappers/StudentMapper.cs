using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using PAZ.Model;

namespace PAZMySQL
{
    class StudentMapper : Mapper
    {
        public StudentMapper(MysqlDb db)
            : base(db)
        {

        }

        private Student ProcessRow(MySqlDataReader Reader)
        {
            Student student = new Student();

            //user data
            student.Id = Reader.GetInt32(0);
            student.Username = Reader.GetString(1);
            student.Firstname = Reader.GetString(2);
            student.Surname = Reader.GetString(3);
            student.Email = Reader.GetString(4);
            student.User_type = Reader.GetString(5);
            student.Status = Reader.GetString(6);
            //student data
            student.Studentnumber = Reader.GetInt32(7);
            student.Study = Reader.GetString(8);
            return student;
        }

        public Student Find(int id)
        {
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT id, username, firstname, surname, email, user_type, status, studentnumber, study FROM user, student WHERE user.id = student.user_id AND id = ?id";
            command.Parameters.Add(new MySqlParameter("?id", MySqlDbType.Int32)).Value = 1;
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            Reader.Read();//Only 1 row
            return this.ProcessRow(Reader);
        }

        public Student[] FindAll()
        {
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT id, username, firstname, surname, email, user_type, status, studentnumber, study FROM user, student WHERE user.id = student.user_id";
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            Student[] result = new Student[170];//Temporary 170, have to find a way to make size dynamic in F*cking C#
            int i = 0;
            while (Reader.Read())
            {
                result[i++] = this.ProcessRow(Reader);
            }
            return result;
        }
    }
}
