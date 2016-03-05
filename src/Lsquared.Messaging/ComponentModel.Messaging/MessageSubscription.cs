using System;
using System.Diagnostics.Contracts;

namespace Lsquared.ComponentModel.Messaging
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name = "TMessage"></typeparam>
    internal sealed class MessageSubscription<TMessage> : IMessageSubscription where TMessage : class
    {
        /// <inheritdoc/>
        public RegistrationToken Token
        {
            get { return _token; }
        }

        /// <inheritdoc/>
        public WeakReference Recipient
        {
            get { return _recipient; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Action<TMessage> Action
        {
            get { return (Action<TMessage>)_deliveryReference.Target; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Predicate<TMessage> Filter
        {
            get { return (Predicate<TMessage>)_filterReference.Target; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name = "token"></param>
        /// <param name = "recipient"></param>
        /// <param name = "deliveryReference"></param>
        /// <param name = "filterReference"></param>
        public MessageSubscription([NotNull] RegistrationToken token, [NotNull] WeakReference recipient, [NotNull] WeakDelegate deliveryReference, [NotNull] WeakDelegate filterReference)
        {
            Check.NotNull(token, nameof(token));
            Check.NotNull(recipient, nameof(recipient));
            Check.NotNull(deliveryReference, nameof(deliveryReference));
            Check.NotNull(filterReference, nameof(filterReference));

            _recipient = recipient;
            _token = token;
            _deliveryReference = deliveryReference;
            _filterReference = filterReference;
        }

        /// <inheritdoc/>
        public Action<object> GetExecutionStrategy()
        {
            var action = Action;
            var filter = Filter;
            if (action != null && filter != null)
            {
                return (arg) =>
                {
                    var message = arg as TMessage;
                    if (filter(message))
                    {
                        Check.NotNull(action, nameof(action));
                        action(message);
                    }
                };
            }

            return null;
        }

        private readonly WeakReference _recipient;
        private readonly WeakDelegate _deliveryReference;
        private readonly WeakDelegate _filterReference;
        private readonly RegistrationToken _token;
    }
}