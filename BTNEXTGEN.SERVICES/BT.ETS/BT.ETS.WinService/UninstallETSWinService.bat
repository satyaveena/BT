@echo off
set SERVICE_NAME="BTNextGen ETS Service"

echo UnInstalling Service...
Sc Delete %SERVICE_NAME%

@echo off
set /P UserInput=Hit enter key to quit.