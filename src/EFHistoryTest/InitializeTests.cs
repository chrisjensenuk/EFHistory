using EFHistory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFHistoryTest
{
    [TestClass]
    public class InitializeTests
    {
        [AssemblyInitialize]
        public static void MyTestInitialize(TestContext testContext)
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<TestHistoryDbContext>());
        }
    }
}
