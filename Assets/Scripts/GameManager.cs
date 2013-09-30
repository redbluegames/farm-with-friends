using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    GameObject player2;
    GameObject player2Camera;
    
    // On start, turn off non P1 players.
    void Start ()
    {
        // Store references to additional players here
        player2 = GameObject.Find ("Player2");
        player2Camera = GameObject.Find ("Player2 Camera");
        player2.SetActive (false);
        player2Camera.SetActive (false);
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetButtonDown ("Pause_2")) {
            // Unity can't Find these objects again here if they are inactive.
            // This is okay too because a GetObject in an Update is too expensive.
            player2.SetActive (true);
            player2Camera.SetActive (true);
            GameObject.Find ("HUD").GetComponent<HUD> ().AddSecondPlayerHUD ();
        }
    }
}
