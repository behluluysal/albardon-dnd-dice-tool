using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FaceDetector : MonoBehaviour
{
    private Rigidbody rb;

    public GameObject[] faceDetectors;

    public static event Action<int> OnDieStopped;

    public int[] faceNumbers; // Make sure this matches the faceDetectors array

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!rb.IsSleeping()) return;

        int topNumberIndex = FindFaceResult();
        int topNumber = faceNumbers[topNumberIndex];
        OnDieStopped?.Invoke(topNumber); // Notify the DiceRoller of the result

        this.enabled = false;
    }

    private int FindFaceResult()
    {
        int maxIndex = 0;
        float maxY = float.MinValue;

        for (int i = 0; i < faceDetectors.Length; i++)
        {
            if (faceDetectors[i].transform.position.y > maxY)
            {
                maxY = faceDetectors[i].transform.position.y;
                maxIndex = i;
            }
        }

        return maxIndex;
    }
}