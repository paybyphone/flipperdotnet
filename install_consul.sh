#!/bin/bash

wget 'https://dl.bintray.com/mitchellh/consul/0.5.2_linux_amd64.zip' -O consul.zip
unzip consul.zip
chmod +x consul

./consul agent -server -client 0.0.0.0 -bootstrap-expect 1 -data-dir /tmp/consul &
