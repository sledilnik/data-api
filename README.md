# slo-covid-19 rest-server
Provides REST API to data collected in [csv files](https://github.com/slo-covid-19/data). The sources are [stats](https://covid19.rthand.com/api/stats), 
[regions](https://covid19.rthand.com/api/regions), [patients](https://covid19.rthand.com/api/patients), 
[hospitals](https://covid19.rthand.com/api/hospitals), [hospitals-list](https://covid19.rthand.com/api/hospitals-list), 
[municipalities-list](https://covid19.rthand.com/api/municipalities-list), 
[deceased-regions](https://covid19.rthand.com/api/deceased-regions), [municipalities](https://covid19.rthand.com/api/municipalities) and 
[health-centers](https://covid19.rthand.com/api/health-centers).
Only GET method is supported, no parameters are available. If necessary filtering parameters will be added eventually.

A running instance is available at https://covid19.rthand.com/api/ endpoint.

Current Docker container for this project is available at [mihamarkic/slo-covid19-server](https://hub.docker.com/r/mihamarkic/slo-covid19-server).

Response compression is supported, ETag/If-None-Match as well.

Swagger endpoint is at [https://covid19.rthand.com/swagger](https://covid19.rthand.com/swagger).

In case of failures a notification is set to slack channel #alert through Data API bot defined by a secret (see sample docker-compose file below).

## Changelog

## 1.5.14

- Add ActiveCases to municipalities endpoint
- Updates SchemaVersion to 19

## 1.5.13

- Fixes Timestamp in headers

## 1.5.12

- Fixes bug in patients mixing today and toDate in deceased in hospitals

## 1.5.11

- Modifies patient's deceased schema
- Updates SchemaVersion to 18

## 1.5.10

- Adds health-centers endpoint
- Updates SchemaVersion to 17

## 1.5.9

- Adds municipalities endpoint
- Adds Timestamp header
- Removes unused regions-pivot endpoint
- Updates SchemaVersion to 16

### 1.5.8

- Adds cases.recovered.todate and rename cases.active (was .todate)
- Updates SchemaVersion to 15

### 1.5.7

- Adds tests.regular and tests.ns-apr20
- Updates SchemaVersion to 14

### 1.5.6

- Adds state.deceased.hospital.icu to patients
- Makes requests to source fail faster #29
- Switches to new URL for CSV retrieval #30

### 1.5.5

- Adds api/decased_regions
- Adds state.<hospital>.icu.todate and state.<hospital>.critical.todate to patients
- Updates SchemaVersion to 13

### 1.5.4

- Adds deceased.*.todate to stats
- Updates SchemaVersion to 12

### 1.5.3
   
- Adds cases.unclassified.confirmed.todate to stats
- Changes behavior in case of failure against CSV source - data is returned from cache, slack notification is sent
- Updates SchemaVersion to 11

### 1.5.2

- Requests to CSV source are now cached for a minute

### 1.5.1

- Refactors communicator caching
- Adds retirement-homes and retirement-homes-list endpoints
- Updates SchemaVersion to 10

### 1.5.0
 
- Adds prometheus metrics available at /metrics endpoint

### 1.4.5

- Adds regions-pivot, a pivoted view on regions
- Updates SchemaVersion to 9

### 1.4.4

- Removes *.needs_o2 and state.in_care from patients
- Updates Schema version to 8

### 1.4.3
- Adds municipalities-list endpoint
- Updates SchemaVersion to 7

### 1.4.2

- Adds cases.confirmed, cases.confirmed.todate, cases.closed.todate and cases.active.todate to stats
- Removes legacy buckets 0-15, 16-29, 30-49, 50-59 and 60+
- Updates SchemaVersion to 6

### 1.4.1

- Removes 14h data from stats
- Removes facilities and sources from stats
- Updates SchemaVersion to 5

### 1.4.0

- Adds hospitals and hospitals-list endpoints
- Updates SchemaVersion to 4

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
