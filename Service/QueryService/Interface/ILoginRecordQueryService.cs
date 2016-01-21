using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IR46.WebHost.Dtos.DisplayDto.LoginRecord;
using IR46.WebHost.Models.Paging;
using IR46.WebHost.ViewModels.LoginRecord;

namespace IR46.WebHost.Service.QueryService.Interface
{
    public interface ILoginRecordQueryService
    {


        /// <summary>
        /// 获取登录日志列表查询结果
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="searchModel">查询条件</param>
        /// <returns></returns>
        PagingLinkViewModel<DispDtoShowLoginRecordOfList> GetLoginRecordListSearchResult(int page, int pageSize,
            LoginRecordListSearchModel searchModel);







    }
}
