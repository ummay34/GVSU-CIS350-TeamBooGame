using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene1 : MonoBehaviour
{
    [SerializeField] private string loadLevel;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GlobalControl.Instance.collectedPages == 1)
        {
            SceneManager.LoadScene(loadLevel);
        }
    }
}
