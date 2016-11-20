using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vextractor
{
    class HtmlElementprocess
    {
        public static String getElementString(HtmlElement e)
        {
            string temp = e.InnerText;
            if (e.InnerText != null)
            {  //&& !temp.Contains("window._") && !temp.Contains("function()")

                if (e.TagName != "SCRIPT" && e.TagName != "STYLE" && e.TagName != "NOSCRIPT")
                {
                    // 处理掉子节点中的script和style代码,将待删除字符串从大到小的顺序排列，再删除
                    ArrayList script = new ArrayList();
                    foreach (HtmlElement ec in e.GetElementsByTagName("script"))
                    {
                        if (ec.InnerText != null)
                        {

                            String delt = ec.InnerText.Trim(); //必须除掉空前后格，否则可能出现不匹配                                      
                            if (script.Count > 0 && script[0].ToString().Length < delt.Length)
                            {
                                script.Insert(0, delt);
                            }
                            else { script.Add(delt); }
                        }
                    }
                    foreach (HtmlElement ec in e.GetElementsByTagName("style"))
                    {
                        if (ec.InnerText != null)
                        {
                            String delt = ec.InnerText.Trim();
                            if (script.Count > 0 && script[0].ToString().Length < delt.Length)
                            {
                                script.Insert(0, delt);
                            }
                            else { script.Add(delt); }
                        }
                    }
                    foreach (HtmlElement ec in e.GetElementsByTagName("noscript"))
                    {
                        if (ec.InnerText != null)
                        {
                            String delt = ec.InnerText.Trim();
                            if (script.Count > 0 && script[0].ToString().Length < delt.Length)
                            {
                                script.Insert(0, delt);
                            }
                            else { script.Add(delt); }
                        }
                    }
                    foreach (String del in script)
                    {
                        temp = temp.Replace(del, "");
                        // Console.Out.WriteLine("找到script" + del);
                    }
                    return temp.Trim();
                }
            }
            return null;
        }
    }
}
