#!/bin/bash

# Check if socat is installed; install if not
sudo yum install -y socat

# Function to run on master node
setup_master() {
    echo "****** Master node detected, initializing Kubernetes cluster ******"

    ### --pod-network-cidr=192.168.0.0/16: we refer this value because we plan to use Calico plugin
    kubeadm init --apiserver-advertise-address=192.168.50.10 --pod-network-cidr=192.168.0.0/16 || { echo "Kubeadm init failed"; exit 1; }

    
    ### Copy the admin configuration file admin.conf to the user's home directory and set proper permissions so the kubectl command can be run
    mkdir -p $HOME/.kube
    sudo cp -i /etc/kubernetes/admin.conf $HOME/.kube/config
    sudo chown $(id -u):$(id -g) $HOME/.kube/config 

    ### Apply Calico for networking
    kubectl apply -f https://raw.githubusercontent.com/projectcalico/calico/v3.28.2/manifests/calico.yaml

    ### Update kubelet configuration to specify the node IP
    echo "Updating kubelet configuration..."
    echo 'KUBELET_KUBEADM_ARGS="--container-runtime-endpoint=unix:///var/run/containerd/containerd.sock --pod-infra-container-image=registry.k8s.io/pause:3.10 --node-ip=192.168.50.10"' | sudo tee /var/lib/kubelet/kubeadm-flags.env
    sudo systemctl daemon-reload
    sudo systemctl restart kubelet

    ### Create and share the join-command, basically we just need to generate the command by this command and run this command on the worker node.
    ### In that case, the worker node can join to the cluster
    kubeadm token create --print-join-command > /join-command.sh
    chmod +x /join-command.sh

    ### Start HTTP server and ensure it's running
    ### Purpose is used to worker node can access to the master node, to get the join-command and run it automatically
    cd /
    echo "Starting HTTP server to serve join command..."
    sudo yum install -y python3
    sudo python3 -m http.server 8000 &
    
    ### Install net-tools to get netstat, to helth check HTTP server is running or not
    sudo yum install -y net-tools

    ### Check if the server has started
    echo "Checking if the HTTP server is running..."
    while ! netstat -tuln | grep ":8000"; do
        echo "Waiting for HTTP server to start..."
        sleep 2
    done

    echo "****** Master node setup complete ******"
}

# Function to run on worker nodes
setup_worker() {
    echo "****** Worker node detected, joining cluster ******"

    ### Install wget to get the file from the master node
    sudo yum install -y wget

    sleep 60
    
    echo "Fetching join command from master node..."

    ### Get join-command.sh from master node running at port 8000
    wget http://192.168.50.10:8000/join-command.sh -O /join-command.sh

    if [ -f /join-command.sh ]; then
        chmod +x /join-command.sh
        echo "Executing join command..."
        /join-command.sh      ### Execute this command to start joining to the cluster
    else
        echo "Join command file not found. Please check the master node and try again."
    fi

    echo "****** Worker node setup complete ******"
}

# Main script logic
if [ "$HOSTNAME" == "staging-k8s-master" ]; then
    setup_master
elif [[ "$HOSTNAME" =~ staging-k8s-worker* ]]; then
    setup_worker
else
    echo "Unknown node type. Exiting."
    exit 1
fi