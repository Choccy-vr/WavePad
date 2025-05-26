Build a system with a custom YAML layer to install locally stored Debian packages.

First, copy the .deb files to examples/debstore/pkgs then:

```bash
./build.sh -c deb12-store -D ./examples/debstore/ -o ./examples/debstore/my.options
```

my.options leverages ```IGconf_ext_dir``` which is set and propagated by rpi-image-gen core whenever -D is used. Usage of this variable in the options file allows the location of the locally stored packages in examples/debstore/pkgs to be specified by using ```debstore=${IGconf_ext_dir}/pkgs```. This 'debstore' variable is translated to ```IGconf_debstore``` after the options file is read. ```IGconf_debstore``` is referenced in ```examples/debstore/meta/debstore-installer.yaml``` so it's available to mmdebstrap at rootfs creation time.
