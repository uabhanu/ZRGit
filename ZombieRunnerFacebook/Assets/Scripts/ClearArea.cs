using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearArea : MonoBehaviour 
{
    [SerializeField] float m_timeSinceLastTrigger = 0f;

	void Start() 
    {
		
	}

	void Update() 
    {
		if(Time.timeScale == 0)
            return;

        m_timeSinceLastTrigger += Time.deltaTime;

        if(m_timeSinceLastTrigger > 1.2f)
        {
            Debug.Log("Found Clear Area");
        }
	}

    void OnTriggerStay()
    {
        m_timeSinceLastTrigger = 0f;
    }
}
