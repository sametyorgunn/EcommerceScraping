using CommentScraping;
using CommentScraping.Model;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace CommentScraping
{
    public class GetCategory
    {
        public void GetCategoryList()
        {
            var options = new ChromeOptions();
            using (IWebDriver driver = new ChromeDriver(options))
            {
                driver.Navigate().GoToUrl("https://www.trendyol.com/");
                var allCategoriesList = driver.FindElements(By.CssSelector("li.tab-link"));

                foreach (var cat in allCategoriesList)
                {
                    var MainCategory = cat.FindElement(By.CssSelector("a.category-header")).Text;
                    var SubCategory = cat.FindElement(By.CssSelector("a.sub-category-header")).Text;
                    var SubCategories = driver.FindElements(By.CssSelector("div.category-box"));
                    foreach(var a in SubCategories)
                    {
                        var asd = a.FindElement(By.CssSelector("div.category-box a")).Text;
                    }
                }
            }
        }
    }
}
