#!/bin/bash

wget 'https://releases.hashicorp.com/consul/0.6.4/consul_0.6.4_linux_amd64.zip' -O consul.zip
unzip consul.zip
chmod +x consul

./consul agent -server -client 0.0.0.0 -bootstrap-expect 1 -data-dir /tmp/consul &
