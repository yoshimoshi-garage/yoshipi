# Automatically Running you App on Boot

Once you have your application writting, you will typically want it to launch when the device powers up.

We recommend you do this with `systemd`

Create a unit file:

```
sudo touch /lib/systemd/system/yoshipi.service
sudo chmod 644 /lib/systemd/system/yoshipi.service
sudo nano /lib/systemd/system/yoshipi.service
```

Add the following:

```
[Unit]
Description=WateringCan
After=multi-user.target

[Service]
User=pi
Type=idle
WorkingDirectory=/home/pi
ExecStart=/home/pi/.dotnet/dotnet /home/pi/wateringcan/App.dll

[Install]
WantedBy=multi-user.target
```

Save

`<ctrl-s><ctrl-x>`


```
sudo systemctl enable sample.service
sudo systemctl enable yoshipi.service
sudo systemctl start yoshipi.service
```

## Debugging an auto-run app

You can view the output from your service by using `journalctl` and specifying only your service

```
 journalctl -u yoshipi.service
```

You can watch output continuously at runtime by adding a `follow` parameter to the call

```
 journalctl -u yoshipi.service -f
```

## Stopping and Disabling an auto-run app

You can stop execution of the service with `systemctl`

```
sudo systemctl stop yoshipi.service
```

You can completely disable it (so prevent further auto-run) with `systemctl`

```
sudo systemctl disable yoshipi.service
```