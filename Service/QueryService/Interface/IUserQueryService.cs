using IR46.WebHost.Dtos.DisplayDto.User;
using IR46.WebHost.Dtos.InputDto.User;
using IR46.WebHost.Infrastructure.Identity;
using IR46.WebHost.Models.Paging;
using IR46.WebHost.ViewModels.User;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using IR46.Domain.Entities;

namespace IR46.WebHost.Service.QueryService.Interface
{
    public interface IUserQueryService
    {

        /// <summary>
        /// 通过用户Id获取用户姓名
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        string GetNameByUserId(string userId);


        /// <summary>
        /// 通过用户Id获取对应菜单列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        List<Sys_Menus> GetMenusByUserId(string userId);



        /// <summary>
        /// 通过角色列表获取菜单列表
        /// </summary>
        /// <param name="roleList">角色列表</param>
        /// <returns></returns>
        List<Sys_Menus> GetMenusByRoleList(List<IdentityUserRole> roleList);



        /// <summary>
        /// 通过用户Id获取角色信息
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns></returns>
        List<IdentityUserRole> GetRolesByUserId(string id);



        /// <summary>
        /// 通过Id获取用户信息
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        StoreUser GetUserById(string id);



        /// <summary>
        /// 通过角色Id获取该角色对应的菜单列表
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns></returns>
        List<Sys_Menus> GetMenusByRoleId(string roleId);




        /// <summary>
        /// 通过查询条件获取用户列表查询结果
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="searchModel">查询条件</param>
        /// <returns></returns>
        PagingLinkViewModel<DispDtoShowUserOfList> GetUserListSearchResult(int page, int pageSize,
            UserListSearchModel searchModel);




        /// <summary>
        /// 获取全部角色的可选择Dto
        /// </summary>
        /// <returns></returns>
        List<InputDtoSelectRoleItem> GetAllRolesInputDto(bool isSuperAdmin = false);




        /// <summary>
        /// 获取全部角色中指定用户角色为true的列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="isSuperAdmin">是否为超级用户</param>
        /// <returns></returns>
        List<InputDtoSelectRoleItem> GetUserRolesWithAllRolesByUserId(string userId, bool isSuperAdmin = false);







    }
}
