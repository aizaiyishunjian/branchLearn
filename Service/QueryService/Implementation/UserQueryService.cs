using IR46.WebHost.Dtos.DisplayDto.User;
using IR46.WebHost.Dtos.InputDto.User;
using IR46.WebHost.Infrastructure;
using IR46.WebHost.Infrastructure.Identity;
using IR46.WebHost.Models.Paging;
using IR46.WebHost.Repository.BaseRepository;
using IR46.WebHost.Service.QueryService.Interface;
using IR46.WebHost.ViewModels.User;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Ninject;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IR46.Domain.Entities;

namespace IR46.WebHost.Service.QueryService.Implementation
{
    public class UserQueryService : IUserQueryService
    {

        [Inject]
        public IQueryRepository<Sys_RoleMenus> RepoSysRoleMenusQueryRepository { get; set; }

        [Inject]
        public IQueryRepository<AspNetUsers> RepoAspNetUsersQueryRepository { get; set; }

        [Inject]
        public IQueryRepository<AspNetRoles> RepoAspNetRolesQueryRepository { get; set; }





        /// <summary>
        /// 通过用户Id获取用户姓名
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public string GetNameByUserId(string userId)
        {
            if (userId == null)
            {
                return null;
            }

            var user = GetUserById(userId);

            if (user == null)
            {
                return null;
            }

            return user.姓名;
        }




        /// <summary>
        /// 通过用户Id获取对应菜单列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public List<Sys_Menus> GetMenusByUserId(string userId)
        {
            var roleList = GetRolesByUserId(userId);

            return GetMenusByRoleList(roleList);
        }



        /// <summary>
        /// 通过角色列表获取菜单列表
        /// </summary>
        /// <param name="roleList">角色列表</param>
        /// <returns></returns>
        public List<Sys_Menus> GetMenusByRoleList(List<IdentityUserRole> roleList)
        {
            var menusList = new List<Sys_Menus>();

            if (roleList != null && roleList.Count > 0)
            {
                foreach (var role in roleList)
                {
                    menusList.AddRange(GetMenusByRoleId(role.RoleId));
                }
            }

            return menusList.Where(c => c.Parameter != null).Distinct().OrderBy(c => c.Rank).ToList();
        }




        /// <summary>
        /// 通过用户Id获取角色信息
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns></returns>
        public List<IdentityUserRole> GetRolesByUserId(string id)
        {
            var user = GetUserById(id);

            return user != null ? user.Roles.ToList() : null;
        }




        /// <summary>
        /// 通过角色Id获取该角色对应的菜单列表
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns></returns>
        public List<Sys_Menus> GetMenusByRoleId(string roleId)
        {
            var roleMenus = RepoSysRoleMenusQueryRepository.Filter(c => c.RoleId == roleId).ToList();

            return roleMenus.Select(roleMenu => roleMenu.Sys_Menus)
                .OrderBy(c => c.Rank).ToList();
        }





        /// <summary>
        /// 通过Id获取用户信息
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        public StoreUser GetUserById(string id)
        {
            var userMgr = GetStoreUserManager();
            return userMgr.FindById(id);
        }





        /// <summary>
        /// 通过查询条件获取用户列表查询结果
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="searchModel">查询条件</param>
        /// <returns></returns>
        public PagingLinkViewModel<DispDtoShowUserOfList> GetUserListSearchResult(int page, int pageSize,
            UserListSearchModel searchModel)
        {

            if (searchModel == null)
            {
                return new PagingLinkViewModel<DispDtoShowUserOfList>();
            }


            var dataList = RepoAspNetUsersQueryRepository.All();

            //按照用户名查找
            if (!Common.Common.IsNull(searchModel.用户名))
            {
                dataList = dataList.Where(user => user.UserName.StartsWith(searchModel.用户名));
            }

            //按照姓名查找
            if (!Common.Common.IsNull(searchModel.姓名))
            {
                dataList = dataList.Where(c => c.姓名 != null && c.姓名.Contains(searchModel.姓名));
            }



            //实例化分页数据
            var result = new PagingLinkViewModel<DispDtoShowUserOfList>
            {
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = dataList.Count()
                }
            };


            //查询结果选择
            dataList = dataList
                .OrderBy(c => c.UserName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            result.DataList = Conversion.Convert<AspNetUsers, DispDtoShowUserOfList>(dataList.ToList());

            return result;
        }





        /// <summary>
        /// 获取全部角色的可选择Dto
        /// </summary>
        /// <returns></returns>
        public List<InputDtoSelectRoleItem> GetAllRolesInputDto(bool isSuperAdmin = false)
        {
            var dataList = RepoAspNetRolesQueryRepository
                .All<AspNetRoles>();

            if (isSuperAdmin == false)
            {
                //非超级管理员
                dataList = dataList.Where(c => c.Rank > 0);
            }

            return dataList.OrderBy(c => c.Rank)
                .Select(role => new InputDtoSelectRoleItem
                {
                    IsSelected = false,
                    RoleId = role.Id,
                    RoleName = role.Name,
                    ShowName = role.ShowName
                }).ToList();
        }




        /// <summary>
        /// 获取全部角色中指定用户角色为true的列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="isSuperAdmin">是否为超级用户</param>
        /// <returns></returns>
        public List<InputDtoSelectRoleItem> GetUserRolesWithAllRolesByUserId(string userId, bool isSuperAdmin = false)
        {
            var allRoles = GetAllRolesInputDto(isSuperAdmin);
            var userRoles = GetRolesByUserId(userId);

            foreach (var userRole in userRoles)
            {
                var role = allRoles.FirstOrDefault(c => c.RoleId == userRole.RoleId);

                if (role != null)
                {
                    role.IsSelected = true;
                }
            }

            return allRoles;
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




    }
}