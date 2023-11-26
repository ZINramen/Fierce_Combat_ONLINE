using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private float hp;
    private float attackForce = 0;
    private float thrustpower = 0;

    public Movement movement;

    private float waitTime = 0;

    public float maxHP = 100;
    public LayerMask target;
    public GameObject attackPos;
    public float attackLength;
    public bool isDie = false; //캐릭터가 죽었는지 살았는지 여부

    public float flyingAttackForce = 0;
    public float flyingDamagedPower = 0;

    public AnimationManager aManager;

    public enum DefenseStatus { Nope,Guard, invincible, Warning }

    public DefenseStatus DamageBlock = DefenseStatus.Nope;


    [Header("Combo")]
    [SerializeField]private int currentCombo = 0;
    public int maxcombo = 10;
    public ComboView ComboUI;

    [Header("Additional Effect")]
    public GameObject HitEffect;
    public GameObject StrongHitEffect;

    public GameObject HitTextEffect;
    public GameObject StrongHitTextEffect;

    public GameObject CoolTextEffect;
    
    
    private void Awake()
    {
        movement = GetComponent<Movement>();
        aManager = GetComponent<AnimationManager>();
        
        aManager.owner = this;
        movement.owner = this;
        hp = maxHP;
    }

    private void Update()
    {
        if (attackPos)
        {
            Vector3 start = attackPos.transform.position;
            Debug.DrawRay(start, attackPos.transform.right*attackLength, Color.red);
        }
        if(waitTime > 0) 
        {
            waitTime -= Time.deltaTime;
        }
        else if(waitTime < 0)
        {
            waitTime = 0;
        }
        //생사 상태 확인
        if (hp <= 0 && !isDie)
        {
            DamageBlock = DefenseStatus.invincible;
            isDie = true;
            hp = 0;
            aManager.Die();
        }
    }

    public void SetPower(float powerValue) 
    {
        attackForce = powerValue;
        thrustpower = powerValue * 0.5f;
    }
    public void Attack() 
    {
        Vector2 start = attackPos.transform.position;
        RaycastHit2D[] hit;
        hit = Physics2D.RaycastAll(start, transform.right, attackLength, target);

        foreach (RaycastHit2D hitTarget in hit) 
        {
            if (hitTarget.collider.gameObject != gameObject)
            {
                Entity enemy = hitTarget.collider.gameObject.GetComponent<Entity>();
                if (enemy)
                {
                    enemy.flyingDamagedPower = flyingAttackForce;
                    if (transform.localEulerAngles.y == 180)
                        enemy.Dameged(attackForce, -thrustpower);
                    else
                        enemy.Dameged(attackForce, thrustpower);
                }
            }
        }
    }
    public void Teleport() 
    {
        Vector2 start = transform.position;
        RaycastHit2D[] hit;
        hit = Physics2D.RaycastAll(start, transform.right, 100, target);

        foreach (RaycastHit2D hitTarget in hit)
        {
            if (hitTarget.collider.gameObject != gameObject)
            {
                Entity enemy = hitTarget.collider.gameObject.GetComponent<Entity>();
                if (enemy)
                {
                    if (enemy.transform.position.x <= transform.position.x)
                        transform.position = new Vector2(enemy.transform.position.x + 0.5f, transform.position.y);
                    else
                        transform.position = new Vector2(enemy.transform.position.x - 0.5f, transform.position.y);

                    break;
                }
            }
        }
    }
    // 날라가지 않고 데미지만
    public void Internal_Dameged(float damageValue)
    {
        if (DamageBlock == DefenseStatus.invincible) return;
        if (waitTime == 0)
        {
            hp -= damageValue;
            waitTime = 0.2f;
        }
    }
    // hp 임의 변경 : 아이템용이다.
    public void SetHp(float value)
    {
        float temp = hp;
        if (value > maxHP)
            hp = maxHP;
        else hp = value;

        if (temp > hp)
            PlayHitEffect(10);
    }
    public float GetHp()
    {
        return hp;
    }


    public void Dameged(float damageValue, float thrustValue)
    {
        if (DamageBlock == DefenseStatus.invincible) return;
        if (currentCombo < maxcombo && damageValue != 0)
        {
            currentCombo++;
        }
        if(currentCombo == maxcombo)
        {
            aManager.FallDown();
            currentCombo = 0;
        }

        if (waitTime == 0)
        {
            Debug.Log(gameObject);
            if (aManager)
            {
                if (DamageBlock != DefenseStatus.Guard || damageValue == 0)
                aManager.Hit(damageValue);
            }
            if (flyingDamagedPower != 0)
            {
                movement.Jump(flyingDamagedPower);
                flyingDamagedPower = 0;
            }
            if (DamageBlock != DefenseStatus.Guard)
            {
                ComboView.nextOwner = this;
                if (ComboUI)
                    Instantiate(ComboUI);
                hp -= damageValue;

                // 맞는 방향으로 회전
                if (thrustValue < 0)
                    transform.localEulerAngles = new Vector3(0, 0, 0);
                else
                    transform.localEulerAngles = new Vector3(0, 180, 0);

                if (HitEffect && damageValue > 0)
                {
                    PlayHitEffect(damageValue); 
                }
            }
            else
            {
                hp -= (float)damageValue / 2;
                Instantiate(CoolTextEffect).transform.position = transform.position;
            }
            if (DamageBlock == DefenseStatus.Warning)
            {
                hp -= 10;
            }
            waitTime = 0.2f;
            movement.StopMove = true;
            StartCoroutine(ThrustPlayer(thrustValue));
        }
    }

    IEnumerator ThrustPlayer(float thrustValue) 
    {
        yield return new WaitForSeconds(0.01f);
        if (movement)
            movement.SetThrustForceX(thrustValue);
    }

    void PlayHitEffect(float damageValue)
    {
        if (damageValue < 15)
        {
            Instantiate(HitEffect).transform.position = transform.position;
            Instantiate(HitTextEffect).transform.position = transform.position;
        }
        else
        {
            GameObject strongHit = Instantiate(StrongHitEffect);
            strongHit.transform.position = transform.position;
            if (transform.localEulerAngles.y != 0)
                strongHit.transform.localEulerAngles = new Vector3(0, 0, 0);
            else
                strongHit.transform.localEulerAngles = new Vector3(0, -180, 0);
            Instantiate(StrongHitTextEffect).transform.position = transform.position;
        }
    }
}
