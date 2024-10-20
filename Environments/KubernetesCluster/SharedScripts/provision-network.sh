#!/bin/bash

### Disable SELinux
echo "****** Disable SELinux ******"
setenforce 0
sed -i --follow-symlinks 's/^SELINUX=enforcing/SELINUX=disabled/' /etc/sysconfig/selinux

### Disable Firewall
echo "****** Disable Firewall ******"
systemctl disable firewalld >/dev/null 2>&1
systemctl stop firewalld

### sysctl settings
echo "****** Apply sysctl settings ******"
cat >>/etc/sysctl.d/kubernetes.conf<<EOF
net.bridge.bridge-nf-call-ip6tables = 1
net.bridge.bridge-nf-call-iptables = 1
EOF
sysctl --system >/dev/null 2>&1

### Disable swap
echo "****** Disable swap ******"
sed -i '/swap/d' /etc/fstab
swapoff -a

### Configure NetworkManager for Calico networking
echo "****** Configure NetworkManager for Calico networking ******"
cat >>/etc/NetworkManager/conf.d/calico.conf<<EOF
[keyfile]
unmanaged-devices=interface-name:cali*;interface-name:tunl*
EOF

### Fix containerd issue
echo "****** Fix containerd issue ******"
if ! which containerd; then
    yum install -y containerd
fi
sudo systemctl enable containerd
sudo systemctl start containerd

### Modify containerd configuration
echo "****** Modify containerd configuration ******"
sudo tee /etc/containerd/config.toml <<EOF
[plugins."io.containerd.grpc.v1.cri"]
  [plugins."io.containerd.grpc.v1.cri".containerd]
    snapshotter = "overlayfs"
    no_pivot = false
  [plugins."io.containerd.grpc.v1.cri".containerd.runtimes.runc]
    runtime_type = "io.containerd.runc.v2"
EOF

### Restart containerd
echo "****** Restart containerd ******"
sudo systemctl restart containerd

### Fix DNS resolution issue
echo "****** Fix DNS resolution issue ******"
echo "nameserver 8.8.8.8" > /etc/resolv.conf
curl -v https://registry.k8s.io/v2/

### Final message
echo "****** Network configuration completed successfully ******"