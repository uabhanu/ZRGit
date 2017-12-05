using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerVoice : MonoBehaviour 
{
    AudioSource m_audioSource;

    [SerializeField] AudioClip m_goodLandingArea , m_tennisCourt;

	void Start() 
    {
		m_audioSource = GetComponent<AudioSource>();
        m_audioSource.clip = m_tennisCourt;
        m_audioSource.Play();
	}
  
	void OnFindClearArea() 
    {
        Debug.Log(name + "OnFindClearArea");
        m_audioSource.clip = m_goodLandingArea;
        m_audioSource.Play();

        Invoke("CallHeli" , m_goodLandingArea.length + 1.2f);
	}

    void CallHeli()
    {
        SendMessageUpwards("OnInitialHeliCall");
    }
}
