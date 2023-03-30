using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static System.Math;

public class GM : MonoBehaviour {
    public GameObject[] prefabs;
    private GameObject[] objects = new GameObject[16]; // array of scripts (tile script)?

    void Start() {
        init();
    }

    void Update() {
        if (Input.GetKeyDown("space"))
            init();
    }

    void init() {
        foreach (GameObject elem in objects)
            if (elem != null)
                Destroy(elem);

        bool flip = true;
        int index = Random.Range(0, 16);
        List<int> history = new List<int> { index };
        List<int>[] perms = new List<int>[16];
        perms[index] = new int[] { -4, 1, 4, -1 }.OrderBy(elem => Random.Range(0f, 1f)).ToList();

        while (history.Count < 16) {
            int backup = index;
            index += perms[index][0];
            if (0 > index || index >= 16 || Abs(perms[backup][0] % 2) != Abs(backup % 4 - index % 4) || history.Contains(index)) {
                flip = !flip;
                history.Insert(flip ? 0 : history.Count, -1);

                do {
                    history.RemoveAt(flip ? 0 : history.Count - 1);
                    index = flip ? history.First() : history.Last();
                    perms[index].RemoveAt(0);
                } while (perms[index].Count == 0);
            } else {
                if (!flip) history.Add(index);
                else history.Insert(0, index);
                perms[index] = new int[] { -4, 1, 4, -1 }.OrderBy(elem => Random.Range(0f, 1f)).ToList(); // never eat soggy wheat
            }
        }

        history.Insert(0, history[0]);
        history.Add(history.Last());
        float[] pos = new float[] { -3.75f, -1.25f, 1.25f, 3.75f };
        int[] table = new int[] {   hash(0, -4), hash(4, 0), hash(0, 1), hash(-1, 0), hash(0, 4), hash(-4, 0), hash(0, -1), hash(1, 0),
                                    hash(4, 4), hash(-4, -4), hash(1, 1), hash(-1, -1),
                                    hash(4, 1), hash(-1, -4), hash(-1, 4), hash(-4, 1), hash(-4, -1), hash(1, 4), hash(1, -4), hash(4, -1) };
        for (int i = 2; i < 18; ++i) {
            int elem = history[i - 1];
            objects[i - 2] = Instantiate(prefabs[System.Array.IndexOf(table, hash(elem - history[i - 2], history[i] - elem)) / 2], new Vector3(pos[elem % 4], -pos[elem / 4], 0), Quaternion.identity);
        }
    }

    int hash(int x, int y) {
        return 10 * x + y;
    }
}
