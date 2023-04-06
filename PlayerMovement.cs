using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 控制玩家移动
/// </summary>

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;//角色控制器

    public bool onSlope;

    /// <summary>
    /// 行走相关变量
    /// </summary>
    private float speed = 5.0f;//实际移动速度
    public float walkSpeed = 5.0f;//预设行走速度
    public float runSpeed = 10.0f;//预设奔跑速度
    [SerializeField] private bool isRun = false;//判断是否奔跑

    /// <summary>
    /// 跳跃相关变量
    /// </summary>
    public float upForce = 2.0f;//跳跃力度
    public float gravity = -9.8f;//重力
    [SerializeField] private bool isJump = false;//判断是否跳跃

    /// <summary>
    /// 地面检测相关变量
    /// </summary>
    private Transform groundCheck;//用于地面检测
    private LayerMask groundLayer;//地面所在的layer
    [SerializeField] private bool isGround = true;//判断是否在地面上

    /// <summary>
    /// 斜面检测相关变量
    /// </summary>
    private float downForce = 4.0f;//斜面时向下施加力度


    /// <summary>
    /// 声音相关
    /// </summary>
    /*
    public AudioSource audioSource;
    public AudioClip walkClip;
    public AudioClip runClip;
    public AudioClip currentClip;
    */

    public int jumpcount = 1;
    private int currentjumpcount = 0;

    private Vector3 MoveDirection;//移动方向向量

    private float MoveX, MoveZ;//键盘输入，WS对应Z，AD对应X

    [Header("键位设置")]
    [SerializeField][Tooltip("奔跑键位")] private string RunKey = "Run";
    [SerializeField][Tooltip("跳跃键位")] private string JumpKey = "Jump";


    /// <summary>
    /// 动画相关
    /// </summary>
    public Animator FPAnimator;

    //搞一个第三人称的动画
    [SerializeField]public Animator TPAnimator;

    public PlayerHealth health;

    // Start is called before the first frame update
    private void Start()
    {
        //获取角色控制器
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
    /// 移动
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
            //上面这个不行是因为这个没有根据transform来更改，只是按照世界的X,Y,Z更改

            //根据是否奔跑更改速度
            isRun = Input.GetButton(RunKey);
            speed = isRun ? runSpeed : walkSpeed;
        }

        //斜面处理
        if (OnSlope())
        {
            MoveDirection.y -= downForce * characterController.height / 2 * Time.deltaTime;
        }

        

        //boss攻击
        if (health.isBossAttack)
        {
            MoveDirection += transform.forward * -5;
        }


        //实现跳跃
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

        //这里是干嘛的？
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
    /// 播放行走声音
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
    /// 跳跃
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
        //判断是否在地面上，0.1为球体半径，groundLayer表示Ground这一Layer
        isGround = Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);
    }

    public bool OnSlope()
    {
        RaycastHit hit;

        //获取从角色向下射出射线所碰撞的平面信息
        if (Physics.Raycast(transform.position, Vector3.down, out hit, characterController.height, groundLayer))
        {
            //根据得到的平面法线判断是否在斜面上
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
