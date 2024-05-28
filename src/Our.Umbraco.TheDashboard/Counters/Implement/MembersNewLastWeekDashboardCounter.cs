﻿using System;
using System.Threading;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.Cms.Core.Services;

namespace Our.Umbraco.TheDashboard.Counters.Implement
{
    public class MembersNewLastWeekDashboardCounter : IDashboardCounter
    {
        private readonly ILocalizedTextService _localizedTextService;

        public MembersNewLastWeekDashboardCounter(ILocalizedTextService localizedTextService)
        {
            _localizedTextService = localizedTextService;
        }

        public DashboardCounterModel GetModel(IScope scope)
        {
            var count = scope.Database.ExecuteScalar<int>(@"select COUNT(nodeId) from cmsMember
                   inner join umbracoNode on cmsMember.nodeId = umbraconode.id     
                    WHERE createDate > @0", DateTime.Now.AddDays(-7));

            return new DashboardCounterModel()
            {
                Text = _localizedTextService.Localize("theDashboard","newMembersLastWeek",Thread.CurrentThread.CurrentCulture),
                Count = count,
                ClickUrl = "/umbraco#/member/member/list/all-members",
                Style = DashboardCounterModel.CounterStyles.Selected
            };
        }
    }
}
