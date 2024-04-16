using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenuObject : MonoBehaviour
{
    public GameObject gameOverCanvas;

    private void Start()
    {
        gameOverCanvas = GetComponent<GameObject>();
    }

    public void setActive(bool state)
    {
        gameOverCanvas.SetActive(state);
    }
}
