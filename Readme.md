# cenv

[![Build Status](https://travis-ci.com/metadevpro/cenv.svg?branch=master)](https://travis-ci.com/metadevpro/cenv)

**Cenv** creates .NET `*.config` environment files in a safe and reproducible way.

No more manual editing of config files!

## Philosophy

According to the principles of [The 12 Factor Apps](https://12factor.net/), enviroment-aware configuration data should be injected into the runtime enviroment during *deployment* time, not during the *build* time.

There should be *NO hardcoded environment configuration values* in developer code nor in the repository. Hardcoded values lead to environment errors and exposure of secrets such as credentials.

*Cenv* is a simple CLI tool that adheres to this philosophy, enabling deployment automation scenarios such as Continous Integration (CI) and Continous Deployment (CD).

You encode configuration values in [TOML template files](https://en.wikipedia.org/wiki/TOML). Cenv applies and checks these values to your tokenized `*.config` configuration files.

## Example

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

Configuration files are typical config .NET files such as this one:

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

Note the token values marked *TBD* ("to be defined"). They are replaced at a later stage, when the enviroment is set.

*By design*: *Only* `ConnectionStrings` and `AppSettings` sections will be replaced.

## Usage

1. Apply enviroment data to a config file:

```sh
cenv apply dev.toml template.config -o webconfig.xml
```

You can _preview_ this operation, without making the changes, by adding the `--dry-run` option.
```sh
cenv apply dev.toml template.config --dry-run
```

2. Check that a generated file is fully configured, with no unreplaced *TBD* tokens:

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

## Build

Buiding the project requires a .NET Core 3.1 installation.

Then:

```sh
dotnet restore
dotnet build
dotnet test
dotnet run --project cenv
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
