using System;
using System.Text;

namespace Modules.MHGameWork.TheWizards.Utilities
{
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Get name of type, works for generic types
        /// </summary>
        public static string ToPrettyName(this Type type)
        {
            if (type.IsGenericParameter)
            {
                return type.Name;
            }

            if (!type.IsGenericType)
            {
                return type.Name;
            }

            var builder = new StringBuilder();
            var name = type.Name;
            var index = name.IndexOf("`", StringComparison.Ordinal);
            builder.Append(name.Substring(0, index));
            builder.Append('<');
            var first = true;
            foreach (var arg in type.GetGenericArguments())
            {
                if (!first)
                {
                    builder.Append(',');
                }
                builder.Append(arg.ToPrettyName());
                first = false;
            }
            builder.Append('>');
            return builder.ToString();
        }
    }
}