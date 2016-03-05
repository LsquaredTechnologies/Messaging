using System;

namespace Lsquared.ComponentModel.Messaging
{
    /// <summary>
    /// 
    /// </summary>
    internal interface IMessageSubscription
    {
        /// <summary>
        /// 
        /// </summary>
        RegistrationToken Token { get; }

        /// <summary>
        /// 
        /// </summary>
        WeakReference Recipient { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Action<object> GetExecutionStrategy();
    }
}