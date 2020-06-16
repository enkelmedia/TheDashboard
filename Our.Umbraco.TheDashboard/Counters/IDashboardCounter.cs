using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Scoping;

namespace Our.Umbraco.TheDashboard.Counters
{
    public interface IDashboardCounter
    {
        /// <summary>
        /// Returns a <see cref="DashboardCounterModel"/> for the current counter.
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        DashboardCounterModel GetModel(IScope scope);
    }
}
