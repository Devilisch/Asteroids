using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class Asteroid : MonoBehaviour
{
    public int state = 0;
    private Vector3 startPosition;
    private Quaternion startRotation;

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        transform.position = GameObject.Find("systemObject").GetComponent<Events>().checkScreenBounds( transform.position );
    }

    private void OnCollisionEnter2D( Collision2D other ) {
        if ( other.collider.CompareTag( "Bullet" ) || other.collider.CompareTag( "UFO" ) ) {
            Destroy( other.collider.gameObject );
        } else if ( other.collider.CompareTag( "Player" ) ) {
            other.collider.gameObject.GetComponent<Player>().OnRespawn();
        }
    }

    private void OnDestroy() {
        GameObject currentObject = this.gameObject;

        GameObject.Find("systemObject").GetComponent<Events>().addPointsForAsteroid( currentObject.GetComponent<Asteroid>().state, false );

        if ( state < MAX_ASTEROID_STATES - 1 ) {
            for ( int i = 0; i < Random.Range( 0f, MAX_ASTEROID_CHUNKS); i++ ) {
                GameObject.Find("systemObject").GetComponent<Events>().createAsteroidFragment( currentObject, Vector3.left * 0.7f + Vector3.right * 1.4f );
            }
        }
    }
}
