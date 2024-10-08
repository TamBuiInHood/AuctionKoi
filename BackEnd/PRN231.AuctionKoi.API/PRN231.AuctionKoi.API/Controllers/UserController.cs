using KoiAuction.API.Payloads.Requests.AuthenticationRequest;
using KoiAuction.Service.ISerivice;
using KoiAuction.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using PRN231.AuctionKoi.API.Payloads;

namespace KoiAuction.API.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        //[Authorize(Roles = )]
        [HttpPost(APIRoutes.Authentication.Login, Name = "LoginAsync")]
        public async Task<IActionResult> LoginAsync([FromBody] Payloads.Requests.AuthenticationRequest.LoginRequest reqObj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _userService.CheckLogin(reqObj.Email, reqObj.Password);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
