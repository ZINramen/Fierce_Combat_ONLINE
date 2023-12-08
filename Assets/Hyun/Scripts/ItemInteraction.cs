using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteraction : MonoBehaviour
{
    public enum ItemType
    {
        Skill, Normal
    }

    public ItemType item;
    public string itemName;

    private void OnTriggerEneter2D(Collider2D coll)
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
    }
}
