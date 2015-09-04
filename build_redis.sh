#!/bin/bash

if [ ! -e ../StackExchange.Redis ]
then
    git clone https://github.com/StackExchange/StackExchange.Redis.git ../StackExchange.Redis/
fi

cd ../StackExchange.Redis

#./monobuild.bash
mkdir -p StackExchange.Redis/bin/mono
mcs -recurse:StackExchange.Redis/*.cs -out:StackExchange.Redis/bin/mono/StackExchange.Redis.dll -target:library -unsafe+ -o+ -r:System.IO.Compression.dll

