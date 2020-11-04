# slo-covid-19 rest-server
Provides REST API to data collected in [csv files](https://github.com/slo-covid-19/data). The sources are [stats](https://api.sledilnik.org/api/stats), 
[regions](https://api.sledilnik.org/api/regions), [patients](https://api.sledilnik.org/api/patients), 
[hospitals](https://api.sledilnik.org/api/hospitals), [hospitals-list](https://api.sledilnik.org/api/hospitals-list), 
[municipalities-list](https://api.sledilnik.org/api/municipalities-list), 
[deceased-regions](https://api.sledilnik.org/api/deceased-regions), [municipalities](https://api.sledilnik.org/api/municipalities) and 
[health-centers](https://api.sledilnik.org/api/health-centers), [owid](https://api.sledilnik.org/api/owid).
Only GET method is supported, no parameters are available. If necessary filtering parameters will be added eventually.

A running instance is available at https://api.sledilnik.org/api/ endpoint.

Current Docker container for this project is available at [covid19sledilnik/data-api-server](https://hub.docker.com/repository/docker/covid19sledilnik/data-api-server).
(Docker container has beeno moved on 21.9.2020)

Response compression is supported, Etag/If-None-Match as well.

Swagger endpoint is at [https://api.sledilnik.org/swagger](https://api.sledilnik.org/swagger).

In case of failures a notification is set to slack channel #alert through Data API bot defined by a secret (see sample docker-compose file below).

## Changelog

## 1.7.0

- Adds owid data through owid endpoint. Supports application/json and text/csv outputs. CSV output uses InvariantCulture formatting.
  Filter arguments are from, to and countries - all are optional.
  Sample: api/owid?from=2020-06-02&to=2020-06-30&countries=BEL,SLV

## 1.6.8

- Weekly: added in week.investigated and week.healthcare

## 1.6.7

- Patients, Hospitals: added support for psychiatric hospitals (care units only)

## 1.6.6

- Bug fix: toDate was not exposed properly in patients

## 1.6.5

- Patients: add support for care hospitals

## 1.6.4

- Hospital ID fix (Bolnišnica Sežana)

## 1.6.3

- Add all hospitals

## 1.6.2

- Periodic cache refresh in background instead of attempt to refresh on every client request
- Handle weak ETag-s (free cloudflare supports only weak etags)

## 1.6.1

- Add new hospital (SB Šempeter - Nova Gorica)

## 1.6.0

- Add CSV format
- Add ResponseCache (performance imporovement)

## 1.5.25

- Add new hospital (SB Trbovlje)

## 1.5.24

- Add new hospital (SB Ptuj)

## 1.5.23

- Hospitals: added support for care unit capacity

## 1.5.22

- Remove hospital (SB NG)

## 1.5.21

- Adds new hospitals (SB SG, SB NG)

## 1.5.20

- Added stats-weekly

## 1.5.18

- Adds new hospitals (SB Jesenice)

## 1.5.17

- Adds new hospitals (SB MS)

## 1.5.16

- Adds new hospitals
- Moves docker image to [covid19sledilnik/data-api-server](https://hub.docker.com/repository/docker/covid19sledilnik/data-api-server)
- Updates readme with new URLs
- Adds Etag header to exposed headers

## 1.5.15

- Adds filtering by date where applicable

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
