using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// �����������ת
/// </summary>

public class MouseLock : MonoBehaviour
{
    public float MouseSensitivity = 100.0f;//����������
    public Transform Player;//���λ��

    private float MouseX, MouseY;//�������(X),����(Y)
    private float RotateX = 0.0f;//�������X����ת��

    //������
    public AnimationCurve RecoilCurve;
    public Vector2 RecoilRange;// ��Χ

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
        //������
        CalculateRecoilOffset();

        //��������ƶ�����Ҳ���ת���ӽ�������ת
        MouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * MouseSensitivity;
        RotateX -= MouseY;//�ۼ���ת��
        RotateX = Mathf.Clamp(RotateX, -90.0f, 45.0f);//������ת����
        RotateX -= currentRecoil.y * (UnityEngine.Random.value-0.5f);
        transform.localRotation = Quaternion.Euler(RotateX, 0.0f, 0.0f);



        
        //��������ƶ������������ת���ӽ�������ת
        MouseX = Input.GetAxis("Mouse X") * Time.deltaTime * MouseSensitivity;
        MouseX -= currentRecoil.x* (UnityEngine.Random.value - 0.5f);
        Player.Rotate(0.0f, MouseX, 0.0f);
    }

    //���������
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
