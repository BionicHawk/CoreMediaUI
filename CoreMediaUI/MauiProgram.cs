﻿using CoreMediaUI.Source;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CoreMediaUI {
    public static class MauiProgram {

        public static MauiApp CreateMauiApp() {
            GetDNS.GetAvailableIPV4s();
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts => {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
