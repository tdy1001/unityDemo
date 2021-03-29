using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    public Collider2D coll;
    public float speed;
    public float jumpforce;
    public LayerMask ground;
    public int cherry = 0;

    public Text CherryNumber;

    // 初始化私有对象
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // 不同设备根据实际帧数更新
    void FixedUpdate()
    {
        Movement();
        SwitchAnim();
    }

    //移动
    void Movement()
    {
        float horizontalmove = Input.GetAxis("Horizontal");
        float facedirection = Input.GetAxisRaw("Horizontal");
        
        //角色移动
        rb.velocity = new Vector2(horizontalmove * speed * Time.deltaTime, rb.velocity.y);
        anim.SetFloat("running", Mathf.Abs(facedirection));
        if(facedirection != 0)
        {
            transform.localScale = new Vector3(facedirection, 1, 1);
        }

        //角色跳跃
        if(Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.deltaTime);
            anim.SetBool("jumping", true);
        }
    }

    //切换动画
    void SwitchAnim() 
    {
        anim.SetBool("idle", false);
        if(anim.GetBool("jumping"))
        {
            if(rb.velocity.y < 0)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }else if(coll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling", false);
            anim.SetBool("idle", true);
        }
    }

    //收集物品
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Collection")
        {
            Destroy(collision.gameObject);
            cherry++;
            CherryNumber.text = cherry.ToString();
        }
    }

    
}
