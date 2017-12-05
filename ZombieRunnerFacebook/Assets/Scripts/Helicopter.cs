using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter : MonoBehaviour 
{
    AudioSource[] m_audioSources;
    bool m_called = false;
    Rigidbody m_helicopterBody;
    
    [SerializeField] AudioClip m_callSound; 

	void Start() 
    {
		m_audioSources = GetComponents<AudioSource>();
        m_helicopterBody = GetComponent<Rigidbody>();
	}

    public void Call()
    {
        if(!m_called)
        {
            m_called = true;
            m_audioSources[1].clip = m_callSound;
            m_audioSources[1].Play();
            m_helicopterBody.velocity = new Vector3(0f , 0f , 50f);   
        }
    }
}
