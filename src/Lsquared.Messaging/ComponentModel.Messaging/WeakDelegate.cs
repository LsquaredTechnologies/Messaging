#if CORECLR
using System;
using System.Reflection;
#else
using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Reflection;
#endif


namespace Lsquared.ComponentModel.Messaging
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class WeakDelegate
    {
        /// <summary>
        /// 
        /// </summary>
        public Delegate Target
        {
            get { return _delegate != null ? _delegate : TryGetDelegate(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delegate"></param>
        /// <param name="keepReferenceAlive"></param>
#if !CORECLR
        [ExcludeFromCodeCoverage]
#endif
        public WeakDelegate([NotNull] Delegate @delegate, bool keepReferenceAlive)
        {
            Check.NotNull(@delegate, nameof(@delegate));

            if (keepReferenceAlive)
            {
                _delegate = @delegate;
            }
            else
            {
                _weakReference = new WeakReference(@delegate.Target);
                _method = @delegate.GetMethodInfo();
                _delegateType = @delegate.GetType();
            }
        }

#if !CORECLR
        [ExcludeFromCodeCoverage]
#endif
        private Delegate TryGetDelegate()
        {
            Delegate result = null;

            if (_method.IsStatic)
            {
                // Creates a delegate on a static method.
                result = _method.CreateDelegate(_delegateType);
            }
            else
            {
                var target = _weakReference.Target;
                if (target != null)
                {
                    // Creates a delegate on an instance method.
                    result = _method.CreateDelegate(_delegateType, target);
                }
            }

            // No delegate found.
            return result;
        }

        // if keepReferenceAlive is true:
        private readonly Delegate _delegate;
        // else:
        private readonly Type _delegateType;
        private readonly MethodInfo _method;
        private readonly WeakReference _weakReference;
    }
}
