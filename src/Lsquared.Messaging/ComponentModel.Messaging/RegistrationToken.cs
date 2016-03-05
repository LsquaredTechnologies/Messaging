using System;
using System.Diagnostics.Contracts;

namespace Lsquared.ComponentModel.Messaging
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class RegistrationToken : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="messenger"></param>
        /// <param name="messageType"></param>
        public RegistrationToken([NotNull] IMessengerService messenger, [NotNull] Type messageType)
        {
            _messengerWRef = new WeakReference(Check.NotNull(messenger, nameof(messenger)));
            _type = Check.NotNull(messageType, nameof(messageType));
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (_messengerWRef.IsAlive)
            {
                var service = _messengerWRef.Target as IMessengerService;
                if (service != null)
                {
                    service.Unregister(this);
                }
            }

            GC.SuppressFinalize(this);
        }

        private WeakReference _messengerWRef;
        private Type _type;
    }
}