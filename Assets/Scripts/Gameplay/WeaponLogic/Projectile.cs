using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour 
{
    [Range(5, 100)] public float destroyAfter;
    public bool destroyOnImpact = false;
    public float minDestroyTime;
    public float maxDestroyTime;

    [Header("Impact Effect Prefabs")]
    public Transform [] bloodImpactPrefabs;
    public Transform [] metalImpactPrefabs;
    public Transform [] dirtImpactPrefabs;
    public Transform []	concreteImpactPrefabs;

    public void Start()
    {
        StartCoroutine (DestroyAfter ());
    }

    private void OnCollisionEnter (Collision collision)
    {
        if (collision.gameObject.GetComponent<Projectile>() != null)
            return;
        
        if (!destroyOnImpact) 
        {
            StartCoroutine (DestroyTimer ());
        }
        else 
        {
            Destroy (gameObject);
        }

        if (collision.transform.tag == "Enemy") 
        {
            Instantiate (bloodImpactPrefabs [Random.Range 
                    (0, bloodImpactPrefabs.Length)], transform.position, 
                Quaternion.LookRotation (collision.contacts [0].normal));
            Destroy(gameObject);
        }

        if (collision.transform.tag == "Metal") 
        {
            Instantiate (metalImpactPrefabs [Random.Range 
                    (0, metalImpactPrefabs.Length)], transform.position, 
                Quaternion.LookRotation (collision.contacts [0].normal));
            Destroy(gameObject);
        }

        if (collision.transform.tag == "Dirt") 
        {
            Instantiate (dirtImpactPrefabs [Random.Range 
                    (0, dirtImpactPrefabs.Length)], transform.position, 
                Quaternion.LookRotation (collision.contacts [0].normal));
            Destroy(gameObject);
        }

        if (collision.transform.tag == "Concrete") 
        {
            Instantiate (concreteImpactPrefabs [Random.Range 
                    (0, concreteImpactPrefabs.Length)], transform.position, 
                Quaternion.LookRotation (collision.contacts [0].normal));
            Destroy(gameObject);
        }
    }

    private IEnumerator DestroyTimer () 
    {
        yield return new WaitForSeconds
            (Random.Range(minDestroyTime, maxDestroyTime));
        Destroy(gameObject);
    }

    private IEnumerator DestroyAfter () 
    {
        yield return new WaitForSeconds (destroyAfter);
        Destroy (gameObject);
    }
}