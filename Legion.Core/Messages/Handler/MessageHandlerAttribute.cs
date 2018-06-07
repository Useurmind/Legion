using System.Linq;

namespace Legion.Core.Messages.Handler
{
    /// <summary>
    /// Mark message handlers with this attribute that should be available in the
    /// <see cref="MessageHandlerRegistry"/>.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class MessageHandlerAttribute : System.Attribute
    {
    }
}