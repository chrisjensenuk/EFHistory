using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using EFHistory;
using System.Collections.Generic;
using System.Linq;

namespace EFHistoryTest
{
    [TestClass]
    public class HistoryRepositoryTests
    {
        [TestClass]
        public class InsertTests
        {
            [TestMethod]
            public void InsertNewEntity()
            {
                //Arrange
                var msg = new Message { Msg = "InsertNewEntity" };

                //Unit Of Work
                using (var context = new TestHistoryDbContext("userName"))
                {
                    //Generic Repository
                    var messageRep = new HistoryRepository<Message>(context);

                    //Act
                    messageRep.Insert(msg);
                    context.SaveChanges();

                    //Assert
                    //If we got here then nothing errored
                }
            }

        }

        [TestClass]
        public class UpdateTests
        {

            [TestMethod]
            public void UpdateEntity()
            {
                //Arrange
                var msg = new Message { Msg = "UpdateEntity" };

                //Unit Of Work
                using (var context = new TestHistoryDbContext("userName"))
                {
                    //Generic Repository
                    var messageRep = new HistoryRepository<Message>(context);
                    messageRep.Insert(msg);
                    context.SaveChanges();
                    var id = msg.Id;
                    
                    //Act
                    msg.Msg = "UpdateEntity2";
                    context.SaveChanges();

                    //Assert
                    var foundMsg = messageRep.Find(id);
                    Assert.AreEqual("UpdateEntity2", foundMsg.Msg);
                }
            }

            [TestMethod]
            public void UpdateEntityEnsureHistoryRemains()
            {
                //Arrange
                var msg = new Message { Msg = "UpdateEntityEnsureHistoryRemains" };

                //Unit Of Work
                using (var context = new TestHistoryDbContext("userName"))
                {
                    //Generic Repository
                    var messageRep = new HistoryRepository<Message>(context);
                    messageRep.Insert(msg);
                    context.SaveChanges();
                    var id = msg.Id;

                    //Act
                    msg.Msg = "UpdateEntityEnsureHistoryRemains2";
                    context.SaveChanges();
                    var msgs = messageRep.All().Where(m => m.Id == id).OrderBy(m => m.Version).ToList();

                    //Assert
                    Assert.IsTrue(msgs.Count == 2);
                    Assert.AreEqual("UpdateEntityEnsureHistoryRemains", msgs[0].Msg);
                    Assert.AreEqual("UpdateEntityEnsureHistoryRemains2", msgs[1].Msg);
                }
            }

        }

        [TestClass]
        public class DeleteTests
        {

            [TestMethod]
            public void DeleteEntity()
            {
                //Arrange
                var msg = new Message { Msg = "DeleteEntity" };

                //Unit Of Work
                using (var context = new TestHistoryDbContext("userName"))
                {
                    //Generic Repository
                    var messageRep = new HistoryRepository<Message>(context);
                    messageRep.Insert(msg);
                    context.SaveChanges();
                    var id = msg.Id;

                    //Act
                    messageRep.Delete(msg);
                    context.SaveChanges();

                    var foundMsg = messageRep.Find(id);
                    Assert.IsNull(foundMsg);

                    foundMsg = messageRep.CurrentVersions().Where(m => m.Id == id).SingleOrDefault();
                    Assert.IsNull(foundMsg);
                }


            }

        }

        
    }
}
