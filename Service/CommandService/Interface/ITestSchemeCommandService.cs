using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IR46.WebHost.Dtos.InputDto.TestScheme;

namespace IR46.WebHost.Service.CommandService.Interface
{
    public interface ITestSchemeCommandService
    {
        bool SaveAddTestScheme(InputDtoTestScheme inputDto);
        bool SaveEditTestScheme(InputDtoTestScheme inputDto);
    }
}
