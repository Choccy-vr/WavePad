#!/bin/bash

set -u

case ${IGconf_device_storage_type:?} in
   sd|emmc|nvme)
      ;;
   *)
      die "Unsupported device storage type: $IGconf_device_storage_type"
      ;;
esac

case ${IGconf_device_sector_size:?} in
   512)
      ;;
   *)
      die "Unsupported device sector size: $IGconf_device_storage_type"
      ;;
esac
