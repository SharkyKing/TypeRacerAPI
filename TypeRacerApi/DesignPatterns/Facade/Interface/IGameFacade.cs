using Microsoft.AspNetCore.SignalR;
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Data;
using TypeRacerAPI.DesignPatterns.Factory.Player;
using TypeRacerAPI.DesignPatterns.Observer;
using TypeRacerAPI.Hubs;

namespace TypeRacerAPI.DesignPatterns.Facade.Interface
{
    public interface IGameFacade
    {
        public ValueTask<GameClass> Execute(string nickName, string socketId, int activeGameType, int activeGameLevel, int gameId);
    }
}
