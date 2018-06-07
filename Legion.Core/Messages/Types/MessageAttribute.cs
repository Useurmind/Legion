using System;
using System.Linq;

namespace Legion.Core.Messages.Types
{
    /// <summary>
    /// Apply this attribute to a class representing an message that should be registered
    /// in the <see cref="MessageTypeRegistry"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class MessageAttribute : Attribute
    {

    }
}