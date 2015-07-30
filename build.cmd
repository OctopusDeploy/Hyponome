@echo off
cd %~dp0

echo Installing dnx...
CALL dnvm install latest

echo Restoring...
CALL dnu restore

echo Publishing...
CALL dnu publish src/Hyponome.Web --no-source --runtime active