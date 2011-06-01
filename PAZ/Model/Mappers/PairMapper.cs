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
            pair.Student1_id = Reader.GetInt32(2);
            pair.Student2_id = Reader.GetInt32(3);
            return pair;
        }

        public Pair Find(int id)
        {
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT id, number_of_guests, student1, student2 FROM pair WHERE pair.id = ?id";
            command.Parameters.Add(new MySqlParameter("?id", MySqlDbType.Int32)).Value = id;
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            Reader.Read();//Only 1 row
            this._db.CloseConnection();
            return this.ProcessRow(new Pair(), Reader);
        }
    }
}