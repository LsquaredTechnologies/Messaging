using System;
using System.Threading.Tasks;

namespace Lsquared.ComponentModel.Messaging
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMessengerService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="delivery"></param>
        /// <param name="filter"></param>
        /// <param name="keepSubscriberReferenceAlive"></param>
        /// <returns></returns>
        RegistrationToken Register<TMessage>([NotNull] Action<TMessage> delivery, [NotNull] Predicate<TMessage> filter, bool keepSubscriberReferenceAlive) where TMessage : class;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        void Unregister([NotNull] RegistrationToken token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recipient"></param>
        void Unregister([NotNull] object recipient);

        /// <summary>
        /// Sends the specified notification to subscribers of the message on the UI thread.
        /// </summary>
        /// <typeparam name = "TMessage">The type of the message.</typeparam>
        /// <param name = "message">The message to send.</param>
        /// <returns>
        /// </returns>
        Task<TMessage> NotifyAsync<TMessage>([NotNull] TMessage message) where TMessage : class;

        /// <summary>
        /// Sends the specified notification to subscribers of the message on a background thread.
        /// </summary>
        /// <typeparam name = "TMessage">The type of the message.</typeparam>
        /// <param name = "message">The message to send.</param>
        /// <returns>
        /// </returns>
        Task<TMessage> BeginNotifyAsync<TMessage>([NotNull] TMessage message) where TMessage : class;
    }
}