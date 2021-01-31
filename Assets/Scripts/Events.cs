using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Constants;
using static Asteroid;
using static UFO;

public class Events : MonoBehaviour
{
    // public  GameObject asteroid;
    private float newAsteroidTimer = 0f;
    private float newUFOTimer      = UFO_RESPAWN_COOLDOWN;
    private int   score            = 0;
    private int   asteroids        = 0;
    private int   UFOs             = 0;
    private int   lives           = PLAYER_LIVES;

    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {
        if ( newAsteroidTimer > 0f && asteroids > 0 ) {
            newAsteroidTimer -= Time.deltaTime;

            Debug.Log( "Time to spawn asteroid: " + newAsteroidTimer );
        } else if ( asteroids < MAX_ASTEROIDS_ON_SCREEN ) {
            newAsteroidTimer = ASTEROID_RESPAWN_COOLDOWN;

            createRandomAsteroid();
        }

        if ( newUFOTimer > 0f ) {
            newUFOTimer -= Time.deltaTime;

            Debug.Log( "Time to spawn UFO: " + newUFOTimer );
        } else {
            newUFOTimer = UFO_RESPAWN_COOLDOWN;

            createRandomUFO();
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

    public void createBullet() {
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

    public void createAsteroid( int state, Vector3 startPosition, Quaternion startRotation) {
        GameObject asteroid  = new GameObject();
        incrementAsteroidCounter();

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
        rigidbody.AddRelativeForce( new Vector2( Random.Range( -100.0f, 100.0f ), Random.Range( -100.0f, 100.0f ) ) * 50f);
    }

    public void createRandomUFO() {
        GameObject ufo  = new GameObject();
        incrementUFOCounter();

        ufo.name = "ufo";
        ufo.transform.localScale = new Vector3( UFO_SCALE, UFO_SCALE, UFO_SCALE );
        ufo.transform.position = getRandomRespawnPosition();
        ufo.tag = "UFO";

        ufo.AddComponent<SpriteRenderer>();
        ufo.AddComponent<Rigidbody2D>();
        ufo.AddComponent<PolygonCollider2D>();
        ufo.AddComponent<UFO>();

        Rigidbody2D rigidbody = ufo.GetComponent<Rigidbody2D>();
        SpriteRenderer spriteRenderer = ufo.GetComponent<SpriteRenderer>();

        rigidbody.mass         = UFO_MASS;
        rigidbody.gravityScale = 0;

        spriteRenderer.sprite = Resources.Load<Sprite>("Textures/ufo");

        rigidbody.AddRelativeForce( new Vector2( Random.Range( -100.0f, 100.0f ), Random.Range( -100.0f, 100.0f ) ) * 50f);
    }

    public void createRandomAsteroid() {
        createAsteroid( 0, getRandomRespawnPosition(), getRandomRespawnRotation() );
    }

    public void createAsteroidFragment( GameObject prevAsteroid, Vector3 positionOffset ) {
        var startPosition = prevAsteroid.transform.position + positionOffset;
        var startRotation = prevAsteroid.transform.rotation;
        var state         = prevAsteroid.GetComponent<Asteroid>().state + 1;

        createAsteroid(state, startPosition, startRotation);
    }

    public Vector3 getRandomRespawnPosition() {
        float x = Random.Range( SCREEN_LEFT_SIDE   - RESPAWN_SPACE, SCREEN_RIGHT_SIDE + RESPAWN_SPACE );
        float y = Random.Range( SCREEN_BOTTOM_SIDE - RESPAWN_SPACE, SCREEN_TOP_SIDE   + RESPAWN_SPACE );

        if ( x > SCREEN_LEFT_SIDE && x < SCREEN_RIGHT_SIDE && y > SCREEN_BOTTOM_SIDE && y < SCREEN_TOP_SIDE ) {
            if ( x > 0 ) {
                x = SCREEN_RIGHT_SIDE + RESPAWN_SPACE;
            } else {
                x = SCREEN_LEFT_SIDE  - RESPAWN_SPACE;
            }
        }

        // Debug.Log("( " + x + " : " + y + " )");

        return new Vector3( x, y, 0f);
    }

    public Quaternion getRandomRespawnRotation() {
        //доделать позднее
        return Quaternion.identity;
    }

    public void addPointsForAsteroid(int state, bool isObjectKilledInDangerZone) {
        decrementAsteroidCounter();
        score += ( state + 1 ) * ( POINTS_PER_ASTEROID_STATE + (isObjectKilledInDangerZone ? 1 : 0) * DANGER_ZONE_ASTEROID_POINTS );
        GameObject.Find("Score").GetComponent<Text>().text = "SCORE: " + score;
    }

    public void addPointsForUFO( bool isObjectKilledInDangerZone ) {
        decrementUFOCounter();
        score += POINTS_PER_UFO + (isObjectKilledInDangerZone ? 1 : 0) * DANGER_ZONE_UFO_POINTS;
        GameObject.Find("Score").GetComponent<Text>().text = "SCORE: " + score;
    }

    private void incrementAsteroidCounter() {
        asteroids++;
        GameObject.Find("Asteroids").GetComponent<Text>().text = "ASTEROIDS: " + asteroids;
    }

    private void decrementAsteroidCounter() {
        asteroids--;
        GameObject.Find("Asteroids").GetComponent<Text>().text = "ASTEROIDS: " + asteroids;
    }

    private void incrementUFOCounter() {
        UFOs++;
        GameObject.Find("UFOs").GetComponent<Text>().text = "UFOS: " + UFOs;
    }

    private void decrementUFOCounter() {
        UFOs--;
        GameObject.Find("UFOs").GetComponent<Text>().text = "UFOS: " + UFOs;
    }

    public void decrementLivesCounter() {
        lives--;

        var livesString = "";
        for ( int i = 0; i < lives; i++ ) { livesString += LIVE_SYMBOL + " "; }

        GameObject.Find("Lives").GetComponent<Text>().text = "LIVES: " + livesString;
    }

    private void initLivesCounter() {
        var livesString = "";
        for ( int i = 0; i < PLAYER_LIVES; i++ ) { livesString += LIVE_SYMBOL + " "; }

        GameObject.Find("Lives").GetComponent<Text>().text = "LIVES: " + livesString;
    }

    public int getLivesCounter() {
        return lives;
    }

    public void setGameOver() {
        Destroy( GameObject.Find("player") );
        GameObject.Find("GAME OVER").GetComponent<Text>().text = "GAME OVER";
    }
}
