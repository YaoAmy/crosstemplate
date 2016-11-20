using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using System.Windows.Forms;

namespace Vextractor
{
    class Template
    {
         HtmlAttributeCollection attrs;
          String tag;
        String attrString;
        HtmlNode Node;
        public HtmlNode createNode(String s)   //根据纯文本得到封装的节点
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(s);
             Node = doc.DocumentNode.FirstChild;
            return Node;
        }
        public void createTem(String s)  //根据纯文本得到节点属性
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(s);
            HtmlNode node = doc.DocumentNode.FirstChild;           
            tag = node.Name;
             attrs = node.Attributes;
            foreach (var item in attrs)
            {              
                attrString = " and " + "@"+item.Name + "=" +"'"+ item.Value+"'";    //提取节点的属性集合

            }
            attrString = attrString.Substring(5);
            attrString = "//"+tag + "[" + attrString + "]";        
        }
        public String gettag()
        {
            return tag;
        }
        public HtmlAttributeCollection getattr()
        {
            return attrs;
        }

        public static string GetHtmlStr(string url)
        {
            try
            {
                WebRequest rGet = WebRequest.Create(url);
                WebResponse rSet = rGet.GetResponse();
                Stream s = rSet.GetResponseStream();
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                return reader.ReadToEnd();
            }
            catch (WebException)
            {
                //连接失败
                return null;
            }
        }
       
        public void Extract(String url)
        {
            //   HtmlWeb web = new HtmlWeb();
            String pageHtml="";
            try
            {

                WebClient MyWebClient = new WebClient();


                MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据

                Byte[] pageData = MyWebClient.DownloadData(url); //从指定网站下载数据

               // string pageHtml = Encoding.Default.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句            

                pageHtml = Encoding.UTF8.GetString(pageData); //如果获取网站页面采用的是UTF-8，则使用这句

       //         using (StreamWriter sw = new StreamWriter("d:\\testtest.txt"))//将获取的内容写入文本

        //        {

         //           sw.Write(pageHtml);

        //        }                         

            }
            catch (WebException webEx)
            {

                Console.WriteLine(webEx.Message.ToString());

            }
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(pageHtml);
            HtmlNodeCollection rootnodes = doc.DocumentNode.ChildNodes;
            Console.Out.WriteLine(attrString);
            //  Console.Out.WriteLine(rootnode.InnerHtml);


            string strPath = @"D:\testtest.txt";
            string value = doc.DocumentNode.OuterHtml;

            if (!Directory.Exists(Path.GetDirectoryName(strPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(strPath));
            }

            File.AppendAllText(strPath, value, Encoding.Default);

            Console.Out.WriteLine(rootnodes.Count);
            foreach (HtmlNode rootnode in rootnodes)
            {
                HtmlNodeCollection tempnode = rootnode.SelectNodes("//span[@class='js-product-title truncated']");

                if (tempnode != null)
                {
                    foreach (HtmlNode node in tempnode)
                    {
                        Console.Out.WriteLine(node.InnerText);
                    }
                }
                
            }
            Console.Out.WriteLine("finished!");
            //  foreach (HtmlNode node in rootnode.SelectNodes("//span[contains(@class,'js-product-title truncated')]"))
            //  {
            //      Console.Out.WriteLine(node.InnerText);
            //  }


        }
    }
}
