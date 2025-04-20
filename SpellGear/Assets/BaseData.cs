using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseData : ScriptableObject
{
    [SerializeField]
    private string Name;
    [SerializeField]
    private Sprite Sprite;
    [SerializeField]
    private GameObject ActionPrefab;
    [SerializeField]
    private float AttackPower;

    public void IsUseing()
    {

    }


    public string GetName()
    {
        return Name;
    }

    public Sprite GetSprite()
    {
        return Sprite;
    }
    public void SetSprite(Sprite sprite)
    {
        Sprite = sprite;
    }


    public GameObject GetActionPrefab()
    {
        return ActionPrefab;
    }
    public void SetActionPrefab(GameObject prefab)
    {
        ActionPrefab = prefab;
    }

    public float GetAttackPower()
    {
        return AttackPower;
    }
    public void SetAttackPower(float power)
    {
        AttackPower = power;
    }
}


