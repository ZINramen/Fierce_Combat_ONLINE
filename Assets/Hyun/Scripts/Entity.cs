using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Entity : MonoBehaviour
{
    public bool stun = false;
    private int[] keyValues;

    public PhotonPlayer network;
    public bool Network_Catch = false;

    [SerializeField] private float hp;
    [SerializeField] private int mp;
    private float attackForce = 0;
    private float thrustpower = 0;

    public Movement movement;

    public float waitTime = 0;

    public float maxHP = 1000;
    public int maxMp = 100;
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

    public EmoticonController emoticon;
    public RectTransform ultScreen;

    private void Awake()
    {
        keyValues = (int[])System.Enum.GetValues(typeof(KeyCode));
     
        network = GetComponent<PhotonPlayer>();
        movement = GetComponent<Movement>();
        aManager = GetComponent<AnimationManager>();
        
        aManager.owner = this;
        movement.owner = this;
        hp = maxHP;
        mp = 0;
    }

    private void Update()
    {

        if (ultScreen) 
        {
            if(transform.localEulerAngles.y != 0) 
            {
                ultScreen.rotation = Quaternion.Euler(0, 0, 0);
            }
            else 
            {
                ultScreen.rotation = Quaternion.Euler(0, 180, 0);
            }

        }
        if (maxHP == 9999)
            hp = 9999;
        if (emoticon && network && network.pv.IsMine)
            foreach (KeyCode key in keyValues)
            {
                if (Input.GetKeyDown(key))
                {
                    if ((int)key >= 282 && (int)key <= 292)
                    {
                        int keyValue = (int)key - 281;
                        emoticon.SetValue(keyValue);
                        if(network)
                            network.pv.RPC("ShowEmoticon", RpcTarget.Others, keyValue);
                        break;
                    }
                }
            }
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
            aManager.Die();
            hp = 0;
        }
    }
    public int GetMp()
    {
        return mp;
    }
    public void AddMp(int value) 
    {
        mp += value;
        if (mp > maxMp)
            mp = maxMp;
    }
    public void ResetMp()
    {
        mp = 0;

        if (network)
            network.MpChange();
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
                    float temp = enemy.GetHp();
                    enemy.flyingDamagedPower = flyingAttackForce;
                    if (transform.localEulerAngles.y == 180)
                        enemy.Damaged(attackForce, -thrustpower);
                    else
                        enemy.Damaged(attackForce, thrustpower);
                    if (temp > enemy.GetHp())
                    {
                        AddMp(5);
                        if (network) 
                        {
                            network.MpChange();
                        }
                    }
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
    public void Internal_Damaged(float damageValue)
    {
        if (DamageBlock == DefenseStatus.invincible) return;
        AddMp(5);
        if (network)
            network.MpChange();
        if (!network || PhotonNetwork.IsMasterClient)
        {
            if (waitTime == 0)
            {
                hp -= damageValue;
                waitTime = 0.2f;
                if (network)
                    network.HpChange();
            }
        }
    }
    public void SetHpNetwork(float value)
    {
        if (value > maxHP)
            hp = maxHP;
        else hp = value;
    }
    public void SetMpNetwork(int value)
    {
        if (value > maxMp)
            mp = maxMp;
        else mp = value;
    }

    // hp 임의 변경 : 아이템용이다.
    public void SetHp(float value)
    {
        if (hp > value)
        {
            PlayHitEffect(10);
            AddMp(2);
            if (network)
                network.MpChange();
        }
        if (!network || PhotonNetwork.IsMasterClient)
        {
            if (value > maxHP)
                hp = maxHP;
            else hp = value;

            if (network)
                network.HpChange();
        }
    }
    public float GetHp()
    {
        return hp;
    }

    public void SuperDamaged(float damageValue, float thrustValue)
    {
        stun = true;
        Damaged(damageValue, thrustValue);
    }
    
    public void Damaged(float damageValue, float thrustValue)
    {
        if (DamageBlock == DefenseStatus.invincible) return;
        AddMp(10);
        if (network)
            network.MpChange();
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
            }
            if (DamageBlock != DefenseStatus.Guard && damageValue != 0)
            {
                ComboView.nextOwner = this;
                if (ComboUI)
                {
                    Instantiate(ComboUI);
                    if (network)
                        network.ComboShow();
                }

                // 맞는 방향으로 회전
                if (thrustValue < 0)
                    transform.localEulerAngles = new Vector3(0, 0, 0);
                else
                    transform.localEulerAngles = new Vector3(0, 180, 0);

                if (HitEffect && damageValue > 0)
                {
                    PlayHitEffect(damageValue);
                }

                if (!network || PhotonNetwork.IsMasterClient)
                {
                    hp -= damageValue;

                    if (network)
                        network.HpChange();
                }
            }
            else
            {
                if (!network || PhotonNetwork.IsMasterClient)
                {
                    hp -= (float)damageValue / 2;

                    if (network)
                        network.HpChange();
                }
                if(damageValue != 0)
                    Instantiate(CoolTextEffect).transform.position = transform.position;
            }
            if (DamageBlock == DefenseStatus.Warning)
            {
                if (!network || PhotonNetwork.IsMasterClient)
                {
                    hp -= 10;

                    if (network)
                        network.HpChange();
                }
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
        {
            movement.SetThrustForceX(thrustValue);
            if (network)
                network.Thrust(thrustValue);
        }
    }

    public void HitEffectWhenFly() 
    {
        if(flyingDamagedPower != 0) 
        {
            PlayHitEffect(10);
            flyingDamagedPower = 0;
        }
    }

    public void PlayHitEffect(float damageValue)
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
