# -*- mode: ruby -*-
# vi: set ft=ruby :

Vagrant.configure(2) do |config|
  config.vm.box = "hashicorp/precise64"
  config.vm.network "forwarded_port", guest: 8500, host: 8500
  config.vm.provision "shell", inline: <<-SHELL
    sudo apt-get update
    wget -qO- https://get.docker.com/ | sh
    
    # add vagrant to docker group
    sudo usermod -aG docker vagrant
    
    docker run -d -p 8400:8400 -p 8500:8500 -p 8600:53/udp -h node1 progrium/consul -server -bootstrap -ui-dir /ui
  SHELL
end
