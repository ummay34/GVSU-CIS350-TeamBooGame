﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalControl : MonoBehaviour
{
    public static GlobalControl Instance;
    public float battery;
    public float health;
    public int collectedPages = 0;
    public int levelChange = 0;
    public int saveLevel;
    public float batteryChange;
    public float healthChange;
    public int pagesChange;
    public bool isPaused;
    public List<string> pagesList = new List<string>();


    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
