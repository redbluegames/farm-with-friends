using UnityEngine;
using System.Collections;

public class IsometricCameraController : MonoBehaviour
{
    public GameObject target;
    public int viewPortIndex;
    Vector3 offset;
 
    void Start ()
    {
        // Get the offset which this camera will use when following its target
        const int XOFFSET = 0;
        const int YOFFSET = 11;
        const int ZOFFSET = -7;
        offset = new Vector3(XOFFSET, YOFFSET, ZOFFSET);
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
