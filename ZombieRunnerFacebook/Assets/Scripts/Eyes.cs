using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyes : MonoBehaviour 
{
    Camera m_eyes;
    float m_defaultFOV;

	void Start() 
    {
		m_eyes = GetComponent<Camera>();
        m_defaultFOV = m_eyes.fieldOfView;
	}

	void Update() 
    {
		if(Time.timeScale == 0)
        {
            return;    
        }

        if(Input.GetButton("Zoom"))
        {
            m_eyes.fieldOfView = m_defaultFOV / 1.5f;
        }
        else
        {
            m_eyes.fieldOfView = m_defaultFOV;
        }
	}
}
