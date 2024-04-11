using CoreMediaUI.Source;
using System.Diagnostics;

namespace CoreMediaUI.Views.Components.Globals;

public partial class MainActions : ContentView
{
	public static bool ApiStarted { get; private set; } = false;
	public static bool MouseStarted { get; private set; } = false;

	public const string cmd = "powershell.exe";

	public const string APIROUTE = "C:\\dev\\projects\\media_controller";
	public const string App_env = ".venv\\Scripts\\activate";
	public const string Api_script = "app.py";

	private Process? server { get; set; } = null;

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

	public void InitializeEnv() {
		var cmdPI = new ProcessStartInfo() {
			UseShellExecute = false,
			RedirectStandardOutput = false,
			RedirectStandardInput = true,
			RedirectStandardError = false,
			CreateNoWindow = false,
			WorkingDirectory = APIROUTE,
			FileName = cmd
		};

		server = new Process();
		server.StartInfo = cmdPI;
		server?.Start();
	}

	public void InitializeServer() {
		InitializeEnv();
		server?.StandardInput.WriteLine(App_env);
		server?.StandardInput.WriteLine($"python {Api_script} {GetDNS.workingAddress} {GetDNS.workingAPIPort}");
		ApiStarted = true;
	}

    private void StartAPIButton_Clicked(object sender, EventArgs e) {
		if (!ApiStarted) {
			InitializeServer();
			StartAPIButton.Text = "Activo";
			return;
		}
		server?.StandardInput.WriteLine("\x3");
		server?.Close();
		StartAPIButton.Text = "Detenido";
		ApiStarted = false;
    }

    private void StartMouseButton_Clicked(object sender, EventArgs e) {

    }
}