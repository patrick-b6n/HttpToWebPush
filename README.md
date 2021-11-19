# HttpToWebPush

A middleware between Http and [Web Push Notifications](https://developer.mozilla.org/en-US/docs/Web/API/Push_API)

## Technologies
* .NET 6
* ASP.NET Core 6
* Blazor WebAssembly
* Typescript
* TailwindCSS

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