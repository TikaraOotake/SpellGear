using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : BulletBase
{
    [SerializeField]
    float DestroyTimer = 0.0f;
    [SerializeField]
    float DestroyTime = 1.0f;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (rb)
        {
            // –î‚Ì‘¬“x•ûŒü‚ðŒü‚©‚¹‚é
            if (rb.velocity.sqrMagnitude > 0.1f)
            {
                transform.rotation = Quaternion.LookRotation(rb.velocity);
            }
        }

        
    }
    private void Update()
    {
        DestroyTimer += Time.deltaTime;
        if (DestroyTimer >= DestroyTime)
        {
            if(MagicPrefab)
            {
                Instantiate(MagicPrefab, transform.position, Quaternion.identity);
            }
            Destroy(this.gameObject);
        }
    }
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (MagicPrefab)
            {
                Instantiate(MagicPrefab, transform.position, Quaternion.identity);
            }
            Destroy(this.gameObject);
        }
    }
}
