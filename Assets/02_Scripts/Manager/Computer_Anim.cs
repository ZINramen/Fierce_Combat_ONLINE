using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer_Anim : MonoBehaviour
{
    private bool isComputer_On = false;
    public GameObject BootingTexture;

    public void End_ComputerBooting()
    {
        BootingTexture.SetActive(false);
    }

    public void On_Computer()
    {
        isComputer_On = true;
    }

    public bool Get_isComputer_On()
    {
        return isComputer_On;
    }
}
