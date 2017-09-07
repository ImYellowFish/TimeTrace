using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleController : MonoBehaviour {
    public float speed;
    public float jumpSpeed;
    public float gravity;


    public bool isGrounded;
    public Vector3 velocity;

    private CharacterController cc;

    void Start() {
        cc = GetComponent<CharacterController>();
    }

	// Update is called once per frame
	void Update () {
        isGrounded = cc.isGrounded;

        float h = 0;
        float v = 0;
        float j = 0;

        if (Input.GetKey(KeyCode.A)) {
            h = -1;
        }else if (Input.GetKey(KeyCode.D)) {
            h = 1;
        }else if (Input.GetKey(KeyCode.W)) {
            v = 1;
        }else if (Input.GetKey(KeyCode.S)) {
            v = -1;
        }

        if (Input.GetKeyDown(KeyCode.Space) && cc.isGrounded) {
            j = 1;
        }
        
        if(isGrounded)
            velocity = new Vector3(h * speed, j * jumpSpeed, v * speed);

        velocity.y += Time.deltaTime * gravity;

        cc.Move(velocity * Time.deltaTime);
	}
}
