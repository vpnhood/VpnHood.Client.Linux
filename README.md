# VpnHood.Client.Linux
This repository serves as a simple scaffold for contributors developing the VpnHood Client on Linux.

1) Build Project
1) Publish for Linux
1) Run VpnHoodClient on Linux
1) You can open the UI via any Web Browser. http://127.10.10.10:9090
1) You can not connect unless LinuxPacketCapture is implemented.

![image](https://github.com/vpnhood/VpnHood.Client.Linux/assets/72891124/ef7c6613-215c-465e-b679-c6d8d619243e)

To implement LinuxPacketCapture, open LinuxPacketCapture and just implement methods with NotImplementedException. They are:
* SendPacket
* Device_OnPacketArrival
* StartCapture
* StopCapture

