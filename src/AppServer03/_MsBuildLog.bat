msbuild -t:restore
msbuild AppServer03.sln -flp:logfile=MyProjectOutput.txt;verbosity=diagnostic 

pause