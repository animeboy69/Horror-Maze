using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour {

    [SerializeField] GameObject topwall;
    [SerializeField] GameObject bottomwall;
    [SerializeField] GameObject righwall;
    [SerializeField] GameObject leftwall;

    public void Init (bool top, bool bottom, bool right, bool left) {
    
        topwall.SetActive(top);
        bottomwall.SetActive(bottom);
        righwall.SetActive(right);
        leftwall.SetActive(left);

    }

}
