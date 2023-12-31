using MensaAppKlassenBibliothek;
using Microsoft.Extensions.Logging;

namespace MensaHandyApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            Person person = new Person()
            {
                Email = "testuser@gmx.at",
                Password = "hallo123"
            };
            person.SaveObject();
            
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
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