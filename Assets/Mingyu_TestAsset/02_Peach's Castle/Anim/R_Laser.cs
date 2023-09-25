using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R_Laser : MonoBehaviour
{
    public GameObject LaserCtrl;

    void End_Laser()
    {
        LaserCtrl.GetComponent<LaserController>().is_ShootLaser = false;
    }
}
