using System;

namespace Lsquared.ComponentModel.Messaging
{
    /// <summary>
    /// 
    /// </summary>
    internal interface IRegistrationInfo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="recipient"></param>
        /// <param name="actionReference"></param>
        /// <param name="filterReference"></param>
        void Register([NotNull] RegistrationToken token, [NotNull] WeakReference recipient, [NotNull]  WeakDelegate actionReference, [NotNull] WeakDelegate filterReference);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recipient"></param>
        void Unregister([NotNull] object recipient);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        void Unregister([NotNull] RegistrationToken token);
    }
}