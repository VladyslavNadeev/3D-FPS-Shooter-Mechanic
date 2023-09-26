using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Services.Factories.Game
{
    public interface IGameFactory
    {
        GameObject CreateGameHud();
        GameObject CreateStartupHud();
        GameObject CreateWinWindow();
        GameObject CreateLooseWindow();
    }
}