using System;
using MySql.Data.MySqlClient;
using PAZMySQL;

namespace PAZ.Model.Mappers
{
    public class EmailTemplateMapper : Mapper
    {
        public EmailTemplateMapper(MysqlDb db) : base(db)
        {
            
        }

        public EmailTemplate Find(int id)
        {
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT displayname, inleiding, informatie, afsluiting, afzenders FROM email_template WHERE id = ?id";
            command.Parameters.Add(new MySqlParameter("?id", MySqlDbType.Int32)).Value = id;
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            Reader.Read();//Only 1 row
            this._db.CloseConnection();
            
            int index = -1;
            return new EmailTemplate(Reader.GetString(++index), Reader.GetString(++index), Reader.GetString(++index), Reader.GetString(++index), Reader.GetString(++index));
        }

        //public void Save(EmailTemplate emailTemplate)
        //{
        //    this._db.OpenConnection();
        //    MySqlCommand command = this._db.CreateCommand();

        //    command.CommandText = "UPDATE email_template (displayname, inleiding, informatie, afsluiting, afzenders) VALUES WHERE user_id = 1";

        //    command.Parameters.Add(new MySqlParameter("?id", MySqlDbType.Int32)).Value = emailTemplate.Id;
        //    command.Parameters.Add(new MySqlParameter("?room_number", MySqlDbType.String)).Value = emailTemplate.Room_number.ToString();
        //    this._db.ExecuteCommand(command);
        //    this._db.CloseConnection();
        //}
    }
}
