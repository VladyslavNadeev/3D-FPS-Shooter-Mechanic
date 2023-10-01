using System.Collections;
using UnityEngine;

public class ImpactScript : MonoBehaviour 
{
    [Header("Impact Despawn Timer")]
    [SerializeField] private float _spawnTimer = 10.0f;

    private void Start() 
    {
        StartCoroutine (SpawnTimer ());
    }
	
    private IEnumerator SpawnTimer() 
    {
        yield return new WaitForSeconds (_spawnTimer);
        Destroy (gameObject);
    }
}