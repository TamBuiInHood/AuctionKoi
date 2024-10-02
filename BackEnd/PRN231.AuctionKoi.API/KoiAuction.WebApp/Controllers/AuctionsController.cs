using KoiAuction.BussinessModels.Pagination;
using KoiAuction.Common;
using KoiAuction.Repository.Entities;
using KoiAuction.Service.Base;
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
            var response = await _httpClient.GetAsync(Const.APIEndPoint + "api/Auction");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<BusinessResult<PageEntity<Auction>>>(content);

                if (result != null && result.Data != null)
                {
                    var auctions = result.Data.List; // Access the List field in the Data property
                    return View(auctions); // Pass the list of auctions to the view
                }
            }

            return View(new List<Auction>()); // In case of failure, return an empty list
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
                var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                if (result != null && result.Data != null)
                {
                    var auction = JsonConvert.DeserializeObject<Auction>(result.Data.ToString());

                    // Check if the Type is null
                    if (auction.Type == null)
                    {
                        ModelState.AddModelError(string.Empty, "Auction type information is missing.");
                    }

                    return View(auction);
                }
            }

            return NotFound();
        }


        // GET: Auctions/Create
        public async Task<IActionResult> Create()
        {
            // Fetch auction types for the dropdown
            var response = await _httpClient.GetAsync(Const.APIEndPoint + "api/Auction/types");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                if (result != null && result.Data != null)
                {
                    var auctionTypes = JsonConvert.DeserializeObject<List<AuctionType>>(result.Data.ToString());
                    ViewBag.TypeId = new SelectList(auctionTypes, "TypeId", "TypeName"); // Populate ViewBag
                }
            }
            else
            {
                // Handle error, for instance by adding a fallback option
                ViewBag.TypeId = new SelectList(new List<AuctionType>());
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AuctionId,AuctionName,AuctionDate,StartTime,EndTime,MinIncrement,Status,Description,CreateDate,AuctionMethod,AuctionCode,TimeSpan,TypeId")] Auction auction)
        {
            // If the model state is not valid, reload the auction types for the dropdown
            if (!ModelState.IsValid)
            {
                // Fetch auction types again to repopulate the dropdown list in case of validation errors
                var response = await _httpClient.GetAsync($"{Const.APIEndPoint}api/Auction/types");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                    // Check if the response contains a successful status and auction types
                    if (result != null  && result.Data != null)
                    {
                        var auctionTypes = JsonConvert.DeserializeObject<List<AuctionType>>(result.Data.ToString());
                        ViewData["TypeId"] = new SelectList(auctionTypes, "TypeId", "TypeName", auction.TypeId);
                    }
                }

                return View(auction);
            }

            // If the model is valid, proceed with creating the auction
            var json = JsonConvert.SerializeObject(auction);
            var contentToPost = new StringContent(json, Encoding.UTF8, "application/json");

            // Call the API to create the auction
            var responsePost = await _httpClient.PostAsync($"{Const.APIEndPoint}api/Auction", contentToPost);
            if (responsePost.IsSuccessStatusCode)
            {
                // If the creation is successful, redirect to the index
                return RedirectToAction(nameof(Index));
            }

            // If the API call fails, return the view with the current data
            return View(auction);
        }


        // GET: Auctions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Fetch the auction details
            var response = await _httpClient.GetAsync(Const.APIEndPoint + "api/Auction/" + id);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                if (result != null && result.Data != null)
                {
                    var auction = JsonConvert.DeserializeObject<Auction>(result.Data.ToString());

                    // Fetch auction types for the dropdown
                    var typesResponse = await _httpClient.GetAsync($"{Const.APIEndPoint}api/Auction/types");
                    if (typesResponse.IsSuccessStatusCode)
                    {
                        var typesContent = await typesResponse.Content.ReadAsStringAsync();
                        var typesResult = JsonConvert.DeserializeObject<BusinessResult>(typesContent);

                        if (typesResult != null && typesResult.Data != null)
                        {
                            var auctionTypes = JsonConvert.DeserializeObject<List<AuctionType>>(typesResult.Data.ToString());
                            ViewData["TypeId"] = new SelectList(auctionTypes, "TypeId", "TypeName", auction.TypeId); // Populate ViewBag for dropdown
                            return View(auction); // Pass auction to the view
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
                var response = await _httpClient.GetAsync($"{Const.APIEndPoint}api/Auction/types");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                    if (result != null  && result.Data != null)
                    {
                        var auctionTypes = JsonConvert.DeserializeObject<List<AuctionType>>(result.Data.ToString());
                        ViewData["TypeId"] = new SelectList(auctionTypes, "typeId", "typeName", auction.TypeId);
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

        // GET: Auctions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _httpClient.GetAsync(Const.APIEndPoint + $"api/Auction/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                if (result != null  && result.Data != null)
                {
                    var auction = JsonConvert.DeserializeObject<Auction>(result.Data.ToString());
                    return View(auction);
                }
            }

            return NotFound();
        }

        // POST: Auctions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync(Const.APIEndPoint + $"api/Auction/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }
    }
}
