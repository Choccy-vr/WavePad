#!/bin/bash
# shellcheck disable=SC2154

set -eu

rootfs=$1
outdir=$2

SYFT_VER=v1.22.0

# If host has syft, use it
if ! hash syft 2>/dev/null; then
   curl -sSfL https://raw.githubusercontent.com/anchore/syft/${SYFT_VER}/install.sh \
      | sh -s -- -b "${IGconf_sys_workdir}"/host/bin
fi

SYFT=$(syft --version 2>/dev/null) || die "syft is unusable"

if igconf isset sbom_syft_config ; then
   SYFTCFG=$(realpath -e "$IGconf_sbom_syft_config") || die "Invalid syft config"
else
   die "No syft config"
fi

msg "SBOM: $SYFT scanning $rootfs"

podman unshare syft -c "$SYFTCFG"  scan dir:"$rootfs" \
   --base-path "$rootfs" \
   --source-name "$IGconf_image_name" \
   --source-version "${IGconf_image_version}" \
   > "${outdir}/${IGconf_image_name}.sbom"
