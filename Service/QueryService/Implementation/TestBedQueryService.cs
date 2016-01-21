using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IR46.Domain.Entities;
using IR46.WebHost.Dtos.DisplayDto.TestBed;
using IR46.WebHost.Dtos.InputDto.TestBed;
using IR46.WebHost.Infrastructure;
using IR46.WebHost.Models.Paging;
using IR46.WebHost.Repository.BaseRepository;
using IR46.WebHost.Service.QueryService.Interface;
using IR46.WebHost.ViewModels.TestBed;
using Ninject;

namespace IR46.WebHost.Service.QueryService.Implementation
{
    public class TestBedQueryService:ITestBedQueryService
    {
        [Inject]
        public IQueryRepository<Data_TestBed> RepoTestBedQueryRepository { get; set; }
        [Inject]
        public IQueryRepository<Data_TestItem> RepoTestItemQueryRepository { get; set; }
        [Inject]
        public IQueryRepository<Data_TestBed_Function> RepoTestBedFunctionQueryRepository { get; set; }


        public PagingLinkViewModel<DispDtoTestBed> GetTestBedListSearchResult(int page, int pageSize, TestBedListSearchModel searchModel)
        {
            if (searchModel == null)
            {
                return new PagingLinkViewModel<DispDtoTestBed>();
            }
            var dataList = RepoTestBedQueryRepository.All();

            if (!Common.Common.IsNull(searchModel.检测台编号))
            {
                dataList = dataList.Where(c => c.检测台编号 == searchModel.检测台编号);
            }
            if (!Common.Common.IsNull(searchModel.检测台状态))
            {
                dataList = dataList.Where(c => c.检测台状态 == searchModel.检测台状态);
            }


            //实例化分页数据
            var result = new PagingLinkViewModel<DispDtoTestBed>
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
                .OrderByDescending(c => c.检测台编号)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            result.DataList = Conversion.Convert<Data_TestBed, DispDtoTestBed>(dataList.ToList());

            return result;
        }

        public InputDtoTestBed GetTestBedDtoById(string id)
        {
            if (Common.Common.IsNull(id))
            {
                return null;
            }

            var entity = RepoTestBedQueryRepository.Find(id);

            var dto = Conversion.Convert<Data_TestBed, InputDtoTestBed>(entity);

            SetObjectPropertiesBySet(dto,(HashSet<Data_TestBed_Function>)entity.Data_TestBed_Function);

            return dto;
        }


        public DispDtoTestBedFunctions GetTestBedFunctionsDtoById(string id)
        {
            if (Common.Common.IsNull(id))
            {
                return null;
            }

            var entity = RepoTestBedQueryRepository.Find(id);
            var dto = Conversion.Convert<Data_TestBed, DispDtoTestBedFunctions>(entity);

            dto.Test2=entity.Data_TestBed_Function.Where(c=>c.Data_TestItem.测试项编号.StartsWith("2")).Select(c => c.Data_TestItem.测试项名称).ToArray();
            dto.Test3 = entity.Data_TestBed_Function.Where(c => c.Data_TestItem.测试项编号.StartsWith("3")).Select(c => c.Data_TestItem.测试项名称).ToArray();
            dto.Test4 = entity.Data_TestBed_Function.Where(c => c.Data_TestItem.测试项编号.StartsWith("4")).Select(c => c.Data_TestItem.测试项名称).ToArray();

            return dto;
        }

        
        /// <summary>
        /// 根据属性值集合给对象属性赋值
        /// </summary>
        private void SetObjectPropertiesBySet(InputDtoTestBed dto,HashSet<Data_TestBed_Function> set)
        {
            PropertyInfo[] pi = dto.GetType().GetProperties();
            foreach (var propertyInfo in pi)
            {
                if (!propertyInfo.Name.StartsWith("Test"))
                {
                    continue;
                }
                var propertyValue = set.SingleOrDefault(c => propertyInfo.Name.EndsWith(c.Data_TestItem.测试项编号));
                if (propertyValue != null)
                {
                    propertyInfo.SetValue(dto, true);
                }
                else
                {
                    propertyInfo.SetValue(dto,false);
                }
            }
        }


    }
}