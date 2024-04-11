using CoreMediaUI.Source;
using CoreMediaUI.Source.Api;
using System.Diagnostics;
using System.Net;

namespace CoreMediaUI.Views.Components.Globals;

public partial class MainActions : ContentView
{
	public static bool ApiStarted { get; private set; } = false;
	public static bool MouseStarted { get; private set; } = false;

	public const string cmd = "powershell.exe";

	public const string APIROUTE = "C:\\dev\\projects\\media_controller";
	public const string App_env = ".venv\\Scripts\\activate";
	public const string Api_script = "app.py";

	public MainActions()
	{
		InitializeComponent();
		AtStart();
	}

	private void AtStart() {
		GetDNS.GetListDNS();

		var address = GetDNS.workingAddress;
		var port = GetDNS.workingAPIPort;
		var mousePort = GetDNS.wiringMouseControllerPort;
		var urlAddress = $"http://{address}:{port}/";

		AddressLabel.Text += $" {address}";
		PortLabel.Text += $" {port}";
		CompleteURLLabel.Text += $" {urlAddress}";

	}

    private void StartAPIButton_Clicked(object sender, EventArgs e) {
		if (!ApiStarted) {
			IPAddress? address = GetDNS.workingAddress;
			ushort port = GetDNS.workingAPIPort;
			if (address != null) {
				ApiStarted = Server.TryInitializeServer(address.ToString(), port);
			}
			StartAPIButton.Text = ApiStarted? "Activo" : "Detenido";
			return;
		}

		ApiStarted = !Server.TryCloseServer();
        StartAPIButton.Text = ApiStarted ? "Activo" : "Detenido";
    }

    private void StartMouseButton_Clicked(object sender, EventArgs e) {

    }
}