﻿using System;
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
            command.CommandText = "SELECT id, displayname, inleiding, informatie, afsluiting, afzenders FROM email_template WHERE id = ?id";
            command.Parameters.Add(new MySqlParameter("?id", MySqlDbType.Int32)).Value = id;
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            Reader.Read();//Only 1 row
            this._db.CloseConnection();
            
            int index = -1;
            return new EmailTemplate(Reader.GetInt32(++index), Reader.GetString(++index), Reader.GetString(++index), Reader.GetString(++index), Reader.GetString(++index), Reader.GetString(++index));
        }

        public void Save(EmailTemplate emailTemplate)
        {
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();

            command.CommandText = "UPDATE email_template SET displayname=?displayname, inleiding=?inleiding, informatie=?informatie, afsluiting=?afsluiting, afzenders=?afzenders WHERE id = ?id";

            command.Parameters.Add(new MySqlParameter("?id", MySqlDbType.Int32)).Value = emailTemplate.Id;
            command.Parameters.Add(new MySqlParameter("?displayname", MySqlDbType.String)).Value = emailTemplate.Displayname;
            command.Parameters.Add(new MySqlParameter("?inleiding", MySqlDbType.Text)).Value = emailTemplate.Inleiding;
            command.Parameters.Add(new MySqlParameter("?informatie", MySqlDbType.Text)).Value = emailTemplate.Informatie;
            command.Parameters.Add(new MySqlParameter("?afsluiting", MySqlDbType.Text)).Value = emailTemplate.Afsluiting;
            command.Parameters.Add(new MySqlParameter("?afzenders", MySqlDbType.String)).Value = emailTemplate.Afzenders;
            
            this._db.ExecuteCommand(command);
            this._db.CloseConnection();
        }
    }
}
