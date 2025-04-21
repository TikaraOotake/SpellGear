using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField]
    float DestroyTimer = 0.0f;
    [SerializeField]
    float DestroyTime = 5.0f;

    [SerializeField]
    float Size = 5.0f;
    void Start()
    {

    }

    private void Update()
    {
        Vector3 Scale = transform.localScale;
        Scale.x += (Size - Scale.x) * 2.0f* Time.deltaTime;
        Scale.y += (Size - Scale.y) * 2.0f* Time.deltaTime;
        Scale.z += (Size - Scale.z) * 2.0f* Time.deltaTime;
        transform.localScale = Scale ;
        //transform.localScale += new Vector3(1.0f, 1.0f, 1.0f) * Time.deltaTime * 3.0f;


        DestroyTimer += Time.deltaTime;
        if (DestroyTimer >= 1.5f)
        {
            Size = 0.0f;
        }
        if (DestroyTimer >= DestroyTime)
        {
            Destroy(this.gameObject);
        }
    }
}
