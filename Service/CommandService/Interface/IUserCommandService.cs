using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IR46.WebHost.Dtos.InputDto.User;

namespace IR46.WebHost.Service.CommandService.Interface
{
    public interface IUserCommandService
    {

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="oldPassword">原始密码</param>
        /// <param name="newPassword">新密码</param>
        /// <returns></returns>
        bool ChangeUserPassword(string userId, string oldPassword, string newPassword);




        /// <summary>
        /// 保存添加的用户信息
        /// </summary>
        /// <param name="addUser">添加的用户信息</param>
        /// <returns></returns>
        bool SaveAddUserInfo(InputDtoAddUser addUser);




        /// <summary>
        /// 通过用户Id删除用户
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns></returns>
        bool DeleteUserById(string id);




        /// <summary>
        /// 保存修改后的用户信息
        /// </summary>
        /// <param name="editUserInfo">编辑后的用户信息</param>
        /// <returns></returns>
        bool SaveEditUserInfo(InputDtoEditUserInfo editUserInfo);



        /// <summary>
        /// 通过用户Id重置密码
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="newPassword">新密码</param>
        /// <returns></returns>
        bool ResetPassword(string userId, string newPassword);



        /// <summary>
        /// 锁定用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        bool LockUser(string userId);




        /// <summary>
        /// 解除用户锁定
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        bool UnlockUser(string userId);




        /// <summary>
        /// 保存设置用户的权限信息
        /// </summary>
        /// <param name="model">用户权限设置信息</param>
        /// <returns></returns>
        bool SaveSetUserAuthority(InputDtoSetUserAuthority model);








    }
}
