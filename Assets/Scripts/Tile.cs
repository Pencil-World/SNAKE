using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;

public class Tile : MonoBehaviour {
    private GameObject TheOne;

    void Update() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (Abs(horizontal) == 1 && transform.position.y == TheOne.transform.position.y)
            transform.Translate(new Vector2(horizontal * 2.5f, 0));
        else if (Abs(vertical) == 1 && transform.position.x == TheOne.transform.position.x)
            transform.Translate(new Vector2(0, vertical * 2.5f));
    }

    public void init(GameObject _TheOne) {
        TheOne = _TheOne;
    }

    public void move() {
        Input.GetAxis("Horizontal");
    }
}
