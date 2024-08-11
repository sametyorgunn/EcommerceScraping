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
        //var categories = new TrendyolRequest();
        //var categoryList = categories.GetCategories().Result;

        var categories = new GetCategory();
        categories.GetCategoryList();
        Console.WriteLine("Ürün ismi giriniz....");
        string seacrWord = Console.ReadLine();
        using (IWebDriver driver = new ChromeDriver(options))
        {
            driver.Navigate().GoToUrl("https://www.trendyol.com/");
            Thread.Sleep(1000);
            var searchInput = driver.FindElement(By.ClassName("V8wbcUhU"));
            searchInput.SendKeys(seacrWord);
            searchInput.SendKeys(Keys.Enter);
            Thread.Sleep(1000);
            var catName = "iPhone IOS Cep Telefonları";
            var AllCategoriesList = driver.FindElements(By.CssSelector("div.fltr-item-text")).Where(x => x.Text == catName);
            foreach (var cat in AllCategoriesList)
            {
                Console.WriteLine(cat.Text);
            }
            var productLink = driver.FindElements(By.CssSelector("div.p-card-wrppr ")).Take(1).ToList();
            List<Product> urunler = new List<Product>();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var count = 0;
            foreach ( var product in productLink)
            {
                var ProductId = product.GetAttribute("data-id");
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
                    var ProductBrand = driver.FindElement(By.CssSelector("a.product-brand-name-with-link")).Text;
                    var ProductName = driver.FindElement(By.CssSelector("h1.pr-new-br span")).Text;
                    var ProductRating = driver.FindElement(By.CssSelector("div.rating-line-count")).Text;
                    var ProductPrice = driver.FindElement(By.CssSelector("span.prc-dsc")).Text;
                    var ProductImage = driver.FindElement(By.CssSelector("div.base-product-image img")).GetAttribute("src");
                    var ProductProperties = driver.FindElements(By.ClassName("attribute-item"));
                    Dictionary<string,string> properties = new Dictionary<string,string>();
                    foreach (var item in ProductProperties)
                    {
                        var attributeNameElement = item.FindElement(By.CssSelector(".attribute-label.attr-name"));
                        string attributeName = attributeNameElement.Text;
                        var attributeValueElement = item.FindElement(By.CssSelector(".attribute-value .attr-name.attr-name-w"));
                        string attributeValue = attributeValueElement.Text;
                        properties.Add(attributeName, attributeValue);
                    }
                    IWebElement rating =driver.FindElement(By.ClassName("rvw-cnt-tx"));
                    Thread.Sleep(1000);
                    rating.Click();

                    var elements = driver.FindElements(By.ClassName("comment-text"));

                    Product uruns = new Product
                    {
                        ProductId  = ProductId,
                        ProductBrand = ProductBrand,
                        ProductName = seacrWord,
                        ProductRating = ProductRating,
                        ProductPrice = ProductPrice,
                        ProductImage = ProductImage,
                        ProductProperties = properties,
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

