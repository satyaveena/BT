@echo off
set SERVICE_NAME="TS360 ILS Queue Service"

echo UnInstalling Service...
Sc Delete %SERVICE_NAME%

@echo off
set /P UserInput=Hit enter key to quit.