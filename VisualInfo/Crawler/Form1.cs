using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crawler
{
    public partial class Crawler : Form
    {
        int count = 0;
        int next = 1;
        
        public Crawler()
        {
            InitializeComponent();
            this.wb1.ScriptErrorsSuppressed = true;
        }

        private void crawbutton_Click(object sender, EventArgs e)
        {
            //根据爬取需求修改入口页面
            this.wb1.Navigate("http://www.walmart.com/search/?query=phone");
            count = 0;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            count++;
            
            if (this.wb1.ReadyState == WebBrowserReadyState.Complete || (count > 30&& count < 35 && this.wb1.ReadyState == WebBrowserReadyState.Interactive))
            {
                timer1.Stop();
                foreach (HtmlElement el in this.wb1.Document.GetElementsByTagName("div"))
                {
                    //分析是否是特定的详细页面
                    if (el.GetAttribute("className").Equals("js-tile js-tile-landscape tile-landscape"))
                    {
                        foreach (HtmlElement ela in el.GetElementsByTagName("a"))
                        {
                            if (ela.GetAttribute("className").Equals("js-product-image"))
                            {
                                String detailurl = ela.GetAttribute("href");
                                if (detailurl.ToLower().Contains("/ip/") && detailurl.ToLower().Contains("phone"))
                                {
                                    Console.Out.WriteLine("详细页面是：" + detailurl);
                                }
                                break;
                            }
                           
                        }                                                
                     }
                }
                    next++;
                    Console.Out.WriteLine("下一页是：" + "http://www.walmart.com/search/?query=phone&page=" + next + "&cat_id=0");
                    count = 0;
                this.wb1.Navigate("http://www.walmart.com/search/?query=phone&page=" + next + "&cat_id=0");
                    timer1.Start();
                    return;
               }//页面加载完成  
            if ( count >=35)
            {
                timer1.Stop();
                next++;
                Console.Out.WriteLine("翻页时count"+count);
                Console.Out.WriteLine("下一页是：" + "http://www.walmart.com/search/?query=phone&page=" + next + "&cat_id=0");
                count = 0;
                this.wb1.Navigate("http://www.walmart.com/search/?query=phone&page=" + next + "&cat_id=0");               
                timer1.Start();
                return;
            }
        }

    }
}
