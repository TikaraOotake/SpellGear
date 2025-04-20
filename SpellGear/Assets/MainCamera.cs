using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject Player;

    [SerializeField]
    private float CameraRange;
    [SerializeField]
    private float BaseCameraRange;

    [SerializeField]
    private Vector3 CameraRot;

    [SerializeField]
    private float AddHeight;

    [SerializeField]
    bool IsLookIn;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Player)
        {
            /// Vector3 CameraPos=Player.transform.position;
            // transform.position = CameraPos;
        }

        if (IsLookIn)
        {
            CameraRange -= (CameraRange - 1.0f) * 5.0f * Time.deltaTime;
        }
        else
        {
            CameraRange -= (CameraRange - BaseCameraRange) * 5.0f * Time.deltaTime;
        }

    }

    public void SetCameraCondition(Vector3 _Pos, Vector3 _Rot)
    {
        //位置を計算
        _Pos -= DirectionFromYawPitch(-_Rot.y + 90, -_Rot.x) * CameraRange;

        //注視点をずらす
        Vector3 _AdjustPos = DirectionFromYawPitch(-_Rot.y, 0.0f) * 0.5f;
        _Pos += _AdjustPos;

        //高さを補正
        _Pos.y += AddHeight;

        transform.eulerAngles = _Rot;
        transform.position = _Pos;
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

    public void SetIslookIn(bool _flag)
    {
        IsLookIn = _flag;
    }
}
