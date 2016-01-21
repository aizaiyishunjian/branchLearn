using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using IR46.WebHost.Dtos.DisplayDto.TestBed;
using IR46.WebHost.Dtos.InputDto.TestBed;
using IR46.WebHost.Models.Paging;
using IR46.WebHost.ViewModels.TestBed;

namespace IR46.WebHost.Service.QueryService.Interface
{
    public interface ITestBedQueryService
    {
        PagingLinkViewModel<DispDtoTestBed> GetTestBedListSearchResult(int page, int pageSize, TestBedListSearchModel searchModel);
        InputDtoTestBed GetTestBedDtoById(string id);

        DispDtoTestBedFunctions GetTestBedFunctionsDtoById(string id);
    }
}
