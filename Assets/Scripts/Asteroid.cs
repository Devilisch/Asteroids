using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Constants;

public class Asteroid : MonoBehaviour {
    public int state = 0;

    void Update() {
        if ( SceneManager.GetActiveScene().name == MAIN_SCENE ) {
            transform.position = GameObject.Find( "systemObject" ).GetComponent<Events>().checkScreenBounds( transform.position );
        }
    }

    private void OnCollisionEnter2D( Collision2D other ) {
        if ( other.collider.CompareTag( "Bullet" ) || other.collider.CompareTag( "UFO" ) ) {
            Destroy( other.collider.gameObject );
        } else if ( other.collider.CompareTag( "Player" ) ) {
            other.collider.gameObject.GetComponent<Player>().OnRespawn();
        }
    }

    private void OnDestroy() {
        if ( !GameObject.Find( "systemObject" ).GetComponent<Events>().isMatchOver ) {
            GameObject currentObject = this.gameObject;
            Events     events        = GameObject.Find( "systemObject" ).GetComponent<Events>();

            if ( !events.isMatchOver ) {
                GameObject player = GameObject.Find( "player" );

                events.addPointsForAsteroid( currentObject.GetComponent<Asteroid>().state, events.isAsteroidDestroyedInDangerZone( player.transform.position, currentObject.transform.position ) );
            }

            if ( state < MAX_ASTEROID_STATES - 1 ) {
                int asteroidChunks = MIN_ASTEROID_CHUNKS + (int)Mathf.Round( Random.Range( 0f, MAX_ASTEROID_CHUNKS) ) ;
                for ( int i = 0; i < asteroidChunks; i++ ) {
                    events.createAsteroidFragment( this.gameObject, Vector3.left * SPACE_BETWEEN_CHUNKS * asteroidChunks / 2 + Vector3.right * SPACE_BETWEEN_CHUNKS * i );
                }
            }
        }
    }
}
