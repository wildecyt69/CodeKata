using Core.Api;
using Core.Network.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

//NetworkServiceOptions networkOptions = builder.Configuration.GetOptions<NetworkServiceOptions>(services, "NetworkServices");
//builder.Services.AddApis<BaseAddressAuthorizationMessageHandler>(typeof(IGridDataApi).Assembly, apiUrl);

await builder.Build().RunAsync();
