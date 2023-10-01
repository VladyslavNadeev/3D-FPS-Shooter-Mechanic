using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Services.Factories.Game
{
    public interface IGameFactory
    {
        GameObject MainPlayer { get; set; }
        GameObject Enemy { get; set; }
        GameObject CreateEnemy(Vector3 position);
        GameObject CreateMainPlayer();
        GameObject CreateMainMenuHud();
        GameObject CreateGameHud();
        GameObject CreateStartupHud();
        GameObject CreateWinWindow();
        GameObject CreateLooseWindow();
        GameObject CreateGameContext();
    }
}