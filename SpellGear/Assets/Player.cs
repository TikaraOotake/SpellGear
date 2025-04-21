using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject Camera;//カメラのオブジェクトを取得
    [SerializeField]
    private GameObject Model;//モデルのオブジェクトを取得

    private Image UI_WeaponImage;
    private Image UI_MagicImage;

    [SerializeField]
    private Vector3 CameraRot;

    private float SpeedRate;

    [SerializeField]
    private float BaseSpeed;//基礎速度
    [SerializeField]
    private float BaseAcceleration = 1.0f;//基礎加速度
    [SerializeField]
    private float BaseJumpPower = 1.0f;//基礎ジャンプ力


    [SerializeField]
    private MagicData[] MagicArray = new MagicData[6];
    [SerializeField]
    private WeaponData[] WeaponArray = new WeaponData[6];
    private int MagicIndex;
    private int WeaponIndex;

    private Vector3 InputDirection = new Vector3(0.0f, 0.0f, 0.0f);

    private Rigidbody rb;
    [SerializeField]
    private Animator animator;


    void Start()
    {
        if (Camera == null)
        {
            //カメラを取得
            Camera = GameObject.Find("Main Camera");
        }

        if (Model)
        {
            animator = Model.GetComponent<Animator>();
            if (animator == null)
            {
                Debug.Log("アニメーター取得に失敗");
            }
        }

        UI_WeaponImage = GameObject.Find("UI_Weapon").GetComponent<Image>();
        UI_MagicImage = GameObject.Find("UI_Magic").GetComponent<Image>();

        rb = GetComponent<Rigidbody>();

        // マウスカーソルを画面中央に固定し、非表示にする
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


        if (BaseSpeed == 0)
        {
            Debug.Log("プレイヤーの基礎速度が0です");
        }
        if (BaseAcceleration == 0)
        {
            Debug.Log("プレイヤーの基礎加速度が0です");
        }
        if (BaseJumpPower == 0)
        {
            Debug.Log("プレイヤーの基礎ジャンプ力が0です");
        }


        //テスト用にいくつかセットしてみる
        WeaponData BowData = Resources.Load<WeaponData>("Weapon/Bow");
        WeaponData BottleData = Resources.Load<WeaponData>("Weapon/Bottle");
        WeaponData SwordData = Resources.Load<WeaponData>("Weapon/Sword");
        WeaponData CaneData = Resources.Load<WeaponData>("Weapon/Cane");
        WeaponArray[0] = Instantiate(BowData);
        WeaponArray[1] = Instantiate(BottleData);
        WeaponArray[2] = Instantiate(SwordData);
        WeaponArray[3] = Instantiate(CaneData);
        WeaponArray[4] = Instantiate(BowData);
        WeaponArray[5] = Instantiate(BowData);
        MagicData FireData = Resources.Load<MagicData>("Magic/Fire");
        MagicData IceData = Resources.Load<MagicData>("Magic/Ice");
        MagicArray[0] = Instantiate(FireData);
        MagicArray[1] = Instantiate(IceData);
        MagicArray[2] = Instantiate(FireData);
        MagicArray[3] = Instantiate(IceData);
        MagicArray[4] = Instantiate(FireData);
        MagicArray[5] = Instantiate(IceData);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        Shot();
        RotationWeapon();
        UI_Settings();
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
        //正規化
        tempInputDirection.Normalize();

        //アニメーションさせる
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

        
            

        //移動方向を更新
        InputDirection = tempInputDirection;

        //カメラの角度を反映
        Vector2 tempVec = RotateVector2(new Vector2(InputDirection.x, InputDirection.z), -CameraRot.y);
        Vector3 MoveVec = new Vector3(tempVec.x, 0.0f, tempVec.y);
        
        //モデルを移動方向に合わせる(回転)
        if(Model)
        {
            if (tempInputDirection.x * tempInputDirection.x + tempInputDirection.z * tempInputDirection.z != 0)
            {
                Model.transform.eulerAngles = new Vector3(0.0f, Mathf.Atan2(MoveVec.x, MoveVec.z) * Mathf.Rad2Deg, 0.0f);
            }
        }

        /*//入力があったら加速
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
            //移動方向を更新
            InputDirection = tempInputDirection;
        }
        else//減速
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
                    MagicData data = MagicArray[MagicIndex];
                    if (data)
                    {
                        if(data.GetActionPrefab())
                        {
                            bullet.GetComponent<BulletBase>().SetMagic(data.GetActionPrefab());
                        }
                    }
                    
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
    private void RotationWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ++WeaponIndex;
            WeaponIndex = ((WeaponIndex % 4) + 4) % 4;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ++MagicIndex;
            MagicIndex = ((MagicIndex % 4) + 4) % 4;
        }
    }
    private void UI_Settings()
    {
        if (WeaponIndex >= 0 && WeaponIndex <= 3)
        {
            WeaponData data = WeaponArray[WeaponIndex];
            if (UI_WeaponImage && data)
            {
                UI_WeaponImage.sprite = data.GetSprite();
            }
        }
        if (MagicIndex >= 0 && MagicIndex <= 3)
        {
            MagicData data = MagicArray[MagicIndex];
            if (UI_MagicImage && data)
            {
                UI_MagicImage.sprite = data.GetSprite();
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

    //角度を法線ベクトルに直す
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
