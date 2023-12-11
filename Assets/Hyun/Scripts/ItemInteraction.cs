using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteraction : MonoBehaviour
{
    public enum ItemType
    {
        Skill, Normal
    }
    public bool notDestroy = false;
    public ItemType item;
    public string itemName;

    public GameObject effect;
    private void OnTriggerEnter2D(Collider2D coll)
    {
        SkillManager skills = coll.GetComponent<SkillManager>();
        if (!skills) return;

        switch (item) 
        {
            case ItemType.Skill :
                skills.AddSkill(itemName);
                break;

            case ItemType.Normal:
                break;
        }
        if (!notDestroy)
        {
            GameObject eff = Instantiate(effect);
            eff.transform.position = transform.position;
            Destroy(gameObject);
        }
    }
    public void removeSkill() 
    {
    //    if (name == "Gun")
    //    {
    //        skills[0] = name;
    //    }
    //    if (name == "Sword")
    //    {
    //        skills[1] = name;
    //    }
    //    if (name == "Kunai")
    //    {
    //        skills[2] = name;
    //    }
    //    if (name == "Hammer")
    //    {
    //        skills[3] = name;
    //    }
    //    if (name == "Potion")
    //    {
    //        skills[4] = name;
    //    }
    }
}
