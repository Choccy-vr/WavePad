#!/bin/bash

set -u

if igconf isset image_pmap ; then
   [[ -d "${IGIMAGE}/device" ]] || die "No device directory for pmap $IGconf_image_pmap"
   [[ -f "${IGIMAGE}/device/provisionmap-${IGconf_image_pmap}.json" ]] || die "pmap not found"
fi
