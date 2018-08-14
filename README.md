The Dashboard
=====

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

This is a magic dashboard for Umbraco 7, it's actually "The Dashboard".

**Content dashboard**
Will show editors a summary of the latest activites on the website combined with each editors recent files and other useful information.
 
**Developer dashboard**
Shows information in the developer-section about: Hijacked routes, Events, Application event handlers, Surface controllers and Content finders

> **Note:** This the first release that was developed during breaks at CodeGarden15 and it has been tested on Umbraco 7.1, if you have any issues, please collaborate with me here https://github.com/enkelmedia/TheDashboard

## Build
Just build the package using the included build script and upload the zip file to the Umbraco back-office. 

## Installation in Umbraco CMS
There is a built version of the package here: https://our.umbraco.org/projects/backoffice-extensions/the-dashboard/ just download it and install it in the developers-section.

Or use Nuget

```
Install-Package Our.Umbraco.TheDashboard
```

## Upgrading
When you upgrade, just run the new installer over the old installation - there is no need to uninstall.

This package was created by Enkel Media, http://www.enkelmedia.se
