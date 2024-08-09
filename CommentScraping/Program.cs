using CommentScraping;
using CommentScraping.Model;
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
           
            var productLink = driver.FindElements(By.CssSelector("div.p-card-wrppr a")).Take(5).ToList();
            List<Product> urunler = new List<Product>();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var count = 0;
            foreach ( var product in productLink)
            {

                string originalWindow = driver.CurrentWindowHandle;
                try
                {
                    product.Click();
                }
                catch (Exception ex)
                {
                    Actions actions = new Actions(driver);
                    actions.MoveByOffset(10, 100).Click().Perform();
                    product.Click();
                }
                var windowHandles = driver.WindowHandles;
                driver.SwitchTo().Window(windowHandles[1]);
                try
                {
                    Thread.Sleep(1000);
                    IWebElement rating =driver.FindElement(By.ClassName("rvw-cnt-tx"));
                    Thread.Sleep(1000);
                    rating.Click();

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
                    count++;
                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine("Değerlendirme bağlantısı bulunamadı.");
                }

                driver.Close();
                driver.SwitchTo().Window(originalWindow);
            }
            var analyse = new EmotionalAnalyse();
            var comments = urunler.SelectMany(x => x.Comment).ToList();
            var analyseResult = analyse.Analyze(comments);
            
            foreach(var res in analyseResult)
            {
                Console.WriteLine($"yorum: {res.CommentText} --- {(res.Prediction ? "olumlu" : "olumsuz")}");
            }


        }
        Console.ReadLine();
    }
}

