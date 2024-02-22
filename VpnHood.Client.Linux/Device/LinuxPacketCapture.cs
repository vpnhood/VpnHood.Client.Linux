using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using PacketDotNet;
using SharpPcap;
using VpnHood.Client.Device;
using VpnHood.Common.Logging;
using VpnHood.Common.Net;

namespace VpnHood.Client.Linux.Device;

public class LinuxPacketCapture : IPacketCapture
{
    private bool _disposed;
    private IpNetwork[]? _includeNetworks;
    public event EventHandler<PacketReceivedEventArgs>? OnPacketReceivedFromInbound;
    public event EventHandler? OnStopped;

    public bool Started => false;
    public virtual bool CanSendPacketToOutbound => true;
    public virtual bool IsDnsServersSupported => false;

    public virtual IPAddress[]? DnsServers
    {
        get => throw new NotSupportedException();
        set => throw new NotSupportedException();
    }

    public virtual bool CanProtectSocket => false;

    public virtual void ProtectSocket(Socket socket)
    {
        throw new NotSupportedException(
            $"{nameof(ProtectSocket)} is not supported by {GetType().Name}");
    }

    public void SendPacketToInbound(IEnumerable<IPPacket> ipPackets)
    {
        foreach (var ipPacket in ipPackets)
            SendPacketToInbound(ipPacket);
    }

    public void SendPacketToInbound(IPPacket ipPacket)
    {
        SendPacket(ipPacket, false);
    }

    public void SendPacketToOutbound(IPPacket ipPacket)
    {
        SendPacket(ipPacket, true);
    }

    public void SendPacketToOutbound(IEnumerable<IPPacket> ipPackets)
    {
        foreach (var ipPacket in ipPackets)
            SendPacket(ipPacket, true);
    }

    public IpNetwork[]? IncludeNetworks
    {
        get => _includeNetworks;
        set
        {
            if (Started)
                throw new InvalidOperationException(
                    $"Can't set {nameof(IncludeNetworks)} when {nameof(LinuxPacketCapture)} is started!");
            _includeNetworks = value;
        }
    }

    // ipPacket must send to the network driver
    private void SendPacket(IPPacket ipPacket, bool outbound)
    {
        // send by a device
        throw new NotImplementedException();
    }

    public void StartCapture()
    {
        throw new NotImplementedException();
    }

    public void StopCapture()
    {
        if (!Started)
            return;

        OnStopped?.Invoke(this, EventArgs.Empty);
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        StopCapture();
        _disposed = true;
    }

    // todo: This event must be raised when a packet is received from the network
    private void Device_OnPacketArrival(object sender, PacketCapture e)
    {
        _ = sender;
        var rawPacket = e.GetPacket();
        var packet = Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
        var ipPacket = packet.Extract<IPPacket>();

        ProcessPacketReceivedFromInbound(ipPacket);
    }


    protected virtual void ProcessPacketReceivedFromInbound(IPPacket ipPacket)
    {
        try
        {
            var eventArgs = new PacketReceivedEventArgs([ipPacket], this);
            OnPacketReceivedFromInbound?.Invoke(this, eventArgs);
        }
        catch (Exception ex)
        {
            VhLogger.Instance.Log(LogLevel.Error, ex,
                "Error in processing packet Packet: {Packet}", VhLogger.FormatIpPacket(ipPacket.ToString()!));
        }
    }

    public bool CanExcludeApps => false;
    public bool CanIncludeApps => false;

    public string[]? ExcludeApps
    {
        get => throw new NotSupportedException();
        set => throw new NotSupportedException();
    }

    public string[]? IncludeApps
    {
        get => throw new NotSupportedException();
        set => throw new NotSupportedException();
    }

    public bool IsMtuSupported => false;

    public int Mtu
    {
        get => throw new NotSupportedException();
        set => throw new NotSupportedException();
    }

    public bool IsAddIpV6AddressSupported => false;
    public bool AddIpV6Address
    {
        get => throw new NotSupportedException();
        set => throw new NotSupportedException();
    }
}