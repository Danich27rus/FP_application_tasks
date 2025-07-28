using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace UdpTrafficGeneratorWPF.Services;

public class UdpService
{
    private bool _sending;
    private bool _receiving;

    public void Send(string ip, int port, int bitrateMbps, Action onPacketSent)
    {
        _sending = true;
        var client = new UdpClient();

        int bytesPerSecond = bitrateMbps * 125000;
        byte[] data = new byte[1024];
        new Random().NextBytes(data);

        while (_sending)
        {
            client.Send(data, data.Length, ip, port);
            onPacketSent?.Invoke();
            Thread.Sleep(1000 * data.Length / bytesPerSecond);
        }
    }

    public void Receive(int port, Action<IPAddress, byte[]> onPacketReceived)
    {
        _receiving = true;
        var client = new UdpClient(port);

        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, port);
        while (_receiving)
        {
            var received = client.Receive(ref remoteEP);
            onPacketReceived?.Invoke(remoteEP.Address, received);
        }
    }

    public void Stop() => (_sending, _receiving) = (false, false);
}
