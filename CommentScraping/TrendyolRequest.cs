using CommentScraping.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CommentScraping
{
    public class TrendyolRequest
    {
        public async Task<List<Category>> GetCategories()
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "https://api.trendyol.com/sapigw/product-categories";
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();

                    // JSON'u CategoryResponse tipine deserialize et
                    CategoryResponse categoryResponse = JsonConvert.DeserializeObject<CategoryResponse>(responseData);

                    // Categories listesini döndür
                    return categoryResponse.Categories;
                }
                else
                {
                    Console.WriteLine($"İstek başarısız oldu: {response.StatusCode}");
                    return null;
                }
            }
        }
    }
}
