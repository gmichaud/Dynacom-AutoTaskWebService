# Dynacom-AutoTaskWebService
A simple ASP.NET Web API wrapper for execution of Dynacom Automated Tasks 

* Two endpoints: /api/{databaseName}/AutoTask/List and /api/{databaseName}/AutoTask/Run/{id}
* Basic authentication is used to authenticate against the COM API of Dynacom

The application pool under which this service is running must be enabled for [32 bit applications](https://blogs.msdn.microsoft.com/rakkimk/2007/11/03/iis7-running-32-bit-and-64-bit-asp-net-versions-at-the-same-time-on-different-worker-processes/). The identity should also be a user with proper write access to the same folders Dynacom uses (the same Dynacom Automation Manager account will work)
