using CoreMediaUI.Source;
using CoreMediaUI.Source.Api;
using System.Diagnostics;
using System.Net;

namespace CoreMediaUI.Views.Components.Globals;

public partial class MainActions : ContentView
{
	public static bool ApiStarted { get; private set; } = false;
	public static bool WirelessContollerStarted { get; private set; } = false;
	public static bool MouseStarted { get; private set; } = false;

	public const string cmd = "powershell.exe";

	public const string APIROUTE = "C:\\dev\\projects\\media_controller";
	public const string App_env = ".venv\\Scripts\\activate";
	public const string Api_script = "app.py";

	public MainActions()
	{
		InitializeComponent();
		UpdateConnectionState();
	}

	private void UpdateConnectionState() {
		var address = GetDNS.workingAddress;
		WirelessMouseController.label = caca;
		var port = GetDNS.workingAPIPort;
		var mousePort = GetDNS.wiringMouseControllerPort;
		var urlAddress = address != null? $"http://{address}:{port}/" : "No disponible";

		AddressLabel.Text = $"Dirección IP: {address}";
		PortLabel.Text = $"Puerto: {port}";
		CompleteURLLabel.Text = $"URL: {urlAddress}";
		ipPicker.ItemsSource = GetDNS.AvailableAddresses;

		StartAPIButton.IsEnabled = address != null;
		StartMouseButton.IsEnabled = address != null;
	}

    private void StartAPIButton_Clicked(object sender, EventArgs e) {
		if (!ApiStarted) {
			IPAddress? address = GetDNS.workingAddress;
			ushort port = GetDNS.workingAPIPort;
			if (address != null) {
				ApiStarted = Server.TryInitializeServer(address.ToString(), port);
			}
			StartAPIButton.Text = ApiStarted? "Activo" : "Detenido";
			ipPicker.IsEnabled = !ApiStarted && !WirelessContollerStarted;
			return;
		}

		ApiStarted = !Server.TryCloseServer();
        StartAPIButton.Text = ApiStarted ? "Activo" : "Detenido";
        ipPicker.IsEnabled = !ApiStarted && !WirelessContollerStarted;
    }

    private void StartMouseButton_Clicked(object sender, EventArgs e) {
		// TODO: this is not implemented yet, because the component for this is not available
		if (!WirelessContollerStarted) {
			WirelessContollerStarted = WirelessMouseController.TryInitializeService();
			StartMouseButton.Text = WirelessContollerStarted ? "Activo" : "Detenido";
            ipPicker.IsEnabled = !WirelessContollerStarted && !ApiStarted;
            return;
		}
        WirelessContollerStarted = !WirelessMouseController.ShutdownService();
        StartMouseButton.Text = WirelessContollerStarted ? "Activo" : "Detenido";
        ipPicker.IsEnabled = !WirelessContollerStarted && !ApiStarted;
    }

    private void ipPicker_SelectedIndexChanged(object sender, EventArgs e) {
		GetDNS.workingAddress = GetDNS.AvailableAddresses[ipPicker.SelectedIndex];
		UpdateConnectionState();
    }
}