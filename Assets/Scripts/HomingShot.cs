using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingShot : MonoBehaviour
{

    [SerializeField]
    private int bulletAmount;


    void Start()
    {
        InvokeRepeating("Fire", 0f, 6f);
    }

    private void Fire()
    {
        for (int i = 0; i < bulletAmount + 1; i++)
        {


            GameObject bul = HomingPool.BPHinstance.GetBullet();
            bul.SetActive(true);
        }
    }

}
