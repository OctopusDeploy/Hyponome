@echo off
cd %~dp0

SETLOCAL
SET CACHED_DNVM=%USERPROFILE%\.dnx\bin\dnvm.cmd

IF EXIST %CACHED_DNVM% GOTO dnvminstall
echo Installing dnvm
@powershell -NoProfile -ExecutionPolicy unrestricted -Command "&{$Branch='dev';iex ((new-object net.webclient).DownloadString('https://raw.githubusercontent.com/aspnet/Home/dev/dnvminstall.ps1'))}"

:dnvminstall
echo Installing dnx...
CALL dnvm install latest

echo Restoring...
CALL dnu restore

echo Publishing...
CALL dnu publish src/Hyponome.Web --no-source --out artifacts/build/ --runtime active