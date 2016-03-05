using System;
using System.Diagnostics.Contracts;

namespace Lsquared.ComponentModel.Messaging
{
    /// <summary>
    /// 
    /// </summary>
    public static class MessengerServiceExtensions
    {
        public static RegistrationToken Register<TMessage>([NotNull] this IMessengerService messenger, [NotNull] Action<TMessage> delivery) where TMessage : class
        {
            Check.NotNull(messenger, nameof(messenger));
            Check.NotNull(delivery, nameof(delivery));

            return messenger.Register(delivery, (m) => true, true);
        }

        public static RegistrationToken Register<TMessage>([NotNull] this IMessengerService messenger, [NotNull] Action<TMessage> delivery, bool keepSubscriberReferenceAlive) where TMessage : class
        {
            Check.NotNull(messenger, nameof(messenger));
            Check.NotNull(delivery, nameof(delivery));

            return messenger.Register(delivery, (m) => true, keepSubscriberReferenceAlive);
        }

        public static RegistrationToken Register<TMessage>([NotNull] this IMessengerService messenger, [NotNull] Action<TMessage> delivery, [NotNull] Predicate<TMessage> filter) where TMessage : class
        {
            Check.NotNull(messenger, nameof(messenger));
            Check.NotNull(delivery, nameof(delivery));
            Check.NotNull(filter, nameof(filter));

            return messenger.Register(delivery, filter, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="messenger"></param>
        /// <param name="message"></param>
        public static void Notify<TMessage>([NotNull] this IMessengerService messenger, [NotNull] TMessage message) where TMessage : class
        {
            Check.NotNull(messenger, nameof(messenger));

            messenger.NotifyAsync(message).Wait();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="messenger"></param>
        /// <param name="message"></param>
        /// <param name="callback"></param>
        public static void Notify<TMessage>([NotNull] this IMessengerService messenger, [NotNull] TMessage message, [NotNull] Action callback) where TMessage : class
        {
            Check.NotNull(messenger, nameof(messenger));
            Check.NotNull(message, nameof(message));
            Check.NotNull(callback, nameof(callback));

#pragma warning disable CC0031 // Use Invoke Method To call on delegate
            messenger.NotifyAsync(message).ContinueWith(task => callback());
#pragma warning restore CC0031 // Use Invoke Method To call on delegate
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="messenger"></param>
        /// <param name="message"></param>
        /// <param name="callback"></param>
        public static void Notify<TMessage>([NotNull] this IMessengerService messenger, [NotNull] TMessage message, [NotNull] Action<TMessage> callback) where TMessage : class
        {
            Check.NotNull(messenger, nameof(messenger));
            Check.NotNull(message, nameof(message));
            Check.NotNull(callback, nameof(callback));

#pragma warning disable CC0031 // Use Invoke Method To call on delegate
            messenger.NotifyAsync(message).ContinueWith(task => callback(message));
#pragma warning restore CC0031 // Use Invoke Method To call on delegate
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="messenger"></param>
        /// <param name="message"></param>
        public static void BeginNotify<TMessage>([NotNull] this IMessengerService messenger, [NotNull] TMessage message) where TMessage : class
        {
            Check.NotNull(messenger, nameof(messenger));
            Check.NotNull(message, nameof(message));

            messenger.BeginNotifyAsync(message).Wait();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="messenger"></param>
        /// <param name="message"></param>
        /// <param name="callback"></param>
        public static void BeginNotify<TMessage>([NotNull] this IMessengerService messenger, [NotNull] TMessage message, [NotNull] Action callback) where TMessage : class
        {
            Check.NotNull(messenger, nameof(messenger));
            Check.NotNull(message, nameof(message));
            Check.NotNull(callback, nameof(callback));

#pragma warning disable CC0031 // Use Invoke Method To call on delegate
            messenger.BeginNotifyAsync(message).ContinueWith(task => callback());
#pragma warning restore CC0031 // Use Invoke Method To call on delegate
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="messenger"></param>
        /// <param name="message"></param>
        /// <param name="callback"></param>
        public static void BeginNotify<TMessage>([NotNull] this IMessengerService messenger, [NotNull] TMessage message, [NotNull] Action<TMessage> callback) where TMessage : class
        {
            Check.NotNull(messenger, nameof(messenger));
            Check.NotNull(message, nameof(message));
            Check.NotNull(callback, nameof(callback));

#pragma warning disable CC0031 // Use Invoke Method To call on delegate
            messenger.BeginNotifyAsync(message).ContinueWith(task => callback(message));
#pragma warning restore CC0031 // Use Invoke Method To call on delegate
        }
    }
}
