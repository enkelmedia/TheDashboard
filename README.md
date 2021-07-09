The Dashboard
=====

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

This is a magic dashboard for Umbraco 7,8 and 9, it's actually "The Dashboard". It will show editors a summary of the latest activites on the website combined with each editors recent activities and other useful information.

## Installation in Umbraco CMS
There is a built version of the package here: https://our.umbraco.org/projects/backoffice-extensions/the-dashboard/ just download it and install it in the developers-section.

Command Line

```
dotnet add package Our.Umbraco.TheDashboard
```

Or NuGet

```
Install-Package Our.Umbraco.TheDashboard
```



## Extension Points
There's something called "Counters" listed to the left in the Dashboard. These are actually small classes that implements the IDashboardCounter-interface from the package. 

These classes are instantiated using Umbraco's CollectionBuilders (same as for ie ContentFinders) so you can inject any dependency you need in the constructor. 

Here's an example:

```csharp
public class OrdersDashboardCounter : IDashboardCounter
{
    private readonly ILocalizedTextService _localizedTextService;

    public OrdersDashboardCounter(ILocalizedTextService localizedTextService)
    {
        _localizedTextService = localizedTextService;
    }

    public DashboardCounterModel GetModel(IScope scope)
    {
        var sql = @"SELECT count(id) FROM customOrders";

        var count = scope.Database.ExecuteScalar<int>(sql);
        return new DashboardCounterModel()
        {
            Text = _localizedTextService.Localize("custom/totalOrders"),
            Count = count,
            ClickUrl = "/umbraco#/orders",
            Style = DashboardCounterModel.CounterStyles.Action
        };
    }
}
```

And then you need to create a Composer that adds you’re new Counter.

```csharp
[ComposeAfter(typeof(TheDashboardComposer))]
public class OrdersCounterComposer : IUserComposer
{
    public void Compose(Composition composition)
    {
        // First, remove the member-counters as we don't need them
        composition.TheDashboardCounters().Remove<MembersTotalDashboardCounter>();
        composition.TheDashboardCounters().Remove<MembersNewLastWeekDashboardCounter>();

        // Add my custom counter
        composition.TheDashboardCounters().Append<OrdersDashboardCounter>();
    }
}
```

## Contributions
Are more then welcome but please, before you put a lot of work into it raise and issue and make sure that we're on the same track.

### Branches

* V1 = Umbraco 7 - 7.7.0
* V2 = Umbraco 7.7.0+
* V8 = Umbraco 8
* V9 = Umbraco 9

### Roadmap
The main idea with the package is to provide a super simple dashboard for the content section, we're extremely cautious with adding features as we want to keep the code base super-easy.

In the v8 version of the package the "Developer Dashboard" was remove, there's plenty of other packages, ie "Diplo GodMode" https://our.umbraco.com/packages/developer-tools/diplo-god-mode/ that solves this problem. There is no plans to add it back. 

### Build
To build a Release-version of the package, 

* [ ] Make sure to update package.build.xml and set `PackageVersion` to the right version.
* [ ] Run `dotnet pack --configuration Release` inside the `Our.Umbraco.TheDashboard`-folder to create packages
* [ ] Artifacts created in /Package/-folder


This package was created by Enkel Media, http://www.enkelmedia.se
