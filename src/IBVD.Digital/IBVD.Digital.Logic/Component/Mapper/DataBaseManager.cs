using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace IBVD.Digital.Logic.Component.Mapper
{
    public static class DataBaseManager
    {
        private static readonly Database db = DatabaseFactory.CreateDatabase("DB");

        public static Database GetDataBase()
        {
            return db;
        }
    }
}
