using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float jumpPower;
    public float speed;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        //transform.position의 값을 가져와도, 가져온 값을 바꾸는 것만으로는 오브젝트의 transform이 수정되지 않는다.
        
        
        //물리적인 이동을 구현할 때는 transform.position을 바꾸기보다 Rigidbody.MovePosition을 사용한다.
        Vector3 pos = rb.position;
        pos += new Vector3(inputX * speed * Time.deltaTime, 0, inputY * speed * Time.deltaTime);
        //transform.position = pos;
        //rb.MovePosition(pos);

        //또는 velocity를 이용
        rb.velocity = new Vector3(inputX * speed * Time.deltaTime * 60, rb.velocity.y, inputY * speed * Time.deltaTime * 60);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Mathf.Abs(rb.velocity.y) <= 0.01f)
            {
                rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }
        }
        
    }
}
