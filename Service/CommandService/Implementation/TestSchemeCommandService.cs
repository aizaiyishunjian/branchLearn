using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using IR46.Domain.Entities;
using IR46.WebHost.Dtos.InputDto.TestScheme;
using IR46.WebHost.Infrastructure;
using IR46.WebHost.Repository.BaseRepository;
using IR46.WebHost.Service.CommandService.Interface;
using Ninject;

namespace IR46.WebHost.Service.CommandService.Implementation
{
    public class TestSchemeCommandService:ITestSchemeCommandService
    {
        [Inject]
        public ICommandRepository<Data_TestScheme> RepoTestSchemeCommandRepository { get; set; }
        [Inject]
        public ICommandRepository<Data_TestScheme_Details> RepoTestSchemeDetailsCommandRepository { get; set; }
        [Inject]
        public IQueryRepository<Data_TestScheme> RepoTestSchemeQueryRepository { get; set; }
        [Inject]
        public IQueryRepository<Data_TestScheme_Details> RepoTestSchemeDetailsQueryRepository { get; set; }
        [Inject]
        public IQueryRepository<Data_TestItem> RepoTestItemQueryRepository { get; set; }


        public bool SaveAddTestScheme(InputDtoTestScheme inputDto)
        {
            if (inputDto == null)
            {
                return false;
            }

            var entity = Conversion.Convert<InputDtoTestScheme, Data_TestScheme>(inputDto);
            entity.Id = Guid.NewGuid().ToString();
            entity.Data_TestScheme_Details = GenFunctionSetByProperties(inputDto, entity.Id);

            RepoTestSchemeCommandRepository.Create(entity);
            return RepoTestSchemeCommandRepository.SaveChange();
        }


        public bool SaveEditTestScheme(InputDtoTestScheme inputDto)
        {
            if (inputDto == null)
            {
                return false;
            }
            var testScheme = RepoTestSchemeQueryRepository.Find(inputDto.Id);
            if (testScheme == null)
            {
                return false;
            }

            Conversion.Convert(inputDto,testScheme);
            var newDetailsSet = GenFunctionSetByProperties(inputDto, testScheme.Id);
            var oldDetailsSet = (HashSet<Data_TestScheme_Details>) testScheme.Data_TestScheme_Details;

            var newDetailsSetBak=new HashSet<Data_TestScheme_Details>();
            newDetailsSetBak.UnionWith(newDetailsSet);

            newDetailsSet =
                (HashSet<Data_TestScheme_Details>) newDetailsSet.Except(oldDetailsSet, new TestSchemeDetailsComparer());
            oldDetailsSet =
                (HashSet<Data_TestScheme_Details>)
                    oldDetailsSet.Except(newDetailsSetBak, new TestSchemeDetailsComparer());

            foreach (var dataTestSchemeDetailse in oldDetailsSet)
            {
                RepoTestSchemeDetailsCommandRepository.Delete(dataTestSchemeDetailse.Id);
            }

            foreach (var dataTestSchemeDetailse in newDetailsSet)
            {
                RepoTestSchemeDetailsCommandRepository.Create(dataTestSchemeDetailse);
            }

            return RepoTestSchemeCommandRepository.SaveChange();
        }


        private class TestSchemeDetailsComparer : IEqualityComparer<Data_TestScheme_Details>
        {
            public bool Equals(Data_TestScheme_Details x, Data_TestScheme_Details y)
            {
                return x.检测项Id.Equals(y.检测项Id);
            }

            public int GetHashCode(Data_TestScheme_Details obj)
            {
                return obj.检测项Id.GetHashCode();
            }
        }


        private HashSet<Data_TestScheme_Details> GenFunctionSetByProperties(InputDtoTestScheme inputDto, string testBedId)
        {
            var testBedFunctionSet = new HashSet<Data_TestScheme_Details>();

            PropertyInfo[] pi = inputDto.GetType().GetProperties();
            foreach (var propertyInfo in pi)
            {
                if (propertyInfo.Name.StartsWith("Test") && (bool)propertyInfo.GetValue(inputDto))
                {
                    var testItemNo = propertyInfo.Name.Substring(4);
                    var function = new Data_TestScheme_Details();
                    function.Id = Guid.NewGuid().ToString();
                    function.检测台Id = testBedId;
                    function.检测项Id =
                        RepoTestItemQueryRepository.Filter(c => c.测试项编号 == testItemNo).Single().Id;
                    testBedFunctionSet.Add(function);
                }
            }

            return testBedFunctionSet;
        }
    }
}