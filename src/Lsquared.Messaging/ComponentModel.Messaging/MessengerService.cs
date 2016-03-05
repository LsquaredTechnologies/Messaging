using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace Lsquared.ComponentModel.Messaging
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class MessengerService : IMessengerService
    {
        #region Publish methods

        /// <inheritdoc />
        public async Task<TMessage> NotifyAsync<TMessage>([NotNull] TMessage message) where TMessage : class
        {
            Check.NotNull(message, nameof(message));

            var subscriptionInfo = GetMessageSubscriptionInformation<TMessage>() as RegistrationInfo<TMessage>;
            return await subscriptionInfo.NotifyAsync(message);
        }

        /// <inheritdoc />
        public async Task<TMessage> BeginNotifyAsync<TMessage>([NotNull] TMessage message) where TMessage : class
        {
            Check.NotNull(message, nameof(message));


            var subscriptionInfo = GetMessageSubscriptionInformation<TMessage>() as RegistrationInfo<TMessage>;
            return await subscriptionInfo.BeginNotifyAsync(message);
        }

        #endregion

        #region Register / Unregister methods

        public RegistrationToken Register<TMessage>([NotNull] Action<TMessage> delivery, [CanBeNull] Predicate<TMessage> filter, bool keepSubscriberReferenceAlive) where TMessage : class
        {
            Check.NotNull(delivery, nameof(delivery));
            Check.NotNull(filter, nameof(filter));

            return RegisterInternal(delivery, filter, keepSubscriberReferenceAlive);
        }

        public void Unregister([NotNull] object recipient)
        {
            Check.NotNull(recipient, nameof(recipient));

            IRegistrationInfo info;
            foreach (var key in _subscriptions.Keys)
            {
                if (_subscriptions.TryGetValue(key, out info))
                {
                    info.Unregister(recipient);
                }
            }
        }

        public void Unregister([NotNull] RegistrationToken token)
        {
            Check.NotNull(token, nameof(token));

            IRegistrationInfo info;
            foreach (var key in _subscriptions.Keys)
            {
                if (_subscriptions.TryGetValue(key, out info))
                {
                    info.Unregister(token);
                }
            }
        }

        #endregion

        #region Private methods

        private RegistrationToken RegisterInternal<TMessage>([NotNull] Action<TMessage> action, [CanBeNull] Predicate<TMessage> filter, bool keepSubscriberReferenceAlive) where TMessage : class
        {
            Check.NotNull(action, nameof(action));

            WeakDelegate actionReference;
            WeakDelegate filterReference;
            actionReference = new WeakDelegate(action, keepSubscriberReferenceAlive);
            if (filter == null)
            {
                filterReference = new WeakDelegate(new Predicate<TMessage>(delegate { return true; }), true);
            }
            else
            {
                filterReference = new WeakDelegate(filter, keepSubscriberReferenceAlive);
            }

            var token = new RegistrationToken(this, typeof(TMessage));
            var recipient = actionReference.Target.Target == null ? null : new WeakReference(actionReference.Target.Target);
            var subscriptionInfo = GetMessageSubscriptionInformation<TMessage>();
            subscriptionInfo.Register(token, recipient, actionReference, filterReference);
            return token;
        }

        private IRegistrationInfo GetMessageSubscriptionInformation<TMessage>() where TMessage : class
        {
            IRegistrationInfo subscriptionInfo = null;
            lock (_subscriptionsLock)
            {
                var key = new RegistrationKey(typeof(TMessage));
                if (!_subscriptions.TryGetValue(key, out subscriptionInfo))
                {
                    subscriptionInfo = new RegistrationInfo<TMessage>();
                    _subscriptions[key] = subscriptionInfo;
                }
            }

            return subscriptionInfo;
        }

        #endregion

        private readonly object _subscriptionsLock = new object ();
        private readonly ConcurrentDictionary<RegistrationKey, IRegistrationInfo> _subscriptions = new ConcurrentDictionary<RegistrationKey, IRegistrationInfo>();
    }
}