using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lsquared.ComponentModel.Messaging
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name = "TMessage"></typeparam>
    internal class RegistrationInfo<TMessage> : IRegistrationInfo where TMessage : class
    {
        /// <inheritdoc />
        public virtual async Task<TMessage> NotifyAsync([NotNull] TMessage message)
        {
            Check.NotNull(message, nameof(message));

            var context = CreateContext();
            return await context.ExecuteAsync(message);
        }

        /// <inheritdoc />
        public virtual async Task<TMessage> BeginNotifyAsync([NotNull] TMessage message)
        {
            Check.NotNull(message, nameof(message));

            return await Task.Run(async () =>
            {
                var context = CreateContext();
                return await context.ExecuteAsync(message);
            });
        }

        /// <inheritdoc />
        public void Register([NotNull] RegistrationToken token, [NotNull] WeakReference recipient, [NotNull] WeakDelegate actionReference, [NotNull] WeakDelegate filterReference)
        {
            Check.NotNull(token, nameof(token));
            Check.NotNull(recipient, nameof(recipient));
            Check.NotNull(actionReference, nameof(actionReference));
            Check.NotNull(filterReference, nameof(filterReference));

            IMessageSubscription subscription = null;
            subscription = new MessageSubscription<TMessage>(token, recipient, actionReference, filterReference);
            RegisterInternal(subscription);
        }

        /// <inheritdoc />
        public void Unregister([NotNull] object recipient)
        {
            Check.NotNull(recipient, nameof(recipient));

            for (var i = _subscriptions.Count - 1 ; i >= 0 ; i--)
            {
                var subscription = _subscriptions[i];
                if (ReferenceEquals(subscription.Recipient.Target, recipient))
                {
                    _subscriptions.RemoveAt(i);
                }
            }
        }

        /// <inheritdoc />
        public void Unregister([NotNull] RegistrationToken token)
        {
            Check.NotNull(token, nameof(token));

            for (var i = _subscriptions.Count - 1 ; i >= 0 ; i--)
            {
                var subscription = _subscriptions[i];
                if (ReferenceEquals(subscription.Token, token))
                {
                    _subscriptions.RemoveAt(i);
                }
            }
        }

        private NotifyContext CreateContext()
        {
            return new NotifyContext(PruneAndReturnStrategies());
        }

        private void RegisterInternal([NotNull] IMessageSubscription subscription)
        {
            Check.NotNull(subscription, nameof(subscription));

            lock (_subscriptions)
            {
                _subscriptions.Add(subscription);
            }
        }

        private IList<Action<object>> PruneAndReturnStrategies()
        {
            var result = new List<Action<object>>();
            lock (_subscriptions)
            {
                for (var i = _subscriptions.Count - 1 ; i >= 0 ; i--)
                {
                    var item = _subscriptions[i].GetExecutionStrategy();
                    if (item == null)
                    {
                        // Prune from main list. Log?
                        _subscriptions.RemoveAt(i);
                    }
                    else
                    {
                        result.Add(item);
                    }
                }
            }

            return result;
        }
                
        private readonly IList<IMessageSubscription> _subscriptions = new List<IMessageSubscription>();
    }
}