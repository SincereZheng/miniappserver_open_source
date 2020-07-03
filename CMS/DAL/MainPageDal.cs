using CommonLibrary;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MainPageDal
    {
        #region 首页公告
        public static TPhoneRetParam AddMessage(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();
            using (DbHelper dbHelper = new DbHelper())
            {
                dbHelper.AddParameter("@title", APhonePubParam.Data.GetValue<string>("Title", ""));
                dbHelper.AddParameter("@picpath", APhonePubParam.Data.GetValue<string>("PicPath", ""));
                dbHelper.AddParameter("@author", APhonePubParam.Data.GetValue<string>("Author", ""));
                dbHelper.AddParameter("@description", APhonePubParam.Data.GetValue<string>("Description", ""));
                dbHelper.AddParameter("@order", APhonePubParam.Data.GetValue<int>("Order", 0));
                dbHelper.ExecuteProcedure<DataTable>("P_AddMessage");
                
                return result;
            }
        }
        public static TPhoneRetParam DeleteMessage(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();
            using (DbHelper dbHelper = new DbHelper())
            {
                dbHelper.AddParameter("@id", APhonePubParam.Data.GetValue<int>("Id", 0));
                DataTable errmsg = dbHelper.ExecuteProcedureWithOneTable<DataTable>("P_DeleteMessage");
                if (errmsg.Rows.Count > 0)
                    result.RetCode = -1;
                result.RetMsg = errmsg.ToJson();

                return result;
            }
        }
        public static TPhoneRetParam ModifyMessage(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();
            using (DbHelper dbHelper = new DbHelper())
            {
                dbHelper.AddParameter("@title", APhonePubParam.Data.GetValue<string>("Title", ""));
                dbHelper.AddParameter("@picpath", APhonePubParam.Data.GetValue<string>("PicPath", ""));
                dbHelper.AddParameter("@author", APhonePubParam.Data.GetValue<string>("Author", ""));
                dbHelper.AddParameter("@description", APhonePubParam.Data.GetValue<string>("Description", ""));
                dbHelper.AddParameter("@order", APhonePubParam.Data.GetValue<int>("Order", 0));
                dbHelper.AddParameter("@id", APhonePubParam.Data.GetValue<int>("Id", 0));
                DataTable errmsg = dbHelper.ExecuteProcedureWithOneTable<DataTable>("P_ModifyMessage");
                if (errmsg.Rows.Count > 0)
                    result.RetCode = -1;
                result.RetMsg = errmsg.ToJson();

                return result;
            }
        }
        public static TPhoneRetParam GetMessage(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();
            using (DbHelper dbHelper = new DbHelper())
            {
                dbHelper.AddParameter("@Count", APhonePubParam.Data.GetValue<int>("Count", 0));
                DataTable ret = dbHelper.ExecuteProcedureWithOneTable<DataTable>("P_GetMessage");
                result.RetJson = ret.ToJson();
                return result;
            }
        }
        #endregion

        #region 首页广告轮播图
        public static TPhoneRetParam AddAdvertisment(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();
            using (DbHelper dbHelper = new DbHelper())
            {
                dbHelper.AddParameter("@name", APhonePubParam.Data.GetValue<string>("Name", ""));
                dbHelper.AddParameter("@url", APhonePubParam.Data.GetValue<string>("Url", ""));
                dbHelper.AddParameter("@picpath", APhonePubParam.Data.GetValue<string>("PicPath", ""));
                dbHelper.AddParameter("@author", APhonePubParam.Data.GetValue<string>("Author", ""));
                dbHelper.AddParameter("@order", APhonePubParam.Data.GetValue<int>("Order", 0));
                DataTable errmsg = dbHelper.ExecuteProcedureWithOneTable<DataTable>("P_AddAdvertisment");
                if (errmsg.Rows.Count > 0)
                    result.RetMsg = errmsg.ToJson();

                return result;
            }
        }
        public static TPhoneRetParam DeleteAdvertisment(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();
            using (DbHelper dbHelper = new DbHelper())
            {
                dbHelper.AddParameter("@id", APhonePubParam.Data.GetValue<int>("Id", 0));
                DataTable errmsg = dbHelper.ExecuteProcedureWithOneTable<DataTable>("P_DeleteAdvertisment");
                if (errmsg.Rows.Count > 0)
                    result.RetCode = -1;
                result.RetMsg = errmsg.ToJson();

                return result;
            }
        }
        public static TPhoneRetParam ModifyAdvertisment(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();
            using (DbHelper dbHelper = new DbHelper())
            {
                dbHelper.AddParameter("@name", APhonePubParam.Data.GetValue<string>("Name", ""));
                dbHelper.AddParameter("@url", APhonePubParam.Data.GetValue<string>("Url", ""));
                dbHelper.AddParameter("@picpath", APhonePubParam.Data.GetValue<string>("PicPath", ""));
                dbHelper.AddParameter("@author", APhonePubParam.Data.GetValue<string>("Author", ""));
                dbHelper.AddParameter("@order", APhonePubParam.Data.GetValue<int>("Order", 0));
                dbHelper.AddParameter("@id", APhonePubParam.Data.GetValue<int>("Id", 0));
                DataTable errmsg = dbHelper.ExecuteProcedureWithOneTable<DataTable>("P_ModifyAdvertisment");
                if (errmsg.Rows.Count > 0)
                    result.RetCode = -1;
                result.RetMsg = errmsg.ToJson();

                return result;
            }
        }
        public static TPhoneRetParam GetAdvertisment(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();
            using (DbHelper dbHelper = new DbHelper())
            {
                DataTable ret = dbHelper.ExecuteProcedureWithOneTable<DataTable>("P_GetAdvertisment");
                result.RetJson = ret.ToJson();
                return result;
            }
        }
        #endregion
    }
}
