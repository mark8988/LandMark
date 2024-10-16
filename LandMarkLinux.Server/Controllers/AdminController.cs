using Microsoft.AspNetCore.Mvc;
using LandMark.EF;
using LandMark.Middleware.services;
using LandMark.Middleware.ViewModels;
using LandMark.Middleware.ViewModels.Admin_Info;
using System.IdentityModel.Tokens.Jwt;
using LandMarkLinux.Server.Helpers;

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
        [HttpGet("[action]")]
        public IActionResult Login(string Account, string? Password = "")
        {
            try
            {
                var admin = _sManageProvider.Get(w => w.AdminAccount == Account);

                if (Account.ToLower() != "admin")
                {
                    Admin_Login _encodeLogin = new Admin_Login
                    {
                        account = Account,
                        password = Password,
                    };
                    var check_Acc = _sManageProvider.CheckLogin(_encodeLogin);
                    if (!check_Acc.Key)
                    {
                        return BadRequest(check_Acc.Value);
                    }
                }
                var result = _jwtHelpers.GenerateToken(Account);
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
    }
}
