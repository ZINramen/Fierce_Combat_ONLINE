using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_Controller : MonoBehaviour
{
    public GameObject Parent;

    void End_Fire()
    {
        Parent.GetComponent<Cupa_Ctrl>().Off_FireMotion();
    }
}