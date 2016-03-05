using System;
using Lsquared.ComponentModel.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lsquared.Messaging.Tests
{
    [TestClass]
    public class PublishMessageContextTests
    {
        [TestMethod]
        public void ExecuteAsync_WithTwoActions_BothActionsAreInvoked()
        {
            // Arrange
            var i = 0;
            Action<object> action1 = (o) => i++;
            Action<object> action2 = (o) => i++;
            var ctx = new NotifyContext(new Action<object>[] { action1, action2 });

            // Act
            var t = ctx.ExecuteAsync("String");
            t.Wait();

            // Assert
            Assert.AreEqual(2, i);
        }
    }
}
