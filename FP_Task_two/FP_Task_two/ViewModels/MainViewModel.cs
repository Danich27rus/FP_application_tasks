using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Packets.Ethernet;
using FP_Task_two.Utilities;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;
using System.ComponentModel;
using System.Xml.Linq;
using System.Windows.Controls;
using System.IO;
using System.Threading.Tasks;
using PcapDotNet.Packets.Arp;
using System.Windows.Threading;

namespace FP_Task_two.ViewModels
{

    //private UdpClient Udp { get; set; }

    public class MainViewModel : BaseViewModel, IDataErrorInfo
    {

        #region Приватные переменные
        private int _sentPackets;
        private int _receivedPackets;
        private int _lostPackets;
        private String _logBox;
        private bool _isConnected;
        private bool _isFilterActive;
        private bool _highPayload;

        private String _ipSource;
        private String _macSource;
        private int _portSource;
        private int _portDestination;
        private bool _isEnabled;

        private MacAddress sourceMac;
        private MacAddress targetMac;
        private IPAddress sourceIp;
        private IPAddress targetIp;

        private byte[] _aLotData;
        //private String _ipDestination;
        //private String _macDestination;

        private PacketBuilder _builder;
        private PacketDevice _selectedDevice;
        private PacketCommunicator _communicator;
        private string _device;
        #endregion

        public IList<LivePacketDevice> AllDevices { get; set; }

        #region Публичные переменные
        public String LogBox
        {
            get => _logBox;
            set => RaisePropertyChanged(ref _logBox, value);
        }

        public String IpSource
        {
            get => _ipSource;
            set => RaisePropertyChanged(ref _ipSource, value);
        }

        public String MacSource
        {
            get => _macSource;
            set => RaisePropertyChanged(ref _macSource, value);
        }

        public String IpDestination
        {
            get => _ipSource;
            set => RaisePropertyChanged(ref _ipSource, value);
        }

        public String MacDestination
        {
            get => _macSource;
            set => RaisePropertyChanged(ref _macSource, value);
        }

        public bool IsConnected
        {
            get => _isConnected;
            set => RaisePropertyChanged(ref _isConnected, value);
        }

        public bool IsFilterActive
        {
            get => _isFilterActive;
            set => RaisePropertyChanged(ref _isFilterActive, value);
        }

        public bool HighPayload
        {
            get => _highPayload;
            set => RaisePropertyChanged(ref _highPayload, value);
        }

        public string Device
        {
            get => _device;
            set => RaisePropertyChanged(ref _device, value);
        }

        public int PortSource
        {
            get => _portSource;
            set => RaisePropertyChanged(ref _portSource, value);
        }

        public int PortDestination
        {
            get => _portDestination;
            set => RaisePropertyChanged(ref _portDestination, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => RaisePropertyChanged(ref _isEnabled, value);
        }

        public int SentPackets
        {
            get => _sentPackets;
            set => RaisePropertyChanged(ref _sentPackets, value);
        }

        public int ReceivedPackets
        {
            get => _receivedPackets;
            set => RaisePropertyChanged(ref _receivedPackets, value);
        }

        public int LostPackets
        {
            get => _lostPackets;
            set => RaisePropertyChanged(ref _lostPackets, value);
        }
        #endregion

        #region Комманды
        public Command SendUDPPackageCommand { get; }

        public Command ConnectUDPCommand { get; }
        #endregion

        #region Валидация полей через IDataErrorInfo

        string IDataErrorInfo.Error
        {
            get { return null; }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case "PortSource":
                        if ((PortSource < 0) || (PortSource > 65535))
                        {
                            error = "Порт должен быть в районе от 0 до 65535";
                        }
                        break;
                    case "PortDestination":
                        if ((PortDestination < 0) || (PortDestination > 65535))
                        {
                            error = "Порт должен быть в районе от 0 до 65535";
                        }
                        break;
                    case "IpSource":
                        if (IpSource != null)
                        {
                            if (IpSource.Split('.').Length != 4)
                            {
                                error = "Неккоректно введён IP";
                            }
                            //проверка что при записи вида '1.1.1.' после крайней точки не пустота
                            if (IpSource.Split('.').Length > 3)
                            {
                                if (IpSource.Split('.')[3] == "")
                                {
                                    error = "Неккоректно введён IP";
                                }
                            }
                        }
                        break;
                    case "IpDestination":
                        if (IpDestination != null)
                        {
                            if (IpDestination.Split('.').Length != 4)
                            {
                                error = "Неккоректно введён IP";
                            }
                            //проверка что при записи вида '1.1.1.' после крайней точки не пустота
                            if (IpSource.Split('.').Length > 3)
                            {
                                if (IpSource.Split('.')[3] == "")
                                {
                                    error = "Неккоректно введён IP";
                                }
                            }
                        }
                        break;
                    case "MacSource":
                        if (MacSource != null)
                        {
                            if (MacSource.Split(':').Length != 6)
                            {
                                error = "Неккоректно введён MAC-адрес";
                            }
                            //проверка что при записи вида '1.1.1.' после крайней точки не пустота
                            for (int i = 0; i < MacSource.Split(':').Length; ++i)
                            {
                                if (MacSource.Split(':')[i] == "")
                                {
                                    error = "Неккоректно введён MAC-адрес";
                                }
                            }
                        }
                        break;
                    case "MacDestination":
                        if (MacDestination != null)
                        {
                            if (MacDestination.Split(':').Length != 6)
                            {
                                error = "Неккоректно введён MAC-адрес";
                            }
                            //проверка что при записи вида '1.1.1.' после крайней точки не пустота
                            for (int i = 0; i < MacSource.Split(':').Length; ++i)
                            {
                                if (MacSource.Split(':')[i] == "")
                                {
                                    error = "Неккоректно введён MAC-адрес";
                                }
                            }
                        }
                        break;
                }
                return error;
            }
        }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }
        #endregion

        public MainViewModel()
        {
            AllDevices = LivePacketDevice.AllLocalMachine;

            if (AllDevices.Count == 0)
            {
                LogBox += "Интерфейсы не были определены! Убедитесь, что WinCap установлен.\n";
                return;
            }

            IsEnabled = !IsEnabled;

            SendUDPPackageCommand = new Command(SendUDPPackage);
            ConnectUDPCommand = new Command(DisconnectConnectUDP);
        }

        public async void SendUDPPackage()
        {
            if (_communicator == null)
            {
                LogBox += $"Соединение не было установлено.\n";
                return;
            }

            if (HighPayload)
            {
                Random random = new Random();
                int payloadBoard = random.Next(15, 1024 * 15);
                // Генерируем случайный размер данных от 1Мб до 1Гб

                for (int payloadSize = 0; payloadSize <= payloadBoard; ++payloadSize)
                {
                    _aLotData = new byte[1024];
                    random.NextBytes(_aLotData);

                    byte[] srcMac = MacSource.Split(':').Select(x => Convert.ToByte(x, 16)).ToArray();
                    byte[] targMac = MacDestination.Split(':').Select(x => Convert.ToByte(x, 16)).ToArray();
                    byte[] srcIP = sourceIp.GetAddressBytes();
                    byte[] destIP = targetIp.GetAddressBytes();

                    _builder = new PacketBuilder(
                            new EthernetLayer
                            {
                                Source = sourceMac,
                                Destination = targetMac,
                                EtherType = EthernetType.None,
                            },
                            new IpV4Layer
                            {
                                Source = new IpV4Address(sourceIp.ToString()),
                                CurrentDestination = new IpV4Address(targetIp.ToString()),
                                Fragmentation = IpV4Fragmentation.None,
                                HeaderChecksum = null,
                                Identification = 123,
                                Options = IpV4Options.None,
                                Protocol = null,
                                Ttl = 128,
                                TypeOfService = 0,
                            },
                            new UdpLayer
                            {
                                SourcePort = (ushort)PortSource,
                                DestinationPort = (ushort)PortDestination,
                                Checksum = null, // Pcap автоматом считает чексумму
                            },
                            new PayloadLayer
                            {
                                Data = new Datagram(_aLotData),
                            });

                    Task send = Task.Run(() =>
                    {
                        Packet packet = _builder.Build(DateTime.Now);
                        _communicator.SendPacket(packet);

                        SentPackets += 1;

                        LogBox += $"UDP пакет был отправлен.\n";
                    });
                    await send;
                }
                return;
            }
            else
            {

                Packet packet = _builder.Build(DateTime.Now);
                Task send = Task.Run(() =>
                {
                    _communicator.SendPacket(packet);

                    SentPackets += 1;

                    LogBox += $"UDP пакет был отправлен.\n";
                });
                await send;
                return;
            }
        }

        public void DisconnectConnectUDP()
        {
            if (IsConnected)
            {
                IsConnected = false;
                _communicator.Break();
                _communicator.Dispose();
                _communicator = null;
                IsEnabled = true;
                return;
            }
            if (Device == null)
            {
                LogBox += $"Устройство не выбрано.\n";
                return;
            }

            IsEnabled = false;
            //MacSource = "01:00:5E:40:98:8F";//"70:2E:22:66:4D:78";
            //MacDestination = "FF:FF:FF:FF:FF:FF";
            //IpSource = "239.192.152.143";
            //IpDestination = "169.254.255.255";

            sourceMac = new MacAddress(MacSource);
            targetMac = new MacAddress(MacDestination);
            sourceIp = IPAddress.Parse(IpSource);
            targetIp = IPAddress.Parse(IpDestination);

            byte[] srcMac = MacSource.Split(':').Select(x => Convert.ToByte(x, 16)).ToArray();
            byte[] targMac = MacDestination.Split(':').Select(x => Convert.ToByte(x, 16)).ToArray();
            byte[] srcIP = sourceIp.GetAddressBytes();
            byte[] destIP = targetIp.GetAddressBytes();

            _builder = new PacketBuilder(
                    new EthernetLayer
                    {
                        Source = sourceMac,
                        Destination = targetMac,
                        EtherType = EthernetType.None,
                    },
                    new IpV4Layer
                    {
                        Source = new IpV4Address(sourceIp.ToString()),
                        CurrentDestination = new IpV4Address(targetIp.ToString()),
                        Fragmentation = IpV4Fragmentation.None,
                        HeaderChecksum = null,
                        Identification = 123,
                        Options = IpV4Options.None,
                        Protocol = null,
                        Ttl = 128,
                        TypeOfService = 0,
                    },
                    new UdpLayer
                    {
                        SourcePort = (ushort)PortSource,
                        DestinationPort = (ushort)PortDestination,
                        Checksum = null, // Pcap автоматом считает чексумму
                    },
                    new PayloadLayer
                    {
                        Data = new Datagram(Encoding.ASCII.GetBytes("Hello, UDP!")),
                    });

            IsConnected = true;
            LivePacketDevice device = AllDevices.Single(s => s.Description == Device);
            _selectedDevice = device; //Vritual Host-Only for tests
            _communicator = _selectedDevice.Open(100, PacketDeviceOpenAttributes.Promiscuous, 1000);


            if (IsFilterActive)
            {
                string filterExpression = $"ether dst {sourceMac}";
                _communicator.SetFilter(filterExpression);
            }

            if (!HighPayload)
            {
                _ = Task.Run(() =>
                {
                    _communicator.ReceivePackets(0, PacketHandler);
                });
            }
        }

        private void PacketHandler(Packet packet)
        {
            ReceivedPackets += 1;

            LogBox += $"UDP пакет был принят (Возможно фрагмент пакета).\n";
            EthernetDatagram ethernetLayer = packet.Ethernet;

            if (ethernetLayer.EtherType == EthernetType.IpV4)
            {
                IpV4Datagram ipV4Layer = packet.Ethernet.IpV4;
                LogBox += $"Протокол передачи: {ipV4Layer.Protocol} \n";

                if (ipV4Layer.Protocol == IpV4Protocol.Udp)
                {
                    UdpDatagram udpLayer = packet.Ethernet.IpV4.Udp;
                    LogBox += $"Данные пакета: {BitConverter.ToString(udpLayer.Payload.ToArray())}\n";
                }
                else if (ipV4Layer.Protocol == IpV4Protocol.Tcp)
                {
                    TcpDatagram tcpLayer = packet.Ethernet.IpV4.Tcp;
                    LogBox += $"Данные пакета: {BitConverter.ToString(tcpLayer.Payload.ToArray())}\n";
                }
            }
        }
    }
}
