using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IR46.Domain.Entities;
using IR46.WebHost.Dtos.InputDto.User;
using IR46.WebHost.Infrastructure;
using IR46.WebHost.Infrastructure.Configure;
using IR46.WebHost.Infrastructure.Identity;
using IR46.WebHost.Repository.BaseRepository;
using IR46.WebHost.Service.CommandService.Interface;
using IR46.WebHost.Service.QueryService.Interface;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using Ninject;

namespace IR46.WebHost.Service.CommandService.Implementation
{
    public class UserCommandService : IUserCommandService
    {
        [Inject]
        public IUserQueryService ServiceUserQueryService { get; set; }

        [Inject]
        public IQueryRepository<AspNetUsers> RepoAspNetUsersQueryRepository { get; set; }

        [Inject]
        public ICommandRepository<AspNetUsers> RepoAspNetUsersCommandRepository { get; set; }



        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="oldPassword">原始密码</param>
        /// <param name="newPassword">新密码</param>
        /// <returns></returns>
        public bool ChangeUserPassword(string userId, string oldPassword, string newPassword)
        {
            var user = ServiceUserQueryService.GetUserById(userId);

            if (user == null)
            {
                return false;
            }

            //修改密码
            var userMgr = GetStoreUserManager();
            var result = userMgr.ChangePassword(userId, oldPassword, newPassword);

            return result.Succeeded;
        }





        /// <summary>
        /// 保存添加的用户信息
        /// </summary>
        /// <param name="addUser">添加的用户信息</param>
        /// <returns></returns>
        public bool SaveAddUserInfo(InputDtoAddUser addUser)
        {
            if (addUser == null)
            {
                throw new ArgumentNullException("addUser");
            }

            //向新增用户基本信息设置默认值
            var addStoreUser = new StoreUser();

            Conversion.Convert(addUser, addStoreUser);

            addStoreUser.Id = Guid.NewGuid().ToString();
            addStoreUser.IsLocked = false;

            //添加用户信息
            var userMgr = GetStoreUserManager();
            var addUserResult = userMgr.Create(addStoreUser, Configure.InitialPassword);

            if (addUserResult.Succeeded == false)
            {
                //添加用户失败，主要原因为用户名已存在
                return false;
            }

            //添加该用户对应的角色信息
            AddRolesToUser(addStoreUser.Id,
                addUser.RoleList.Where(c => c.IsSelected == true).Select(c => c.RoleName).ToArray());

            return true;
        }




        /// <summary>
        /// 将多个角色添加至指定用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="roles">角色数组</param>
        public void AddRolesToUser(string userId, string[] roles)
        {
            foreach (var role in roles)
            {
                AddRoleToUser(userId, role);
            }
        }




        /// <summary>
        /// 添加单个角色到指定用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="role">角色</param>
        public void AddRoleToUser(string userId, string role)
        {
            var userMgr = GetStoreUserManager();

            if (userMgr.IsInRole(userId, role) == false)
            {
                userMgr.AddToRole(userId, role);
            }
        }





        /// <summary>
        /// 通过用户Id删除用户
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns></returns>
        public bool DeleteUserById(string id)
        {
            if (id == null)
            {
                return false;
            }

            var userMgr = GetStoreUserManager();

            //查找待删除用户
            var user = userMgr.FindById(id);

            //若用户不存在，默认已删除
            if (user == null) return true;

            //调用Delete删除时则会删除该用户关联的角色
            var result = userMgr.Delete(user);

            return result.Succeeded;
        }





        /// <summary>
        /// 保存修改后的用户信息
        /// </summary>
        /// <param name="editUserInfo">编辑后的用户信息</param>
        /// <returns></returns>
        public bool SaveEditUserInfo(InputDtoEditUserInfo editUserInfo)
        {
            //获取待修改用户信息
            var storeUser = ServiceUserQueryService.GetUserById(editUserInfo.Id);

            //判断用户是否存在
            if (storeUser == null)
            {
                return false;
            }

            //修改用户编辑后的信息
            Conversion.Convert(editUserInfo, storeUser);


            //创建用户管理对象并将修改后数据提交至数据库
            var userMgr = GetStoreUserManager();
            var result = userMgr.Update(storeUser);

            //返回操作结果
            return result.Succeeded;
        }




        /// <summary>
        /// 通过用户Id重置密码
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="newPassword">新密码</param>
        /// <returns></returns>
        public bool ResetPassword(string userId, string newPassword)
        {
            var userMgr = GetStoreUserManager();

            //设置生成Token
            var provider = new DpapiDataProtectionProvider("CoalWeb.WebHost");
            userMgr.UserTokenProvider = new DataProtectorTokenProvider<StoreUser, string>(provider.Create("UserToken"));

            //生成修改用户密码Token
            var userToken = userMgr.GeneratePasswordResetToken(userId);

            //重置密码
            var result = userMgr.ResetPassword(userId, userToken, newPassword);

            return result.Succeeded;
        }





        /// <summary>
        /// 锁定用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public bool LockUser(string userId)
        {
            //查询用户
            var user = RepoAspNetUsersQueryRepository.Find<AspNetUsers>(c => c.Id == userId);

            if (user == null)
            {
                return false;
            }

            user.IsLocked = true;

            RepoAspNetUsersCommandRepository.Update(user);
            RepoAspNetUsersCommandRepository.SaveChange();

            return true;
        }




        /// <summary>
        /// 解除用户锁定
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public bool UnlockUser(string userId)
        {
            //查询用户
            var user = RepoAspNetUsersQueryRepository.Find<AspNetUsers>(c => c.Id == userId);

            if (user == null)
            {
                return false;
            }

            user.IsLocked = false;

            RepoAspNetUsersCommandRepository.Update(user);
            RepoAspNetUsersCommandRepository.SaveChange();

            return true;
        }




        /// <summary>
        /// 保存设置用户的权限信息
        /// </summary>
        /// <param name="model">用户权限设置信息</param>
        /// <returns></returns>
        public bool SaveSetUserAuthority(InputDtoSetUserAuthority model)
        {
            if (model == null)
            {
                return false;
            }

            //清除用户的角色信息
            ClearUserRoles(model.UserId, model.Roles.Select(c => c.RoleName).ToArray());


            //添加该用户对应的角色信息
            AddRolesToUser(model.UserId,
                model.Roles.Where(c => c.IsSelected == true).Select(c => c.RoleName).ToArray());

            return true;
        }


        //私有方法

        /// <summary>
        /// 获取StoreUserManager对象
        /// </summary>
        /// <returns></returns>
        private StoreUserManager GetStoreUserManager()
        {
            return HttpContext.Current.GetOwinContext().GetUserManager<StoreUserManager>();
        }



        /// <summary>
        /// 清除用户的角色信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roles"></param>
        private void ClearUserRoles(string userId, string[] roles)
        {
            var userMgr = GetStoreUserManager();
            var user = userMgr.FindById(userId);

            if (user == null) return;

            if (roles != null)
            {
                foreach (var role in roles)
                {
                    if (userMgr.IsInRole(userId, role))
                    {
                        userMgr.RemoveFromRole(userId, role);
                    }
                }
            }
        }







    }
}