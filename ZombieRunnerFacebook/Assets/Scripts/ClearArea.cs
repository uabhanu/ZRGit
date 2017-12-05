using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearArea : MonoBehaviour 
{
    bool m_foundClearArea = false;

    [SerializeField] float m_timeSinceLastTrigger = 0f;
   
	void Update() 
    {
		if(Time.timeScale == 0)
            return;

        m_timeSinceLastTrigger += Time.deltaTime;

        if(m_timeSinceLastTrigger > 1.2f && !m_foundClearArea)
        {
            SendMessageUpwards("OnFindClearArea");
            m_foundClearArea = true;
        }
	}

    void OnTriggerStay(Collider tri)
    {
        if(!tri.tag.Equals("Player"))
        {
            m_timeSinceLastTrigger = 0f;   
        }
    }
}
