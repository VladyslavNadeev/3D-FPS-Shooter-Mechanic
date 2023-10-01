using System;
using System.Collections.Generic;
using Infrastructure.Services.Factories.Game;
using Infrastructure.Services.PersistenceProgress;
using UnityEngine;


public class GameContext : MonoBehaviour
{
    private IPersistenceProgressService _persistenceProgressService;
    private IGameFactory _gameFactory;
    private PlayerVitality _playerVitality;
    private List<Enemy> _enemies;
    private Player _player;
    private PlayerMovement _playerMovement;
    private LookCamera _lookCamera;
    private bool _hasWon;
    private bool _hasLose;
    private GameObject _gameHud;

    public void Init(
        IPersistenceProgressService persistenceProgressService,
        IGameFactory gameFactory,
        PlayerVitality playerVitality,
        Player player,
        PlayerMovement playerMovement,
        List<Enemy> enemies,
        LookCamera lookCamera,
        GameObject gameHud)
    {
        _gameHud = gameHud;
        _lookCamera = lookCamera;
        _playerMovement = playerMovement;
        _player = player;
        _enemies = enemies;
        _playerVitality = playerVitality;
        _gameFactory = gameFactory;
        _persistenceProgressService = persistenceProgressService;
        _persistenceProgressService.PlayerData.Progress.OnEnemyCountChanged += CheckConditionByEnemyCount;
        _playerVitality.OnDeath += Loose;
    }

    private void Loose(Vector3 position)
    {
        if (!_hasLose)
        {
            Destroy(_gameHud);
            Cursor.visible = true;
            _player.CursorLocked = false;
            _player.UpdateCursorState();
            foreach (var enemy in _enemies)
            {
                if (enemy != null)
                {
                    enemy.enabled = false;
                }
            }
            _player.enabled = false;
            _playerMovement.enabled = false;
            _lookCamera.enabled = false;
            _persistenceProgressService.PlayerData.Progress.AddLoseCount(1);
            _gameFactory.CreateLooseWindow();
            _hasLose = true;
        }
    }

    private void CheckConditionByEnemyCount(int enemyCount)
    {
        if (!_hasWon && enemyCount == 0)
        {
            Win();
            _hasWon = true;
        }
    }

    private void Win()
    {
        Destroy(_gameHud);
        Cursor.visible = true;
        if (_player != null)
        {
            _player.CursorLocked = false;
            _player.UpdateCursorState();
            _player.enabled = false;
        }
    
        if (_playerMovement != null)
        {
            _playerMovement.enabled = false;
        }
    
        if (_lookCamera != null)
        {
            _lookCamera.enabled = false;
        }

        _persistenceProgressService.PlayerData.Progress.AddWinCount(1);
        _gameFactory.CreateWinWindow();
    }

    private void OnDestroy()
    {
        _playerVitality.OnDeath -= Loose;
    }
}