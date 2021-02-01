using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class UFO : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        GetComponent<AudioSource>().PlayOneShot( Resources.Load<AudioClip>("Sounds/ufo"), 0.05f );
    }

    // Update is called once per frame
    void Update() {
        transform.position = GameObject.Find("systemObject").GetComponent<Events>().checkScreenBounds( transform.position );

        Vector3 playerPosition = GameObject.Find("player").transform.position;

        if ( Vector3.Distance( playerPosition, transform.position ) < UFO_AGRO_ZONE ) {
            moveToPlayer( playerPosition - transform.position, Time.deltaTime );
        }
    }

    private void OnDestroy() {
        GameObject.Find("systemObject").GetComponent<Events>().addPointsForUFO( false );
    }

    private void moveToPlayer( Vector3 playerPosition, float dT ) {
        transform.Translate (playerPosition * 1.5f * dT);
    }

    private void OnCollisionEnter2D( Collision2D other ) {
        if ( other.collider.CompareTag( "Bullet" ) ) {
            Destroy( other.collider.gameObject );
        } else if ( other.collider.CompareTag( "Player" ) ) {
            other.collider.gameObject.GetComponent<Player>().OnRespawn();
        }
    }
}
