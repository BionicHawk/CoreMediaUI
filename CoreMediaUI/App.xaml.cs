namespace CoreMediaUI {
    public partial class App : Application {

        private const int DefaultHeight = 480;
        private const int DefaultWidth = 854;

        public App() {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override Window CreateWindow(IActivationState? activationState) {
            var window = base.CreateWindow(activationState);
            
            window.Width = DefaultWidth;
            window.Height = DefaultHeight;
            
            return window;
        }

    }
}
