using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IR46.WebHost.Dtos.DisplayDto.TestScheme;
using IR46.WebHost.Dtos.InputDto.TestScheme;
using IR46.WebHost.Models.Paging;
using IR46.WebHost.ViewModels.TestScheme;

namespace IR46.WebHost.Service.QueryService.Interface
{
    public interface ITestSchemeQueryService
    {
        PagingLinkViewModel<DispDtoTestScheme> GetTestSchemeListSearchResult(int page, int pageSize, TestSchemeListSearchModel searchModel);
        InputDtoTestScheme GetTestSchemeDtoById(string id);
    }
}
