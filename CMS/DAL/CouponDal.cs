using System;
using CommonLibrary;
using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CouponDal
    {
        public static TPhoneRetParam CouponLimitList(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();

            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@cid", APhonePubParam.Data.GetValue<string>("cid", ""));
                DataTable dt = db.ExecuteProcedureWithOneTable<DataTable>("P_GetCouponLimitWx");
                result.RetCode = 0;
                result.RetJson = dt.Rows[0]["Result"].ToString();
                result.data = JsonConvert.DeserializeObject(result.RetJson);
                return result;
            }
        }
        public static TPhoneRetParam GetUserCouponList(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();

            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@token", APhonePubParam.Data.GetValue<string>("token", ""));
                db.AddParameter("@type", APhonePubParam.Data.GetValue<string>("type", ""));
                DataTable dt = db.ExecuteProcedureWithOneTable<DataTable>("P_GetUserCouponList");
                result.RetCode = 0;
                result.RetJson = dt.Rows[0]["Result"].ToString();
                result.data = JsonConvert.DeserializeObject(result.RetJson);
                return result;
            }
        }
        public static TPhoneRetParam ShareCoupon(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();

            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@token", APhonePubParam.Data.GetValue<string>("token", ""));
                db.AddParameter("@random", APhonePubParam.Data.GetValue<string>("random", ""));
                db.AddParameter("@couponid", APhonePubParam.Data.GetValue<string>("couponid", ""));
                DataTable dt = db.ExecuteProcedureWithOneTable<DataTable>("P_ShareCoupon");
                result.RetCode = 0;
                result.RetJson = dt.Rows[0]["Result"].ToString();
                result.data = JsonConvert.DeserializeObject(result.RetJson);
                return result;
            }
        }
        public static TPhoneRetParam GetCoupon(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();

            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@token", APhonePubParam.Data.GetValue<string>("token", ""));
                db.AddParameter("@randomid", APhonePubParam.Data.GetValue<string>("random", ""));
                db.AddParameter("@couponid", APhonePubParam.Data.GetValue<string>("couponid", ""));
                db.AddParameter("@msg", "",ParameterDirection.Output);
                db.ExecuteProcedure<DataTable>("P_GetCoupon");
                result.RetCode = 0;
                result.RetJson = db.GetParameterValue("@msg").ToString();
                result.data = result.RetJson;
                return result;
            }
        }
    }
    

}
