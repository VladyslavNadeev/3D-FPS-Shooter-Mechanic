using System;
using UnityEngine.Serialization;

namespace Infrastructure.Services.PersistenceProgress
{
    [Serializable]
    public class ProgressData
    {
        public Action<int> OnEnemyCountChanged;
        public Action<int> OnLoseCountChanged;
        public Action<int> OnWinCountChanged;
        
        public int Enemies;
        public int WinCount;
        public int LoseCount;

        public void AddEnemyCount(int value)
        {
            Enemies += value;
            OnEnemyCountChanged?.Invoke(Enemies);
        }
        
        public void WithdrawEnemyCount(int value)
        {
            Enemies -= value;
            OnEnemyCountChanged?.Invoke(Enemies);
        }
        
        public void AddWinCount(int value)
        {
            WinCount += value;
            OnWinCountChanged?.Invoke(Enemies);
        }
        
        public void WithdrawWinCount(int value)
        {
            WinCount -= value;
            OnWinCountChanged?.Invoke(Enemies);
        }
        
        public void AddLoseCount(int value)
        {
            LoseCount += value;
            OnLoseCountChanged?.Invoke(Enemies);
        }
        
        public void WithdrawLoseCount(int value)
        {
            LoseCount -= value;
            OnLoseCountChanged?.Invoke(Enemies);
        }
    }
}