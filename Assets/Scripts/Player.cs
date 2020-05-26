using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed = 0.05f;
    public float jumpPower = 16f;
    public float gravity = -20f;

    private bool onGround = false;
    public bool facingRight = true;
    private float towardsY;

    public GameObject model; 

    private Rigidbody rigid; 
    private Animator animator;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float jump = Input.GetAxis("Jump");
        animator.SetFloat("forward", Mathf.Abs(hAxis));

        if(hAxis > 0f) {
            towardsY = 0f;
            facingRight = true;
        } else if(hAxis < 0f) {
            towardsY = -180f;
            facingRight = false;
        }
        RaycastHit hitInfo;
        bool atWall = Physics.Raycast(transform.position, Vector3.right * 2f, out hitInfo, 2f);
        Debug.DrawRay(transform.position + (Vector3.up * 0.1f), Vector3.right * hitInfo.distance, Color.yellow);
        if(!atWall) transform.position += hAxis * speed * transform.forward;

        model.transform.rotation = Quaternion.Lerp(model.transform.rotation, Quaternion.Euler(0f, towardsY, 0f), Time.deltaTime * 10f);
        RaycastHit vHitInfo;

        onGround = Physics.Raycast(transform.position, Vector3.down * 2f, out vHitInfo, 0.12f);
        animator.SetBool("onGround", onGround);
        Debug.DrawRay(transform.position, Vector3.down * vHitInfo.distance, Color.red);

        if(jump > 0f && onGround == true) {
            Vector3 power = rigid.velocity;
            power.y = jumpPower;
            rigid.velocity = power;
        }
        rigid.AddForce(new Vector3(0f, gravity, 0f));
    }
}
