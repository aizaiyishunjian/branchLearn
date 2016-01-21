using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IR46.Domain.Entities;
using IR46.WebHost.Dtos.DisplayDto.LoginRecord;
using IR46.WebHost.Infrastructure;
using IR46.WebHost.Models.Paging;
using IR46.WebHost.Repository.BaseRepository;
using IR46.WebHost.Service.QueryService.Interface;
using IR46.WebHost.ViewModels.LoginRecord;
using Ninject;

namespace IR46.WebHost.Service.QueryService.Implementation
{
    public class LoginRecordQueryService : ILoginRecordQueryService
    {
        [Inject]
        public IQueryRepository<Data_LoginRecords> RepoDataLoginRecordsQueryRepository { get; set; }





        /// <summary>
        /// 获取登录日志列表查询结果
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="searchModel">查询条件</param>
        /// <returns></returns>
        public PagingLinkViewModel<DispDtoShowLoginRecordOfList> GetLoginRecordListSearchResult(int page, int pageSize,
            LoginRecordListSearchModel searchModel)
        {
            if (searchModel == null)
            {
                return new PagingLinkViewModel<DispDtoShowLoginRecordOfList>();
            }

            var currentUserId = CurrentUserCookie.GetCurrentUserId();
            var dataList =
                RepoDataLoginRecordsQueryRepository.Filter(
                    c => c.用户Id == currentUserId
                         && c.操作时间 >= searchModel.StartDate
                         && c.操作时间 <= searchModel.EndDate);

            //状态
            if (!Common.Common.IsNull(searchModel.状态))
            {
                dataList = dataList.Where(c => c.操作 == searchModel.状态);
            }



            //实例化分页数据
            var result = new PagingLinkViewModel<DispDtoShowLoginRecordOfList>
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
                .OrderByDescending(c => c.操作时间)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            result.DataList = Conversion.Convert<Data_LoginRecords, DispDtoShowLoginRecordOfList>(dataList.ToList());

            return result;
        }









    }
}