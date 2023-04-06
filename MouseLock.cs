using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 控制摄像机旋转
/// </summary>

public class MouseLock : MonoBehaviour
{
    public float MouseSensitivity = 100.0f;//视线灵敏度
    public Transform Player;//玩家位置

    private float MouseX, MouseY;//鼠标左右(X),上下(Y)
    private float RotateX = 0.0f;//摄像机的X轴旋转量

    //后坐力
    public AnimationCurve RecoilCurve;
    public Vector2 RecoilRange;// 范围

    public float RecoilFadeOutTime = 0.3f;
    private float currentRecoilTime;
    private Vector2 currentRecoil;


    // Start is called before the first frame update
    void Start()
    {
        Player = this.transform.parent;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //后坐力
        CalculateRecoilOffset();

        //鼠标上下移动，玩家不旋转，视角上下旋转
        MouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * MouseSensitivity;
        RotateX -= MouseY;//累计旋转量
        RotateX = Mathf.Clamp(RotateX, -90.0f, 45.0f);//限制旋转幅度
        RotateX -= currentRecoil.y * (UnityEngine.Random.value-0.5f);
        transform.localRotation = Quaternion.Euler(RotateX, 0.0f, 0.0f);



        
        //鼠标左右移动，玩家左右旋转，视角左右旋转
        MouseX = Input.GetAxis("Mouse X") * Time.deltaTime * MouseSensitivity;
        MouseX -= currentRecoil.x* (UnityEngine.Random.value - 0.5f);
        Player.Rotate(0.0f, MouseX, 0.0f);
    }

    //计算后坐力
    private void CalculateRecoilOffset()
    {
        currentRecoilTime += Time.deltaTime;
        float tmp_RecoilFraction = currentRecoilTime / RecoilFadeOutTime;
        float tmp_RecoilValue = RecoilCurve.Evaluate(tmp_RecoilFraction);
        
        currentRecoil = Vector2.Lerp(Vector2.zero, currentRecoil, tmp_RecoilValue);
        
    }

    public void FiringForTest()
    {
        currentRecoil += RecoilRange;
        //cameraSpring.StartCameraSpring();
        currentRecoilTime = 0;
    }

}
