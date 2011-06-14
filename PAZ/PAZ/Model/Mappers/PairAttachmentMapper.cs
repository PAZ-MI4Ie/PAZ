using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PAZMySQL;
using MySql.Data.MySqlClient;

namespace PAZ.Model.Mappers
{
	class PairAttachmentMapper : Mapper
	{
		public PairAttachmentMapper(MysqlDb db)
            : base(db)
        {

        }

        protected Pair ProcessRow(Pair pair, MySqlDataReader Reader)
        {
            return pair;
        }

        public Pair Find(int id)
        {
			// moet nog goed uitgewerkt worden.
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT user_id, pair_id FROM pair_attachment WHERE pair_attachment.pair_id = ?id";
            command.Parameters.Add(new MySqlParameter("?id", MySqlDbType.Int32)).Value = id;
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            Reader.Read();//Only 1 row
            this._db.CloseConnection();
            return this.ProcessRow(new Pair(), Reader);
        }
	}
}
