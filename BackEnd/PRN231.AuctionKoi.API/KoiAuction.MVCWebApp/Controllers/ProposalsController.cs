using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using KoiAuction.BussinessModels.Pagination;
using KoiAuction.BussinessModels.Proposal;
using KoiAuction.Common;
using KoiAuction.MVCWebApp.Models;
using KoiAuction.Repository.Entities;
using KoiAuction.Service.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace KoiAuction.MVCWebApp.Controllers
{
    public class ProposalsController : Controller
    {
        //private readonly AuctionKoiOfficialContext _context;

        //public ProposalsController(AuctionKoiOfficialContext context)
        //{
        //    _context = context;
        //}

        // GET: Proposals
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 2, string search = "", string sortBy = "", string direction = "")
        {
            //var auctionKoiOfficialContext = _context.Proposals.Include(p => p.User);
            //return View(await auctionKoiOfficialContext.ToListAsync());
            string apiUrl = $"proposals?page-index={pageIndex}&page-size={pageSize}&search-key={search}&sort-by={sortBy}&direction={direction}";

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
                            var data = JsonConvert.DeserializeObject<PageEntity<ProposalModel>>
                                (result.Data.ToString());
                            // Create and populate the PaginatedViewModel
                            var paginatedViewModel = new PaginatedViewModel<ProposalModel>
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
                           // return View(data.List.ToList());
                        }
                    }
                    return View();
                }

            }
        }

        // GET: Proposals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var proposal = await _context.Proposals
            //    .Include(p => p.User)
            //    .FirstOrDefaultAsync(m => m.FarmId == id);
            //if (proposal == null)
            //{
            //    return NotFound();
            //}

            //return View(proposal);
            using (var httpClient = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true // Bypass SSL validation (for testing)
            }))
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + "proposals/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                        if (result != null && result.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<ProposalModel>
                                (result.Data.ToString());
                            return View(data);
                        }
                    }
                    return View();
                }

            }
        }

        // GET: Proposals/Create
        public async Task<IActionResult> Create()
        {
            ViewData["UserId"] = new SelectList(await this.GetUsers(), "UserId", "FullName");
            return View();
        }

        // POST: Proposals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FarmName,Location,AvatarUrl,CreateDate,Status,Description,Owner,UpdateDate,UserId")] ProposalModel proposal, IFormFile? avatarFile)
        {

            bool saveStatus = false;
            if(proposal.UpdateDate < proposal.CreateDate)
            {
                ModelState.AddModelError("UpdateDate", "Update Date must be greater than Create Date");
            }
            if(ModelState.IsValid)
            {
                using (var httpClient = new HttpClient(new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true // Bypass SSL validation (for testing)
                }))
                {
                    var uploadFirebase = await UploadToFirebase(-1,avatarFile);
                    proposal.AvatarUrl = uploadFirebase;
                    using (var response = await httpClient.PostAsJsonAsync(Const.APIEndPoint + "proposals/create-proposal", proposal))
                    {
                        if(response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync ();
                            var result = JsonConvert.DeserializeObject<BusinessResult> (content);
                            if(result != null && result.Status == Const.SUCCESS_CREATE_CODE)
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
            if(saveStatus)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewData["UserId"] = new SelectList(await this.GetUsers(), "UserId", "FullName");
                return View();
            }
            //if (ModelState.IsValid)
            //{
            //    _context.Add(proposal);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", proposal.UserId);
            //return View(proposal);
        }

        // GET: Proposals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var proposal = new ProposalModel();
            using (var httpClient = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true // Bypass SSL validation (for testing)
            }))
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + "proposals/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if(result != null && result.Data != null)
                        {
                            proposal = JsonConvert.DeserializeObject<ProposalModel>(result.Data.ToString());
                        }
                    }
                }
            }
            ViewData["UserId"] = new SelectList(await this.GetUsers(), "UserId", "FullName", proposal.UserId);
            return View(proposal);
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var proposal = await _context.Proposals.FindAsync(id);
            //if (proposal == null)
            //{
            //    return NotFound();
            //}
            //ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", proposal.UserId);
            //return View(proposal);
        }

        // POST: Proposals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FarmId,FarmCode,FarmName,Location,AvatarUrl,CreateDate,Status,Description,Owner,IsDeleted,UpdateDate,UserId")] ProposalModel proposal, IFormFile? avatarFile)
        {
            bool saveStatus = false;
            if(ModelState.IsValid)
            {
                using (var httpClient = new  HttpClient(new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true // Bypass SSL validation (for testing)
                }))
                {
                    var uploadFirebase = await UploadToFirebase(id, avatarFile);
                    proposal.AvatarUrl = uploadFirebase;
                    using (var response = await httpClient.PutAsJsonAsync(Const.APIEndPoint + "proposals/" + proposal.FarmId, proposal))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<BusinessResult> (content);

                            if(result != null && result.Status == Const.SUCCESS_UPDATE_CODE)
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

            if(saveStatus)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewData["UserId"] = new SelectList(await this.GetUsers(), "UserId", "FullName", proposal.UserId);
                return View(proposal);
            }

            //if (id != proposal.FarmId)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(proposal);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!ProposalExists(proposal.FarmId))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", proposal.UserId);
            //return View(proposal);
        }

        // GET: Proposals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var proposal = new ProposalModel();
            using (var httpClient = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true // Bypass SSL validation (for testing)
            }))
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + "proposals/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);

                        if (result != null && result.Data != null)
                        {
                            proposal = JsonConvert.DeserializeObject<ProposalModel>(result.Data.ToString());
                        }
                    }
                }
            }
            ViewData["UserId"] = new SelectList(await this.GetUsers(), "UserId", "UserFullName", proposal.UserId);
            return View(proposal);
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var proposal = await _context.Proposals
            //    .Include(p => p.User)
            //    .FirstOrDefaultAsync(m => m.FarmId == id);
            //if (proposal == null)
            //{
            //    return NotFound();
            //}

            //return View(proposal);
        }

        // POST: Proposals/Delete/5
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
                    using (var response = await httpClient.DeleteAsync(Const.APIEndPoint + "proposals/" + id))
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

            //var proposal = await _context.Proposals.FindAsync(id);
            //if (proposal != null)
            //{
            //    _context.Proposals.Remove(proposal);
            //}

            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
        }

        //private bool ProposalExists(int id)
        //{
        //    return _context.Proposals.Any(e => e.FarmId == id);
        //}

        public async Task<List<User>> GetUsers()
        {
            using (var httpClient = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true // Bypass SSL validation (for testing)
            }))
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + "proposals/user"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<BusinessResult>(content);
                        if (result != null && result.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<List<User>>
                                (result.Data.ToString());
                            return data;
                        }
                    }
                    return new List<User>();
                }

            }
        }

        public async Task<string> UploadToFirebase(int proposalId, IFormFile file)
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
                using (var response = await httpClient.PostAsync(Const.APIEndPoint + "proposals/upload?proposalId=" + proposalId, contentOfFile))
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
