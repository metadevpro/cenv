# cenv

[![Build Status](https://travis-ci.com/metadevpro/cenv.svg?branch=master)](https://travis-ci.com/metadevpro/cenv)

It Creates Environment Files for *.config files in safe and reproducible way.

No more manual editing of config files!

## Philosophy

Following the principles of [The 12 Factor Apps](https://12factor.net/), environment data must be injected 
when preparing for deployment into an environment (deploy time) and not at *build time*.

*NO hardcode values* for environment must be present in developer code nor in the repository. 
Otherwise this practice produce environment errors and leakage of credentials. 

This tools enforce the separation and provides a simple CLI tool for applying and checking configuration to config templates.
This enables the usage in automation escenarios like Continous Integration or Continous Deployment.

Environment data is encoded in [TOML files](https://en.wikipedia.org/wiki/TOML).
See sample:

```toml
# Application Z. Environment: DEV

[application]
name = "Z"
env = "dev"

[connectionStrings.cnx1]
connectionString = "Data Source=mb-db1 ..."
providerName = "System.Data.SqlClient"

[connectionStrings.cnx2]
connectionString = "Data Source=mb-db2 ..."
providerName = "System.Data.SqlClient"

[connectionStrings.cnx3]
connectionString = "Data Source=mb-db3 ..."
providerName = "System.Data.SqlClient"

[appSettings]
user = "u1" 
password = "sec3" 
path = "tmp/"
redis= "tcp://redis:15678"
```

Configuration files are typical config .NET files. See example:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <connectionStrings>
    <add name="cnx1" connectionString="TBD" providerName="TBD" />
    <add name="cnx2" connectionString="TBD" providerName="TBD" />
  </connectionStrings>
  <appSettings>
    <add key="user" value="TBD" />
    <add key="password" value="TBD" />
    <add key="path" value="TBD" />
    <add key="redis" value="TBD" />
    <add key="exchangeApi" value="TBD" />
  </appSettings>
  <system.webServer>
    <!-- ... -->
  </system.webServer>
</configuration>
```

Please note values marked as *TDB* (to be defined) are intended to be replaced at a later stage (where enviroment is set).


By design: *Only* `ConnectionStrings` and `AppSetting` sections will be replaced.

## Usage

1. To apply enviroment data to a config file:

```sh
cenv apply dev.toml template.config -o webconfig.xml
```

Simulation is allowed with the `--dry-run` option.
```sh
cenv apply dev.toml template.config --dry-run
```

2. To check a generated file is fully configured (no missing data pending to be defined):

```sh
cenv check dev.toml webconfig.xml
```

Sample output:
```
  1 Error U1: connectionStrings/cnx1 @connectionString     Is not defined. Actual='TBD'
  2 Error M1: connectionStrings/cnx1 @connectionString     Does not match. Config: 'Data Source=mb-db1 ...' Actual: 'TBD'
  3 Error U1: connectionStrings/cnx2 @providerName         Is not defined. Actual='TBD'
  4 Error M1: connectionStrings/cnx2 @connectionString     Does not match. Config: 'System.Data.SqlClient' Actual: 'TBD'
  5 Error U2: appSettings/user                             Is not defined. Actual='TBD'
  6 Error M2: appSettings/user                             Does not match. Config: 'u1' Actual: 'TBD'
  7 Error U2: appSettings/exchangeApi                      Is not defined. Actual='TBD'
  8 Error M2: appSettings/exchangeApi                      Does not match. Config: '' Actual: 'TBD'
8 errors were found.
```

## Help

For help Type:

```sh
cenv help
```

## License

This software is licensed under the MIT License.

Copyright 2020, [Metadev](https://metadev.pro)

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation
files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use,
copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the
Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
