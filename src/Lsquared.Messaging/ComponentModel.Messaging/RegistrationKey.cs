using System;
using System.Diagnostics.Contracts;

namespace Lsquared.ComponentModel.Messaging
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class RegistrationKey : IEquatable<RegistrationKey>
    {
        /// <summary>
        /// 
        /// </summary>
        public Type Type
        {
            get { return _type; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public RegistrationKey([NotNull] Type type)
        {
            _type = Check.NotNull(type, nameof(type));
            _hashCode = _type.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals([NotNull] RegistrationKey other)
        {
            if (_type != other._type) { return false; }
            return true;
        }

        /// <inheritdoc />
        public override bool Equals([CanBeNull] object obj)
        {
            var other = obj as RegistrationKey;
            if (other != null) { return Equals(other); }
            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return _hashCode;
        }

        private readonly int _hashCode;
        private readonly Type _type;
    }
}