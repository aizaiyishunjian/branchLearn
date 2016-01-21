using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IR46.WebHost.Dtos.InputDto.Sample;

namespace IR46.WebHost.Service.CommandService.Interface
{
    public interface IDataSampleCommandService
    {

        //保存添加的一条信息
        bool SaveDataSampleAdd(InputDtoDataAddSample addSample);

        //保存编辑的信息
        bool SaveEditSample(InputDtoEditSample editSample);


        //通过Id删除数据
        bool DeleteDataSampleById(string id);


    }
}
