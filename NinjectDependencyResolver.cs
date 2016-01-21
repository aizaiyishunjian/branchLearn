using IR46.WebHost.Repository.BaseRepository;
using IR46.WebHost.Repository.CommandRepository;
using IR46.WebHost.Repository.QueryRepository;
using IR46.WebHost.Service.CommandService.Implementation;
using IR46.WebHost.Service.CommandService.Interface;
using IR46.WebHost.Service.QueryService.Implementation;
using IR46.WebHost.Service.QueryService.Interface;
using Ninject;
using Ninject.Web.Common;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using IR46.Domain.Entities;


namespace IR46.WebHost.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;


        public NinjectDependencyResolver(IKernel kernelParam)
        {
            _kernel = kernelParam;

            AddBindings();
        }



        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }



        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }



        private void AddBindings()
        {
            //---------------------------------------

            //Repository

            //-AspNetRoles-
            _kernel.Bind<IQueryRepository<AspNetRoles>>().To<DbMaitainSysQueryRepository<AspNetRoles>>();
            _kernel.Bind<ICommandRepository<AspNetRoles>>().To<DbMaitainSysCommandRepository<AspNetRoles>>();

            //-AspNetUsers-
            _kernel.Bind<IQueryRepository<AspNetUsers>>().To<DbMaitainSysQueryRepository<AspNetUsers>>();
            _kernel.Bind<ICommandRepository<AspNetUsers>>().To<DbMaitainSysCommandRepository<AspNetUsers>>();

            //-Data_LoginRecords-
            _kernel.Bind<IQueryRepository<Data_LoginRecords>>().To<DbMaitainSysQueryRepository<Data_LoginRecords>>();
            _kernel.Bind<ICommandRepository<Data_LoginRecords>>().To<DbMaitainSysCommandRepository<Data_LoginRecords>>();

            //-Sys_Menus-
            _kernel.Bind<IQueryRepository<Sys_Menus>>().To<DbMaitainSysQueryRepository<Sys_Menus>>();
            _kernel.Bind<ICommandRepository<Sys_Menus>>().To<DbMaitainSysCommandRepository<Sys_Menus>>();

            //-Sys_RoleMenus-
            _kernel.Bind<IQueryRepository<Sys_RoleMenus>>().To<DbMaitainSysQueryRepository<Sys_RoleMenus>>();
            _kernel.Bind<ICommandRepository<Sys_RoleMenus>>().To<DbMaitainSysCommandRepository<Sys_RoleMenus>>();

            //-Data_Sample-
            _kernel.Bind<IQueryRepository<Data_Sample>>().To<DbMaitainSysQueryRepository<Data_Sample>>();
            _kernel.Bind<ICommandRepository<Data_Sample>>().To<DbMaitainSysCommandRepository<Data_Sample>>();

            //-Data_ElectricMeter-
            _kernel.Bind<IQueryRepository<Data_ElectricMeter>>().To<DbMaitainSysQueryRepository<Data_ElectricMeter>>();
            _kernel.Bind<ICommandRepository<Data_ElectricMeter>>()
                .To<DbMaitainSysCommandRepository<Data_ElectricMeter>>();

            //-Data_TestBed
            _kernel.Bind<IQueryRepository<Data_TestBed>>().To<DbMaitainSysQueryRepository<Data_TestBed>>();
            _kernel.Bind<ICommandRepository<Data_TestBed>>().To<DbMaitainSysCommandRepository<Data_TestBed>>();

            //-Data_TestBed_Function
            _kernel.Bind<IQueryRepository<Data_TestBed_Function>>()
                .To<DbMaitainSysQueryRepository<Data_TestBed_Function>>();
            _kernel.Bind<ICommandRepository<Data_TestBed_Function>>()
                .To<DbMaitainSysCommandRepository<Data_TestBed_Function>>();

            //-Data_TestItem
            _kernel.Bind<IQueryRepository<Data_TestItem>>().To<DbMaitainSysQueryRepository<Data_TestItem>>();
            _kernel.Bind<ICommandRepository<Data_TestItem>>().To<DbMaitainSysCommandRepository<Data_TestItem>>();

            //-Data_TestScheme
            _kernel.Bind<IQueryRepository<Data_TestScheme>>().To<DbMaitainSysQueryRepository<Data_TestScheme>>();
            _kernel.Bind<ICommandRepository<Data_TestScheme>>().To<DbMaitainSysCommandRepository<Data_TestScheme>>();

            //-Data_TestScheme_Details
            _kernel.Bind<IQueryRepository<Data_TestScheme_Details>>()
                .To<DbMaitainSysQueryRepository<Data_TestScheme_Details>>();
            _kernel.Bind<ICommandRepository<Data_TestScheme_Details>>()
                .To<DbMaitainSysCommandRepository<Data_TestScheme_Details>>();


            //HotFix
            _kernel.Bind<ICommandRepository<Data_TestScheme_Details>>()
                .To<DbMaitainSysCommandRepository<Data_TestScheme_Details>>();        

            //---------------------------------------

            //Service

            //-LoginService-
            _kernel.Bind<ILoginCommandService>().To<LoginCommandService>();
            _kernel.Bind<ILoginRecordQueryService>().To<LoginRecordQueryService>();

            //-UserService-
            _kernel.Bind<IUserQueryService>().To<UserQueryService>();
            _kernel.Bind<IUserCommandService>().To<UserCommandService>();

            //-Data_Sample-
            _kernel.Bind<IDataSampleQueryService>().To<DataSampleQueryService>();
            _kernel.Bind<IDataSampleCommandService>().To<DataSampleCommandService>();
            //-Data_ElectricMeter-

            //-Data_TestBed
            _kernel.Bind<ITestBedQueryService>().To<TestBedQueryService>();
            _kernel.Bind<ITestBedCommandService>().To<TestBedCommandService>();

            //-Data_TestScheme
            _kernel.Bind<ITestSchemeQueryService>().To<TestSchemeQueryService>();
            _kernel.Bind<ITestSchemeCommandService>().To<TestSchemeCommandService>();


            //---------------------------------------

            //数据库
            _kernel.Bind<IR46Entities>().ToSelf().InRequestScope();


            //---------------------------------------
        }
    }
}