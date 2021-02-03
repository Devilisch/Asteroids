using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class UFO : MonoBehaviour {
    void Start() {
        GetComponent<AudioSource>().PlayOneShot( Resources.Load<AudioClip>( "Sounds/ufo" ), 0.05f );
    }

    void Update() {
        Events events      = GameObject.Find( "systemObject" ).GetComponent<Events>();
        transform.position = events.checkScreenBounds( transform.position );

        if ( !events.isMatchOver ) {
            GameObject player = GameObject.Find( "player" );

            if ( player.CompareTag( "Player" ) && Vector3.Distance( player.transform.position, transform.position ) < UFO_AGRO_ZONE ) {
                moveToPlayer( player.transform.position - transform.position, Time.deltaTime );
            }
        }
    }

    private void OnDestroy() {
        GameObject currentObject = this.gameObject;
        Events     events        = GameObject.Find( "systemObject" ).GetComponent<Events>();

        if ( !events.isMatchOver ) {
            GameObject player = GameObject.Find( "player" );

            events.addPointsForUFO( events.isUFODestroyedInDangerZone( player.transform.position, currentObject.transform.position ) );
        }
    }

    private void moveToPlayer( Vector3 playerPosition, float dT ) {
        transform.Translate( playerPosition * UFO_AGRO_FORCE * dT );
    }

    private void OnCollisionEnter2D( Collision2D other ) {
        if ( other.collider.CompareTag( "Bullet" ) ) {
            Destroy( other.collider.gameObject );
        } else if ( other.collider.CompareTag( "Player" ) ) {
            other.collider.gameObject.GetComponent<Player>().OnRespawn();
        }
    }
}
