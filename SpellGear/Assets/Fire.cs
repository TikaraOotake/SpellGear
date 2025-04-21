using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField]
    float DestroyTimer = 0.0f;
    [SerializeField]
    float DestroyTime = 1.0f;
    void Start()
    {

    }

    private void Update()
    {
        transform.localScale += new Vector3(1.0f, 1.0f, 1.0f) * Time.deltaTime * 2.0f;

        DestroyTimer += Time.deltaTime;
        if (DestroyTimer >= DestroyTime)
        {
            Destroy(this.gameObject);
        }
    }
}
