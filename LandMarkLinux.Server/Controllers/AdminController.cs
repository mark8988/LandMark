using Microsoft.AspNetCore.Mvc;
using LandMark.EF;
using LandMark.Middleware.services;
using LandMark.Middleware.ViewModels;
using LandMark.Middleware.ViewModels.Admin_Info;
using System.IdentityModel.Tokens.Jwt;
using LandMarkLinux.Server.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Data;

namespace LandMarkLinux.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;
        private readonly JwtHelper _jwtHelpers;
        private SAdminManage _sManageProvider;
        private LandMarkContext _db;

        public AdminController(ILogger<AdminController> logger, JwtHelper jwtHelpers, SAdminManage provider, LandMarkContext db)
        {
            _logger = logger;
            _jwtHelpers = jwtHelpers;
            _sManageProvider = provider;
            _db = db;
        }

        /// <summary>
        /// 登入驗證，若驗證成功則發Token
        /// </summary>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult Login([FromBody] Admin_Login request)
        {
            try
            {
                var admin = _sManageProvider.Get(w => w.AdminAccount == request.account);

                if (request.account.ToLower() != "admin")
                {
                    Admin_Login _encodeLogin = new Admin_Login
                    {
                        account = request.account,
                        password = request.password,
                    };
                    var check_Acc = _sManageProvider.CheckLogin(_encodeLogin);
                    if (!check_Acc.Key)
                    {
                        return BadRequest(check_Acc.Value);
                    }
                }
                var result = _jwtHelpers.GenerateToken(request.account);
                if (admin != null)
                {
                    return Ok(result.Value);
                }
                else
                {
                    return BadRequest("查無此帳號資料");
                }
            }
            catch (Exception ex)
            {
                Response msg = new Response()
                {
                    result_message = ex.Message,
                    result_stacktrace = ex.StackTrace,
                    result_status = false,
                    result_content = null
                };

                return BadRequest(msg);
            }
        }

        [HttpPost("validate"), Authorize(Roles = "admin")]
        [Authorize] 
        public IActionResult ValidateToken()
        {
            // 獲取當前用戶的 JWT，這裡使用 HttpContext
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { message = "Token is required" });
            }

            var handler = new JwtSecurityTokenHandler();
            try
            {
                var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

                // 檢查 token 是否有效
                if (jwtToken != null && jwtToken.ValidTo > DateTime.UtcNow)
                {
                    var userId = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

                    // 可以返回用戶資訊或其他相關資料
                    return Ok(new
                    {
                        status = "success",
                        userId = userId,
                        message = "Token is valid"
                    });
                }
                else
                {
                    return Unauthorized(new { message = "Token is invalid" });
                }
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = "Token is invalid", error = ex.Message });
            }
        }
    }
}
