#!/bin/bash

echo "****** Fix CentOS 7 repository URLs to point to the Vault ******"
sed -i 's|^mirrorlist=|#mirrorlist=|g' /etc/yum.repos.d/CentOS-Base.repo
sed -i 's|^#baseurl=http://mirror.centos.org|baseurl=http://vault.centos.org|g' /etc/yum.repos.d/CentOS-Base.repo
yum clean all
yum update -y

### Install Docker
echo "****** Install Docker ******"
yum install -y yum-utils device-mapper-persistent-data lvm2
yum-config-manager --add-repo https://download.docker.com/linux/centos/docker-ce.repo
yum update -y && yum install docker-ce -y
usermod -aG docker $(whoami)

### Create /etc/docker directory.
mkdir -p /etc/docker

### Setup Docker daemon.
cat > /etc/docker/daemon.json <<EOF
{
  "exec-opts": ["native.cgroupdriver=systemd"],
  "log-driver": "json-file",
  "log-opts": {
    "max-size": "100m"
  },
  "storage-driver": "overlay2"
}
EOF

mkdir -p /etc/systemd/system/docker.service.d

### Restart Docker
echo "****** Restart Docker ******"
systemctl enable docker.service
systemctl daemon-reload
systemctl restart docker