using UnityEngine;
using System.Collections;

public class IsometricCameraController : MonoBehaviour
{
    public GameObject target;
    public int viewPortIndex;
    private Vector3 offset;
 
    void Start ()
    {
        offset = transform.position;
    }

    void LateUpdate ()
    {
        transform.position = target.transform.position + offset;
    }

    /*
     * Split the cameras up among the players.
     */
    public void SplitScreenView ()
    {
        //TODO Let's consider tagging our player cameras as "PlayerCamera".
        GameObject[] cameras = GameObject.FindGameObjectsWithTag("MainCamera");
        GameManager gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager> ();
        float border = 0.002f;
        float portion = (1.0f / gameManager.NumPlayers) - (border);
        foreach (GameObject cameraObj in cameras) {
            Camera camera = (Camera) cameraObj.camera;
            int camNum = cameraObj.GetComponent<IsometricCameraController> ().viewPortIndex;
            if (!camera.CompareTag ("MainCamera")) {
                Debug.LogError ("SetUpForMultiplayer picked up non-player cameras. This will " +
                 "   break the split screen logic.");
            }
            float spacing = camNum * border;
            camera.rect = new Rect ((camNum * portion) + spacing, 0, portion, 1);
        }
    }
}
