# slo-covid-19 rest-server
Provides REST API to data collected in [csv files](https://github.com/slo-covid-19/data). The sources are [stats](https://api.sledilnik.org/api/stats),
[regions](https://api.sledilnik.org/api/regions), [patients](https://api.sledilnik.org/api/patients),
[hospitals](https://api.sledilnik.org/api/hospitals), [hospitals-list](https://api.sledilnik.org/api/hospitals-list),
[municipalities-list](https://api.sledilnik.org/api/municipalities-list),
[municipalities](https://api.sledilnik.org/api/municipalities),
[health-centers](https://api.sledilnik.org/api/health-centers), [owid](https://api.sledilnik.org/api/owid),
[monthly_deaths_slovenia](https://api.sledilnik.org/api/monthly-deaths-slovenia),
[lab-tests](https://api.sledilnik.org/api/lab-tests),
[daily-deaths-slovenia](https://api.sledilnik.org/api/daily-deaths-slovenia),
[age-daily-deaths-slovenia](https://api.sledilnik.org/api/age-daily-deaths-slovenia),
[summary](https://api.sledilnik.org/api/summary),
[sewage](https://api.sledilnik.org/api/sewage),
[schools](https://api.sledilnik.org/api/schools),
[school-status](https://api.sledilnik.org/api/school-status) and
[vaccinations](https://api.sledilnik.org/api/vaccinations).

Only GET method is supported, most of endpoints support parameters `from` and `to` (both dates).

A running instance is available at https://api.sledilnik.org/api/ endpoint.

Current Docker container for this project is available at [covid19sledilnik/data-api-server](https://hub.docker.com/repository/docker/covid19sledilnik/data-api-server).
(Docker container has been moved on 21.9.2020)

Response compression is supported, Etag/If-None-Match as well.

Swagger endpoint is at [https://api.sledilnik.org/swagger](https://api.sledilnik.org/swagger).

In case of failures a notification is set to slack channel #alert through Data API bot defined by a secret (see sample docker-compose file below).

## Changelog

## 1.13.3

* stats: added vaccinated confirmed cases

## 1.13.2

* Population update to SURS 2021/H1

## 1.13.1

* Vaccination info card: only show fully vaccinated
* Active100k info card: 14-day incidence

## 1.13.0

* Vaccinations: add used by manufacturer
* Vaccinations: add administered by age groups

## 1.12.2

* Sewage: fix parsing float numbers with exponent

## 1.12.1

* Returns only results with absences and/or regimes when filtering school-status by date
* Makes date parsing tolerant to included time

## 1.12.0

* Adds `vaccination.csv` to API as `/api/vaccinations` endpoint
* Schema version 41

## 1.11.1

* Adds from/to filter to `api/school-status` endpoint
* Changes school property to string in `api/school-status` endpoint

## 1.11.0

* Adds `api/school-status` endpoint. It combines `schools-absences.csv` and `schools-regimes.csv` data into single source. Filtering on schools is available through argument id in URL.
* Schema version 40

## 1.10.1

* Adds today data for vaccination.administered
* Schema version 39

## 1.10.0

* Adds `schools-cases.csv` to API as `/api/schools` endpoint
* Schema version 38

## 1.9.21

* Adds deceased.todate and deceased to stats endpoint
* Schema version 37

## 1.9.20

* Reduces logging

## 1.9.19

* Schema version 36
* Adds `sewage.csv` to API as `/api/sewage` endpoint

## 1.9.18

* Schema version 35
* Add vaccination.administered2nd vaccination.used to `/api/stats`
* Extend vaccinationSummary with 2nd dose in `/api/summary`

## 1.9.17

* Schema version 34
* Update Slovenia population to SURS H2/2020
* Add healthcare male/female to `/api/stats-weekly`

## 1.9.16

* Schema version 33
* Add patients.niv to `/api/patients`

## 1.9.15

* Schema version 32
* Add vaccination.delivered to `/api/stats`

## 1.9.14

* Schema version 31
* Removed `api/deceased-regions`
* `/api/region-cases` endpoint not uses `region-cases.csv` as source
* `/api/municipalities` endpoint not uses `municipalities-cases.csv` as source

## 1.9.13

* stats: remove obsolete recovered field
* weekly-stats: remove vaccination field (now only in daily)
* municipalities: switch to municipality-cases.csv
* regions: switch to municipality-confirmed.csv (obsolete)
* deceased-regions: switch to municipality-deceased.csv (obsolete)
* Schema version 30

## 1.9.12

* Switch VaccinationSummary to daily numbers
* Updates stats with vaccination.administered
* Schema version 29

## 1.9.11

* Add simple VaccinationSummary to summary
* Updates stats-weekly with vaccination.administered
* Schema version 28

## 1.9.10

* Replaces `age_daily_deaths_slovenia.csv` source with `daily_deaths_slovenia_by_age.csv`

## 1.9.9

* Preloads cache before starting API

## 1.9.8

* Summary: add TestsTodayHAT
* Schema version 27

## 1.9.7

* Updates stats with deceased RH occupant
* Schema version 26

## 1.9.0

* Adds summary endpoint as /api/summary
* Schema version 25

## 1.8.7

* Updates stats-weekly with rhoccupant (rh-occupant) and loc (locations)
* Schema version 24

## 1.8.6

* Adds age_daily_deaths_slovenia.csv to API as /api/age-daily-deaths-slovenia endpoint
* Schema version is 23

## 1.8.5

* Adds daily_deaths_slovenia.csv to API as /api/daily-deaths-slovenia endpoint
* Schema version is 22

## 1.8.4

* Fixes bug in lab-tests
* Updates NSwag

## 1.8.3

* Adds lab_tests.csv to API as /api/lab-tests endpoint
* Schema version is 21

## 1.8.2

* Adds monthly_deaths_slovenia.csv to API as /api/monthly_deaths_slovenia endpoint
* Schema version is 20

## 1.8.1

* Adds columns parameter to owid to allow selection of arbitrary columns. Columns isoCode and date are always present regardles of columns parameter.

## 1.8.0

* Updates to .Net 5.0
* Upgrades classes to records where applicable and switches to expressions

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
