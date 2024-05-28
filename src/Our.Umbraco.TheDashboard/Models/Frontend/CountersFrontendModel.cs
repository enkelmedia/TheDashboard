using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Our.Umbraco.TheDashboard.Counters;

namespace Our.Umbraco.TheDashboard.Models.Frontend
{
    public class CountersFrontendModel
    {
        public CountersFrontendModel()
        {
            Counters = new List<DashboardCounterModel>();
        }

        public List<DashboardCounterModel> Counters { get; set; }
    }
}
