//using System;.
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
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

class Program
{
    static void Main(string[] args)
    {
        // ChromeDriver için tarayıcı seçeneklerini ayarlayın
        var options = new ChromeOptions();
        //options.AddArgument("--headless");


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
            var productLink =driver.FindElements(By.CssSelector("div.p-card-wrppr a"));
            List<Product> urunler = new List<Product>();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            foreach ( var product in productLink)
            {

                string originalWindow = driver.CurrentWindowHandle;
                Thread.Sleep(1000);
                try
                {
                    product.Click();
                }
                catch (Exception ex)
                {
                    product.SendKeys(Keys.Escape); // Escape tuşuna bas
                }
                // Yeni pencereye geçiş yapın
                var windowHandles = driver.WindowHandles;
                driver.SwitchTo().Window(windowHandles[1]);
                try
                {
                    // Değerlendirme bağlantısını bulun ve tıklayın
                    Thread.Sleep(1000);
                    IWebElement rating =driver.FindElement(By.ClassName("rvw-cnt-tx"));
                    Thread.Sleep(1000);
                    rating.Click();

                    // Yorumları çekin
                    var elements = driver.FindElements(By.ClassName("comment-text"));

                    Product uruns = new Product
                    {
                        ProductName = seacrWord,
                        Comment = new List<Comment>()
                    };
                    foreach (var element in elements)
                    {
                        uruns.Comment.Add(new Comment { CommentText = element.Text });
                    }
                    urunler.Add(uruns);
                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine("Değerlendirme bağlantısı bulunamadı.");
                }

                // Yeni pencereyi kapatın ve eski pencereye geri dönün
                driver.Close();
                driver.SwitchTo().Window(originalWindow);
            }
          
        
        }
        Console.ReadLine();
    }
}

public class Product
{
    public string ProductName { get; set; }
    public List<Comment> Comment { get; set; }
}
public class Comment
{
    public string CommentText { get; set; }
}
