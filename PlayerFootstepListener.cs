using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootstepListener : MonoBehaviour
{
    public FootstepAudioData FootstepAudioData;
    public AudioSource FootstepAudioSource;

    private CharacterController characterController;
    private Transform footstepTransform;

    private float nextPlayTime;
    public LayerMask LayerMask;
    public enum State
    {
        Idle,
        Walk,
        Sprinting,
        Crouching,
        Others
    }

    public State characterState;

    //Q:��ɫ���������ıر�����
    //A:��ɫ�ƶ����߷����ϴ���ȶ�����ʱ�򷢳�����


    //Q.��μ���ɫ�Ƿ����ƶ�
    //A:��Physic API���


    //Q:���ʵ�ֽ�ɫ��̤λ�õĶ�Ӧ���ʵ�����
    //A:��Physic API���


    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        footstepTransform = transform;
    }

   
    private void FixedUpdate()
    {
        if (characterController.isGrounded)
        {
            if (characterController.velocity.normalized.magnitude >= 0.1f)
            {
                nextPlayTime += Time.deltaTime;
                Debug.Log(nextPlayTime);
                if (characterController.velocity.magnitude >= 4)
                {
                    characterState = State.Sprinting;
                }
                
                bool tmp_IsHit = Physics.Linecast(footstepTransform.position,
                    footstepTransform.position +
                    Vector3.down * (characterController.height / 2 + characterController.skinWidth - characterController.center.y),
                    out RaycastHit tmp_HitInfo);
                Debug.Log("check hit");
                Debug.Log(tmp_IsHit);
#if UNITY_EDITOR
                Debug.DrawLine(footstepTransform.position,
                    footstepTransform.position +
                    Vector3.down * (characterController.height / 2 + characterController.skinWidth-characterController.center.y),
                    Color.red,
                    0.25f);
#endif

                if (tmp_IsHit)
                {

                    //TODO:�������
                    foreach (var tmp_AudioElement in FootstepAudioData.FootstepAudios)
                    {
                        Debug.Log("tmp_HitInfo.collider");
                        Debug.Log(tmp_HitInfo.collider.tag);
                        if (tmp_HitInfo.collider.CompareTag(tmp_AudioElement.Tag))
                        {
                            Debug.Log("get here");
                            if (nextPlayTime >= tmp_AudioElement.Delay)
                            {
                                //TODO:�����ƶ�����
                                int tmp_AudioCount = tmp_AudioElement.AudioClips.Count;
                                int tmp_AudioIndex = UnityEngine.Random.Range(0, tmp_AudioCount);
                                
                                AudioClip tmp_FootstepAudioClip = tmp_AudioElement.AudioClips[tmp_AudioIndex];
                                FootstepAudioSource.clip = tmp_FootstepAudioClip;
                                FootstepAudioSource.Play();
                                nextPlayTime = 0;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
