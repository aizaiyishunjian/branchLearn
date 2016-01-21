using AutoMapper;
using IR46.Domain.Entities;
using IR46.WebHost.Dtos.DisplayDto.DataSample;
using IR46.WebHost.Dtos.DisplayDto.LoginRecord;
using IR46.WebHost.Dtos.DisplayDto.TestBed;
using IR46.WebHost.Dtos.DisplayDto.TestScheme;
using IR46.WebHost.Dtos.DisplayDto.User;
using IR46.WebHost.Dtos.InputDto.Sample;
using IR46.WebHost.Dtos.InputDto.TestBed;
using IR46.WebHost.Dtos.InputDto.TestScheme;
using IR46.WebHost.Dtos.InputDto.User;
using IR46.WebHost.Infrastructure.Identity;

namespace IR46.WebHost.Infrastructure
{
    public class AutoMapperConfig
    {

        /// <summary>
        /// 注册使用AutoMapper进行转换的类
        /// </summary>
        public static void RegisterMaps()
        {
            //双向转换
            Register<StoreUser, InputDtoEditUserInfo>();
            Register<Data_TestBed,InputDtoTestBed>();
            Register<Data_Sample,InputDtoDataAddSample>();
            Register<Data_Sample,InputDtoEditSample>();
            Register<Data_TestScheme,InputDtoTestScheme>();
            Register<InputDtoDataAddSample, InputDtoEditSample>();




            //单向转换
            Mapper.CreateMap<Data_LoginRecords, DispDtoShowLoginRecordOfList>();
            Mapper.CreateMap<AspNetUsers, DispDtoShowUserOfList>();
            Mapper.CreateMap<Data_TestBed, DispDtoTestBed>();
            Mapper.CreateMap<Data_TestBed, DispDtoTestBedFunctions>();
            Mapper.CreateMap<Data_TestScheme, DispDtoTestScheme>();
            Mapper.CreateMap<Data_Sample, DispDtoTestScheme>();
            Mapper.CreateMap<Data_Sample,DispDtoShowDataSampleOfList>();

            ////由于两个模型中均存在List类型，为防止转换过程中出错，特配置指定属性不进行转换
            //Mapper.CreateMap<DtoKuangJingModel, Sys_KJ>()
            //    .ForMember(c => c.Sys_KJ_Coal, opt => opt.Ignore())
            //    .ForMember(c => c.Sys_KJ_Unit, opt => opt.Ignore())
            //    .ForMember(c => c.Sys_KJ_ZCD, opt => opt.Ignore());
            //Mapper.CreateMap<Sys_KJ, DtoKuangJingModel>()
            //    .ForMember(c => c.SysCoal, opt => opt.Ignore())
            //    .ForMember(c => c.SysUnits, opt => opt.Ignore())
            //    .ForMember(c => c.SysZcd, opt => opt.Ignore());




        }


        /// <summary>
        /// 注册Dto到Entity的双向转换
        /// </summary>
        /// <typeparam name="TEntity">Entity</typeparam>
        /// <typeparam name="TDto">Dto</typeparam>
        protected static void Register<TEntity, TDto>()
        {
            Mapper.CreateMap<TEntity, TDto>();
            Mapper.CreateMap<TDto, TEntity>();
        }




    }
}