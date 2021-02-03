using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    private void OnCollisionEnter2D( Collision2D other ) {
        if ( other.collider.CompareTag( "Asteroid" ) || other.collider.CompareTag( "UFO" ) ) {
            Destroy( other.collider.gameObject );
        }
    }
}
