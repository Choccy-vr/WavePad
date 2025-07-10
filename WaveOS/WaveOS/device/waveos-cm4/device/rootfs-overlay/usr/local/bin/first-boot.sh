#!/bin/bash

FIRST_BOOT_FLAG="/var/lib/first-boot-complete"
OOBE_FLAG="/var/lib/oobe-complete" 

if [ ! -f "$FIRST_BOOT_FLAG" ]; then
    echo "=== FIRST BOOT DETECTED ==="
    # System setup (format storage, create users, etc.)

    # Create a new partition (assuming /dev/mmcblk0p3 is available)
    fdisk /dev/mmcblk0  # Create partition 3

    # Format the new partition
    mkfs.fat -F32 /dev/mmcblk0p3

    # Mount it permanently
    echo "/dev/mmcblk0p3 /mnt/macropad_storage vfat defaults 0 0" >> /etc/fstab
    mount /mnt/WavePad_ext_storage
    
    # Enable USB gadget on next boot (OOBE should restart system before going to normal operation)
    systemctl enable usb-gadget.service
    
    # Mark first boot complete
    touch "$FIRST_BOOT_FLAG"
    
    echo "=== FIRST BOOT SETUP COMPLETE ==="
fi

if [ ! -f "$OOBE_FLAG" ]; then
    echo "=== STARTING OOBE ==="
    # Out of box experience
    systemctl start OOBE.service
else
    echo "=== STARTING NORMAL OPERATION ==="
    # Start normal opperation
    systemctl start WavePad.service
fi