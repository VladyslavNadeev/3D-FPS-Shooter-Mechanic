using System;
using System.Collections.Generic;
using Logic;
using UnityEngine;
using UnityEngine.Serialization;

namespace StaticData
{
    [CreateAssetMenu(menuName = "StaticData/Level", fileName = "LevelStaticData", order = 0)]
    public class LevelStaticData : ScriptableObject
    {
        public string SceneName;
        public List<EnemySpawnConfig> EnemySpawnConfigs = new();
    }

    [Serializable]
    public class EnemySpawnConfig
    {
        public Vector3 Position;
        public Vector3 Range;
    }
}