using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace IBVD.Digital.Logic.Helper
{
    public static class DataBaseHelper
    {
        private static Database db = DatabaseFactory.CreateDatabase("DB");

        public static void BeginTransaction()
        {
        }

    }
}
