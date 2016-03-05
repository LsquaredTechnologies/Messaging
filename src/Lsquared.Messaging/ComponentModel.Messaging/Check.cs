using System;
using System.Collections;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Lsquared.ComponentModel.Messaging
{
    /// <summary>
    /// 
    /// </summary>
    [DebuggerStepThrough]
    internal static class Check
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "value"></param>
        /// <param name = "parameterName"></param>
        /// <returns></returns>
        public static T NotNull<T>(T value, string parameterName)where T : class
        {
            if (value == null)
                throw new ArgumentNullException(parameterName);
            Contract.EndContractBlock();
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name = "value"></param>
        /// <param name = "parameterName"></param>
        /// <returns></returns>
        public static string NotEmpty(string value, string parameterName)
        {
            if (value == null)
                throw new ArgumentNullException(parameterName);
            if (value.Length == 0)
                throw new ArgumentException("value cannot be empty", parameterName);
            Contract.EndContractBlock();
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "list"></param>
        /// <param name = "parameterName"></param>
        /// <returns></returns>
        public static T NotEmpty<T>(T list, string parameterName)where T : ICollection
        {
            if (list == null)
                throw new ArgumentNullException(parameterName);
            if (list.Count == 0)
                throw new ArgumentException("list cannot be empty", parameterName);
            Contract.EndContractBlock();
            return list;
        }
    }
}
