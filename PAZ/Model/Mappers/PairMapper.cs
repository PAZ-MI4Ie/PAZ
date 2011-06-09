using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PAZMySQL;
using MySql.Data.MySqlClient;

namespace PAZ.Model.Mappers
{
    public class PairMapper : Mapper
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

        public List<Pair> FindAll()
        {
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT id, number_of_guests, student1, student2 FROM pair";
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            List<Pair> result = new List<Pair>();
            while (Reader.Read())
            {
                result.Add(this.ProcessRow(new Pair(), Reader));
            }
            this._db.CloseConnection();
            return result;
        }

        public Dictionary<int, string> FindAttachments(int pairId)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT user_id, user_type FROM pair_attachment LEFT JOIN user ON user.id = user_id WHERE pair_id = ?id";
            command.Parameters.Add(new MySqlParameter("?id", MySqlDbType.Int32)).Value = pairId;
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            while (Reader.Read())
            {
                result.Add(Reader.GetInt32(0), Reader.GetString(1));
            }
            this._db.CloseConnection();
            if (result.Count <= 0)
            {
                result.Add(21, "teacher");
                result.Add(23, "teacher");
                result.Add(26, "expert");
                result.Add(27, "expert");
            }
            return result;
        }
    }
}