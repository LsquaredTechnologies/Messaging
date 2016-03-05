using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lsquared.ComponentModel.Messaging
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class NotifyContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionStrategies"></param>
        public NotifyContext([NotNull] IList<Action<object>> executionStrategies)
        {
            Check.NotNull(executionStrategies, nameof(executionStrategies));

            _executionStrategies = executionStrategies;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<TMessage> ExecuteAsync<TMessage>([NotNull] TMessage message)
            where TMessage : class
        {
            Check.NotNull(message, nameof(message));

            foreach (var executionStrategy in _executionStrategies)
            {
                executionStrategy(message);
            }
            return await Task.FromResult(message);
        }

        private IList<Action<object>> _executionStrategies;
    }
}