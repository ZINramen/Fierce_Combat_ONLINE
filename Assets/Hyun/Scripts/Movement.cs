using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    float h;
    float v;
    Rigidbody2D body;
    Animator animator;

    // Public Area
    [Header("Movement Value")]
    [Tooltip("이동 속도")]
    public float speed = 5f;

    [Tooltip("점프력")]
    public float JumpPower = 5f;

    [Space]
    [Header("Addtional Setting")]
    [Tooltip("플레이어가 조종 가능 여부")]
    public bool PlayerType = false;

    [Tooltip("움직임 봉쇄")]
    public bool StopMove = false;

    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (StopMove)
            h = 0;
        else
        {
            if (PlayerType)
                Move();
        }
    }
    private void Move()
    {
        h = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(h * 100 * speed * Time.deltaTime, body.velocity.y);

        if (h != 0)
        {
            animator.SetBool("isWalk", true);
            if (h < 0)
                transform.localEulerAngles = new Vector3(0, 180, 0);
            else
                transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else
            animator.SetBool("isWalk", false);
    }
    public void Jump(float bonus_value)
    {
        if (bonus_value == 0)
            body.velocity = new Vector2(body.velocity.x, JumpPower * 2);
        else
            body.velocity = new Vector2(body.velocity.x, bonus_value * 2);
    }
    public void SetMovementForceX(float x)
    {
        int plus = 1;
        if (transform.localEulerAngles.y == 180) plus = -1;
        body.AddForce(new Vector2(x * 100 * plus, 0));
    }
    public void SetThrustForceX(float x)
    {
        if (x < 0)
            transform.localEulerAngles = new Vector3(0, 0, 0);
        else
            transform.localEulerAngles = new Vector3(0, 180, 0);
        body.AddForce(new Vector2(x * 30, 0));
    }

    public void SetVelocityZero()
    {
        body.velocity = new Vector3(0, 0, 0);
    }
    public void UnFreeze()
    {
        body.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void Freeze()
    {
        body.constraints = RigidbodyConstraints2D.FreezePositionY & RigidbodyConstraints2D.FreezeRotation;
    }
}
