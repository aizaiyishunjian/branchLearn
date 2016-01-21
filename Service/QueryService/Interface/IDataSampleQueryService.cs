using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IR46.WebHost.Dtos.DisplayDto.DataSample;
using IR46.WebHost.Dtos.InputDto.Sample;
using IR46.WebHost.Models.Paging;
using IR46.WebHost.ViewModels.DataSample;

namespace IR46.WebHost.Service.QueryService.Interface
{
    public interface IDataSampleQueryService
    {


        PagingLinkViewModel<DispDtoShowDataSampleOfList> GetDataSample(int page, int pageSize,
            DataSampleListSearchModel model);

        InputDtoDataAddSample GetDataSampleById(string sampleId);
        string GetDataSampleNameById(string id);
    }
}
