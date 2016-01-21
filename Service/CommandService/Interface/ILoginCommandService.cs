
namespace IR46.WebHost.Service.CommandService.Interface
{
    public interface ILoginCommandService
    {
        /// <summary>
        /// 添加登录日志
        /// </summary>
        /// <param name="userName">登录信息</param>
        /// <param name="desc">登录信息</param>
        /// <param name="isSuccess">登录信息</param>
        /// <param name="userId">用户Id</param>
        void WriteLoginRecord(string userName, string desc, bool isSuccess, string userId = null);



    }
}
