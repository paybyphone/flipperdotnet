# -*- mode: ruby -*-
# vi: set ft=ruby :

Vagrant.configure(2) do |config|
  config.vm.box = "hashicorp/precise64"
  config.vm.define :consul do |consul|
    consul.vm.network "forwarded_port", guest: 8500, host: 8500
    consul.vm.provision "shell", inline: <<-SHELL
      sudo apt-get update
      sudo apt-get install unzip
    
      wget 'https://releases.hashicorp.com/consul/0.6.4/consul_0.6.4_linux_amd64.zip' -O consul.zip
      unzip consul.zip
      sudo chmod +x consul
      sudo mv consul /usr/bin/consul
      
      wget 'https://releases.hashicorp.com/consul/0.6.4/consul_0.6.4_web_ui.zip' -O ui.zip
      unzip ui.zip
      
      consul agent -server -client 0.0.0.0 -bootstrap-expect 1 -data-dir /tmp/consul -ui-dir /home/vagrant/dist > /var/log/consul.log 2>&1 &
    SHELL
  end
  config.vm.define :redis do |redis|
    redis.vm.network "forwarded_port", guest: 6379, host: 6379
    redis.vm.provision "shell", inline: <<-SHELL
      sudo apt-get update
      sudo apt-get install redis-server
      
      sudo sed -e '/^bind/ s/^#*/#/' -i /etc/redis/redis.conf
      sudo service redis-server restart
      SHELL
  end
end
