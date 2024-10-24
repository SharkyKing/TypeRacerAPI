using TypeRacerAPI.BaseClasses;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;


namespace TypeRacerAPI.AbstractFactory.Game
{
    public class GameFactory : IGameFactory
    {
        public GameBase CreateBeginnerGame()
        {
            GameBase game = new BeginnerGame();
            return game;
        }

        public GameBase CreateNormalGame()
        {
            GameBase game = new NormalGame();
            return game;
        }

        public GameBase CreateAdvancedGame()
        {
            GameBase game = new AdvancedGame();
            return game;
        }
    }
}
