using KoiAuction.BussinessModels.Pagination;
using KoiAuction.Common;
using KoiAuction.Repository.Entities;
using KoiAuction.Service.Base;
using KoiAuction.Service.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.WebApp.Controllers
{
    public class AuctionsController : Controller
    {
        private readonly HttpClient _httpClient;

        public AuctionsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET: Auctions
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync(Const.APIEndPoint + "api/Auction/auction-koi/auctions");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ServiceResult<PageEntity<Auction>>>(content);

                if (result != null && result.IsSuccess && result.Data != null)
                {
                    return View(result.Data);
                }
            }

            return View(new PageEntity<Auction> { List = new List<Auction>() });
        }

        // GET: Auctions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _httpClient.GetAsync(Const.APIEndPoint + "api/Auction/" + id);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ServiceResult<Auction>>(content);

                if (result != null && result.IsSuccess && result.Data != null)
                {
                    return View(result.Data);
                }
            }

            return NotFound();
        }

        // GET: Auctions/Create
        public async Task<IActionResult> Create()
        {
            var response = await _httpClient.GetAsync(Const.APIEndPoint + "api/Auction/auction-koi/auctions/types");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ServiceResult<List<AuctionType>>>(content);

                if (result != null && result.IsSuccess && result.Data != null)
                {
                    ViewData["TypeId"] = new SelectList(result.Data, "TypeId", "TypeName");
                    return View();
                }
            }

            return View(new Auction());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AuctionId,AuctionName,AuctionDate,StartTime,EndTime,MinIncrement,Status,Description,CreateDate,AuctionMethod,AuctionCode,TimeSpan,TypeId")] Auction auction)
        {
            if (!ModelState.IsValid)
            {
                var response = await _httpClient.GetAsync(Const.APIEndPoint + "api/Auction/auction-koi/auctions/types");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ServiceResult<List<AuctionType>>>(content);
                    ViewData["TypeId"] = new SelectList(result.Data, "TypeId", "TypeName", auction.TypeId);
                }
                return View(auction);
            }

            var json = JsonConvert.SerializeObject(auction);
            var contentToPost = new StringContent(json, Encoding.UTF8, "application/json");

            // Ensure the API endpoint is correct and absolute
            var responsePost = await _httpClient.PostAsync($"{Const.APIEndPoint}api/Auction/auction-koi/auctions", contentToPost);

            if (responsePost.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(auction);
        }


        // GET: Auctions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _httpClient.GetAsync(Const.APIEndPoint + $"api/auction/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var auction = JsonConvert.DeserializeObject<ServiceResult<Auction>>(content);

                if (auction != null && auction.IsSuccess && auction.Data != null)
                {
                    var typesResponse = await _httpClient.GetAsync(Const.APIEndPoint + "api/Auction/auction-koi/auctions/types");
                    if (typesResponse.IsSuccessStatusCode)
                    {
                        var typesContent = await typesResponse.Content.ReadAsStringAsync();
                        var typesResult = JsonConvert.DeserializeObject<ServiceResult<List<AuctionType>>>(typesContent);

                        if (typesResult != null && typesResult.IsSuccess && typesResult.Data != null)
                        {
                            ViewData["TypeId"] = new SelectList(typesResult.Data, "TypeId", "TypeName", auction.Data.TypeId);
                            return View(auction.Data);
                        }
                    }
                }
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AuctionId,AuctionName,AuctionDate,StartTime,EndTime,MinIncrement,Status,Description,CreateDate,AuctionMethod,AuctionCode,TimeSpan,TypeId")] Auction auction)
        {
            if (id != auction.AuctionId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                var response = await _httpClient.GetAsync(Const.APIEndPoint + "api/Auction/auction-koi/auctions/types");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ServiceResult<List<AuctionType>>>(content);
                    if (result != null && result.IsSuccess)
                    {
                        ViewData["TypeId"] = new SelectList(result.Data, "TypeId", "TypeName", auction.TypeId);
                    }
                }
                return View(auction);
            }

            var json = JsonConvert.SerializeObject(auction);
            var contentToPost = new StringContent(json, Encoding.UTF8, "application/json");

            var responsePut = await _httpClient.PutAsync($"{Const.APIEndPoint}api/Auction/{id}", contentToPost);

            if (responsePut.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(auction);
        }

        // DELETE: Auctions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool deleteStatus = false;

            if (ModelState.IsValid)
            {
                var response = await _httpClient.DeleteAsync(Const.APIEndPoint + $"api/auction/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ServiceResult<object>>(content);

                    if (result != null && result.IsSuccess)
                    {
                        deleteStatus = true;
                    }
                }
            }
            if (deleteStatus)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View();
            }
        }
    }
}
