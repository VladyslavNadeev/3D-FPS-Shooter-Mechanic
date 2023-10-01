using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasingScript : MonoBehaviour
{
    [SerializeField] private Rigidbody _casingRigidbody;
    
    [Header("Force X")]
    public float minimumXForce;		
    public float maximumXForce;
    
    [Header("Force Y")]
    public float minimumYForce;
    public float maximumYForce;
    
    [Header("Force Z")]
    public float minimumZForce;
    public float maximumZForce;
    
    [Header("Rotation Force")]
    public float minimumRotation;
    public float maximumRotation;
    
    [Header("Spawn Time")]
    public float spawnTime;

    [Header("Spin Settings")]
    public float speed = 2500.0f;

    public void Init()
    {
        _casingRigidbody.AddRelativeTorque (
            Random.Range(minimumRotation, maximumRotation),
            Random.Range(minimumRotation, maximumRotation),
            Random.Range(minimumRotation, maximumRotation)
            * Time.deltaTime);

        _casingRigidbody.AddRelativeForce (
            Random.Range (minimumXForce, maximumXForce),  
            Random.Range (minimumYForce, maximumYForce),  
            Random.Range (minimumZForce, maximumZForce)); 	
        
        StartCoroutine (RemoveCasing ());
        transform.rotation = Random.rotation;
    }

    private void FixedUpdate () 
    {
        transform.Rotate (Vector3.right, speed * Time.deltaTime);
        transform.Rotate (Vector3.down, speed * Time.deltaTime);
    }

    private IEnumerator RemoveCasing () 
    {
        yield return new WaitForSeconds (spawnTime);
        Destroy (gameObject);
    }
}