using Cards.Data.Game;
using Cards.Pages;
using Cards.Pages.Game;
using Cards.Server.Hubs;
using Cards.Services;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace Cards.ComponentBase
{
    public abstract class CardGameBase : Microsoft.AspNetCore.Components.ComponentBase
    {
        protected PlayerList playerList = null!;
        protected Hand hand = null!;
        protected bool inGame;

        [Inject]
        private HttpContextService HttpContextService { get; set; } = null!;

        protected abstract string GameName { get; }

        protected Player? CurrentPlayer => this.playerList.players.SingleOrDefault(player => player.Name == this.HttpContextService.CurrentUser.Name);

        protected bool IsPlaying => this.CurrentPlayer is not null;

        protected bool IsReady => this.IsPlaying & this.CurrentPlayer!.Ready;

        protected bool IsHost => this.IsPlaying & this.playerList.players[0] == this.CurrentPlayer!;

        protected async void Play()
        {
            await this.playerList.hubConnection
                .InvokeAsync("Update", PlayerListHub.Action.Signup, this.HttpContextService.CurrentUser.Name, this.GameName);
            this.StateHasChanged();
        }

        protected async void Quit()
        {
            await this.playerList.hubConnection
                .InvokeAsync("Update", PlayerListHub.Action.Quit, this.HttpContextService.CurrentUser.Name, this.GameName);
            this.StateHasChanged();
        }

        protected async void Ready()
        {
            await this.playerList.hubConnection
                .InvokeAsync("Update", PlayerListHub.Action.Ready, this.HttpContextService.CurrentUser.Name, this.GameName);
            this.StateHasChanged();
        }

        protected async void Unready()
        {
            await this.playerList.hubConnection
                .InvokeAsync("Update", PlayerListHub.Action.Unready, this.HttpContextService.CurrentUser.Name, this.GameName);
            this.StateHasChanged();
        }

        protected abstract void Launch();
    }
}
