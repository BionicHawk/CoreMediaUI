namespace CoreMediaUI.Views.Components.Globals;

public partial class MouseSettings : ContentView
{
	public static double MouseSensibility { get; private set; } = 100.0;

	public MouseSettings()
	{
		InitializeComponent();
		InitialSettings();
	}

	private void InitialSettings() {
		MouseSensibilitySlider.Value = MouseSensibility;
	}

    private void Slider_ValueChanged(object sender, ValueChangedEventArgs e) {
		MouseSensibility = MouseSensibilitySlider.Value;
    }
}