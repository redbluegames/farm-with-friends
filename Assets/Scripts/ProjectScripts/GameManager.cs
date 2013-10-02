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

        // Deactive players, cameras, and the hud, but store references to activate them later.
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
        if (NumPlayers != maxPlayers) {
            PollForNewPlayer ();
        }
    }

    /*
     * Checks all input sources for a player pressing "Start" and binds them when they join.
     */
    void PollForNewPlayer ()
    {
        int nextPlayerIndex = NumPlayers;
        foreach (InputDevice device in InputDevices.GetAllInputDevices()) {
            if (RBInput.GetButtonDownForPlayer (InputStrings.PAUSE, nextPlayerIndex, device)) {
                BindNextPlayer (device);

                // Deactivate the splash screen once a player is bound. This is NOT ideal, but
                // neither is putting a splash screen into every scene. It should be it's own scene.
                WorldTime worldTime = (WorldTime) GetComponent<WorldTime>();
                Transform startPoint = worldTime.startPointP2;
                if (NumPlayers == 1) {
                    HideSplashScreen();
                    worldTime.Reset();
                }
                else {
                    players [nextPlayerIndex].GetComponent<PlayerController>().SnapToPoint(startPoint);
                }
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
        foreach (GameObject cameraObj in cameras) {
            cameraObj.GetComponent<IsometricCameraController> ().SplitScreenView (NumPlayers);
        }

        players [playerIndex].GetComponent<PlayerController> ().BindPlayer (playerIndex, device);

        hud.SetActive (true);
    }

    /*
     * Hide the splash screen game object, if there is one.
     */
    void HideSplashScreen ()
    {
        GameObject splashScene = GameObject.Find ("SplashScreen");
        if (splashScene != null) {
            splashScene.SetActive (false);
        }
    }
}
