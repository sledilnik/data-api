# slo-covid-19 rest-server
Provides REST API to data collected in [csv files](https://github.com/slo-covid-19/data). The three sources are [stats](https://covid19.rthand.com/api/stats), [regions](https://covid19.rthand.com/api/regions) and [patients](https://covid19.rthand.com/api/patients). Only GET method is supported, no parameters are available. If necessary filtering parameters will be added eventually.

A running instance is available at https://covid19.rthand.com/api/ endpoint.

Current Docker container for this project is available at [mihamarkic/slo-covid19-server](https://hub.docker.com/r/mihamarkic/slo-covid19-server).

Swagger endpoint is at [https://covid19.rthand.com/swagger](https://covid19.rthand.com/swagger).

## Build Docker container

Run build.ps1 -Target BuildImage

## About

Repository maintainer: [Miha Markič](https://twitter.com/MihaMarkic), [Righthand](https://blog.rthand.com/)
