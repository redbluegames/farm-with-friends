using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    GameObject player1;
    GameObject player1Camera;
    
    // On start, turn off non P1 players.
    void Start ()
    {
        // Store references to additional players here
        player1 = GameObject.Find ("Player1");
        player1Camera = GameObject.Find ("Player1Camera");
        player1.SetActive (false);
        player1Camera.SetActive (false);
    }

    // Update is called once per frame
    void Update ()
    {
        //if (RBInput.GetButtonDownForPlayer (InputStrings.PAUSE, 1)) {
            // Unity can't Find these objects again here if they are inactive.
            // This is okay too because a GetObject in an Update is too expensive.
//            player1.SetActive (true);
            //player1Camera.SetActive (true);
            //GameObject.Find ("HUD").GetComponent<HUD> ().AddSecondPlayerHUD ();
        //}
    }
}
