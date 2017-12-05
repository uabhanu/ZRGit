using Random = UnityEngine.Random;
using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof (CharacterController))]
    [RequireComponent(typeof (AudioSource))]
    public class Bhanu : MonoBehaviour
    {
        AudioSource m_AudioSource;
        bool m_Jump , m_jumping , m_previouslyGrounded;
        Camera m_Camera;
        CharacterController m_characterController;
        CollisionFlags m_collisionFlags;
        float m_nextStep , m_stepCycle , m_YRotation;
        Vector2 m_Input;
        Vector3 m_MoveDir = Vector3.zero , m_originalCameraPosition;

        [SerializeField] AudioClip[] m_footstepSounds;    // an array of footstep sounds that will be randomly selected from.
        [SerializeField] AudioClip m_jumpSound , m_landSound; // m_jumpSound - the sound played when character leaves the ground & m_landSound - the sound played when character touches back on ground.
        [SerializeField] bool m_isWalking , m_useFovKick , m_useHeadBob;
        [SerializeField] CurveControlledBob m_headBob = new CurveControlledBob();
        [SerializeField] float m_gravityMultiplier , m_jumpSpeed , m_runSpeed , m_stepInterval , m_stickToGroundForce , m_walkSpeed;
        [SerializeField] [Range(0f , 1f)] float m_runstepLenghten;
        [SerializeField] FOVKick m_fovKick = new FOVKick();
        [SerializeField] Helicopter m_helicopter;
        [SerializeField] LerpControlledBob m_jumpBob = new LerpControlledBob();
        [SerializeField] MouseLook m_mouseLook;

        void Start()
        {
            m_characterController = GetComponent<CharacterController>();
            m_Camera = Camera.main;
            m_originalCameraPosition = m_Camera.transform.localPosition;
            m_fovKick.Setup(m_Camera);
            m_headBob.Setup(m_Camera, m_stepInterval);
            m_stepCycle = 0f;
            m_nextStep = m_stepCycle/2f;
            m_jumping = false;
            m_AudioSource = GetComponent<AudioSource>();
            m_mouseLook.Init(transform , m_Camera.transform);
        }

        void Update()
        {
            RotateView();
            // the jump state needs to read here to make sure it is not missed
            if(!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

            if(!m_previouslyGrounded && m_characterController.isGrounded)
            {
                StartCoroutine(m_jumpBob.DoBobCycle());
                PlayLandingSound();
                m_MoveDir.y = 0f;
                m_jumping = false;
            }

            if(!m_characterController.isGrounded && !m_jumping && m_previouslyGrounded)
            {
                m_MoveDir.y = 0f;
            }

            m_previouslyGrounded = m_characterController.isGrounded;
        }

        void FixedUpdate()
        {
            float speed;
            GetInput(out speed);
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward*m_Input.y + transform.right*m_Input.x;

            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_characterController.radius, Vector3.down, out hitInfo,
                               m_characterController.height/2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            m_MoveDir.x = desiredMove.x*speed;
            m_MoveDir.z = desiredMove.z*speed;


            if(m_characterController.isGrounded)
            {
                m_MoveDir.y = -m_stickToGroundForce;

                if(m_Jump)
                {
                    m_MoveDir.y = m_jumpSpeed;
                    PlayJumpSound();
                    m_Jump = false;
                    m_jumping = true;
                }
            }
            else
            {
                m_MoveDir += Physics.gravity*m_gravityMultiplier*Time.fixedDeltaTime;
            }

            m_collisionFlags = m_characterController.Move(m_MoveDir*Time.fixedDeltaTime);

            ProgressStepCycle(speed);
            UpdateCameraPosition(speed);

            m_mouseLook.UpdateCursorLock();
        }

        void GetInput(out float speed)
        {
            // Read input
            float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            float vertical = CrossPlatformInputManager.GetAxis("Vertical");

            bool waswalking = m_isWalking;

#if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
            m_isWalking = !Input.GetKey(KeyCode.LeftShift);
#endif
            // set the desired speed to be walking or running
            speed = m_isWalking ? m_walkSpeed : m_runSpeed;
            m_Input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if(m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            if(m_isWalking != waswalking && m_useFovKick && m_characterController.velocity.sqrMagnitude > 0)
            {
                StopAllCoroutines();
                StartCoroutine(!m_isWalking ? m_fovKick.FOVKickUp() : m_fovKick.FOVKickDown());
            }
        }

        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if(m_collisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if(body == null || body.isKinematic)
            {
                return;
            }

            body.AddForceAtPosition(m_characterController.velocity*0.1f, hit.point, ForceMode.Impulse);
        }

        void OnFindClearArea()
        {
            Debug.Log("Found Clear Area");
            m_helicopter.Call();
        }

        void PlayFootStepAudio()
        {
            if (!m_characterController.isGrounded)
            {
                return;
            }
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            int n = Random.Range(1, m_footstepSounds.Length);
            m_AudioSource.clip = m_footstepSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            m_footstepSounds[n] = m_footstepSounds[0];
            m_footstepSounds[0] = m_AudioSource.clip;
        }

        void PlayJumpSound()
        {
            m_AudioSource.clip = m_jumpSound;
            m_AudioSource.Play();
        }
      
        void PlayLandingSound()
        {
            m_AudioSource.clip = m_landSound;
            m_AudioSource.Play();
            m_nextStep = m_stepCycle + .5f;
        }

        void ProgressStepCycle(float speed)
        {
            if(m_characterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_stepCycle += (m_characterController.velocity.magnitude + (speed*(m_isWalking ? 1f : m_runstepLenghten)))*
                             Time.fixedDeltaTime;
            }

            if(!(m_stepCycle > m_nextStep))
            {
                return;
            }

            m_nextStep = m_stepCycle + m_stepInterval;

            PlayFootStepAudio();
        }

        void RotateView()
        {
            m_mouseLook.LookRotation(transform , m_Camera.transform);
        }

        void UpdateCameraPosition(float speed)
        {
            Vector3 newCameraPosition;

            if(!m_useHeadBob)
            {
                return;
            }

            if(m_characterController.velocity.magnitude > 0 && m_characterController.isGrounded)
            {
                m_Camera.transform.localPosition =
                    m_headBob.DoHeadBob(m_characterController.velocity.magnitude +
                                      (speed*(m_isWalking ? 1f : m_runstepLenghten)));
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_Camera.transform.localPosition.y - m_jumpBob.Offset();
            }
            else
            {
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_originalCameraPosition.y - m_jumpBob.Offset();
            }

            m_Camera.transform.localPosition = newCameraPosition;
        }
    }
}
