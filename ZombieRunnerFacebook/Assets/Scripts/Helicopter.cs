using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter : MonoBehaviour 
{
    bool m_called = false;
    
    [SerializeField] AudioClip m_callSound; 

	void Start() 
    {
		
	}

	void Update() 
    {
		if(Time.timeScale == 0)
            return;

        if(Input.GetButtonDown("CallHeli") && !m_called)
        {
            m_called = true;
        }
	}
}
