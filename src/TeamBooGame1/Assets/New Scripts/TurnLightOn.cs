using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnLightOn : MonoBehaviour
{
    public GameObject Light;
    // Start is called before the first frame update
    void OnTriggerStay(Collider collider)
    {
        if(collider.CompareTag("Player") && Input.GetKeyDown(KeyCode.P))
        {
            Light.SetActive(true);
        }
    }
}
