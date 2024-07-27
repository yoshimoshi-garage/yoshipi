# `yoshipub.bat`

Place the following contents in a file named `yoshipub.bat` inside the `.vscode` folder of your Workspace

```
dotnet publish -r linux-arm /p:ShowLinkerSizeComparison=true 
pushd .\bin\Debug\net8.0\linux-arm64\publish
scp -i "$HOME\.ssh\yoshikey" .\* pi@yoshipi.local:~/yoshipi
popd
```