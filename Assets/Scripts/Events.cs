using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using static Asteroid;

public class Events : MonoBehaviour
{
    // public  GameObject asteroid;
    public  float      newAsteroidCooldown = 5f;
    private float      newAsteroidTimer    = 0f;

    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {
        if ( newAsteroidTimer > 0f ) {
            newAsteroidTimer -= Time.deltaTime;
            Debug.Log( "Time to spawn: " + newAsteroidTimer );
        } else {
            newAsteroidTimer += newAsteroidCooldown;

            createRandomAsteroid();
        }
    }

    public Vector3 checkScreenBounds( Vector3 position ) {
        if ( position.x < SCREEN_LEFT_SIDE ) {
            position.x = SCREEN_RIGHT_SIDE;
        } else if ( position.x > SCREEN_RIGHT_SIDE ) {
            position.x = SCREEN_LEFT_SIDE;
        }

        if ( position.y < SCREEN_BOTTOM_SIDE ) {
            position.y = SCREEN_TOP_SIDE;
        } else if ( position.y > SCREEN_TOP_SIDE ) {
            position.y = SCREEN_BOTTOM_SIDE;
        }

        return position;
    }
    public void createNewBullet() {
        GameObject bullet  = new GameObject();

        bullet.name = "bullet";
        bullet.transform.localScale = new Vector3( BULLET_SCALE, BULLET_SCALE, BULLET_SCALE );
        bullet.transform.position = GameObject.Find("bulletRespawn").transform.position;
        bullet.transform.rotation = GameObject.Find("player").transform.rotation;
        bullet.tag = "Bullet";

        bullet.AddComponent<SpriteRenderer>();
        bullet.AddComponent<Rigidbody2D>();
        bullet.AddComponent<PolygonCollider2D>();
        bullet.AddComponent<Bullet>();

        Rigidbody2D rigidbody = bullet.GetComponent<Rigidbody2D>();
        rigidbody.gravityScale = 0;

        SpriteRenderer spriteRenderer = bullet.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>("Textures/bullet");

        rigidbody.AddRelativeForce( Vector2.up * BULLET_FORCE );
        Destroy( bullet, BULLET_LIFE_TIME );
    }

    public void createNewAsteroid( int state, Vector3 startPosition, Quaternion startRotation) {
        GameObject asteroid  = new GameObject();

        asteroid.name = "asteroid" + ASTEROID_STATE_NAMES[state];
        asteroid.transform.localScale = new Vector3( ASTEROID_STATE_SCALES[state], ASTEROID_STATE_SCALES[state], ASTEROID_STATE_SCALES[state] );
        asteroid.transform.position = startPosition;
        asteroid.transform.rotation = startRotation;
        asteroid.tag = "Asteroid";

        asteroid.AddComponent<SpriteRenderer>();
        asteroid.AddComponent<Rigidbody2D>();
        asteroid.AddComponent<PolygonCollider2D>();
        asteroid.AddComponent<Asteroid>();

        Rigidbody2D rigidbody = asteroid.GetComponent<Rigidbody2D>();
        SpriteRenderer spriteRenderer = asteroid.GetComponent<SpriteRenderer>();
        Asteroid asteroidScript = asteroid.GetComponent<Asteroid>();

        rigidbody.mass         = ASTEROID_STATE_MASS[state];
        rigidbody.gravityScale = 0;

        spriteRenderer.sprite = Resources.Load<Sprite>("Textures/asteroid");

        asteroidScript.state = state;

        rigidbody.AddTorque( Random.Range( -1000.0f, 1000.0f ) );
        rigidbody.AddRelativeForce( new Vector2( Random.Range( -100.0f, 100.0f ), Random.Range( -100.0f, 100.0f ) ) * 10f);
    }
    public void createRandomAsteroid() {
        createNewAsteroid( 0, getRandomRespawnPosition(), getRandomRespawnRotation() );
    }

    public void createAsteroidFragment( GameObject prevAsteroid, Vector3 positionOffset ) {
        var startPosition = prevAsteroid.transform.position + positionOffset;
        var startRotation = prevAsteroid.transform.rotation;
        var state         = prevAsteroid.GetComponent<Asteroid>().state + 1;

        createNewAsteroid(state, startPosition, startRotation);
    }

    public Vector3 getRandomRespawnPosition() {
        // доделать позднее
        return Vector3.zero;
    }

    public Quaternion getRandomRespawnRotation() {
        //доделать позднее
        return Quaternion.identity;
    }
}
