using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using static Constants;

public class Highscore : MonoBehaviour {
    void Start() {
        using (StreamReader streamReader = File.OpenText( HIGHSCORE_PATH )) {
            string highscore = streamReader.ReadLine();

            if ( highscore != null ) {
                GetComponent<Text>().text = "HIGHSCORE\n" + highscore;
            }
        }
    }
}
