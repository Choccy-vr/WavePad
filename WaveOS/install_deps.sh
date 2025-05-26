#!/bin/bash

set -eu

IGTOP=$(readlink -f "$(dirname "$0")")
source "${IGTOP}/scripts/dependencies_check"
depf=("${IGTOP}/depends")
for f in "$@" ; do
   depf+=($(realpath -e "$f"))
done
dependencies_install "${depf[@]}"
