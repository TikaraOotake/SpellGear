using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject Camera;//�J�����̃I�u�W�F�N�g���擾
    [SerializeField]
    private GameObject Model;//���f���̃I�u�W�F�N�g���擾

    [SerializeField]
    private Vector3 CameraRot;

    private float SpeedRate;

    [SerializeField]
    private float BaseSpeed;//��b���x
    [SerializeField]
    private float BaseAcceleration = 1.0f;//��b�����x
    [SerializeField]
    private float BaseJumpPower = 1.0f;//��b�W�����v��


    [SerializeField]
    private MagicData[] MagicArray = new MagicData[6];
    [SerializeField]
    private WeaponData[] WeaponArray = new WeaponData[6];

    private Vector3 InputDirection = new Vector3(0.0f, 0.0f, 0.0f);

    private Rigidbody rb;
    [SerializeField]
    private Animator animator;


    void Start()
    {
        if (Camera == null)
        {
            //�J�������擾
            Camera = GameObject.Find("Main Camera");
        }

        if (Model)
        {
            animator = Model.GetComponent<Animator>();
            if (animator == null)
            {
                Debug.Log("�A�j���[�^�[�擾�Ɏ��s");
            }
        }

        rb = GetComponent<Rigidbody>();

        // �}�E�X�J�[�\������ʒ����ɌŒ肵�A��\���ɂ���
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


        if (BaseSpeed == 0)
        {
            Debug.Log("�v���C���[�̊�b���x��0�ł�");
        }
        if (BaseAcceleration == 0)
        {
            Debug.Log("�v���C���[�̊�b�����x��0�ł�");
        }
        if (BaseJumpPower == 0)
        {
            Debug.Log("�v���C���[�̊�b�W�����v�͂�0�ł�");
        }


        //�e�X�g�p�ɂ������Z�b�g���Ă݂�
        WeaponData BowData = Resources.Load<WeaponData>("Weapon/Bow");
        WeaponArray[0] = Instantiate(BowData);
        WeaponArray[1] = Instantiate(BowData);
        WeaponArray[2] = Instantiate(BowData);
        WeaponArray[3] = Instantiate(BowData);
        WeaponArray[4] = Instantiate(BowData);
        WeaponArray[5] = Instantiate(BowData);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        Shot();
        UpdateCameraCondition();
    }

    private void Move()
    {
        Vector3 tempInputDirection = new Vector3(0.0f, 0.0f, 0.0f);
        if (Input.GetKey(KeyCode.W))
        {
            tempInputDirection.z += 1.0f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            tempInputDirection.z -= 1.0f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            tempInputDirection.x += 1.0f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            tempInputDirection.x -= 1.0f;
        }
        //���K��
        tempInputDirection.Normalize();

        //�A�j���[�V����������
        if(animator)
        {
            if (tempInputDirection.x * tempInputDirection.x + tempInputDirection.z * tempInputDirection.z != 0)
            {
                animator.SetInteger("Action", 1);
            }
            else
            {
                animator.SetInteger("Action", 0);
            }
        }

        
            

        //�ړ��������X�V
        InputDirection = tempInputDirection;

        //�J�����̊p�x�𔽉f
        Vector2 tempVec = RotateVector2(new Vector2(InputDirection.x, InputDirection.z), -CameraRot.y);
        Vector3 MoveVec = new Vector3(tempVec.x, 0.0f, tempVec.y);
        
        //���f�����ړ������ɍ��킹��(��])
        if(Model)
        {
            if (tempInputDirection.x * tempInputDirection.x + tempInputDirection.z * tempInputDirection.z != 0)
            {
                Model.transform.eulerAngles = new Vector3(0.0f, Mathf.Atan2(MoveVec.x, MoveVec.z) * Mathf.Rad2Deg, 0.0f);
            }
        }

        /*//���͂������������
        if (tempInputDirection.x * tempInputDirection.x + tempInputDirection.z * tempInputDirection.z != 0)
        {
            if (SpeedRate < 1.0f)
            {
                SpeedRate += Time.deltaTime * BaseAcceleration;
            }
            else
            {
                SpeedRate = 1.0f;
            }
            //�ړ��������X�V
            InputDirection = tempInputDirection;
        }
        else//����
        {
            if (SpeedRate > 0.0f)
            {
                SpeedRate -= Time.deltaTime * BaseAcceleration;
            }
            else
            {
                SpeedRate = 0.0f;
            }
        }
         */


        Vector3 Vel = rb.velocity;

        if (Vel.x * Vel.x + Vel.z * Vel.z <= BaseSpeed * BaseSpeed)
        {
            rb.AddForce(MoveVec * BaseSpeed);
        }


        //rb.velocity = new Vector3(InputDirection.x * BaseSpeed * SpeedRate, Vel.y, InputDirection.z * BaseSpeed * SpeedRate);
    }
    private void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(0.0f,BaseJumpPower,0.0f);
        }
    }
    private void Shot()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (WeaponArray[0])
            {
                if (WeaponArray[0].GetActionPrefab())
                {
                    GameObject bullet = Instantiate(WeaponArray[0].GetActionPrefab(), new Vector3(0.0f, 1.5f, 0.0f) + transform.position, Quaternion.identity);
                    bullet.GetComponent<BulletBase>().SetVelocty(Camera.transform.forward * 30.0f);
                    bullet.GetComponent<BulletBase>().SetRotation(CameraRot);
                }

            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (Camera)
            {
                Camera.GetComponent<MainCamera>().SetIslookIn(true);
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            if (Camera)
            {
                Camera.GetComponent<MainCamera>().SetIslookIn(false);
            }
        }
    }
    private void UpdateCameraCondition()
    {
        float sensitiveRotate = 2.0f;
        if (!Input.GetKey(KeyCode.L))
        {
            float rotateX = Input.GetAxis("Mouse X") * sensitiveRotate;
            float rotateY = Input.GetAxis("Mouse Y") * sensitiveRotate;

            CameraRot.x -= rotateY;
            CameraRot.y += rotateX;
        }

        if(Camera)
        {
            Camera.GetComponent<MainCamera>().SetCameraCondition(transform.position,CameraRot);
        }
    }

    //�p�x��@���x�N�g���ɒ���
    Vector3 DirectionFromYawPitch(float yawDeg, float pitchDeg)
    {
        float yaw = Mathf.Deg2Rad * yawDeg;
        float pitch = Mathf.Deg2Rad * pitchDeg;

        float x = Mathf.Cos(pitch) * Mathf.Cos(yaw);
        float y = Mathf.Sin(pitch);
        float z = Mathf.Cos(pitch) * Mathf.Sin(yaw);

        return new Vector3(x, y, z).normalized;
    }
    public Vector2 RotateVector2(Vector2 vec, float angleDeg)
    {
        float rad = Mathf.Deg2Rad * angleDeg;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        float x = vec.x * cos - vec.y * sin;
        float y = vec.x * sin + vec.y * cos;

        return new Vector2(x, y);
    }
}
