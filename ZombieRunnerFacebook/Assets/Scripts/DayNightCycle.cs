using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour 
{
    Quaternion m_startRotation;

    [Tooltip ("Number of minutes per second that pass, try 60")] [SerializeField] float m_minsPerSec;

	void Start() 
    {
		m_startRotation = transform.rotation;
	}

	void Update() 
    {
		if(Time.timeScale == 0)
            return;

        float angleThisFrame = Time.deltaTime / 360 * m_minsPerSec;
        transform.RotateAround(transform.position , Vector3.forward , angleThisFrame);
	}
}
