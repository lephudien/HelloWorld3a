msbuild -t:restore
msbuild AdapterDB3.sln -flp:logfile=MyProjectOutput.txt;verbosity=diagnostic 

pause