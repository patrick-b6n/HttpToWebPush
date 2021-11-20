# HttpToWebPush

A middleware between Http and [Web Push Notifications](https://developer.mozilla.org/en-US/docs/Web/API/Push_API)

## Technologies
* .NET 6
* ASP.NET Core 6
* Blazor WebAssembly
* Typescript
* TailwindCSS

## Deploy
**appsettings.json**
```json
{
  "ConnectionStrings": {
    "HttpToWebPush": ""
  },
  "HttpToWebPush": {
    "PushApi": {
      "subject": "",
      "publicKey": "",
      "privateKey": ""
    }
  }
}
```

**docker-compose.yml**
```yaml
version: '3'

services:
  grafana:
    image: ghcr.io/patrick-b6n/http-to-web-push:latest
    container_name: http-to-web-push
    restart: always
    volumes:
      - ${PWD}/appsettings.json:/app/appsettings.json
```

## Development
**Database**
```shell
docker-compose up
```
**Backend**
```shell
cd ./src/HttpToWebPush.Server/

dotnet watch
```
**Frontend**
```shell
cd ./src/HttpToWebPush.UI/

yarn watch-ts   # watch for typescript changes
yarn watch-css  # watch for css changes 
yarn build-sw   # build serviceworker

# TODO: combine into one command
```

## Thanks
* Reference implementation and Lib [Tomasz Pęczek](https://github.com/tpeczek/Lib.Net.Http.WebPush)