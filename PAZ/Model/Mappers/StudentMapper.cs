using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using PAZ.Model;

namespace PAZMySQL
{
    class StudentMapper : UserMapper
    {
        public StudentMapper(MysqlDb db)
            : base(db)
        {

        }

        protected Student ProcessRow(Student student, MySqlDataReader Reader)
        {
            return this.ProcessRow(student, Reader, 0);
        }

        protected Student ProcessRow(Student student, MySqlDataReader Reader, int offset)
        {
            base.ProcessRow(student, Reader, offset);
            //student data
            student.Studentnumber = Reader.GetInt32(7 + offset);
            student.Study = Reader.GetString(8 + offset);
            return student;
        }

        public Student Find(int id)
        {
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT id, username, firstname, surname, email, user_type, status, studentnumber, study FROM user, student WHERE user.id = student.user_id AND user.id = ?id";
            command.Parameters.Add(new MySqlParameter("?id", MySqlDbType.Int32)).Value = id;
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            Reader.Read();//Only 1 row
            this._db.CloseConnection();
            return this.ProcessRow(new Student(), Reader);
        }

        public List<Student> FindAll()
        {
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT id, username, firstname, surname, email, user_type, status, studentnumber, study FROM user, student WHERE user.id = student.user_id";
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            List<Student> result = new List<Student>();
            while (Reader.Read())
            {
                result.Add(this.ProcessRow(new Student(), Reader));
            }
            this._db.CloseConnection();
            return result;
        }

        public void Save(Student student)
        {
            Boolean insert = true;
            if (student.Id != 0)
            {
                insert = false;
            }
            base.Save(student);
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            if (insert)
            {
                command.CommandText = "INSERT INTO student (user_id, studentnumber, study) VALUES " +
                "(?user_id, ?studentnumber, ?study)";
            }
            else
            {
                command.CommandText = "UPDATE student (studentnumber, study) VALUES " +
                "(?studentnumber, ?study) WHERE user_id = ?user_id";
            }
            command.Parameters.Add(new MySqlParameter("?user_id", MySqlDbType.Int32)).Value = student.Id;
            command.Parameters.Add(new MySqlParameter("?studentnumber", MySqlDbType.Int32)).Value = student.Studentnumber;
            command.Parameters.Add(new MySqlParameter("?study", MySqlDbType.String)).Value = student.Study;
            this._db.ExecuteCommand(command);
            this._db.CloseConnection();
        }
    }
}
