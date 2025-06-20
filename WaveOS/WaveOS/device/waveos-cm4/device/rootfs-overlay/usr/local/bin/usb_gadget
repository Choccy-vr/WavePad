#!/bin/bash

# Load necessary modules
modprobe dwc2
modprobe libcomposite

# Remove any existing gadget
if [ -d /sys/kernel/config/usb_gadget/wavepad ]; then
    echo "" > /sys/kernel/config/usb_gadget/wavepad/UDC
    rm -rf /sys/kernel/config/usb_gadget/wavepad
fi

# Create a gadget
cd /sys/kernel/config/usb_gadget/
mkdir -p wavepad
cd wavepad

# Define USB specifications - customize these for your product
echo 0x1d6b > idVendor      # Linux Foundation (don't use for commercial products)
echo 0x0104 > idProduct     # Multifunction Composite Gadget
echo 0x0100 > bcdDevice     # v1.0.0
echo 0x0200 > bcdUSB        # USB 2.0

# Set customized information
mkdir -p strings/0x409
echo "fedcba9876543210" > strings/0x409/serialnumber
echo "Ginobeano" > strings/0x409/manufacturer
echo "WavePad" > strings/0x409/product

# Create configuration
mkdir -p configs/c.1/strings/0x409
echo "Config 1: Serial and Mass Storage" > configs/c.1/strings/0x409/configuration
echo 500 > configs/c.1/MaxPower  # 500 = 1000mA (USB 2.0 maximum)

# Set up mass storage function with full filesystem access
mkdir -p functions/mass_storage.0

# Create a file-backed mass storage that represents the entire filesystem
# Option 1: Using File-backed storage with access to the entire /home directory
echo /dev/mmcblk0p2 > functions/mass_storage.0/lun.0/file  # Use actual root partition
echo 1 > functions/mass_storage.0/lun.0/removable
echo 0 > functions/mass_storage.0/lun.0/ro  # Set to 0 for read/write, 1 for read-only
echo 0 > functions/mass_storage.0/lun.0/cdrom  # Not a CD-ROM
echo 0 > functions/mass_storage.0/lun.0/nofua  # Force Unit Access for better write sync

ln -s functions/mass_storage.0 configs/c.1/

# Set up serial function
mkdir -p functions/acm.usb0
ln -s functions/acm.usb0 configs/c.1/

# Enable gadget
ls /sys/class/udc > UDC

echo "USB Gadget setup complete - Full filesystem access enabled"