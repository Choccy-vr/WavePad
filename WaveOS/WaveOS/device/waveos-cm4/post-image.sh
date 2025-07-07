#!/bin/bash

# Mark first boot script as executable
chmod +x ${TARGET_DIR}/usr/local/bin/first-boot-setup.sh

# Enable the first-boot service
chroot ${TARGET_DIR} systemctl enable first-boot.service