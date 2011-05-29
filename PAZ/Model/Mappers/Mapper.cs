using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAZMySQL
{
    class Mapper
    {
        protected MysqlDb _db;

        public Mapper(MysqlDb db)
        {
            this._db = db;
        }
    }
}
