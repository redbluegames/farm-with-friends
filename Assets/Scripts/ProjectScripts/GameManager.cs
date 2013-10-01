using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    GameObject[] players;
    GameObject[] cameras;
    GameObject hud;

    public int NumPlayers{ get; private set; }

    int maxPlayers;
    bool isPCBound;
    bool isXBoxBound;

    // On start, turn off non P1 players.
    void Start ()
    {
        NumPlayers = 0;
        maxPlayers = 2;
        players = new GameObject[maxPlayers];
        cameras = new GameObject[maxPlayers];
        for (int i = 0; i < maxPlayers; i++) {
            players [i] = GameObject.Find ("Player" + i.ToString ());
            cameras [i] = GameObject.Find ("PlayerCamera" + i.ToString ());
            players [i].SetActive (false);
            cameras [i].SetActive (false);
        }
        hud = GameObject.Find ("HUD");
        hud.SetActive (false);
    }

    // Update is called once per frame
    void Update ()
    {
        if(NumPlayers != maxPlayers)
        {
            PollForNewPlayer();
        }
    }

    /*
     * Checks all input sources for a player pressing "Start" and binds them when they join.
     */
    void PollForNewPlayer()
    {
        int nextPlayerIndex = NumPlayers;
        foreach (InputDevice device in InputDevices.GetAllInputDevices()) {
            if (RBInput.GetButtonDownForPlayer (InputStrings.PAUSE, nextPlayerIndex, device)) {
                BindNextPlayer (device);
            }
        }
    }

    /*
     * Binds the next player object to an input device and player index.
     */
    void BindNextPlayer (InputDevice device)
    {
        int playerIndex = NumPlayers;
        NumPlayers++;
        players [playerIndex].SetActive (true);
        cameras [playerIndex].SetActive (true);

        // Split the viewports
        foreach(GameObject cameraObj in cameras)
        {
            cameraObj.GetComponent<IsometricCameraController> ().SplitScreenView (NumPlayers);
        }

        players [playerIndex].GetComponent<PlayerController> ().BindPlayer (playerIndex, device);

        hud.SetActive (true);
    }
}
