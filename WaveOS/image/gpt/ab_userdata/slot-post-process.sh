#!/bin/bash

set -u

COMP=$1

echo "pre-process $IMAGEMOUNTPATH for $COMP" 1>&2

case $COMP in
   SYSTEM)
      cat << EOF > $IMAGEMOUNTPATH/etc/fstab
/dev/disk/by-slot/active/system /              ext4 rw,relatime,errors=remount-ro 0 1
/dev/disk/by-slot/active/boot   /boot/firmware vfat defaults,rw,nofail  0 2
LABEL=USERDATA                  /data          ext4 rw,relatime,nofail 0 2
LABEL=BOOTFS                    /bootfs        vfat defaults,rw 0 2
EOF
      ;;
   BOOT)
      sed -i "s|root=\([^ ]*\)|root=\/dev\/ram0|" $IMAGEMOUNTPATH/cmdline.txt
      ;;
   *)
      ;;
esac
