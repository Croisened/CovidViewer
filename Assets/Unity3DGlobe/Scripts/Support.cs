using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Support
{

    //Using the System.Serializable will convert generic objects to an object that Unity can read and understand
    [System.Serializable]
    public class Boundaries
    {
        [Header("Horizontal Information")]
        public float minX;
        public float maxX;

        [Header("Vertical Information")]
        public float minY;
        public float maxY;

    }



}
