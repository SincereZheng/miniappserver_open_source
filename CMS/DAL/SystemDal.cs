using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class SystemDal
    {
        public static HashObject GetLoginPageInfo()
        {
            using (DbHelper dbHelper = new DbHelper())
            {
                DataTable dt = dbHelper.ExecuteProcedureWithOneTable<DataTable>("P_GetLoginPageInfo");

                return dt.Rows[0].ToHashObject();
            }
        }
    }
}
