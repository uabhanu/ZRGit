using System;
using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class Zombie : MonoBehaviour
    {
        Transform m_bhanuTrans;

        public UnityEngine.AI.NavMeshAgent m_agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter m_character { get; private set; } // the character we are controlling
        public Transform m_target;                                    // target to aim for


        void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            m_agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            m_character = GetComponent<ThirdPersonCharacter>();

            m_agent.updateRotation = false;
            m_agent.updatePosition = true;

            StartCoroutine("FindBhanuRoutine");
        }


        void Update()
        {
            if(Time.timeScale == 0)
                return;

            if (m_target != null)
                m_agent.SetDestination(m_target.position);

            if (m_agent.remainingDistance > m_agent.stoppingDistance)
                m_character.Move(m_agent.desiredVelocity, false, false);
            else
                m_character.Move(Vector3.zero, false, false);
        }

        IEnumerator FindBhanuRoutine()
        {
            yield return new WaitForSeconds(0.65f);

            if(m_bhanuTrans == null)
            {
                Debug.Log("Find Bhanu Routine");
                m_bhanuTrans = GameObject.FindGameObjectWithTag("Player").transform;
                m_target = m_bhanuTrans;
                StartCoroutine("FindBhanuRoutine");   
            }
        }

        public void SetTarget(Transform target)
        {
            this.m_target = target;
        }
    }
}
