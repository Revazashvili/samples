# Prometheus Application Monitoring

To run commands below, navigate to this directory (where this readme is located)

## Start Dashboard and Application

To start everything run command

``` sh
docker-compose up
```

## Dashboard Url

``` txt
http://localhost:3000/?orgId=1
```

## Prometheus Url

``` txt
http://localhost:9091/
```

## Application Url

Reload this page several times to see activity in grafana charts

``` txt
http://localhost:5000/WeatherForecast
```

## Prometheus Queries

Requests Per Seconds

``` txt
irate(dotnet_request_operations_total[2m])
```

Request Duration

``` txt
rate(dotnet_request_duration_seconds_sum[2m]) / rate(dotnet_request_operations_total[2m])
```
