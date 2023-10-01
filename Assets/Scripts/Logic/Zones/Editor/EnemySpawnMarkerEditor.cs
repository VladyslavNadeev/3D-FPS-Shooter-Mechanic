using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemySpawnMarker))]
public class EnemySpawnMarkerEditor : Editor
{
    [DrawGizmo(GizmoType.NotInSelectionHierarchy | GizmoType.Selected)]
    private static void GizmoTest(EnemySpawnMarker marker, GizmoType aGizmoType)
    {
        if (marker == null) 
            return;
            
        Gizmos.color = Color.red;

        Gizmos.DrawCube(marker.transform.position, Vector3.one);
    }
}