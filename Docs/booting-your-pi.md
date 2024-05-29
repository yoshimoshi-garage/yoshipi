# Booting your YoshiPi

Insert the SD card you created (insert link)

[insert picture]

Power your Raspberry Pi Zero 2 W.

It is not required, but is often helpful at this point to have a monitor connected to your device via HDMI to allow you to see boot progress, monitor any boot errors, and to see the IP address assigned to the device.  If you have difficulties connecting to your device, connect a monitor.

First boot will take a while (~2 minutes).  The device will generate SSH keys, resize your SD card volume and reboot a couple times.  At the end, if all went well, it will come to a login prompt, and just above that prompt will be the DNS-assigned IP address.

[insert picture]

At this point, if you assigned a hostname to the device during OS setup, from any PC on your network you should be able to ping the device.

now, from a terminal on a dev PC, SSH into your Raspberry Pi Zero 2 W.

```
> ssh pi@192.168.4.69
The authenticity of host '192.168.4.69 (192.168.4.69)' can't be established.
ED25519 key fingerprint is SHA256:yUApFvHXHqXNQQ/+MlwIXWA5wEWRO1cSMjizvYfrJ0I.
This key is not known by any other names
Are you sure you want to continue connecting (yes/no/[fingerprint])? yes
pi@192.168.4.69's password:
Linux yoshipi 6.6.20+rpt-rpi-v8 #1 SMP PREEMPT Debian 1:6.6.20-1+rpt1 (2024-03-07) aarch64

The programs included with the Debian GNU/Linux system are free software;
the exact distribution terms for each program are described in the
individual files in /usr/share/doc/*/copyright.

Debian GNU/Linux comes with ABSOLUTELY NO WARRANTY, to the extent
permitted by applicable law.
pi@yoshipi:~ $
```

