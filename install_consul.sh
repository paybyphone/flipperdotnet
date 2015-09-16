#!/bin/bash

sudo apt-get update
sudo apt-get install unzip

wget 'https://dl.bintray.com/mitchellh/consul/0.5.2_linux_amd64.zip' -O consul.zip
unzip consul.zip
sudo chmod +x consul
sudo mv consul /usr/bin/consul

consul agent -server -client 0.0.0.0 -bootstrap-expect 1 -data-dir /tmp/consul &
