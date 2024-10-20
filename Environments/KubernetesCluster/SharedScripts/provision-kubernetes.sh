#!/bin/bash

### Fix CentOS 7 repository URLs to point to the Vault
echo "****** Fix CentOS 7 repository URLs to point to the Vault ******"
sudo sed -i 's|^mirrorlist=|#mirrorlist=|g' /etc/yum.repos.d/CentOS-Base.repo
sudo sed -i 's|^#baseurl=http://mirror.centos.org|baseurl=http://vault.centos.org|g' /etc/yum.repos.d/CentOS-Base.repo

### Clean all yum caches
echo "****** Clean all yum caches ******"
sudo yum clean all

### Update the package index
echo "****** Update the package index ******"
sudo yum update -y

### Add yum repo file for Kubernetes
echo "****** Add Kubernetes repository ******"
sudo tee /etc/yum.repos.d/kubernetes.repo <<EOF
[kubernetes]
name=Kubernetes
baseurl=https://pkgs.k8s.io/core:/stable:/v1.31/rpm/
enabled=1
gpgcheck=1
gpgkey=https://pkgs.k8s.io/core:/stable:/v1.31/rpm/repodata/repomd.xml.key
EOF

### Install Kubernetes components
echo "****** Install Kubernetes components ******"
yum install -y -q kubeadm kubelet kubectl

### Enable and start kubelet
echo "****** Enable and start kubelet ******"
systemctl enable kubelet
systemctl start kubelet