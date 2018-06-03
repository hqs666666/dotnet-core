#！/bin/bash
VERSION=1.0
dotnet publish
docker build -t pony/dotnetapi:$VERSION .
docker tag pony/dotnetapi:$VERSION www.pony.com:32768/pony/dotnetapi:$VERSION
docker push www.pony.com:32768/pony/dotnetapi:$VERSION