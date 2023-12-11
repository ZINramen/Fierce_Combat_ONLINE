using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public string[] skills = new string[6];
   
    public void AddSkill(string name) 
    {
        if(name == "Gun") 
        {
            skills[0] = name;
        }
        if (name == "Sword")
        {
            skills[1] = name;
        }
        if (name == "Kunai")
        {
            skills[2] = name;
        }
        if (name == "Hammer")
        {
            skills[3] = name;
        }
        if (name == "Potion")
        {
            skills[4] = name;
        }

    }
}
