using FluentAssertions;
using Moq;
using MyApp.Core.Models;
using MyApp.Core.MyEntities;
using MyApp.Core.MyEntities.Operations;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyApp.Core.Test.MyEntities.Operations
{
    [TestFixture]
    public class MyEntityUpdateOperationTest
    {
        [Test]
        public void MyEntity_Update_Test()
        {
            var id = 1;
            var trans = new TestDataTransaction();
            var loader = new Mock<IMyEntityLoader>();

            loader.Setup(x => x.Get(id)).Returns(new MyEntity
            {
                Name = "Original Name"
            });

            var ctx = new MyEntityUpdateContext(id, e =>
            {
                e.Name = "New Name";
            }, loader.Object, trans);

            var op = new MyEntityUpdateOperation(ctx);
            op.Load();
            op.StageChanges();

            trans.UpdatedEntityOfType<MyEntity>().Count().Should().Be(1);
            ctx.Entity!.Name.Should().Be("New Name");
        }
    }
}