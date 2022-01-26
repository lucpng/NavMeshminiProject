using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractionEntry : MonoBehaviour
{ 
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Visitors>())
        {
            other.GetComponent<Visitors>().state = Visitors.visitorStates.atAttractionEntry;
        }   
    }
}
