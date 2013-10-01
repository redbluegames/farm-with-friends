using UnityEngine;
using System.Collections;

public class Trigger_Shop : MonoBehaviour
{
    void OnTriggerEnter (Collider other)
    {
        if (other.tag == "Player") {
            PlayerController playerController = (PlayerController)other.GetComponent<PlayerController> ();
            other.transform.position = transform.position;
            Shop shop = (Shop)GameObject.FindGameObjectWithTag ("Shop").GetComponent<Shop> ();
            shop.StartBuying (playerController.PlayerIndex);
        }
    }

    void OnTriggerExit (Collider other)
    {
        if (other.tag == "Player") {
            PlayerController playerController = (PlayerController)other.GetComponent<PlayerController> ();
            Shop shop = (Shop)GameObject.FindGameObjectWithTag ("Shop").GetComponent<Shop> ();
            shop.StopShopping (playerController.PlayerIndex);
        }
    }
}
