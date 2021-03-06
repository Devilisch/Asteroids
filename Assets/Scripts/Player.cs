﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class Player : MonoBehaviour {
    private float   impulseInput          = 0f;
    private bool    fireInput             = false;
    private Vector3 mouseInput;

    void Update() {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();

        impulseInput = Input.GetAxis( "Vertical" );
        fireInput    = Input.GetButtonDown( "Fire1" );
        mouseInput   = Input.mousePosition;

        if ( CompareTag( "Player" ) && fireInput ) { GameObject.Find( "systemObject" ).GetComponent<Events>().createBullet(); }
        if ( Mathf.Abs( impulseInput ) > 0.1f )    { rigidbody.AddRelativeForce( Vector2.up * impulseInput ); }

        if ( Mathf.Abs( transform.position.x - mouseInput.x) > 0.01f ||
             Mathf.Abs( transform.position.y - mouseInput.y) > 0.01f ) { transform.rotation = rotateShip( transform.position, mouseInput ); }

        this.transform.position = GameObject.Find( "systemObject" ).GetComponent<Events>().checkScreenBounds( transform.position );
    }

    private Quaternion rotateShip(Vector3 playerPosition, Vector3 mousePosition) {
        Quaternion result = Quaternion.identity;

        mousePosition      = Camera.main.ScreenToWorldPoint( mousePosition );
        var angle          = Vector2.Angle( Vector2.right, mousePosition - playerPosition );
        result.eulerAngles = new Vector3( 0f, 0f, playerPosition.y < mousePosition.y ? angle - 90 : -angle - 90 );

        return result;
    }

    public void OnRespawn() {
        var events = GameObject.Find( "systemObject" ).GetComponent<Events>();

        events.decrementLivesCounter();

        if ( events.getLivesCounter() > 0 ) {
            this.transform.position = Vector3.zero;
            this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            this.GetComponent<AudioSource>().PlayOneShot( Resources.Load<AudioClip>( "Sounds/death" ), 0.1f );
            events.makePlayerImmortal();
        } else {
            events.setGameOver();
        }
    }

    private void OnCollisionEnter2D( Collision2D other ) {
        if ( CompareTag( "Player" ) && ( other.collider.CompareTag( "Asteroid" ) || other.collider.CompareTag( "UFO" ) ) ) {
            Destroy( other.collider.gameObject );
        }
    }
}
