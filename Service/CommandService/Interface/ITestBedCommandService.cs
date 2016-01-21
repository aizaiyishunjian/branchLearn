using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IR46.WebHost.Dtos.InputDto.TestBed;

namespace IR46.WebHost.Service.CommandService.Interface
{
    public interface ITestBedCommandService
    {
        bool SaveAddTestBed(InputDtoTestBed inputDto);
        bool SaveEditTestBed(InputDtoTestBed inputDto);
        bool DeleteTestBedById(string id);
    }
}
