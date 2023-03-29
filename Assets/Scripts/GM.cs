using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GM : MonoBehaviour {
    private enum NESW { Up, Right, Down, Left } // never eat soggy wheat
    public GameObject[] prefabs;

    void Start() {
        int index = Random.Range(0, 16);
        List<int> history = new List<int> { index };
        for (List<NESW>[] permuations = new List<NESW>[16]; history.Count < 16;) {
            if (permuations[index].Count == 0)
                permuations[index] = new NESW[] { NESW.Up, NESW.Right, NESW.Down, NESW.Left }.OrderBy(elem => Random.Range(0f, 1f)).ToList();
            switch (permuations[index][0]) {
                case NESW.Up: index += -4; break;
                case NESW.Right: index += 1; break;
                case NESW.Down: index += 4; break;
                case NESW.Left: index += -1; break;
            }

            if (history.Contains(index)) {
                index = history.Last();
                permuations[index].RemoveAt(0);
                while (permuations[index].Count == 0) {
                    history.RemoveAt(history.Count - 1);
                    index = history.Last();
                }
            } else
                history.Add(index);
        }

        float[] positions = new float[] { -3.75f, -1.25f, 1.25f, 3.75f };
        int prev = -1;
        foreach (int item in history) {

        }
        for (int i = 0; i < 16; ++i) {
            Instantiate(prefabs[0], new Vector3(i % 4, i / 4, 0), Quaternion.identity);
        }
    }

    void Update() {
        
    }
}
