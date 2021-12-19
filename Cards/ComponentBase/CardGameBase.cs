using Cards.Data.Game;
using Cards.Pages;
using Cards.Pages.Game;
using Cards.Server.Clients;
using Cards.Server.Hubs;
using Cards.Services;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace Cards.ComponentBase
{
    public abstract class CardGameBase : Microsoft.AspNetCore.Components.ComponentBase, ICardGameClient
    {
        protected PlayerList playerList = null!;
        protected Hand hand = null!;
        protected HubConnection hubConnection = null!;
        protected bool inGame = false;

        [Inject]
        private HttpContextService HttpContextService { get; set; } = null!;
        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;

        protected abstract Game Game { get; }

        protected Player? CurrentPlayer => this.playerList.players.SingleOrDefault(player => player.Name == this.HttpContextService.CurrentUser.Name);

        protected bool IsPlaying => this.CurrentPlayer is not null;

        protected bool IsReady => this.IsPlaying & this.CurrentPlayer!.Ready;

        protected bool IsHost => this.IsPlaying & this.playerList.players[0] == this.CurrentPlayer!;

        protected abstract bool IsGameReady { get; }

        protected override async Task OnInitializedAsync()
        {
            this.hubConnection = new HubConnectionBuilder()
                .WithUrl(this.NavigationManager.ToAbsoluteUri($"/Hubs/{this.Game}"))
                .Build();
            this.hubConnection.On<IEnumerable<Card>>("Start", this.Start);
            await this.hubConnection.StartAsync();
        }

        protected async void Play()
        {
            await this.playerList.hubConnection
                .InvokeAsync("Update", PlayerListHub.Action.Signup, this.HttpContextService.CurrentUser.Name, this.Game);
            this.StateHasChanged();
        }

        protected async void Quit()
        {
            await this.playerList.hubConnection
                .InvokeAsync("Update", PlayerListHub.Action.Quit, this.HttpContextService.CurrentUser.Name, this.Game);
            this.StateHasChanged();
        }

        protected async void Ready()
        {
            await this.playerList.hubConnection
                .InvokeAsync("Update", PlayerListHub.Action.Ready, this.HttpContextService.CurrentUser.Name, this.Game);
            this.StateHasChanged();
        }

        protected async void Unready()
        {
            await this.playerList.hubConnection
                .InvokeAsync("Update", PlayerListHub.Action.Unready, this.HttpContextService.CurrentUser.Name, this.Game);
            this.StateHasChanged();
        }

        public abstract Task Start(IEnumerable<Card> hand);

        protected async void Launch() =>
            await this.hubConnection.InvokeAsync("Launch");
    }
}
