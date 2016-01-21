using IR46.WebHost.Repository.BaseRepository;
using IR46.WebHost.Service.CommandService.Interface;
using Ninject;
using System;
using System.Web;
using IR46.Domain.Entities;

namespace IR46.WebHost.Service.CommandService.Implementation
{
    public class LoginCommandService : ILoginCommandService
    {
        /// <summary>
        /// 依赖注入
        /// </summary>
        [Inject]
        public ICommandRepository<Data_LoginRecords> RepoDataLoginRecordsCommandRepository { get; set; }


        /// <summary>
        /// 添加登录日志
        /// </summary>
        /// <param name="userName">登录信息</param>
        /// <param name="desc">登录信息</param>
        /// <param name="isSuccess">登录信息</param>
        /// <param name="userId">用户Id</param>
        public void WriteLoginRecord(string userName, string desc, bool isSuccess, string userId = null)
        {
            RepoDataLoginRecordsCommandRepository.Create(new Data_LoginRecords()
            {
                Id = Guid.NewGuid().ToString(),
                用户Id = userId,
                用户名 = userName,
                操作 = desc,
                是否成功 = isSuccess,
                操作时间 = DateTime.Now,
                IP地址 = HttpContext.Current.Request.UserHostAddress
            });

            RepoDataLoginRecordsCommandRepository.SaveChange();
        }




    }
}