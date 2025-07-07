#!/bin/bash

# Now setup USB gadget
modprobe libcomposite

GADGET_DIR="/sys/kernel/config/usb_gadget"
GADGET_NAME="WavePad"

# Clean up existing
echo "" > ${GADGET_DIR}/${GADGET_NAME}/UDC 2>/dev/null || true
rm -rf ${GADGET_DIR}/${GADGET_NAME} 2>/dev/null || true

mkdir -p ${GADGET_DIR}/${GADGET_NAME}
cd ${GADGET_DIR}/${GADGET_NAME}

# Device identity
echo 0x1d6b > idVendor
echo 0x1781 > idProduct
echo 0x0100 > bcdDevice
echo 0x0200 > bcdUSB  # USB 2.0 because of the CM4 USB controller

# Strings
mkdir -p strings/0x409
echo "WVPD000001$(date +%Y%m%d)" > strings/0x409/serialnumber
echo "Ginobeano" > strings/0x409/manufacturer
echo "WavePad" > strings/0x409/product

# Configuration with 900mA
mkdir -p configs/c.1
mkdir -p configs/c.1/strings/0x409
echo "High Power Config" > configs/c.1/strings/0x409/configuration
echo 900 > configs/c.1/MaxPower  # Request 900mA (USB 3.0 max) because runnning all of the system
echo 0x80 > configs/c.1/bmAttributes  # Bus powered

# HID Keyboard
mkdir -p functions/hid.keyboard
echo 1 > functions/hid.keyboard/protocol
echo 1 > functions/hid.keyboard/subclass
echo 8 > functions/hid.keyboard/report_length

# HID Consumer Control (Media Keys)
mkdir -p functions/hid.consumer
echo 0 > functions/hid.consumer/protocol
echo 0 > functions/hid.consumer/subclass
echo 2 > functions/hid.consumer/report_length

# Mass Storage
mkdir -p functions/mass_storage.flash
echo "/dev/mmcblk0p3" > functions/mass_storage.flash/lun.0/file # change the whatever partition is available
echo 1 > functions/mass_storage.flash/lun.0/removable
echo 0 > functions/mass_storage.flash/lun.0/cdrom
echo 0 > functions/mass_storage.flash/lun.0/ro  # Read-write

# Set options
ln -s functions/hid.keyboard configs/c.1/
ln -s functions/hid.consumer configs/c.1/
ln -s functions/mass_storage.flash configs/c.1/

# Activate USB
UDC_NAME=$(ls /sys/class/udc | head -n1)
echo "$UDC_NAME" > UDC

echo "Setup complete!"