using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    GameObject[] players;
    GameObject[] cameras;
    GameObject hud;
    public int NumPlayers{ get; private set;}

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
        for( int i = 0; i < maxPlayers; i++)
        {
            players[i] = GameObject.Find ("Player" + i.ToString());
            cameras[i] = GameObject.Find ("PlayerCamera" + i.ToString());
            players[i].SetActive(false);
            cameras[i].SetActive(false);
        }
        hud = GameObject.Find("HUD");
        hud.SetActive(false);
    }

    // Update is called once per frame
    void Update ()
    {
        int nextPlayerIndex = NumPlayers;
        //InputDevices.InputDevice nextDevice = InputDevices.KEYBOARD;
        /*foreach (InputDevices.InputDevice device in InputDevices))
         *{
         *  if( RBInput.GetButtonDownForPlayer (InputStrings.PAUSE, device, nextPlayerIndex))
         * {
         *  BindNextPlayer( device, nextPlayerIndex);
         * }
         **
         */
        InputDevices.InputDevice device = InputDevices.KEYBOARD;
        if (!isPCBound && RBInput.GetButtonDownForPlayer (InputStrings.PAUSE, nextPlayerIndex, device)) {
            BindNextPlayer (device);
            isPCBound = true;
        }
        device = InputDevices.XBOX;
        if (!isXBoxBound && RBInput.GetButtonDownForPlayer (InputStrings.PAUSE, nextPlayerIndex, device)) {
            BindNextPlayer (device);
            isXBoxBound = true;
        }
        //if (RBInput.GetButtonDownForPlayer (InputStrings.PAUSE, 1)) {
        // Unity can't Find these objects again here if they are inactive.
        // This is okay too because a GetObject in an Update is too expensive.
//            player1.SetActive (true);
        //player1Camera.SetActive (true);
        //GameObject.Find ("HUD").GetComponent<HUD> ().AddSecondPlayerHUD ();
        //}
    }

    void BindNextPlayer (InputDevices.InputDevice device)
    {
        int playerIndex = NumPlayers;
        players [playerIndex].SetActive(true);
        cameras[playerIndex].SetActive(true);
        players [playerIndex].GetComponent<PlayerController> ().BindPlayer (playerIndex, device);
        NumPlayers++;
        hud.SetActive(true);
    }
}
