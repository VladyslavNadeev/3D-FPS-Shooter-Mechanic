using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StaticData.Editor
{
    [CustomEditor(typeof(LevelStaticData))]
    public class LevelStaticDataEditor : UnityEditor.Editor
    {
        private LevelStaticData _levelData;

        private void OnEnable()
        {
            _levelData = (LevelStaticData)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if (GUILayout.Button("Refresh"))
            {
                RefreshData(_levelData);
                EditorUtility.SetDirty(_levelData);
            }
        }

        private void RefreshData(LevelStaticData levelStaticData)
        {
            levelStaticData.SceneName = SceneManager.GetActiveScene().name;

            levelStaticData.EnemySpawnConfigs = GameObject.FindObjectsOfType<EnemySpawnMarker>()
                .Select(x => new EnemySpawnConfig()
                {
                    Position = x.transform.position
                })
                .ToList();
        }
    }
}