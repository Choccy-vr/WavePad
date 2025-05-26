#!/bin/bash

set -u

case ${IGconf_device_storage_type:?} in
   sd|emmc|nvme)
      ;;
   *)
      die "Unsupported device storage type: $IGconf_device_storage_type"
      ;;
esac
