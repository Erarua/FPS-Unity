using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ��������ƶ�
/// </summary>

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;//��ɫ������

    public bool onSlope;

    /// <summary>
    /// ������ر���
    /// </summary>
    private float speed = 5.0f;//ʵ���ƶ��ٶ�
    public float walkSpeed = 5.0f;//Ԥ�������ٶ�
    public float runSpeed = 10.0f;//Ԥ�豼���ٶ�
    [SerializeField] private bool isRun = false;//�ж��Ƿ���

    /// <summary>
    /// ��Ծ��ر���
    /// </summary>
    public float upForce = 2.0f;//��Ծ����
    public float gravity = -9.8f;//����
    [SerializeField] private bool isJump = false;//�ж��Ƿ���Ծ

    /// <summary>
    /// ��������ر���
    /// </summary>
    private Transform groundCheck;//���ڵ�����
    private LayerMask groundLayer;//�������ڵ�layer
    [SerializeField] private bool isGround = true;//�ж��Ƿ��ڵ�����

    /// <summary>
    /// б������ر���
    /// </summary>
    private float downForce = 4.0f;//б��ʱ����ʩ������


    /// <summary>
    /// �������
    /// </summary>
    /*
    public AudioSource audioSource;
    public AudioClip walkClip;
    public AudioClip runClip;
    public AudioClip currentClip;
    */

    public int jumpcount = 1;
    private int currentjumpcount = 0;

    private Vector3 MoveDirection;//�ƶ���������

    private float MoveX, MoveZ;//�������룬WS��ӦZ��AD��ӦX

    [Header("��λ����")]
    [SerializeField][Tooltip("���ܼ�λ")] private string RunKey = "Run";
    [SerializeField][Tooltip("��Ծ��λ")] private string JumpKey = "Jump";


    /// <summary>
    /// �������
    /// </summary>
    public Animator FPAnimator;

    //��һ�������˳ƵĶ���
    [SerializeField]public Animator TPAnimator;

    public PlayerHealth health;

    // Start is called before the first frame update
    private void Start()
    {
        //��ȡ��ɫ������
        characterController = GetComponent<CharacterController>();
        groundCheck = transform.Find("CheckGround");
        groundLayer = LayerMask.GetMask("Ground");
        health = GetComponent<PlayerHealth>();

        
        //audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (isGround)
        {
            currentjumpcount = 0;
        }
        CheckGround();
        Move();
    }

    /// <summary>
    /// �ƶ�
    /// </summary>
    public void Move()
    {
        if (isGround)
        {
            MoveX = Input.GetAxisRaw("Horizontal");
            MoveZ = Input.GetAxisRaw("Vertical");
            //Debug.Log(MoveX);
            MoveDirection = (transform.right * MoveX + transform.forward * MoveZ).normalized;
            MoveDirection = transform.TransformDirection(new Vector3(MoveX, 0, MoveZ));
            //MoveDirection = new Vector3(MoveX, 0.0f, MoveZ);
            //���������������Ϊ���û�и���transform�����ģ�ֻ�ǰ��������X,Y,Z����

            //�����Ƿ��ܸ����ٶ�
            isRun = Input.GetButton(RunKey);
            speed = isRun ? runSpeed : walkSpeed;
        }

        //б�洦��
        if (OnSlope())
        {
            MoveDirection.y -= downForce * characterController.height / 2 * Time.deltaTime;
        }

        

        //boss����
        if (health.isBossAttack)
        {
            MoveDirection += transform.forward * -5;
        }


        //ʵ����Ծ
        Jump();
        MoveDirection.y += gravity * Time.deltaTime;
        characterController.Move(MoveDirection * Time.deltaTime * speed);
        var Speed = characterController.velocity;
        Speed.y = 0;
        
        FPAnimator.SetFloat("Speed", Speed.magnitude, 0.25f, Time.deltaTime);
        TPAnimator.SetFloat("Speed", Speed.magnitude, 0.25f, Time.deltaTime);
        if(TPAnimator.GetFloat("Speed") > 0f)
        {
            TPAnimator.SetFloat("Movement_X", MoveX, 0, Time.deltaTime);
            TPAnimator.SetFloat("Movement_Y", MoveZ, 0, Time.deltaTime);
        }

        //�����Ǹ���ģ�
        if (FPAnimator.GetFloat("Speed") < 0.01f)
        {
            FPAnimator.SetFloat("Speed", 0f);
        }

        if (TPAnimator.GetFloat("Speed") < 0.01f)
        {
            TPAnimator.SetFloat("Speed", 0f);
        }




        //Debug.Log(characterController.velocity.magnitude);

        //PlayAudio();
    }

    /// <summary>
    /// ������������
    /// </summary>
    /*
    public void PlayAudio()
    {
        audioSource.clip = isRun ? runClip : walkClip;
        if (isGround && MoveDirection.sqrMagnitude > 0)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
                currentClip = audioSource.clip;
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
            }
        }
    }
    */

    /// <summary>
    /// ��Ծ
    /// </summary>
    public void Jump()
    {
        isJump = Input.GetButtonDown(JumpKey);
        //isGround
        if (isJump && (currentjumpcount < jumpcount))
        {
            currentjumpcount++;
            MoveDirection.y = upForce;
        }
    }

    public void CheckGround()
    {
        //�ж��Ƿ��ڵ����ϣ�0.1Ϊ����뾶��groundLayer��ʾGround��һLayer
        isGround = Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);
    }

    public bool OnSlope()
    {
        RaycastHit hit;

        //��ȡ�ӽ�ɫ���������������ײ��ƽ����Ϣ
        if (Physics.Raycast(transform.position, Vector3.down, out hit, characterController.height, groundLayer))
        {
            //���ݵõ���ƽ�淨���ж��Ƿ���б����
            if (hit.normal != Vector3.up)
            {
                onSlope = true;
                return true;
            }
        }
        onSlope = false;
        return false;
    }
}
