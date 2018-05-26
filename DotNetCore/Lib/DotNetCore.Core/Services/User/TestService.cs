using System;
using System.Collections.Generic;
using System.Text;
using DotNetCore.Core.Base.Services.User;

namespace DotNetCore.Core.Services.User
{
    public class TestService: ITestService
    {
        private readonly string mC;
        public TestService(string a,string b)
        {
            mC = a + b;
        }

        public string Get()
        {
            return mC;
        }
    }
}
