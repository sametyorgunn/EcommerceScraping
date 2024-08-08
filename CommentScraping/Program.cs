//using System;
//using System.Net.Http;
//using System.Threading.Tasks;
//using HtmlAgilityPack;

//class Program
//{
//    static async Task Main(string[] args)
//    {
//        //var url = "https://asrekagit.com/";
//        //var httpClient = new HttpClient();
//        //var response = await httpClient.GetStringAsync(url);

//        //var htmlDoc = new HtmlDocument();
//        //htmlDoc.LoadHtml(response);

//        //// Yorumları çekmek için doğru XPath ifadesini bulun
//        //var comments = htmlDoc.DocumentNode.SelectNodes("//span[@class='thumb-info-inner line-height-1 font-weight-bold text-dark position-relative top-3']");

//        //if (comments != null)
//        //{

//        //        Console.WriteLine(comments);

//        //}


//        var url = "https://www.trendyol.com/";
//        var httpClient = new HttpClient();
//        var response = await httpClient.GetStringAsync(url);

//        var htmlDoc = new HtmlDocument();
//        htmlDoc.LoadHtml(response);

//        // Yorumları çekmek için doğru XPath ifadesini bulun
//        var comments = htmlDoc.DocumentNode.SelectNodes("//div[@class='widget-product']");

//        if (comments != null)
//        {

//            Console.WriteLine(comments);

//        }
//    }
//}

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

class Program
{
    static void Main(string[] args)
    {
        // ChromeDriver için tarayıcı seçeneklerini ayarlayın
        var options = new ChromeOptions();
        /*options.AddArgument("--headless");*/  // Tarayıcıyı arka planda çalıştırmak için (görünmez)


        Console.WriteLine("Ürün ismi giriniz....");
        string seacrWord = Console.ReadLine();
        using (IWebDriver driver = new ChromeDriver(options))
        {
            driver.Navigate().GoToUrl("https://www.trendyol.com/");
            Thread.Sleep(1000);
            var searchInput = driver.FindElement(By.ClassName("V8wbcUhU"));
            searchInput.SendKeys(seacrWord);
            Thread.Sleep(1000);
            searchInput.SendKeys(Keys.Enter);
            //var products = driver.FindElements(By.ClassName("p-card-wrppr"));
            var productLink =driver.FindElements(By.CssSelector("div.p-card-wrppr a"));

            foreach ( var product in productLink)
            {
                //product.SendKeys(Keys.Enter);
                //Thread.Sleep(3000);
                //IWebElement rating = driver.FindElement(By.ClassName("rvw-cnt-tx"));
                //rating.Click();

                string originalWindow = driver.CurrentWindowHandle;
                product.Click();
               
                var windowHandles = driver.WindowHandles;
                driver.SwitchTo().Window(windowHandles[1]);
                IWebElement rating = driver.FindElement(By.ClassName("rvw-cnt-tx"));
                rating.Click();
                var element = driver.FindElements(By.ClassName("comment-text"));
                foreach (var elemt in element)
                {
                    Console.WriteLine(elemt.Text);
                }
            }
          
        
        }
    }
}
