using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IR46.Domain.Entities;
using IR46.WebHost.Dtos.InputDto.Sample;
using IR46.WebHost.Infrastructure;
using IR46.WebHost.Repository.BaseRepository;
using IR46.WebHost.Service.CommandService.Interface;
using Ninject;

namespace IR46.WebHost.Service.CommandService.Implementation
{
    public class DataSampleCommandService :IDataSampleCommandService
    {
        [Inject]
        public IQueryRepository<Data_Sample> RepoDataSampleQueryRepository { get; set; }

        [Inject]
        public ICommandRepository<Data_Sample> RepoDataSampleCommandRepository { get; set; }




        /// <summary>
        /// 添加一条信息
        /// </summary>
        /// <param name="addSample">添加模型</param>
        /// <returns></returns>
        public bool SaveDataSampleAdd(InputDtoDataAddSample addSample)
        {
            try
            {
                if (addSample == null)
                {
                    throw new Exception("addSample");
                }
                var entity = new Data_Sample();

                Conversion.Convert(addSample, entity);
                //entity.Id = Guid.NewGuid().ToString();
                //entity.产品名称 = 

                return RepoDataSampleCommandRepository.SaveChange();
            }
            catch (Exception)
            {
                return false;
            }
        }



        /// <summary>
        /// 保存编辑的数据
        /// </summary>
        /// <param name="editSample">编辑模型</param>
        /// <returns></returns>
        public bool SaveEditSample(InputDtoEditSample editSample)
        {
            try
            {
                if (editSample == null)
                {
                    throw new Exception("editSample");
                }

                var entity = RepoDataSampleQueryRepository.Find(editSample.Id);
                if (entity == null)
                {
                    return false;
                }

                Conversion.Convert(editSample,entity);

                RepoDataSampleCommandRepository.Create(entity);
                return RepoDataSampleCommandRepository.SaveChange();
            }
            catch (Exception)
            {
                return false;
            }
        }




        /// <summary>
        /// 通过数据Id删除数据
        /// </summary>
        /// <param name="id">数据Id</param>
        /// <returns></returns>
        public bool DeleteDataSampleById(string id)
        {
            if (id == null)
            {
                return false;
            }

            RepoDataSampleCommandRepository.Delete(id);
            return RepoDataSampleCommandRepository.SaveChange();
        }

    }
}