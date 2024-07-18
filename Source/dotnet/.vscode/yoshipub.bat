echo %1
pushd %1\bin\Debug\net8.0
scp -i "%USERPROFILE%\.ssh\yoshikey" .\* pi@yoshipi.local:~/yoshipi
popd