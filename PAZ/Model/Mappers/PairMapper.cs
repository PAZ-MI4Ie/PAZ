using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PAZMySQL;
using MySql.Data.MySqlClient;

namespace PAZ.Model.Mappers
{
    class PairMapper : Mapper
    {
        public PairMapper(MysqlDb db)
            : base(db)
        {

        }

        protected Pair ProcessRow(Pair pair, MySqlDataReader Reader)
        {
            pair.ID = Reader.GetInt32(0);
            pair.Number_of_guests = Reader.GetInt32(1);
            StudentMapper studentmapper = new StudentMapper(MysqlDb.GetInstance());
            pair.Student1 = studentmapper.ProcessRow(new Student(), Reader, 2);
            pair.Student2 = studentmapper.ProcessRow(new Student(), Reader, 11);
            return pair;
        }

        public Pair Find(int id)
        {
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT pair.id, number_of_guests, userstudent1.id, userstudent1.username, userstudent1.firstname, userstudent1.surname, userstudent1.email, userstudent1.user_type, userstudent1.status, student1.studentnumber, student1.study, userstudent2.id, userstudent2.username, userstudent2.firstname, userstudent2.surname, userstudent2.email, userstudent2.user_type, userstudent2.status, student2.studentnumber, student2.study FROM pair, user AS userstudent1, student AS student1, user AS userstudent2, student AS student2 WHERE student1.user_id = pair.student1 AND userstudent1.id = student1.user_id AND student2.user_id = pair.student2 AND userstudent2.id = student2.user_id AND pair.id = ?id";
            command.Parameters.Add(new MySqlParameter("?id", MySqlDbType.Int32)).Value = id;
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            Reader.Read();//Only 1 row
            this._db.CloseConnection();
            return this.ProcessRow(new Pair(), Reader);
        }
    }
}