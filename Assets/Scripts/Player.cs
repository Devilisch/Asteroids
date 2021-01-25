using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rigidbody;
    public GameObject  bullet;
    public float       impulseSpeedMultipier = 1f;
    public float       rotateAngleMultipier  = 1f;
    public float       bulletForce           = 1f;
    public float       bulletLifeTime        = 3.0f;
    public float       screenTopSide         = 10f;
    public float       screenBottomSide      = -10f;
    public float       screenRightSide       = 10f;
    public float       screenLeftSide        = -10f;

    private float   impulseInput = 0f;
    private float   rotateInput  = 0f;
    private bool    fireInput    = false;
    private Vector3 mouseInput;


    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {
        impulseInput = Input.GetAxis("Vertical");
        rotateInput  = Input.GetAxis("Horizontal");
        fireInput    = Input.GetButtonDown("Fire1");
        mouseInput   = Input.mousePosition;

        if ( fireInput )                        { createNewBullet(); }
        if ( Mathf.Abs( impulseInput ) > 0.1f ) { rigidbody.AddRelativeForce(Vector2.up * impulseInput * impulseSpeedMultipier); }
        if ( Mathf.Abs( rotateInput )  > 0.1f ) { rigidbody.AddTorque(-rotateInput * rotateAngleMultipier); }

        transform.position = checkOutOfScreenBounds(transform.position);
    }

    private void createNewBullet() {
        GameObject newBullet = Instantiate( bullet, transform.position, transform.rotation );
        newBullet.GetComponent<Rigidbody2D>().AddRelativeForce( Vector2.up * bulletForce );
        Destroy( newBullet, bulletLifeTime );
    }

    private Vector3 checkOutOfScreenBounds(Vector3 position) {
        if ( position.x < screenLeftSide ) {
            position.x = screenRightSide;
        } else if ( position.x > screenRightSide ) {
            position.x = screenLeftSide;
        }

        if ( position.y < screenBottomSide ) {
            position.y = screenTopSide;
        } else if ( position.y > screenTopSide ) {
            position.y = screenBottomSide;
        }

        return position;
    }

    void FixedUpdate() {}
}
