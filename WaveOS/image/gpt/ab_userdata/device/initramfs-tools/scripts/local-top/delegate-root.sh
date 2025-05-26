#!/bin/sh

export "PATH=/usr/bin:$PATH"

case $1 in
   prereqs) echo ""; exit 0;;
esac

. /scripts/functions

# Get the fully qualified matching slot device for the current boot
# partition. Only one way out if this fails.
SROOT=$(rpi-slot -f -m)

if [ $? -eq 0 -a -b "$SROOT" ] ; then
   log_success_msg "AB: delegated $SROOT"
   echo "ROOT=$SROOT" >> /conf/param.conf
else
   log_failure_msg "AB: no delegate! To infinity, and beyond!!"
   reboot -f
fi
