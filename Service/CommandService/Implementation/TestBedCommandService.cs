using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using IR46.Domain.Entities;
using IR46.WebHost.Dtos.InputDto.TestBed;
using IR46.WebHost.Infrastructure;
using IR46.WebHost.Repository.BaseRepository;
using IR46.WebHost.Service.CommandService.Interface;
using log4net.Util;
using Ninject;

namespace IR46.WebHost.Service.CommandService.Implementation
{
    public class TestBedCommandService:ITestBedCommandService
    {
        [Inject]
        public ICommandRepository<Data_TestBed> RepoTestBedCommandRepository { get; set; }
        [Inject]
        public ICommandRepository<Data_TestBed_Function> RepoTestBedFunctionCommandRepository { get; set; }
        [Inject]
        public IQueryRepository<Data_TestItem> RepoTestItemQueryRepository { get; set; } 
        [Inject]
        public IQueryRepository<Data_TestBed> RepoTestBedQueryRepository { get; set; } 
 


        public bool SaveAddTestBed(InputDtoTestBed inputDto)
        {
            if (inputDto == null)
            {
                return false;
            }
            
            var testBed=new Data_TestBed();
            Conversion.Convert(inputDto, testBed);
            testBed.Id = Guid.NewGuid().ToString();
            testBed.Data_TestBed_Function = GenFunctionSetByProperties(inputDto, testBed.Id);

            RepoTestBedCommandRepository.Create(testBed);
            return RepoTestBedCommandRepository.SaveChange();
        }



        public bool SaveEditTestBed(InputDtoTestBed inputDto)
        {
            if (inputDto == null)
            {
                return false;
            }
            var testBed = RepoTestBedQueryRepository.Find(inputDto.Id);
            if (testBed == null)
            {
                return false;
            }

            Conversion.Convert(inputDto,testBed);
            var newFunctionSet = GenFunctionSetByProperties(inputDto, testBed.Id);
            var oldFunctionSet = (HashSet<Data_TestBed_Function>)testBed.Data_TestBed_Function;


            var oldFunctionSetBak=new HashSet<Data_TestBed_Function>();
            oldFunctionSetBak.UnionWith(oldFunctionSet);
            var newFunctionSetBak=new HashSet<Data_TestBed_Function>();
            newFunctionSetBak.UnionWith(newFunctionSet);


            var addFunctionSet=newFunctionSetBak.Except(oldFunctionSetBak, new TestBedFunctionComparer());
            var deleteFunctionSet =oldFunctionSetBak.Except(newFunctionSetBak, new TestBedFunctionComparer());


            foreach (var dataTestBedFunction in addFunctionSet)
            {
                testBed.Data_TestBed_Function.Add(dataTestBedFunction);
                RepoTestBedFunctionCommandRepository.Create(dataTestBedFunction);
            }
            foreach (var dataTestBedFunction in deleteFunctionSet)
            {
                testBed.Data_TestBed_Function.Remove(dataTestBedFunction);
                RepoTestBedFunctionCommandRepository.Delete(dataTestBedFunction.Id);
            }

            
            //testBed.Data_TestBed_Function = oldFunctionSet;

            return RepoTestBedCommandRepository.SaveChange();
        }


        public bool DeleteTestBedById(string id)
        {
            if (Common.Common.IsNull(id))
            {
                return false;
            }

            var entity = RepoTestBedQueryRepository.Find(id);
            if (entity == null)
            {
                return true;
            }

            RepoTestBedCommandRepository.Delete(entity);

            return RepoTestBedCommandRepository.SaveChange();
        }



        private class TestBedFunctionComparer:IEqualityComparer<Data_TestBed_Function>
        {
            public bool Equals(Data_TestBed_Function x, Data_TestBed_Function y)
            {
                return x.检测项Id.Equals(y.检测项Id);
            }

            public int GetHashCode(Data_TestBed_Function obj)
            {
                return obj.检测项Id.GetHashCode();
            }
        }


        private HashSet<Data_TestBed_Function> GenFunctionSetByProperties(InputDtoTestBed inputDto,string testBedId)
        {
            var testBedFunctionSet=new HashSet<Data_TestBed_Function>();

            PropertyInfo[] pi = inputDto.GetType().GetProperties();
            foreach (var propertyInfo in pi)
            {
                if (propertyInfo.Name.StartsWith("Test")&&(bool)propertyInfo.GetValue(inputDto))
                {
                    var testItemNo = propertyInfo.Name.Substring(4);
                    var function=new Data_TestBed_Function();
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