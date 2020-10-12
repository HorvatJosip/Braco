using System;

namespace Braco.Utilities
{
    /// <summary>
    /// Indicates that the decorated property should be
    /// included when performing search.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class SearchAttribute : Attribute
    {
    }
}