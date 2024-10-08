using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KoiAuction.Repository.Entities;
using KoiAuction.BussinessModels.Pagination;
using KoiAuction.BussinessModels.Proposal;
using KoiAuction.Common;
using KoiAuction.Service.Base;
using Newtonsoft.Json;
using KoiAuction.BussinessModels.DetailProposalModel;
using KoiAuction.MVCWebApp.Models;


namespace KoiAuction.MVCWebApp.Controllers
{
    public class DetailProposalsController : Controller
    {
        //private readonly Fa24Se1716Prn231G5KoiauctionContext _context;

        //public DetailProposalsController(Fa24Se1716Prn231G5KoiauctionContext context)
        //{
        //    _context = context;
        //}

        // GET: DetailProposals
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 2, string search = "", string sortBy = "", string direction = "")
        {
            //var fa24Se1716Prn231G5KoiauctionContext = _context.DetailProposals.Include(d => d.Auction).Include(d => d.Farm).Include(d => d.FishType);
            //return View(await fa24Se1716Prn231G5KoiauctionContext.ToListAsync());
            string apiUrl = $"detailProposal?page-index={pageIndex}&page-size={pageSize}&search-key={search}&sort-by={sortBy}&direction={direction}";

            using (var httpClient = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true // Bypass SSL validation (for testing)
            }))
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + apiUrl))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                        if (result != null && result.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<PageEntity<DetailProposalModel>>
                                (result.Data.ToString());
                            var paginatedViewModel = new PaginatedViewModel<DetailProposalModel>
                            {
                                Items = data.List.ToList(),
                                PageIndex = pageIndex,
                                PageSize = pageSize,
                                TotalPages = data.TotalPage,
                                SortBy = sortBy,
                                SortDirection = direction
                            };

                            // Pass the full view model to the view
                            return View(paginatedViewModel);
                            //return View(data.List.ToList());
                        }
                    }
                    return View();
                }

            }
        }

        // GET: DetailProposals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var detailProposal = await _context.DetailProposals
            //    .Include(d => d.Auction)
            //    .Include(d => d.Farm)
            //    .Include(d => d.FishType)
            //    .FirstOrDefaultAsync(m => m.FishId == id);
            //if (detailProposal == null)
            //{
            //    return NotFound();
            //}

            //return View(detailProposal);

            using (var httpClient = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true // Bypass SSL validation (for testing)
            }))
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + "detailProposal/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                        if (result != null && result.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<DetailProposalModel>
                                (result.Data.ToString());
                            return View(data);
                        }
                    }
                    return View();
                }

            }
        }

        // GET: DetailProposals/Create
        public async Task<IActionResult> Create()
        {
            ViewData["AuctionId"] = new SelectList(await this.GetAuctions(), "AuctionId", "AuctionName");
            ViewData["FarmId"] = new SelectList(await this.GetProposals(), "FarmId", "FarmName");
            ViewData["FishTypeId"] = new SelectList(await this.GetProposalTypes(), "FishTypeId", "FishTypeName");
            return View();
        }

        // POST: DetailProposals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FishId,FishCode,FishName,Gender,Age,Length,Weight,Rating,Status,CreateDate,UpdateDate,Description,ImageUrl,VideoUrl,Color,InitialPrice,FinalPrice,Index,TimeSpan,MinIncrement,FishTypeId,FarmId,AuctionId,AuctionFee")] DetailProposalModel detailProposal, IFormFile? imageFile, IFormFile? videoFile)
        {
            //    if (ModelState.IsValid)
            //    {
            //        _context.Add(detailProposal);
            //        await _context.SaveChangesAsync();
            //        return RedirectToAction(nameof(Index));
            //    }

            bool saveStatus = false;
            
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient(new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true // Bypass SSL validation (for testing)
                }))
                {
                    var uploadImageToFirebase = await UploadToFirebase(1, -1,imageFile);
                    var uploadVideoToFirebase = await UploadToFirebase(2, -1,videoFile);
                    detailProposal.ImageUrl = uploadImageToFirebase;
                    detailProposal.VideoUrl = uploadVideoToFirebase;
                    using (var response = await httpClient.PostAsJsonAsync(Const.APIEndPoint + "detailProposal/create-detail-proposal", detailProposal))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                            if (result != null && result.Status == Const.SUCCESS_CREATE_CODE)
                            {
                                saveStatus = true;
                            }
                            else
                            {
                                saveStatus = false;
                            }
                        }
                    }
                }
            }
            if (saveStatus)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewData["AuctionId"] = new SelectList(await this.GetAuctions(), "AuctionId", "AuctionName", detailProposal.AuctionId);
                ViewData["FarmId"] = new SelectList(await this.GetProposals(), "FarmId", "FarmName", detailProposal.FarmId);
                ViewData["FishTypeId"] = new SelectList(await this.GetProposalTypes(), "FishTypeId", "FishTypeName", detailProposal.FishTypeId);
                return View();
            }
        }

        // GET: DetailProposals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var detailProposal = new DetailProposalModel();
            using (var httpClient = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true // Bypass SSL validation (for testing)
            }))
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + "detailProposal/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Data != null)
                        {
                            detailProposal = JsonConvert.DeserializeObject<DetailProposalModel>(result.Data.ToString());
                        }
                    }
                }
            }
            ViewData["AuctionId"] = new SelectList(await this.GetAuctions(), "AuctionId", "AuctionName", detailProposal.AuctionId);
            ViewData["FarmId"] = new SelectList(await this.GetProposals(), "FarmId", "FarmName", detailProposal.FarmId);
            ViewData["FishTypeId"] = new SelectList(await this.GetProposalTypes(), "FishTypeId", "FishTypeName", detailProposal.FishTypeId);
            return View(detailProposal);
        }

        // POST: DetailProposals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FishId,FishCode,FishName,Gender,Age,Length,Weight,Rating,Status,CreateDate,UpdateDate,Description,ImageUrl,VideoUrl,Color,InitialPrice,FinalPrice,Index,TimeSpan,MinIncrement,FishTypeId,FarmId,AuctionId,AuctionFee")] DetailProposalModel detailProposal, IFormFile? imageFile, IFormFile? videoFile)
        {
            bool saveStatus = false;
            if(detailProposal.UpdateDate == null)
            {
                detailProposal.UpdateDate = DateOnly.FromDateTime(DateTime.Now);
            }
            if (detailProposal.UpdateDate < detailProposal.CreateDate)
            {
                ModelState.AddModelError("UpdateDate", "Update Date must be greater than Create Date");
            }

            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient(new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true // Bypass SSL validation (for testing)
                }))
                {
                    var uploadImageToFirebase = await UploadToFirebase(1, id, imageFile);
                    var uploadVideoToFirebase = await UploadToFirebase(2, id, videoFile);
                    detailProposal.ImageUrl = uploadImageToFirebase;
                    detailProposal.VideoUrl = uploadVideoToFirebase;
                    using (var response = await httpClient.PutAsJsonAsync(Const.APIEndPoint + "detailProposal/" + detailProposal.FishId, detailProposal))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                            if (result != null && result.Status == Const.SUCCESS_UPDATE_CODE)
                            {
                                saveStatus = true;
                            }
                            else
                            {
                                saveStatus = false;
                            }
                        }
                    }
                }
            }

            if (saveStatus)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewData["AuctionId"] = new SelectList(await this.GetAuctions(), "AuctionId", "AuctionName", detailProposal.AuctionId);
                ViewData["FarmId"] = new SelectList(await this.GetProposals(), "FarmId", "FarmName", detailProposal.FarmId);
                ViewData["FishTypeId"] = new SelectList(await this.GetProposalTypes(), "FishTypeId", "FishTypeName", detailProposal.FishTypeId);
                return View(detailProposal);
            }
        }

        // GET: DetailProposals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var detailProposal = new DetailProposalModel();
            using (var httpClient = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true // Bypass SSL validation (for testing)
            }))
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + "detailProposal/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Data != null)
                        {
                            detailProposal = JsonConvert.DeserializeObject<DetailProposalModel>(result.Data.ToString());
                        }
                    }
                }
            }
            ViewData["AuctionId"] = new SelectList(await this.GetAuctions(), "AuctionId", "AuctionName", detailProposal.AuctionId);
            ViewData["FarmId"] = new SelectList(await this.GetProposals(), "FarmId", "FarmName", detailProposal.FarmId);
            ViewData["FishTypeId"] = new SelectList(await this.GetProposalTypes(), "FishTypeId", "FishTypeName", detailProposal.FishTypeId);
            return View(detailProposal);
        }

        // POST: DetailProposals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool saveStatus = false;
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient(new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true // Bypass SSL validation (for testing)
                }))
                {
                    using (var response = await httpClient.DeleteAsync(Const.APIEndPoint + "detailProposal/" + id))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                            if (result != null && result.Status == Const.SUCCESS_DELETE_CODE)
                            {
                                saveStatus = true;
                            }
                            else
                            {
                                saveStatus = false;
                            }
                        }
                    }
                }
            }

            if (saveStatus)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View();
            }
        }

        public async Task<List<Auction>> GetAuctions()
        {
            using (var httpClient = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true // Bypass SSL validation (for testing)
            }))
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + "detailProposal/auction"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                        if (result != null && result.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<List<Auction>>
                                (result.Data.ToString());
                            return data;
                        }
                    }
                    return new List<Auction>();
                }

            }
        }

        public async Task<List<FishType>> GetProposalTypes()
        {
            using (var httpClient = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true // Bypass SSL validation (for testing)
            }))
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + "detailProposal/type"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                        if (result != null && result.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<List<FishType>>
                                (result.Data.ToString());
                            return data;
                        }
                    }
                    return new List<FishType>();
                }

            }
        }

        public async Task<List<Proposal>> GetProposals()
        {
            using (var httpClient = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true // Bypass SSL validation (for testing)
            }))
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + "detailProposal/proposal"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                        if (result != null && result.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<List<Proposal>>
                                (result.Data.ToString());
                            return data;
                        }
                    }
                    return new List<Proposal>();
                }

            }
        }

        public async Task<string> UploadToFirebase(int type, int detailProposalId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return "";
            }

            using (var httpClient = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true // Bypass SSL validation (for testing)
            }))
            {
                var contentOfFile = new MultipartFormDataContent();
                var stream = new MemoryStream();

                await file.CopyToAsync(stream);
                stream.Position = 0; // Reset the stream position to the beginning

                var fileContent = new StreamContent(stream);
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

                // Add the file content to the multipart form data
                contentOfFile.Add(fileContent, "file", file.FileName);
                using (var response = await httpClient.PostAsync(Const.APIEndPoint + "detailProposals/upload?detailProposalId=" + detailProposalId +"&type=" + type, contentOfFile))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                        if (result != null && result.Data != null)
                        {
                            var data = result.Data.ToString();
                            return data;
                        }
                    }
                    return "";
                }

            }
        }
    }
}
