@echo off
set SERVICE_PATH="C:\Program Files (x86)\Baker&Taylor\TS360ETSWinService\BT.ETS.WinService.exe"
set SERVICE_NAME="BTNextGen ETS Service"

echo Installing Service...
Sc Create %SERVICE_NAME% Start= auto binPath= "%SERVICE_PATH%" DisplayName= "%SERVICE_NAME%"
Sc Description %SERVICE_NAME% "The purpose of the ETS windows service application is to get ETS requests from queue (like CartReceived, DupCheck or Product Pricing) and process."

@echo off
set /P UserInput=Hit enter key to quit.