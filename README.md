The Dashboard
=====

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![NuGet version (Our.Umbraco.TheDashboard)](https://img.shields.io/nuget/v/Our.Umbraco.TheDashboard.svg?style=flat-square)](https://www.nuget.org/packages/Our.Umbraco.TheDashboard/)

This is a magic dashboard for Umbraco CMS, it's actually "The Dashboard". It will show editors a summary of the latest activites on the website combined with each editor's recent activities and other useful information.

![The Dasboard Screenshot](documentation/the-dashboard-screenshot.png)

## Installation in Umbraco CMS
For Umbraco 7 & 8, there is a built version of the package here: https://our.umbraco.org/projects/backoffice-extensions/the-dashboard/ — download it and install it in the Developer section.

For Umbraco 9 and later use the following:

**Command Line**

```
dotnet add package Our.Umbraco.TheDashboard
```

**Or NuGet**

```
Install-Package Our.Umbraco.TheDashboard
```

## Dashboard Content

## Recent Activities
Shows a list of all recent activities in the CMS by both the current user and other users. Only shows content that the users have access to.

## Your recent activity
Shows a list of recent nodes that the current user have interacted with.

## Pending content
Shows content that is published but have an unscheduled change, this mostly happens when editors press the "Save and Preview" button without publishing the changes afterwards and when this icon is shown:

![image](https://user-images.githubusercontent.com/1782524/132513572-0d8f6b01-2422-427c-b555-aa56b49f64ab.png)


## Extension Points
There's something called "Counters" listed to the right in the Dashboard. These are actually small classes that implements the `IDashboardCounter` interface from the package.

These classes are instantiated using Umbraco's `CollectionBuilders` (same as i.e. `ContentFinders`) so you can inject any dependency you need in the constructor.

Here's an example:

```csharp
public class OrdersDashboardCounter : IDashboardCounter
{
    private readonly ISomeService _someService;

    public OrdersDashboardCounter(ISomeService _someService)
    {
    }

    public DashboardCounterModel GetModel(IScope scope)
    {
        var sql = @"SELECT count(id) FROM customOrders";

        var count = scope.Database.ExecuteScalar<int>(sql);

        return new DashboardCounterModel()
        {
            LocalizationKey = "custom_totalOrders", //Must be loaded into the backoffice
            Count = count,
            ClickUrl = "/umbraco#/orders",
            Style = DashboardCounterModel.CounterStyles.Action
        };
    }
}
```

And then you need to create a `Composer` that adds your new counter:

```csharp
[ComposeAfter(typeof(TheDashboardComposer))]
public class OrdersCounterComposer : IComposer
{
  public void Compose(IUmbracoBuilder builder)
  {
	// First, remove the member counters as we don't need them
	builder.TheDashboardCounters().Remove<MembersTotalDashboardCounter>();
	builder.TheDashboardCounters().Remove<MembersNewLastWeekDashboardCounter>();

	// Add my custom counter
	builder.TheDashboardCounters().Append<OrdersDashboardCounter>();
  }
}
```

## Contributions
Are more then welcome but please, before you put a lot of work into it raise an issue and make sure that we're on the same track.

### Roadmap
The main idea with the package is to provide a super simple dashboard for the Content section. We're extremely cautious with adding features, as we want to keep the codebase super-easy.

In the v8 version of the package the "Developer Dashboard" was removed. There's plenty of other packages, i.e. [Diplo GodMode](https://our.umbraco.com/packages/developer-tools/diplo-god-mode/) that solves this problem. There are no plans to add it back.

### Build
To build a Release-version of the package,

* [ ] Make sure to update the version in the .csproj file
* [ ] Run `.\build.ps1` inside the root, files is created in the `Artifacts`-folder

This package was created by Enkel Media, https://www.enkelmedia.se and is supported by https://www.obviuse.se

### Branches

* **main** contains the latest stable version
* **next** contains any upcoming work for new Umbraco-versions
* **vXX/main** branch per major version, created when after a new stable major version has been released.

**Branch History**
* V1 = Umbraco 7 - 7.7.0
* V2 = Umbraco 7.7.0+
* V8 = Umbraco 8
* V9 = Umbraco 9
* V10 = Umbraco 10
* V11 = Umbraco 11
* V12 = Umbraco 12
* V13 = Umbraco 13
* V14 = Umbraco 14
* And so on
