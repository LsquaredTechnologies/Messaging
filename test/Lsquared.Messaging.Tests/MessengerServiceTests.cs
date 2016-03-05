using System;
using System.Threading.Tasks;
using Lsquared.ComponentModel.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lsquared.Messaging.Tests
{
    [TestClass]
    public class MessengerServiceTests
    {
        [TestMethod]
        public void Publish_WithPubSub_MessageIsReceived()
        {
            // Arrange
            var messenger = new MessengerService();
            var pub = new Publisher(messenger);
            var sub = new Subscriber(messenger);
            var msg = new Message { Content = "Sent message" };

            // Act
            pub.Publish(msg).Wait();

            // Assert
            Assert.AreEqual("Message received!", msg.Content);
        }

        class Message
        {
            public string Content { get; set; }
        }

        class Publisher
        {
            public Publisher(IMessengerService messenger)
            {
                _messenger = messenger;
            }

            public Task Publish(Message msg)
            {
                return _messenger.NotifyAsync(msg);
            }

            private readonly IMessengerService _messenger;
        }

        class Subscriber
        {
            public Subscriber(IMessengerService messenger)
            {
                messenger.Register<Message>(OnMessageReceived, null, false);
            }

            public void OnMessageReceived(Message msg)
            {
                msg.Content = "Message received!";
            }
        }
    }
}
