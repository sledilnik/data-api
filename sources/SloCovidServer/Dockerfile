FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["SloCovidServer/SloCovidServer.csproj", "SloCovidServer/"]
RUN dotnet restore "SloCovidServer/SloCovidServer.csproj"
COPY . .
WORKDIR "/src/SloCovidServer"
RUN dotnet build "SloCovidServer.csproj" -c Release -o /app/build
WORKDIR "/src/Test/SloCovidServer.Test"
RUN dotnet test "SloCovidServer.Test.csproj" -c Release

WORKDIR "/src/SloCovidServer"
FROM build AS publish
RUN dotnet publish "SloCovidServer.csproj" -c Release -o /app/publish

FROM base AS final
# install slovene language
RUN apt-get clean && apt-get -y update && apt-get install -y locales && locale-gen sl_SI.UTF-8 && locale -a \
	&& apt-get clean
# && localedef -i sl_SI -c -f UTF-8 -A /usr/share/locale/locale.alias sl_SI.UTF-8 && LANG sl_SI.utf8 
WORKDIR /app
COPY --from=publish /app/publish .
# use non-root user
USER 9000
EXPOSE 5000
ENTRYPOINT ["dotnet", "SloCovidServer.dll"]