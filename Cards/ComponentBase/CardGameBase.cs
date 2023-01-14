using Cards.Data.Game;
using Cards.Pages;
using Cards.Pages.Game;
using Cards.Server.Hubs;
using Cards.Services;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace Cards.ComponentBase
{
    public abstract class CardGameBase : Microsoft.AspNetCore.Components.ComponentBase, IAsyncDisposable
    {
        protected HubConnection hubConnection = null!;
        protected PlayerList playerList = null!;
        protected Hand hand = null!;
        protected readonly bool inGame;

        protected CardGameBase()
        {
        }
        
        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        protected HttpContextService HttpContextService { get; set; } = default!;
        
        protected abstract string GameName { get; }

        protected Player? CurrentPlayer => this.playerList.players.SingleOrDefault(player => player.Name == this.HttpContextService.CurrentUser.Name);

        protected bool IsPlaying => this.CurrentPlayer is not null;

        protected bool IsReady => this.IsPlaying & this.CurrentPlayer!.Ready;

        protected bool IsHost => this.IsPlaying & this.playerList.players[0] == this.CurrentPlayer!;

        protected override async Task OnInitializedAsync()
        {
            this.hubConnection = new HubConnectionBuilder()
                .WithUrl(this.NavigationManager.ToAbsoluteUri("/Hubs/PlayerList"))
                .Build();

            await this.hubConnection.StartAsync();
            await this.hubConnection.InvokeAsync("EnterLobby", this.GameName);
        }

        protected async void Play() =>
            await this.playerList.hubConnection
                .InvokeAsync("Update", PlayerListAction.Signup, this.HttpContextService.CurrentUser.Name, this.GameName);

        protected async void Quit() =>
            await this.playerList.hubConnection
                .InvokeAsync("Update", PlayerListAction.Quit, this.HttpContextService.CurrentUser.Name, this.GameName);

        protected async void Ready() =>
            await this.playerList.hubConnection
                .InvokeAsync("Update", PlayerListAction.Ready, this.HttpContextService.CurrentUser.Name, this.GameName);

        protected async void Unready() =>
            await this.playerList.hubConnection
                .InvokeAsync("Update", PlayerListAction.Unready, this.HttpContextService.CurrentUser.Name, this.GameName);

        public async ValueTask DisposeAsync()
        {
            if (this.hubConnection is not null)
                await this.hubConnection.DisposeAsync();
        }
    }
}
