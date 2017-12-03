using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BhanuSpawner : MonoBehaviour 
{
    Transform[] m_spawnPoints;

    [SerializeField] bool m_reSpawn = false;
    [SerializeField] GameObject m_bhanuPrefab;
    [SerializeField] Transform m_bhanuSpawnPoints;

	void Start() 
    {
        m_spawnPoints = m_bhanuSpawnPoints.GetComponentsInChildren<Transform>();
	}

    void Update()
    {
        if(Time.timeScale == 0)
        {
            return;
        }

        ReSpawn();
    }

	void ReSpawn() 
    {
        if(!m_reSpawn)
        {
            int randomValue = Random.Range(1 , m_spawnPoints.Length);   
            GameObject bhanuPlayer = Instantiate(m_bhanuPrefab);
            bhanuPlayer.transform.position = m_spawnPoints[randomValue].transform.position;      
            m_reSpawn = true;
        }
	}
}
