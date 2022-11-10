using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{

    public WaveSpawner waveSpawner;

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Button>().interactable = !waveSpawner.waveOnGoing;
    }
}
