FROM mcr.microsoft.com/dotnet/sdk:5.0 as BUILD_WEB
WORKDIR /opt/app
COPY . /opt/app
RUN dotnet publish -r linux-x64 -c Release -p:IncludeWeb=true -o out QuartierLatin.Admin.csproj

FROM mcr.microsoft.com/dotnet/aspnet:3.1-bionic as FINAL_WEB

RUN mkdir -v /var/blob /var/shared &&\
    apt update && apt install -y wget &&\
     wget -O wkhtml.deb\
      https://github.com/wkhtmltopdf/packaging/releases/download/0.12.6-1/wkhtmltox_0.12.6-1.bionic_amd64.deb &&\
      apt install -y ./wkhtml.deb && rm wkhtml.deb && apt-get clean

WORKDIR /opt/app

COPY --from=BUILD_WEB /opt/app/out/ /opt/app

VOLUME /data
VOLUME /var/blob
VOLUME /var/shared

EXPOSE 12321

ENTRYPOINT ./QuartierLatin.Admin