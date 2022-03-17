using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class playerMovement : MonoBehaviour
{
    public Text skill1text;
    public Text skill2text;

    private float  activeMovespeed ;
    public float moveSpeed = 5f;
    public float skill1speed = 20f;
    public float dashspeed;
    public float dashcooldown=1f;
    public float dashlenght = 0.5f;
    public float skill1cd = 1;
    private float skill1counter;
    private float dashcounter;
    private float dashcoolcounter;

    public Rigidbody2D rb;
    public Camera cam;
    public Transform firepoint;
    public GameObject skill1prefab;

    public static int  playerHealth;
    Vector2 movement;
    Vector2 mousePos;
    Vector2 lookdir;
    private void Start()
    {
        playerHealth = 100;
        skill1counter = 0;
        activeMovespeed = moveSpeed;
    }
    // Update is called once per frame
    void Update()
    {
        rb.drag = (float)playerHealth/20;
        gameObject.GetComponent<SpriteRenderer>().color =  new Color (playerHealth/100f, 0, 0,1f);
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        hudUpdate();
        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (dashcoolcounter <= 0 && dashcounter <= 0)
            {
                activeMovespeed = dashspeed;
                dashcoolcounter = dashcooldown;
                dashcounter = dashlenght;
            }
           

        }
        if (dashcoolcounter > 0)
        {
            dashcoolcounter -= Time.deltaTime;

        }
        if (dashcounter > 0)
        {
            dashcounter -= Time.deltaTime;

        }
        if (dashcounter <= 0)
        {
            activeMovespeed = moveSpeed;
        }

        
        if (Input.GetMouseButtonDown(0)&&skill1counter<=0)
        {
            shootS1();

        }
        if (skill1counter > 0) 
        {
            skill1counter -= Time.deltaTime;        
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * activeMovespeed * Time.fixedDeltaTime);

        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }



    void shootS1()
    {
        skill1counter = skill1cd;
        GameObject skill = Instantiate(skill1prefab, firepoint.position, firepoint.rotation);

        Rigidbody2D rb = skill.GetComponent<Rigidbody2D>();
        rb.AddForce(firepoint.up * skill1speed, ForceMode2D.Impulse);

    }
    void castS2()
    {
        if (dashcoolcounter <= 0 && dashcounter <= 0) 
        {
            activeMovespeed = dashspeed;
            dashcoolcounter = dashcooldown;
        }
    }
    void hudUpdate() 
    {

        skill2text.text = (dashcoolcounter).ToString("F2");

        skill1text.text = (skill1counter).ToString("F2");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("skill1"))
        {
            Destroy(collision.gameObject);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("skill1"))
        {
            
            rb.AddForce(collision.transform.up * 10000, ForceMode2D.Impulse);
            
            Destroy(collision.gameObject);
        }
    }

}
