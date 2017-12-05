using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter : MonoBehaviour 
{
    bool m_called = false;
    Rigidbody m_helicopterBody;

	void Start() 
    {
        m_helicopterBody = GetComponent<Rigidbody>();
	}

    void OnDispatch()
    {
        m_called = true;
        m_helicopterBody.velocity = new Vector3(0f , 0f , 50f);      
    }
}
