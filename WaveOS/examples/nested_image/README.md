Build a system with a squashfs image that's resident in the rootfs and which gets mounted at boot.
There are a few different ways this could be accomplished.
Please make sure to install the dependencies required by the example.

```bash
sudo ./install_deps.sh examples/nested_image/deps

./build.sh -D ./examples/nested_image/ -c nested -o ./examples/nested_image/my.options
```
