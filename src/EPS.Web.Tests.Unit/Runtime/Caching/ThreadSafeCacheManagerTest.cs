using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EPS.Runtime.Caching;
using FakeItEasy;
using Xunit;

namespace EPS.Runtime.Caching.Tests.Unit
{
    public interface IThreadSafeCacheManagerTest
    {
        void DoSomething();
    }
    public class ThreadSafeCacheManagerTest : IThreadSafeCacheManagerTest
    {
        [Fact(Skip = "An exhaustive suite of tests needs to be implemented on ThreadSafeCacheManager")]
        public void DoSomething()
        {
            var cacheManager = ThreadSafeCacheManager<object>.Construct("myObjectCache", TimeSpan.MaxValue);            
        }
    }
}
