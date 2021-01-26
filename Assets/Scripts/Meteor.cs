using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class Meteor : MonoBehaviour
{
    public GameObject  nextStage;
    public Rigidbody2D rigidbody;
    // Start is called before the first frame update
    void Start() {
        rigidbody.AddTorque( Random.Range( -1000.0f, 1000.0f ) );
        rigidbody.AddRelativeForce( new Vector2( Random.Range( -100.0f, 100.0f ), Random.Range( -100.0f, 100.0f ) ) * 10f);
    }

    // Update is called once per frame
    void Update() {
        transform.position = checkOutOfScreenBounds(transform.position);
    }

    private Vector3 checkOutOfScreenBounds(Vector3 position) {
        if ( position.x < Constants.screenLeftSide ) {
            position.x = Constants.screenRightSide;
        } else if ( position.x > Constants.screenRightSide ) {
            position.x = Constants.screenLeftSide;
        }

        if ( position.y < Constants.screenBottomSide ) {
            position.y = Constants.screenTopSide;
        } else if ( position.y > Constants.screenTopSide ) {
            position.y = Constants.screenBottomSide;
        }

        return position;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if ( other.collider.CompareTag("Bullet") ) {
            Destroy( other.collider.gameObject );
        }
    }

    private void OnDestroy() {
        if ( nextStage.CompareTag("Asteroid") ) {
            GameObject asteroid1 = Instantiate( nextStage, transform.position + Vector3.left * 0.7f, transform.rotation );
            GameObject asteroid2 = Instantiate( nextStage, transform.position + Vector3.right * 0.7f, transform.rotation );
        }
    }
}
