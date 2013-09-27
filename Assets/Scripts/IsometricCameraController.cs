using UnityEngine;
using System.Collections;

public class IsometricCameraController : MonoBehaviour
{
    public GameObject target;
    private Vector3 offset;
    public PlayerNum playerNum;
 
    void Start ()
    {
        SetUpForMultiplayer ();
        offset = transform.position;
    }

    void LateUpdate ()
    {
        transform.position = target.transform.position + offset;
    }

    /*
     * Split the cameras up among the players.
     */
    void SetUpForMultiplayer ()
    {
        Camera[] cameras = Camera.allCameras;
        float border = 0.002f;
        float portion = (1.0f / cameras.Length) - (border);
        int camNum = 0;
        foreach (Camera camera in cameras) {
            if (!camera.CompareTag ("MainCamera")) {
                Debug.LogError ("SetUpForMultiplayer picked up non-player cameras. This will " +
                 "   break the split screen logic.");
            }
            float spacing = camNum * border;
            camera.rect = new Rect ((camNum * portion) + spacing, 0, portion, 1);
            camNum++;
        }
    }
}
