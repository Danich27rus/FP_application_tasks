using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Input;
using UdpTrafficGeneratorWPF.Helpers;
using UdpTrafficGeneratorWPF.Services;

namespace UdpTrafficGeneratorWPF.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    public ObservableCollection<NetworkInterface> Interfaces { get; } = new(NetworkInterface.GetAllNetworkInterfaces());
    public NetworkInterface SelectedInterface { get; set; }

    public string TargetIP { get; set; } = "127.0.0.1";
    public int TargetPort { get; set; } = 9000;
    public int BitrateMbps { get; set; } = 10;

    public string SentPacketsText => $"Отправлено: {sentPackets}";
    public string ReceivedPacketsText => $"Получено: {receivedPackets}";

    private int sentPackets;
    private int receivedPackets;

    public ICommand StartSendCommand { get; }
    public ICommand StartReceiveCommand { get; }

    private UdpService udpService = new();

    public MainViewModel()
    {
        StartSendCommand = new RelayCommand(_ => StartSending());
        StartReceiveCommand = new RelayCommand(_ => StartReceiving());
    }

    private async void StartSending()
    {
        sentPackets = 0;
        await Task.Run(() =>
        {
            udpService.Send(TargetIP, TargetPort, BitrateMbps, () =>
            {
                sentPackets++;
                OnPropertyChanged(nameof(SentPacketsText));
            });
        });
    }

    private async void StartReceiving()
    {
        receivedPackets = 0;
        await Task.Run(() =>
        {
            udpService.Receive(TargetPort, (ip, data) =>
            {
                receivedPackets++;
                OnPropertyChanged(nameof(ReceivedPacketsText));
            });
        });
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
