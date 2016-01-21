using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IR46.Domain.Entities;
using IR46.WebHost.Dtos.DisplayDto.DataSample;
using IR46.WebHost.Dtos.InputDto.Sample;
using IR46.WebHost.Infrastructure;
using IR46.WebHost.Models.Paging;
using IR46.WebHost.Repository.BaseRepository;
using IR46.WebHost.Service.QueryService.Interface;
using IR46.WebHost.ViewModels.DataSample;
using Microsoft.Owin.Security.Provider;
using Ninject;

namespace IR46.WebHost.Service.QueryService.Implementation
{
    public class DataSampleQueryService:IDataSampleQueryService
    {
        [Inject]
        public IQueryRepository<Data_Sample> RepoDataSampleQueryRepository { get; set; }



        /// <summary>
        /// 通过获取查询条件查询对应的送检样品列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="model"></param>
        /// <returns></returns>

        public PagingLinkViewModel<DispDtoShowDataSampleOfList> GetDataSample(int page,int pageSize,DataSampleListSearchModel model)
        {
            var sampleList = RepoDataSampleQueryRepository.All();

            //if (CurrentUserCookie.IsManager())
            //{
            //    sampleList = sampleList.Where(c=>c.样本编号 == CurrentUserCookie.)
            //}

            //实例化分页
            var result = new PagingLinkViewModel<DispDtoShowDataSampleOfList>
            {
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = sampleList.Count()
                }
            };


            //查询结果选择
            sampleList = sampleList.OrderBy(c => c.检验日期)
                .Skip((page - 1)*pageSize)
                .Take(pageSize);

            result.DataList = Conversion.Convert<Data_Sample, DispDtoShowDataSampleOfList>(sampleList.ToList());
            return result;
        }




        /// <summary>
        /// 通过样本Id查询
        /// </summary>
        /// <param name="sampleId">样品Id</param>
        /// <returns></returns>
        public InputDtoDataAddSample GetDataSampleById(string sampleId)
        {
            try
            {
                var model = RepoDataSampleQueryRepository.Find(sampleId);
                var entity = Conversion.Convert<Data_Sample, InputDtoDataAddSample>(model);
                return entity;

            }
            catch(Exception)
            {
                return null;
            }
        }




        //通过样本Id获取产品名称
        public string GetDataSampleNameById(string id)
        {
            if (Common.Common.IsNull(id))
            {
                return null;
            }

            var sample = RepoDataSampleQueryRepository.Find(id);
            if (sample == null)
            {
                return null;
            }
            return sample.产品名称;
        }
    }
}