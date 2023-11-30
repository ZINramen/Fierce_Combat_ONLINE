using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class AnimationManager : MonoBehaviour
{
    PhotonPlayer network;
    public Entity owner;
    public GameObject temp;
    private bool onGround = true;

    public bool ecActive = false;
    public float rotationZ = 0;

    public enum AnimationState 
    { Normal, Jump, Fall, Emotion, Stun }
    
    public Animator ani;
    public AnimationState State = AnimationState.Normal;

    [Header("PlayerSet")] 
    [Tooltip("조종할 플레이어 캐릭터의 경우 True")]
    public bool isPlayer = false;
    public bool isHuman = false;

    public EffectCreator Ec;

    [Header("Key Mapping")]
    public KeyCode Punch;
    public KeyCode Kick;
    public KeyCode Guard;
    public KeyCode Catch;
    public KeyCode Dash;
    public KeyCode Backstep;
    public KeyCode Jump;

    public KeyCode DownArrow;
    public KeyCode UpArrow;

    public KeyCode Emotion_1;
    public KeyCode Heal;

    // Start is called before the first frame update
    void Start()
    {
        network = GetComponent<PhotonPlayer>();
        ani = GetComponent<Animator>();
        if (network)
            network.am = this;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit;
        if (hit = Physics2D.Raycast(transform.position - new Vector3(0, transform.localScale.y / 2), Vector3.down, 0.2f))
        {
            if (hit.collider.transform.parent != transform && (!hit.collider.GetComponent<EffectCreator>() || owner.movement.StopMove))
            if (State == AnimationState.Fall)
            {
                onGround = true;
                if (isHuman)
                {
                    ani.ResetTrigger("Jump");
                    ani.SetTrigger("Landing");
                    if (network)
                        network.RunTriggerRpc("Landing");
                }
                State = AnimationState.Normal;
                if (Ec && ecActive)
                {
                    Ec.PlayEffect("bang", hit);

                    ecActive = false;
                }
            }
        }
        else
        {
            if (State == AnimationState.Normal)
            {
                if (isHuman)
                {
                    ani.SetTrigger("fall");

                    if (network)
                        network.RunTriggerRpc("fall");
                }
            }
            if (onGround)
            {
                onGround = false;
                if (isHuman)
                {
                    ani.ResetTrigger("Landing");
                }
            }
            else
            {
                if (isPlayer)
                {
                    if (Input.GetKeyDown(Kick))
                    {
                        ani.SetTrigger("Kick");

                        if (network)
                            network.RunTriggerRpc("Kick");
                        ecActive = true;
                    }
                    if (Input.GetKeyDown(Punch))
                    {
                        ani.SetTrigger("Punch");
                        if (network)
                            network.RunTriggerRpc("Punch");
                    }
                }
            }
        }
        if (isPlayer)
            PlayerAnimation();
        
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, rotationZ);
    }
    void ResetTriggerEvent(string name) 
    {
        ani.ResetTrigger(name);
    }
    void ResetAttackTriggerEvent()
    {
        ani.ResetTrigger("Punch_Up");
        ani.ResetTrigger("Punch");
        ani.ResetTrigger("Kick");
        ani.ResetTrigger("Catch");
        ani.ResetTrigger("Dodge");
        ani.ResetTrigger("Dash");
    }

    void StateChange(AnimationState newState) 
    {
        State = newState;
        ani.ResetTrigger("Jump");
        ani.ResetTrigger("Breaktime");
    }
    public void Hit(float power)
    {
        DynamicCamera actionCam = Camera.main.GetComponent<DynamicCamera>();
        if (Math.Abs(power) > 10)
        {
            if(actionCam)
                actionCam.ShakeScreen(5);
            ani.SetTrigger("Hit_Upgrade");
            if (network)
                network.RunTriggerRpc("Hit_Upgrade");
        }
        else
        {
            if (actionCam)
                actionCam.ShakeScreen(5);
            ani.SetTrigger("Hit");
            if (network)
                network.RunTriggerRpc("Hit");
        }

    }

    public void FallDown()
    {
        ani.SetTrigger("ComboEnd");
        if (network)
            network.RunTriggerRpc("ComboEnd");
    }

    public void Die()
    {
        ani.SetTrigger("Death");
        if (network)
            network.RunTriggerRpc("Death");
    }

    public void Network_SetTrigger(string name) 
    {
        ani.SetTrigger(name);
    } 

    public void Network_Effect()
    {
        RaycastHit2D hit;
        if (hit = Physics2D.Raycast(transform.position - new Vector3(0, transform.localScale.y / 2), Vector3.down, 500f))
        {
            GameObject effect = Instantiate(temp);
            effect.transform.position = hit.point;
        }
    }

    void PlayerAnimation() // 조종하는 플레이어 캐릭터의 애니메이션 관리 -> 입력에 반응
    {
        if (Physics2D.Raycast(transform.position - new Vector3(0, transform.localScale.y/2), Vector3.down, 0.2f) && Input.GetKeyDown(Jump) && !Input.GetKey(DownArrow))
        {
            State = AnimationState.Jump;
            ani.SetTrigger("Jump");
            if (network)
                network.RunTriggerRpc("Jump");
                
        }
        if (State == AnimationState.Normal) 
        {
            if (Input.GetAxis("Horizontal") == 0 && Input.GetKeyDown(Emotion_1))
            {
                State = AnimationState.Emotion;
                ani.SetTrigger("Breaktime");
                if (network)
                    network.RunTriggerRpc("Breaktime");
            }
            if (Input.GetKeyDown(Punch))
            {
                if (Input.GetKey(UpArrow))
                {
                    ani.SetTrigger("Punch_Up");
                    if (network)
                        network.RunTriggerRpc("Punch_Up");
                }
                else if (!ani.GetBool("Down"))
                {
                    ani.SetTrigger("Punch");
                    if (network)
                        network.RunTriggerRpc("Punch");
                }
            }
            if (Input.GetKeyDown(Kick))
            {
                ani.SetTrigger("Kick");
                if (network)
                    network.RunTriggerRpc("Kick");
            }
            if (Input.GetKey(DownArrow))
            {
                ani.SetBool("Down", true);
            }
            if (Input.GetKeyUp(DownArrow))
            {
                ani.SetBool("Down", false);
            }
            if (Input.GetKey(Guard))
            {
                ani.SetBool("Defense", true);
            }
            if (Input.GetKeyUp(Guard))
            {
                ani.SetBool("Defense", false);
            }
            if (Input.GetAxis("Horizontal") == 0)
            {
                if (Input.GetKeyDown(Dash) && !ani.GetBool("Down"))
                {
                    ani.SetTrigger("Dash");
                    if (network)
                        network.RunTriggerRpc("Dash");
                }
            }
            if (Input.GetKeyUp(Catch))
            {
                ani.SetTrigger("Catch");
                if (network)
                    network.RunTriggerRpc("Catch");
            }
            if (Input.GetKeyUp(Backstep))
            {
                if (network)
                    network.RunTriggerRpc("Dodge");
            }
            if (Input.GetKey(Heal))
            {
                ani.SetBool("Heal", true);
            }
            if (Input.GetKeyUp(Heal))
            {
                ani.SetBool("Heal", false);
            }
        }
    }
}
