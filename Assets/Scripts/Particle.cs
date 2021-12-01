using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    
    
    private float deadline;
    private Particles currentSpawner;
    

    void Update()
    {
        deadline -= Time.deltaTime;
        if (deadline < 0.0)
            currentSpawner.ObjectPoolReturn(gameObject);
        
        transform.Rotate(100.0f * Time.deltaTime, 0f, 0f);
        transform.position += Vector3.down * Time.deltaTime;
    }
    
    public void ParticleStart(Particles spawner)
    {
        deadline = 1.0f;
        currentSpawner = spawner;
    }
}
