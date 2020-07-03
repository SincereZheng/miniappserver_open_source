using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CMS.Common
{
    public class RegisterPhoneFunc
    {
        private static Dictionary<string, IPhoneFunc> funcTypes = new Dictionary<string, IPhoneFunc>();

        public static void RegisterAllFunc()
        {
            #region 地址管理
            Register("GetAddressList", new GetAddressList());
            Register("AddAddress", new AddAddress());
            Register("ModifyAddress", new ModifyAddress());
            Register("GetDefaultAddress", new GetDefaultAddress());
            Register("SetDefaultAddress", new SetDefaultAddress());
            Register("GetAddressDetail", new GetAddressDetail());
            Register("DeleteAddress", new DeleteAddress()); 
            #endregion

            #region 首页广告轮播图
            Register("AddAdvertisment", new AddAdvertisment());
            Register("DeleteAdvertisment", new DeleteAdvertisment());
            Register("ModifyAdvertisment", new ModifyAdvertisment());
            Register("GetAdvertisment", new GetAdvertisment());
            #endregion

            #region 首页公告
            Register("AddMessage", new AddMessage());
            Register("DeleteMessage", new DeleteMessage());
            Register("ModifyMessage", new ModifyMessage());
            Register("GetMessage", new GetMessage());
            #endregion

            #region 小程序信息

            Register("GetWxApp", new GetWxApp());
            
            Register("GetIndexPageData", new GetIndexPageData());
            Register("UserLogin", new UserLogin());
            Register("GetGoodsDetail", new GetGoodsDetail());
            Register("GetGoodsComment", new GetGoodsComment());
            Register("WxappHelp", new WxappHelp());
            Register("WxUserIsAdmin", new WxUserIsAdmin());
            


            #endregion

            #region 购物车

            Register("GetCartLists", new GetCartLists());
            Register("CartAdd", new CartAdd());
            Register("CartDecAndAdd", new CartDecAndAdd());
            Register("SetCartListChecked", new SetCartListChecked());

            #endregion

            #region 商品分类

            Register("GetCategoryLists", new GetCategoryLists());

            #endregion

            #region 小程序订单
            Register("GetUserOrderCount", new GetUserOrderCount());
            Register("GetUserOrderList", new GetUserOrderList());
            Register("OrderCart", new OrderCart()); 
            Register("OrderBuyNow", new OrderBuyNow());
            Register("SubmitOrderCart", new SubmitOrderCart());
            Register("SubmitOrderBuyNow", new SubmitOrderBuyNow());
            Register("GetUserOrderDetail", new GetUserOrderDetail());
            Register("GetGoodsList", new GetGoodsList());
            Register("PayOrder", new PayOrder());
            Register("CancelOrder", new CancelOrder());
            Register("UserOrderReceipt", new UserOrderReceipt());
            Register("OrderTradeInfo", new OrderTradeInfo());
            Register("BeforeOrderCartCheck", new BeforeOrderCartCheck());
            Register("AddComment", new AddComment());
            Register("GetUserOrderGoodsComment", new GetUserOrderGoodsComment());

            Register("UploadFile", new UploadFile());
            #endregion

            #region 优惠券
            Register("CouponLimitList", new CouponLimitList());
            Register("GetUserCouponList", new GetUserCouponList());
            Register("ShareCoupon", new ShareCoupon());
            Register("GetCoupon", new GetCoupon());
            #endregion
        }

        public class AddAdvertisment : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return MainPageDal.AddAdvertisment(APhonePubParam);
            }
        }
        
        public class WxappHelp : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return WxAppDal.WxappHelp(APhonePubParam);
            }
        }
        public class CouponLimitList : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return CouponDal.CouponLimitList(APhonePubParam);
            }
        }
        public class DeleteAdvertisment : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return MainPageDal.DeleteAdvertisment(APhonePubParam);
            }
        }
        public class ModifyAdvertisment : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return MainPageDal.ModifyAdvertisment(APhonePubParam);
            }
        }
        public class GetAdvertisment : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return MainPageDal.GetAdvertisment(APhonePubParam);
            }
        }
        public class AddMessage : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return MainPageDal.AddMessage(APhonePubParam);
            }
        }
        public class DeleteMessage : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return MainPageDal.DeleteMessage(APhonePubParam);
            }
        }
        public class ModifyMessage : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return MainPageDal.ModifyMessage(APhonePubParam);
            }
        }
        public class GetMessage : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return MainPageDal.GetMessage(APhonePubParam);
            }
        }

        public static TPhoneRetParam Process(string key, TPhonePubParam APhonePubParam)
        {
            IPhoneFunc func;

            if(funcTypes.TryGetValue(key,out func))
                return func.ProcessFunc(APhonePubParam);
            else
                throw new InvalidOperationException(string.Format("{0}未注册", key));
        }
        
        public class GetAddressList : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return AddresDal.GetAddressList(APhonePubParam);
            }
        }
        public class GetAddressDetail : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return AddresDal.GetAddressDetail(APhonePubParam);
            }
        }
        public class AddAddress : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return AddresDal.AddAddress(APhonePubParam);
            }
        }
        public class ModifyAddress : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return AddresDal.ModifyAddress(APhonePubParam);
            }
        }
        public class GetUserOrderList : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return WxOrderDal.GetUserOrderList(APhonePubParam);
            }
        }
        
        public class OrderCart : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return WxOrderDal.OrderCart(APhonePubParam);
            }
        }
        public class OrderBuyNow : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return WxOrderDal.OrderBuyNow(APhonePubParam);
            }
        }
        public class SubmitOrderCart : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return WxOrderDal.SubmitOrderCart(APhonePubParam);
            }
        }
        
        public class SubmitOrderBuyNow : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return WxOrderDal.SubmitOrderBuyNow(APhonePubParam);
            }
        }
        public class CancelOrder : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return WxOrderDal.CancelOrder(APhonePubParam);
            }
        }
        public class UserOrderReceipt : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return WxOrderDal.UserOrderReceipt(APhonePubParam);
            }
        }
        
        public class SetCartListChecked : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return WxAppDal.SetCartListChecked(APhonePubParam);
            }
        }
        
        public class BeforeOrderCartCheck : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return WxOrderDal.BeforeOrderCartCheck(APhonePubParam);
            }
        }
        

        public class AddComment : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return WxOrderDal.AddComment(APhonePubParam);
            }
        }
        public class UploadFile : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return WxOrderDal.UploadFile(APhonePubParam);
            }
        }
        public class GetUserOrderGoodsComment : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return WxOrderDal.GetUserOrderGoodsComment(APhonePubParam);
            }
        }
        public class OrderTradeInfo : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return WxOrderDal.OrderTradeInfo(APhonePubParam);
            }
        }
        
        public class GetUserOrderDetail : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return WxOrderDal.GetUserOrderDetail(APhonePubParam);
            }
        }
        public class GetGoodsList : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return WxOrderDal.GetGoodsList(APhonePubParam);
            }
        }
        public class PayOrder : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return WxOrderDal.PayOrder(APhonePubParam);
            }
        }
        
        public class DeleteAddress : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return AddresDal.DeleteAddress(APhonePubParam);
            }
        }
        public class GetDefaultAddress : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return AddresDal.GetDefaultAddress(APhonePubParam);
            }
        }
        public class SetDefaultAddress : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return AddresDal.SetDefaultAddress(APhonePubParam);
            }
        }

        public class GetWxApp : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return WxAppDal.GetWxApp(APhonePubParam);
            }
        }

        public class GetIndexPageData : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return WxAppDal.GetIndexPageData(APhonePubParam);
            }
        }

        public class GetCategoryLists : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return WxAppDal.GetCategoryLists(APhonePubParam);
            }
        }
        public class GetUserOrderCount : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return WxAppDal.GetUserOrderCount(APhonePubParam);
            }
        }
        
        public class UserLogin : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return WxAppDal.UserLogin(APhonePubParam);
            }
        }
        public class WxUserIsAdmin : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return WxAppDal.WxUserIsAdmin(APhonePubParam);
            }
        }
        
        public class GetUserCouponList : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return CouponDal.GetUserCouponList(APhonePubParam);
            }
        }
        
        public class ShareCoupon : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return CouponDal.ShareCoupon(APhonePubParam);
            }
        }
        public class GetCoupon : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return CouponDal.GetCoupon(APhonePubParam);
            }
        }

        public class GetCartLists : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return WxAppDal.GetCartLists(APhonePubParam);
            }
        }

        public class CartAdd : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return WxAppDal.CartAdd(APhonePubParam);
            }
        }

        
        public class GetGoodsComment : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return WxAppDal.GetGoodsComment(APhonePubParam);
            }
        }
        public class GetGoodsDetail : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return WxAppDal.GetGoodsDetail(APhonePubParam);
            }
        }
        public class CartDecAndAdd : IPhoneFunc
        {
            public TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam)
            {
                return WxAppDal.CartDecAndAdd(APhonePubParam);
            }
        }
        public static void Register(string AKey, IPhoneFunc func)
        {
            if (funcTypes.ContainsKey(AKey))
                throw new InvalidOperationException(string.Format("{0}已注册，不能重复注册", AKey));
            else
                funcTypes[AKey] = func;
        }
    }
    
}
