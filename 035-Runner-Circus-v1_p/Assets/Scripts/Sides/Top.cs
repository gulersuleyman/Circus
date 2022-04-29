using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class Top : Sides
{
    public bool isTop = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Start"))
        {
            isTop = true;
            Swipe(other);

        }
    }
    
}
