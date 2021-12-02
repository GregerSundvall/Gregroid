using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour
{
    public GameObject ParticlePrefab;
    private float timer;

    private ConcurrentQueue<GameObject> freeGameObjectsPool;

    
    private void Start()
    {
        ObjectPoolWarmUp();
    }
    
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0.0f) // first frame and every 100-500ms
        {
            for (var i = 0; i < 300; ++i)
            {
                var go = ObjectPoolSpawn();
                go.GetComponent<Particle>().ParticleStart(this);
                
                // random location
                go.transform.position = new Vector3(
                    Random.Range(0, 32.0f),
                    Random.Range(0f, 18.0f),
                    Random.Range(-5.0f, 30.0f));
            }

            timer = Random.Range(0.1f, 0.5f);
        }
    }
    
    private void ObjectPoolWarmUp()
    {
        freeGameObjectsPool = new ConcurrentQueue<GameObject>();
        
        for (var i = 0; i < 10000; ++i)
        {
            var go = Instantiate(ParticlePrefab);
            go.SetActive(false);
            freeGameObjectsPool.Enqueue(go);
        }
    }

    public GameObject ObjectPoolSpawn()
    {
        if (freeGameObjectsPool.TryDequeue(out var result))
        {
            result.SetActive(true);
            return result;
        }

        // of we can return null if our pool is "fixed" size
        return Instantiate(ParticlePrefab);
    }
    
    public void ObjectPoolReturn(GameObject go)
    {
        go.SetActive(false);
        freeGameObjectsPool.Enqueue(go);
    }
    
    
}
