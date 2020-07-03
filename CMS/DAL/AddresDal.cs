using CommonLibrary;
using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class AddresDal
    {
        public static TPhoneRetParam SetDefaultAddress(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();
            using(DbHelper dbHelper = new DbHelper())
            {
                dbHelper.AddParameter("@token", APhonePubParam.Data.GetValue<string>("token", ""));
                dbHelper.AddParameter("@Person", APhonePubParam.Data.GetValue<string>("name", ""));
                dbHelper.AddParameter("@Tel", APhonePubParam.Data.GetValue<string>("phone", ""));
                dbHelper.AddParameter("@Mobile", APhonePubParam.Data.GetValue<string>("Mobile", ""));
                dbHelper.AddParameter("@Province", APhonePubParam.Data.GetValue<string>("region", ""));
                dbHelper.AddParameter("@City", APhonePubParam.Data.GetValue<string>("region", ""));
                dbHelper.AddParameter("@County", APhonePubParam.Data.GetValue<string>("region", ""));
                dbHelper.AddParameter("@Address", APhonePubParam.Data.GetValue<string>("detail", ""));
                dbHelper.AddParameter("@Default", APhonePubParam.Data.GetValue<string>("Default", ""));
                dbHelper.AddParameter("@mode", "Default");
                dbHelper.AddParameter("@Id", APhonePubParam.Data.GetValue<int>("address_id", 0));
                dbHelper.ExecuteProcedure<DataTable>("P_OperateAddress");
                result.RetCode = 0;
                result.RetMsg = "";
                return result;
            }
        }
        public static TPhoneRetParam DeleteAddress(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();
            using (DbHelper dbHelper = new DbHelper())
            {

                dbHelper.AddParameter("@token", APhonePubParam.Data.GetValue<string>("token", ""));
                dbHelper.AddParameter("@Person", "");
                dbHelper.AddParameter("@Tel", "");
                dbHelper.AddParameter("@Mobile", "");
                dbHelper.AddParameter("@Province", "");
                dbHelper.AddParameter("@City", "");
                dbHelper.AddParameter("@County", "");
                dbHelper.AddParameter("@Address", "");
                dbHelper.AddParameter("@Default", "");
                dbHelper.AddParameter("@mode", "D");
                dbHelper.AddParameter("@Id", APhonePubParam.Data.GetValue<string>("address_id", ""));
                DataTable dt = dbHelper.ExecuteProcedureWithOneTable<DataTable>("P_OperateAddress");
                result.RetCode = 0;
                result.RetMsg = dt.Rows[0]["Result"].ToString();
                return result;
            }
        }
        public static TPhoneRetParam ModifyAddress(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();
            using (DbHelper dbHelper = new DbHelper())
            {
                string region = APhonePubParam.Data.GetValue<string>("region", "");
                string[] str = region.Split(',');
                dbHelper.AddParameter("@token", APhonePubParam.Data.GetValue<string>("token", ""));
                dbHelper.AddParameter("@Person", APhonePubParam.Data.GetValue<string>("name", ""));
                dbHelper.AddParameter("@Tel", APhonePubParam.Data.GetValue<string>("phone", ""));
                dbHelper.AddParameter("@Mobile", APhonePubParam.Data.GetValue<string>("Mobile", ""));
                dbHelper.AddParameter("@Province", str[0]);
                dbHelper.AddParameter("@City", str[1]);
                dbHelper.AddParameter("@County", str[2]);
                dbHelper.AddParameter("@Address", APhonePubParam.Data.GetValue<string>("detail", ""));
                dbHelper.AddParameter("@Default", APhonePubParam.Data.GetValue<string>("Default", ""));
                dbHelper.AddParameter("@mode", "M");
                dbHelper.AddParameter("@Id", APhonePubParam.Data.GetValue<string>("address_id", ""));
                DataTable dt = dbHelper.ExecuteProcedureWithOneTable<DataTable>("P_OperateAddress");
                result.RetCode = 0;
                result.RetMsg = dt.Rows[0]["Result"].ToString();
                return result;
            }
        }
        public static TPhoneRetParam AddAddress(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();
            using (DbHelper dbHelper = new DbHelper())
            {
                string region = APhonePubParam.Data.GetValue<string>("region", "");
                string[] str = region.Split(',');
                dbHelper.AddParameter("@token", APhonePubParam.Data.GetValue<string>("token", ""));
                dbHelper.AddParameter("@Person", APhonePubParam.Data.GetValue<string>("name", ""));
                dbHelper.AddParameter("@Tel", APhonePubParam.Data.GetValue<string>("phone", ""));
                dbHelper.AddParameter("@Mobile", APhonePubParam.Data.GetValue<string>("Mobile", ""));
                dbHelper.AddParameter("@Province", str[0]);
                dbHelper.AddParameter("@City", str[1]);
                dbHelper.AddParameter("@County", str[2]);
                dbHelper.AddParameter("@Address", APhonePubParam.Data.GetValue<string>("detail", ""));
                dbHelper.AddParameter("@Default", APhonePubParam.Data.GetValue<string>("Default", ""));
                dbHelper.AddParameter("@mode", "A");
                dbHelper.AddParameter("@Id", 0);
                DataTable dt = dbHelper.ExecuteProcedureWithOneTable<DataTable>("P_OperateAddress");
                result.RetCode = 0;
                result.RetMsg = dt.Rows[0]["Result"].ToString();
                return result;
            }
        }
        public static TPhoneRetParam GetAddressList(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();
            using (DbHelper dbHelper = new DbHelper())
            {
                dbHelper.AddParameter("@token", APhonePubParam.Data.GetValue<string>("token", ""));
                DataTable table = dbHelper.ExecuteProcedureWithOneTable<DataTable>("P_GetAddressList");
                result.RetCode = 0;
                result.RetMsg = "";
                result.RetJson = table.Rows[0]["Result"].ToString();
                result.data = JsonConvert.DeserializeObject(result.RetJson);
                return result;
            }
        }
        public static TPhoneRetParam GetAddressDetail(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();
            using (DbHelper dbHelper = new DbHelper())
            {
                dbHelper.AddParameter("@id", APhonePubParam.Data.GetValue<string>("address_id", ""));
                DataTable table = dbHelper.ExecuteProcedureWithOneTable<DataTable>("P_GetAddressDetail");
                result.RetCode = 0;
                result.RetMsg = "";
                result.RetJson = table.Rows[0]["Result"].ToString();
                result.data = JsonConvert.DeserializeObject(result.RetJson);
                return result;
            }
        }
        public static TPhoneRetParam GetDefaultAddress(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();
            string sql = "select * from [T_ShoppingAddress] where cUsercode=@usercode and bdefault=1";
            using (DbHelper dbHelper = new DbHelper())
            {
                dbHelper.AddParameter("@usercode", APhonePubParam.Data.GetValue<string>("usercode", ""));
                DataTable table = dbHelper.ExecuteSqlWithOneTable<DataTable>(sql);
                result.RetCode = 0;
                result.RetMsg = "";
                result.RetJson = table.ToJsonLower();
                return result;
            }
        }

    }
}
