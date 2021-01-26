using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events : MonoBehaviour
{
    public  GameObject meteor;
    public  float      newMeteorCooldown = 15f;
    private float      newMeteorTimer    = 0f;

    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {
        if ( newMeteorTimer > 0f ) {
            newMeteorTimer -= Time.deltaTime;
            Debug.Log( "Time to spawn: " + newMeteorTimer );
        } else {
            newMeteorTimer += newMeteorCooldown;

            createNewMeteor();
        }
    }

    private void createNewMeteor() {
        GameObject newMeteor = Instantiate( meteor, transform.position + Vector3.left * 0.7f, transform.rotation );
    }
}
