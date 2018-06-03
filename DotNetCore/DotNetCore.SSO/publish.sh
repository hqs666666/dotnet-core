#！/bin/bash
VERSION=1.6
dotnet publish
docker build -t pony/dotnetsso:$VERSION .
docker tag pony/dotnetsso:$VERSION www.pony.com:32768/pony/dotnetsso:$VERSION
docker push www.pony.com:32768/pony/dotnetsso:$VERSION