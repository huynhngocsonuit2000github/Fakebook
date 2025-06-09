#!/bin/bash

echo "123" | passwd --stdin root                ### set the password for root account
      sed -i 's/^PasswordAuthentication no/PasswordAuthentication yes/' /etc/ssh/sshd_config      ### sed (stream editor); s/ initale the replace command; ^PasswordAuthentication no start of a line; PasswordAuthentication yes replece with value; /etc/ssh/sshd_config the file want to be updated
      systemctl reload sshd               ### reload the ssh server configuration to apply the changes made to sshd_config, allowing the new settings to take effect immediately

### open the /etc/hosts file for appending and start taking input from terminal until it encounters the EOF; >> operator appends
### EOF second: this marker signifies the end of the input, cat command stops appending to the file
cat >> /etc/hosts << EOF            
192.168.50.10 master.xtl
192.168.50.11 worker1.xtl
EOF
 
# Set DNS nameserver
echo "nameserver 8.8.8.8" > /etc/resolv.conf