using CommonLibrary;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Newtonsoft.Json;
using Model;

namespace CMS.Controllers
{
    public class ProcessController : ApiController
    {
        /// <summary>
        /// 使用post方法是不获取地址栏参数
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        [HttpGet]
        [HttpPost]
        public async Task<object> Post(string func)
        {
            HashObject data = await GetParamKeyValue(this.ActionContext);
            TPhonePubParam FPhonePubParam = new TPhonePubParam();
            FPhonePubParam.Json = data.GetValue<string>("json", "");
            FPhonePubParam.Data = data;

            try
            {
                HttpFileCollection files = HttpContext.Current.Request.Files;
                if (files != null && files.Count > 0)
                {
                    HttpPostedFile postedFile = files[0];
                    FPhonePubParam.PostStream = postedFile.InputStream;
                }
                TPhoneRetParam ret = Common.RegisterPhoneFunc.Process(func, FPhonePubParam);
                LogHelper.WriteLog("调用方法：" + func + "\n" + "请求参数：" + FPhonePubParam.Data.ToQueryNameValueString() + "\n" + "返回json：" + ret.RetJson + "\n" + "返回data：" + JsonConvert.SerializeObject(ret.data));
                return ret;
            }
            catch (Exception e)
            {
                LogHelper.WriteLog("调用方法：" + func + "\n" + "请求参数：" + FPhonePubParam.Data.ToQueryNameValueString() + "\n" + "异常信息：", e);

                return new
                {
                    code = -500,
                    msg = e.Message,
                    //msg = "服务器返回错误，请联系客服人员。",
                    json = JsonConvert.DeserializeObject("{}"),
                    hasdog = 0,
                    dogtype = -1,
                    checkcount = 0
                };
            }
        }
        public async static Task<HashObject> GetParamKeyValue(HttpActionContext actionContext)
        {
            HashObject dic = new HashObject();
            if (actionContext.Request.Method == HttpMethod.Post)
            {
                if (actionContext.Request.Content.IsMimeMultipartContent())
                {
                    try
                    {
                        MultipartFormDataMemoryStreamProvider provider = new MultipartFormDataMemoryStreamProvider();
                        provider = await actionContext.Request.Content.ReadAsMultipartAsync(provider);

                        foreach (var key in provider.FormData.AllKeys)
                        {
                            if (string.IsNullOrEmpty(provider.FormData[key]))
                                continue;
                            dic.Add(key, provider.FormData[key]);
                        }
                        foreach(var file in provider.Files)
                        {
                            dic.Add(file.Headers.ContentDisposition.FileName, file);
                        }
                    }
                    catch (Exception e)
                    {
                        LogHelper.WriteLog(string.Format("解析MultipartContent异常：{0}", e.Message),e);
                        return dic;
                    }
                }
                else
                {
                    Stream stream = actionContext.Request.Content.ReadAsStreamAsync().Result;
                    if (stream == null)
                        return dic;

                    string responseData = "";
                    stream.Seek(0, SeekOrigin.Begin);
                    byte[] inputByts = new byte[stream.Length];
                    stream.Read(inputByts, 0, inputByts.Length);
                    stream.Seek(0, SeekOrigin.Begin);
                    responseData = Encoding.UTF8.GetString(inputByts);
                    try
                    {
                        if (responseData.Contains("&"))
                        {
                            Dictionary<string, string> tempdic = responseData.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries)
                                                                                            .Select(s => s.Split('='))
                                                                                            .ToDictionary(a => a[0].Trim(), a => HttpUtility.UrlDecode(a[1]).Trim());
                            foreach (KeyValuePair<string, string> item in tempdic)
                            {
                                if (string.IsNullOrEmpty(item.Value))
                                    continue;
                                dic.Add(item.Key, item.Value);
                            }
                        }
                        else if (responseData.Contains("="))
                        {
                            string[] paramItems = responseData.Split('=');
                            dic.Add(paramItems[0], HttpUtility.UrlDecode(paramItems[1]));
                        }
                        else
                            dic = (HashObject)JsonConvert.DeserializeObject(HttpUtility.UrlDecode(responseData));
                    }
                    catch (Exception e)
                    {
                        LogHelper.WriteLog(string.Format("解析Content异常：{0}", e.Message),e);
                        return dic;
                    }
                }
            }
            else if (actionContext.Request.Method == HttpMethod.Get)
            {
                foreach (KeyValuePair<string, string> item in actionContext.Request.GetQueryNameValuePairs())
                {
                    if (string.IsNullOrEmpty(item.Value))
                        continue;
                    dic.Add(item.Key, HttpUtility.UrlDecode(item.Value).Trim());
                }
            }
            return dic;
        }
    }
}
