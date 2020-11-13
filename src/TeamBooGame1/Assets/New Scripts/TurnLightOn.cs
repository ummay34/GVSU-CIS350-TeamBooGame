using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnLightOn : MonoBehaviour
{
    public GameObject Light;
    public static TurnLightOn Instance;
    public bool on;
    // Start is called before the first frame update
    void OnTriggerStay(Collider collider)
    {
        if(collider.CompareTag("Player") && Input.GetKeyDown(KeyCode.P) && GlobalControl.Instance.collectedPages == 1)
        {
            Light.SetActive(true);
            on = true;
        }
    }

    void Awake()
    {
        Instance = this;
        on = false;
    }
}
