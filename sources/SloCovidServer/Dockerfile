FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["SloCovidServer/SloCovidServer.csproj", "SloCovidServer/"]
RUN dotnet restore "SloCovidServer/SloCovidServer.csproj"
COPY . .
WORKDIR "/src/SloCovidServer"
RUN dotnet build "SloCovidServer.csproj" -c Release -o /app/build
#WORKDIR "/src/SloCovidServer.Test"
#RUN dotnet test "SloCovidServer.Test.csproj" -c Release

WORKDIR "/src/SloCovidServer"
FROM build AS publish
RUN dotnet publish "SloCovidServer.csproj" -c Release -o /app/publish

EXPOSE 5000
FROM base AS final
# install slovene language
RUN locale-gen sl_SI.UTF-8
RUN locale -a
RUN localedef -i sl_SI -c -f UTF-8 -A /usr/share/locale/locale.alias sl_SI.UTF-8
ENV LANG sl_SI.utf8

WORKDIR /app
COPY --from=publish /app/publish .
# use non-root user
USER 9000
ENTRYPOINT ["dotnet", "SloCovidServer.dll"]