#!/bin/sh

set -eu

rootfs=$1
genimg_in=$2


echo "BOOTA_UUID=\"$(uuidgen | sed 's/-.*//')"\" > ${genimg_in}/fs_uuids
echo "BOOTB_UUID=\"$(uuidgen | sed 's/-.*//')"\" >> ${genimg_in}/fs_uuids
echo "SYSTEMA_UUID=\"$(uuidgen)"\" >> ${genimg_in}/fs_uuids
echo "SYSTEMB_UUID=\"$(uuidgen)"\" >> ${genimg_in}/fs_uuids
. ${genimg_in}/fs_uuids


# Set up the partition layout for tryboot support. Partition numbering
# relates directly to the layout in genimage.cfg.in

cat << EOF > "${genimg_in}/autoboot.txt"
[all]
tryboot_a_b=1
boot_partition=2
[tryboot]
boot_partition=3
EOF

cat genimage.cfg.in | sed \
   -e "s|<IMAGE_DIR>|$IGconf_sys_outputdir|g" \
   -e "s|<IMAGE_NAME>|$IGconf_image_name|g" \
   -e "s|<IMAGE_SUFFIX>|$IGconf_image_suffix|g" \
   -e "s|<FW_SIZE>|$IGconf_image_boot_part_size|g" \
   -e "s|<SYSTEM_SIZE>|$IGconf_image_system_part_size|g" \
   -e "s|<SLOTP>|'$(readlink -ef slot-post-process.sh)'|g" \
   -e "s|<MKE2FSCONF>|'$(readlink -ef mke2fs.conf)'|g" \
   -e "s|<BOOTA_UUID>|$BOOTA_UUID|g" \
   -e "s|<BOOTB_UUID>|$BOOTB_UUID|g" \
   -e "s|<SYSTEMA_UUID>|$SYSTEMA_UUID|g" \
   -e "s|<SYSTEMB_UUID>|$SYSTEMB_UUID|g" \
   > ${genimg_in}/genimage.cfg
