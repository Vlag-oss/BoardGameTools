using BoardGameTools.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BoardGameTools.Client.ViewModels;
using BoardGameTools.Client.ViewModels.Interfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddTransient<IHeroesViewModel, HeroesViewModel>();
builder.Services.AddTransient<ICharacteristicViewModel, CharacteristicViewModel>();
builder.Services.AddTransient<IMonsterViewModel, MonsterViewModel>();
builder.Services.AddTransient<IFightViewModel, FightViewModel>();

await builder.Build().RunAsync();
