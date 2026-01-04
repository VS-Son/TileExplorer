using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectRotation : MonoBehaviour
{
    [SerializeField] private int direction;
    [SerializeField] private float speed;

    private void Start()
    {
        //StartCoroutine(OnEffectRotation());
    }

    IEnumerator OnEffectRotation()
    {
        while (true)
        {
            transform.Rotate(0f,0f,direction * speed* Time.deltaTime);
            yield return null;
        }
    }

    private void Update()
    {
        transform.Rotate(0f,0f,direction * speed* Time.deltaTime);

    }
}
