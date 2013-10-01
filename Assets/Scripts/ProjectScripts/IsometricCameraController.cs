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
    public void SplitScreenView (int numViewports)
    {
        float border = 0.002f;
        float portion = (1.0f / numViewports) - (border);
        float spacing = viewPortIndex * border;
        camera.rect = new Rect ((viewPortIndex * portion) + spacing, 0, portion, 1);
    }
}
