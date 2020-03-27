# slo-covid-19 rest-server
Provides REST API to data collected in [csv files](https://github.com/slo-covid-19/data). The three sources are [stats](https://covid19.rthand.com/api/stats), [regions](https://covid19.rthand.com/api/regions) and [patients](https://covid19.rthand.com/api/patients). Only GET method is supported, no parameters are available. If necessary filtering parameters will be added eventually.

A running instance is available at https://covid19.rthand.com/api/ endpoint.

Current Docker container for this project is available at [mihamarkic/slo-covid19-server](https://hub.docker.com/r/mihamarkic/slo-covid19-server).

Swagger endpoint is at [https://covid19.rthand.com/swagger](https://covid19.rthand.com/swagger).

In case of failures a notification is set to slack channel #alert through Data API bot defined by a secret (see sample docker-compose file below).

## Build Docker container

Run build.ps1 -Target BuildImage

## Run Docker container

Container doesn't store any files and exposed HTTP through port 5000. It also runs as a non-root user with id 9000.

Here is sample docker-compose.yml file

```yaml
version: '2'
services:
  covid19:
    restart: always
    image: mihamarkic/slo-covid19-server:latest
    mem_limit: 400m
    ports:
      - "5000:5000"
    environment:
      - SloCovidServer_Slack_Secret=XXX
```

## About

Repository maintainer: [Miha Markič](https://twitter.com/MihaMarkic), [Righthand](https://blog.rthand.com/)
