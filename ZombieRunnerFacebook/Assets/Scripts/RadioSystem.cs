using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioSystem : MonoBehaviour 
{
    AudioSource m_audioSource;

    [SerializeField] AudioClip m_callReply , m_heliCall;

	void Start() 
    {
		m_audioSource = GetComponent<AudioSource>();
	}

    void OnInitialHeliCall() 
    {
        Debug.Log(name + "OnInitialHeliCall");
        m_audioSource.clip = m_heliCall;
        m_audioSource.Play();

        Invoke("Reply" , m_heliCall.length + 1.2f);
	}

    void Reply()
    {
        m_audioSource.clip = m_callReply;
        m_audioSource.Play();
        BroadcastMessage("OnDispatch");
    }
}
