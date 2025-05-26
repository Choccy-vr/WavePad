#!/bin/sh

set -eu

ROOTFS_DIR="$1"

# Clean up stuff we don't need
rm -rf $1/etc/apt
rm -rf $1/etc/dpkg
rm -rf $1/usr/bin/dpkg
rm -rf $1/var/cache/*

# Exit if not specified
if [ -z "$ROOTFS_DIR" ]; then
  echo "Usage: $0 <rootfs_directory>"
  exit 1
fi
