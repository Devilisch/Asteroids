using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static Constants;
using static Asteroid;
using static UFO;
using static System.String;

public class Events : MonoBehaviour {
    public  bool   isMatchOver         = false;
    private float  newAsteroidTimer    = 0f;
    private float  newUFOTimer         = UFO_RESPAWN_COOLDOWN;
    private float  playerImmortalTimer = 0f;
    private bool   isPlayerImmortal    = false;
    private int    score               = 0;
    private int    asteroids           = 0;
    private int    UFOs                = 0;
    private int    lives               = PLAYER_LIVES;
    private string currentTheme        = THEME_90S;

    void Start() {
        GameObject.Find( "MAIN MENU" ).GetComponent<RectTransform>().localScale = Vector3.zero;
        initLivesCounter();
    }

    void Update() {
        if ( newAsteroidTimer > 0f && asteroids > MIN_ASTEROIDS_ON_SCREEN ) {
            newAsteroidTimer -= Time.deltaTime;

            // Debug.Log( "Asteroid spawn timer: " + newAsteroidTimer );
        } else if ( asteroids < MAX_ASTEROIDS_ON_SCREEN ) {
            newAsteroidTimer = ASTEROID_RESPAWN_COOLDOWN;

            createRandomAsteroid();
        }

        if ( newUFOTimer > 0f ) {
            newUFOTimer -= Time.deltaTime;

            // Debug.Log( "UFO spawn timer: " + newUFOTimer );
        } else {
            newUFOTimer = UFO_RESPAWN_COOLDOWN;

            createRandomUFO();
        }

        if ( isPlayerImmortal ) {
            if ( playerImmortalTimer > 0f ) {
                playerImmortalTimer -= Time.deltaTime;

                // Debug.Log( "Player immortal timer: " + playerImmortalTimer );
                GameObject.Find( "Lives" ).GetComponent<Text>().text = (int)playerImmortalTimer + " sec";
            } else {
                isPlayerImmortal = false;
                GameObject.Find( "player" ).tag = "Player";
                updateLiveCounter();
            }
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

        bullet.name                 = "bullet";
        bullet.transform.localScale = new Vector3( 0.6f * BULLET_SCALE, 1.2f * BULLET_SCALE, BULLET_SCALE );
        bullet.transform.position   = GameObject.Find( "bulletRespawn" ).transform.position;
        bullet.transform.rotation   = GameObject.Find( "player" ).transform.rotation;
        bullet.tag                  = "Bullet";

        bullet.AddComponent<SpriteRenderer>();
        bullet.AddComponent<Rigidbody2D>();
        bullet.AddComponent<Bullet>();
        bullet.AddComponent<AudioSource>();

        Rigidbody2D rigidbody  = bullet.GetComponent<Rigidbody2D>();
        rigidbody.gravityScale = 0;

        SpriteRenderer spriteRenderer = bullet.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite         = Resources.Load<Sprite>( "Textures/" + currentTheme + "/bullet" );

        bullet.AddComponent<PolygonCollider2D>();

        rigidbody.AddRelativeForce( Vector2.up * BULLET_FORCE );
        Destroy( bullet, BULLET_LIFE_TIME );

        GetComponent<AudioSource>().PlayOneShot( Resources.Load<AudioClip>( "Sounds/laser" ), 0.1f );
    }

    public void createAsteroid( int state, Vector3 startPosition, Quaternion startRotation) {
        GameObject asteroid  = new GameObject();
        incrementAsteroidCounter();

        asteroid.name                 = "asteroid" + ASTEROID_STATE_NAMES[state];
        asteroid.transform.localScale = new Vector3( ASTEROID_STATE_SCALES[state], ASTEROID_STATE_SCALES[state], ASTEROID_STATE_SCALES[state] );
        asteroid.transform.position   = startPosition;
        asteroid.transform.rotation   = startRotation;
        asteroid.tag                  = "Asteroid";

        asteroid.AddComponent<Rigidbody2D>();
        asteroid.AddComponent<Asteroid>();

        Rigidbody2D rigidbody  = asteroid.GetComponent<Rigidbody2D>();
        rigidbody.mass         = ASTEROID_STATE_MASS[state];
        rigidbody.gravityScale = 0;

        Asteroid asteroidScript = asteroid.GetComponent<Asteroid>();
        asteroidScript.state    = state;

        addTextureByName(asteroid);
        asteroid.AddComponent<PolygonCollider2D>();

        rigidbody.AddTorque( UnityEngine.Random.Range( -1.0f, 1.0f ) * MAX_ASTEROID_TORQUE );
        rigidbody.AddRelativeForce( new Vector2( UnityEngine.Random.Range( -1.0f, 1.0f ), UnityEngine.Random.Range( -1.0f, 1.0f ) ) * MAX_ASTEROID_FORCE );
    }

    public void createRandomUFO() {
        GameObject ufo  = new GameObject();
        incrementUFOCounter();

        ufo.name                 = "ufo";
        ufo.transform.localScale = new Vector3( UFO_SCALE, UFO_SCALE, UFO_SCALE );
        ufo.transform.position   = getRandomRespawnPosition();
        ufo.tag                  = "UFO";

        ufo.AddComponent<Rigidbody2D>();
        ufo.AddComponent<UFO>();
        ufo.AddComponent<AudioSource>();

        Rigidbody2D rigidbody  = ufo.GetComponent<Rigidbody2D>();
        rigidbody.mass         = UFO_MASS;
        rigidbody.gravityScale = 0;

        addTextureByName(ufo);
        ufo.AddComponent<PolygonCollider2D>();

        rigidbody.AddRelativeForce( getForceVector( ufo.transform.position ) * UFO_FORCE);
    }

    private Vector2 getForceVector(Vector2 position) {
        return new Vector2( -1 * Mathf.Sign( position.x ), -1 * Mathf.Sign( position.y ) );
    }

    public void createRandomAsteroid() {
        createAsteroid( (int)Mathf.Round( UnityEngine.Random.Range( 0f, (float)MAX_ASTEROID_STATES - 1f ) ), getRandomRespawnPosition(), Quaternion.identity );
    }

    public void createAsteroidFragment( GameObject prevAsteroid, Vector3 positionOffset ) {
        var startPosition = prevAsteroid.transform.position + positionOffset;
        var startRotation = prevAsteroid.transform.rotation;
        var state         = prevAsteroid.GetComponent<Asteroid>().state + 1;

        createAsteroid(state, startPosition, startRotation);
    }

    public Vector3 getRandomRespawnPosition() {
        float x = UnityEngine.Random.Range( SCREEN_LEFT_SIDE   - RESPAWN_SPACE, SCREEN_RIGHT_SIDE + RESPAWN_SPACE );
        float y = UnityEngine.Random.Range( SCREEN_BOTTOM_SIDE - RESPAWN_SPACE, SCREEN_TOP_SIDE   + RESPAWN_SPACE );

        if ( x > SCREEN_LEFT_SIDE && x < SCREEN_RIGHT_SIDE && y > SCREEN_BOTTOM_SIDE && y < SCREEN_TOP_SIDE ) {
            if ( x > 0 ) {
                x = SCREEN_RIGHT_SIDE + RESPAWN_SPACE;
            } else {
                x = SCREEN_LEFT_SIDE  - RESPAWN_SPACE;
            }
        }

        // Debug.Log( "( " + x + " : " + y + " )" );

        return new Vector3( x, y, 0f);
    }

    public void addPointsForAsteroid(int state, bool isObjectKilledInDangerZone) {
        decrementAsteroidCounter();

        score += ( state + 1 ) * ( POINTS_PER_ASTEROID_STATE + (isObjectKilledInDangerZone ? 1 : 0) * DANGER_ZONE_ASTEROID_POINTS );

        GameObject.Find( "Score" ).GetComponent<Text>().text = score.ToString();
    }

    public void addPointsForUFO( bool isObjectKilledInDangerZone ) {
        decrementUFOCounter();

        score += POINTS_PER_UFO + (isObjectKilledInDangerZone ? 1 : 0) * DANGER_ZONE_UFO_POINTS;

        GameObject.Find( "Score" ).GetComponent<Text>().text = score.ToString();
    }

    private void incrementAsteroidCounter() {
        asteroids++;

        // GameObject.Find( "Asteroids" ).GetComponent<Text>().text = "AST:" + asteroids;
    }

    private void decrementAsteroidCounter() {
        asteroids--;

        // GameObject.Find( "Asteroids" ).GetComponent<Text>().text = "AST:" + asteroids;
    }

    private void incrementUFOCounter() {
        UFOs++;

        // GameObject.Find( "UFOs" ).GetComponent<Text>().text = "UFO:" + UFOs;
    }

    private void decrementUFOCounter() {
        UFOs--;

        // GameObject.Find( "UFOs" ).GetComponent<Text>().text = "UFO:" + UFOs;
    }

    public void decrementLivesCounter() {
        lives--;

        updateLiveCounter();
    }

    private void initLivesCounter() {
        lives = PLAYER_LIVES;

        updateLiveCounter();
    }

    private void updateLiveCounter() {
        var livesString = "";
        for ( int i = 0; i < lives; i++ ) { livesString += LIVE_SYMBOL; }

        GameObject.Find( "Lives" ).GetComponent<Text>().text = livesString;
    }

    public int getLivesCounter() {
        return lives;
    }

    public void setGameOver() {
        Destroy( GameObject.Find( "player" ) );
        writeScore();
        isMatchOver = true;
        this.GetComponent<AudioSource>().PlayOneShot( Resources.Load<AudioClip>( "Sounds/gameover" ), 0.1f );

        GameObject.Find( "GAME OVER" ).GetComponent<Text>().text = "GAME OVER";
        GameObject.Find( "MAIN MENU" ).GetComponent<RectTransform>().localScale = Vector3.one;
    }

    private void writeScore() {
        int currentHighscore = Convert.ToInt32( File.ReadAllText( HIGHSCORE_PATH ) );
        if ( currentHighscore < score ) { File.WriteAllText( HIGHSCORE_PATH, score.ToString() ); }
    }

    private void addTextureByName( GameObject gameObject ) {
        gameObject.AddComponent<SpriteRenderer>();

        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite         = Resources.Load<Sprite>( "Textures/" + currentTheme + "/" + gameObject.name );
    }

    public bool isUFODestroyedInDangerZone( Vector2 playerPosition, Vector2 UFOPosition ) {
        return (playerPosition - UFOPosition).magnitude < DANGER_ZONE_UFO_RADIUS;
    }

    public bool isAsteroidDestroyedInDangerZone( Vector2 playerPosition, Vector2 asteroidPosition ) {
        return (playerPosition - asteroidPosition).magnitude < DANGER_ZONE_ASTEROID_RADIUS;
    }

    public void makePlayerImmortal() {
        isPlayerImmortal = true;
        playerImmortalTimer = PLAYER_IMMORTAL_TIME;
        GameObject.Find( "player" ).tag = "Immortal";
    }
}
