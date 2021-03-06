using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Caf.Midden.Wasm.Services;
using Caf.Midden.Core.Services.Configuration;
using Microsoft.JSInterop;
using Caf.Midden.Core.Services;

namespace Caf.Midden.Wasm
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(
                sp => new HttpClient { 
                    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) 
            });

            builder.Services.AddScoped<StateContainer>();

            builder.Services.AddScoped<IReadConfiguration>(
                sp => new ConfigurationReaderHttp(
                    sp.GetRequiredService<HttpClient>(),
                    "app-config.json"));

            builder.Services.AddScoped<IReadCatalog>(
                sp => new CatalogReaderHttp(
                    sp.GetRequiredService<HttpClient>()));

            builder.Services.AddAntDesign();

            await builder.Build().RunAsync();
        }
    }
}
