using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    [SerializeField]
    Vector3 RotateSpeed;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rotate = transform.eulerAngles;
        transform.eulerAngles = rotate + RotateSpeed;
    }
}
