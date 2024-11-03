using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SportifMedia_Web.Models;
using System.Diagnostics;

namespace SportifMedia_Web.Controllers
{
    public class HomeController : Controller
    {
        [Authorize(Roles = "admin,user")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var httpClient = new HttpClient();
            var responseMessage = await httpClient.GetAsync("https://localhost:44347/api/Articles/getarticlewithdetails");

            if (responseMessage.IsSuccessStatusCode)
            {
                // Session'dan al�nan veriler null olabilece�i i�in kontrol edelim
                var userId = HttpContext.Session.GetInt32("UserId");
                var userName = HttpContext.Session.GetString("UserName");

                if (userId == null)
                {
                    return RedirectToAction("Login", "Auth"); // Giri� yapmam��sa y�nlendirebilirsiniz
                }

                ViewData["UserId"] = userId;
                ViewData["UserName"] = userName ?? "Bilinmeyen Kullan�c�"; // Null ise varsay�lan isim

                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiListDataResponse<ArticleDetailDto>>(jsonResponse);

                // API'den d�nen verilerin null olup olmad���n� kontrol edelim
                if (apiDataResponse != null && apiDataResponse.Data != null)
                {
                    int myArticle = apiDataResponse.Data.Count(x => x.UserId == (int)ViewData["UserId"]);
                    HttpContext.Session.SetInt32("MyArticle", myArticle);
                    ViewData["MyArticle"] = myArticle;

                    return apiDataResponse.Success ? View(apiDataResponse.Data) : (IActionResult)View("Veri gelmiyor");
                }
                else
                {
                    return View("Veri gelmiyor");
                }
            }

            return View("Veri gelmiyor");
        }
    }
}
