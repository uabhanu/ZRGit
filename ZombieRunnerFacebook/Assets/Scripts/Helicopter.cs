using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter : MonoBehaviour 
{
    AudioSource m_audioSource;
    bool m_called = false;
    
    [SerializeField] AudioClip m_callSound; 

	void Start() 
    {
		m_audioSource = GetComponent<AudioSource>();
	}

	void Update() 
    {
		if(Time.timeScale == 0)
            return;

        if(Input.GetButtonDown("CallHeli") && !m_called)
        {
            m_called = true;
            m_audioSource.clip = m_callSound;
            m_audioSource.Play();
        }
	}
}
