using CommonLibrary;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface IPhoneFunc
    {
        TPhoneRetParam ProcessFunc(TPhonePubParam APhonePubParam);
    }
    public class TPhoneRetParam
    {
        public int RetCode = 0;
        public string RetMsg = "";
        public string RetJson = "";
        public object data = new object();
    }
    public class TPhonePubParam
    {
        public string Host = "";            //用于图片返回url地址
        public string PhoneIP = "";              //手机ip地址，服务器列表显示
        public string Method = "";
        public string Companyinfo = "";
        public string OperatorId = "";
        public string Version = "";
        public string TimeStamp = "";
        public string DeviceId = "";
        public string FromDevice = "";
        public string DataBaseName = "";
        public string Clothingcloud = "";
        public string ColudMobile = "";
        public string ColudServerId = "";
        public string Json = "";
        //public string URI = "";          //BS 不需要这个参数了，屏蔽掉，直接用Host里面的公用链接，处理一下之后返回图片地址
        public string PhoneAppName = "";
        public string PhoneMachineName = "";
        public bool IsQuery = false;
        public string LocalSerialNo = "";
        public Stream PostStream = null;  //图片压缩文件流
        public HashObject Data = null;
    }
}
