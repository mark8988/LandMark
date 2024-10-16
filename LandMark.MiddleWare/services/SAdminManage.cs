using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using LandMark.EF;
using LandMark.Middleware.interfaces;
using LandMark.Middleware.ViewModels.Admin_Info;

namespace LandMark.Middleware.services
{
    public class SAdminManage : BaseService<Admin>, IServiceManage<Admin>
    {
        private LandMarkContext _db;
        //
        public SAdminManage(GlobalSettings globalSettings
            , LandMarkContext db, IHttpContextAccessor httpContextAccessor)
            : base(globalSettings, db, httpContextAccessor)
        {
            _db = db;
        }

        public KeyValuePair<bool, string> CheckLogin(Admin_Login login)
        {
            KeyValuePair<bool, string> result = new KeyValuePair<bool, string>();
            bool resultFlag = false;
            string resultMsg = "";

            var check_Member = _db.Admins.Where(x => x.AdminAccount == login.account && x.AdminPassWord == login.password);
            if (check_Member.Any())
            {
                resultFlag = true;
                resultMsg = "帳號密碼驗證成功";
            }
            else
            {
                resultMsg = "登入失敗，帳號或密碼錯誤?";
            }
            return result = new KeyValuePair<bool, string>(resultFlag, resultMsg);
        }
    }
}
