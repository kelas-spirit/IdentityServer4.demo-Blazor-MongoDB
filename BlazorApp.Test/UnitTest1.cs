using BlazorApp.Test.Service;
using Services.Services;
using System;
using System.Linq;
using Xunit;

namespace BlazorApp.Test
{
    public class UnitTest1
    {
        private readonly ITestService _testService;
        private readonly IIdServerDemoService _idServerDemoService;
        public UnitTest1(ITestService testService, IIdServerDemoService idServerDemoService)
        {
            _testService = testService;
            _idServerDemoService = idServerDemoService;
        }

        [Fact]
        public async System.Threading.Tasks.Task Test1Async()
        {
            var token = await _idServerDemoService.GetApiToken("m2m","secret");
            Assert.NotEqual(token, string.Empty);
        }
        [Fact]
        public async System.Threading.Tasks.Task Test2Async()
        {
            var token = await _idServerDemoService.GetApiToken("m2m", "secret");
            var customers = await _testService.GetCustomersAsync(token);
            Assert.Equal(customers.Where(x=>x.CompanyName == "name27").Select(x=>x.CompanyName).FirstOrDefault(), "name27");
        }
    }
}
