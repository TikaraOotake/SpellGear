using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    enum WeaponType
    {
        None,
        Sword,
        Bow,
        Bottle,
        Cane
    }
    [SerializeField]
    protected GameObject MagicPrefab;

    protected Rigidbody rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetMagic(GameObject _magic)
    {
        MagicPrefab = _magic;
    }
    public void SetVelocty(Vector3 _vel)
    {
        if (rb)
        {
            rb.velocity = _vel;
        }
    }
    public void SetRotation(Vector3 _rot)
    {
        transform.eulerAngles = _rot;
    }

   
}
