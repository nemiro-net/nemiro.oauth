chcp 65001
:: --verbosity normal
:: --configuration "Release"
:: --logger "trx;LogFileName=results.trx"
:: > output.log
dotnet test > output.log
notepad output.log