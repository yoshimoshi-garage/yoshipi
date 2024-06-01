# Installing Prereqs

Before we can start running our applications, we need to install prerequisite libraries and frameworks on the device and enable some things.  We will do this from the SSH session.

## Install .NET

Create and edit a new shell script
```
$ nano raspi-dotnet.sh
```

```
#!/bin/bash

add_to_bashrc() {
    local string="$1"
    if ! grep -q "$string" "$HOME/.bashrc"; then
        echo "$string" >> $HOME/.bashrc
    fi
}

wget --inet4-only https://dot.net/v1/dotnet-install.sh -O - | bash /dev/stdin --version latest
add_to_bashrc "export DOTNET_ROOT=\$HOME/.dotnet"
add_to_bashrc "export PATH=\$PATH:\$HOME/.dotnet"
source ~/.bashrc
```

<ctrl-s><ctrl-x>

List the local files

```
$ ls -l
total 4
-rw-r--r-- 1 pi pi 364 May 28 20:22 raspi-dotnet.sh
```

Make the shell script executable

```
$ chmod +x raspi-dotnet.sh
$ ls -l
total 4
-rwxr-xr-x 1 pi pi 364 May 28 20:22 raspi-dotnet.sh
```

Execute the script

```
 $ ./raspi-dotnet.sh
```

This take a long time, like over 5 minutes long.  Be patient.  In the end you should see something like

```
dotnet-install: Installation finished successfully.
```

Now re-load your environment and verify the install

```
$ source ~/.bashrc
$ dotnet --version
8.0.301
```

## Enable Hardware Things

Now you have to enable access to things like the hardware busses and GPIO

```
$ sudo raspi-config
```

### Enable SPI0

![](Assets/raspi-config-01.png)
![](Assets/raspi-config-02.png)
![](Assets/raspi-config-03.png)
![](Assets/raspi-config-04.png)

### Enable I2C

![](Assets/raspi-config-05.png)
![](Assets/raspi-config-06.png)
![](Assets/raspi-config-07.png)
![](Assets/raspi-config-08.png)

### Enable Serial Port

![](Assets/raspi-config-09.png)
![](Assets/raspi-config-10.png)
![](Assets/raspi-config-11.png)
![](Assets/raspi-config-12.png)

### Enable SPI1

Enabling the second SPI port (`SPI1`) requires directly modifying the device config file.

```
$ sudo nano /boot/firmware/config.txt
```

Navigate to the bottom and add 
```
dtoverlay=spi0-1cs,cs0_pin=44`
dtoverlay=spi1-1cs,cs0_pin=45`
```

Many of these changes will not take effect until after a reboot.

```
$ sudo reboot
```