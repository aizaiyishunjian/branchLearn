using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using IR46.Domain.Entities;
using IR46.WebHost.Dtos.DisplayDto.TestScheme;
using IR46.WebHost.Dtos.InputDto.TestScheme;
using IR46.WebHost.Infrastructure;
using IR46.WebHost.Models.Paging;
using IR46.WebHost.Repository.BaseRepository;
using IR46.WebHost.Service.QueryService.Interface;
using IR46.WebHost.ViewModels.TestScheme;
using Ninject;
using Ninject.Infrastructure.Language;

namespace IR46.WebHost.Service.QueryService.Implementation
{
    public class TestSchemeQueryService:ITestSchemeQueryService
    {
        [Inject]
        public IQueryRepository<Data_TestScheme> RepoTestSchemeQueryRepository { get; set; }
        [Inject]
        public IQueryRepository<Data_TestScheme_Details> RepoTestSchemeDetailsQueryRepository { get; set; } 
        [Inject]
        public IQueryRepository<Data_Sample> RepoDataSampleQueryRepository { get; set; }


        public PagingLinkViewModel<DispDtoTestScheme> GetTestSchemeListSearchResult(int page, int pageSize, TestSchemeListSearchModel searchModel)
        {
            if (searchModel == null)
            {
                return new PagingLinkViewModel<DispDtoTestScheme>();
            }

            var dataList = RepoDataSampleQueryRepository.All();

            if (!Common.Common.IsNull(searchModel.样本编号))
            {
                dataList = dataList.Where(c => c.样本编号 == searchModel.样本编号);
            }
            if (!Common.Common.IsNull(searchModel.检验类别))
            {
                dataList = dataList.Where(c => c.检验类别 == searchModel.检验类别);
            }
            IEnumerable<Data_Sample> sampleList = dataList.ToEnumerable();
            if (!Common.Common.IsNull(searchModel.方案编号))
            {
                sampleList = sampleList.Where(c => c.Data_TestScheme.FirstOrDefault() == null || (c.Data_TestScheme.FirstOrDefault().方案编号 == searchModel.方案编号));
            }
            if (!Common.Common.IsNull(searchModel.执行进度))
            {
                sampleList = sampleList.Where(c => c.Data_TestScheme.FirstOrDefault() == null || c.Data_TestScheme.FirstOrDefault().执行进度 == searchModel.执行进度);
            }
            


            //实例化分页数据
            var result = new PagingLinkViewModel<DispDtoTestScheme>
            {
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = dataList.Count()
                }
            };


            //查询结果选择
            sampleList = sampleList
                .OrderByDescending(c => c.送检日期)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var resultList=new List<DispDtoTestScheme>();
            foreach (var source in dataList.ToList())
            {
                var dto = Conversion.Convert<Data_Sample, DispDtoTestScheme>(source);
                dto.样本Id = source.Id;
                if (source.Data_TestScheme.Any())
                {
                    Conversion.Convert(source.Data_TestScheme.FirstOrDefault(), dto);
                }
                resultList.Add(dto);
            }
            result.DataList = resultList;

            return result;
        }

        public InputDtoTestScheme GetTestSchemeDtoById(string id)
        {
            if (Common.Common.IsNull(id))
            {
                return null;
            }
            var entity = RepoTestSchemeQueryRepository.Find(id);
            if (entity == null)
            {
                return null;
            }

            var dto = Conversion.Convert<Data_TestScheme, InputDtoTestScheme>(entity);
            SetObjectPropertiesBySet(dto,(HashSet<Data_TestScheme_Details>)entity.Data_TestScheme_Details);

            return dto;
        }

        /// <summary>
        /// 根据属性值集合给对象属性赋值
        /// </summary>
        private void SetObjectPropertiesBySet(InputDtoTestScheme dto, HashSet<Data_TestScheme_Details> set)
        {
            PropertyInfo[] pi = dto.GetType().GetProperties();
            foreach (var propertyInfo in pi)
            {
                var propertyValue = set.SingleOrDefault(c => propertyInfo.Name.EndsWith(c.Data_TestItem.测试项编号));
                if (propertyValue != null)
                {
                    propertyInfo.SetValue(dto, true);
                }
                else
                {
                    propertyInfo.SetValue(dto, false);
                }
            }
        }
    }
}