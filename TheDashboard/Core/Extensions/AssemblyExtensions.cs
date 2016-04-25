using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Umbraco.Core;

namespace TheDashboard.Core.Extensions
{
    /// <summary>
    /// Provides extension methods to Assembly
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Safely loads types from a given assembly avoiding errors if certain ones can't be loaded
        /// </summary>
        /// <param name="assembly">Instance of an Assembly</param>
        /// <returns>Enumerable of loadable types</returns>
        /// <remarks>
        /// For further on this and why needed, see: http://haacked.com/archive/2012/07/23/get-all-types-in-an-assembly.aspx/
        /// </remarks>
        public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            Mandate.ParameterNotNull(assembly, "assembly");

            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }
    }
}
