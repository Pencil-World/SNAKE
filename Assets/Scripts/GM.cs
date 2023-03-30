using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static System.Math;

public class GM : MonoBehaviour {
    //private enum NESW { Up, Right, Down, Left } 
    public GameObject[] prefabs;

    void Start() {
        bool flip = true;
        int index, alt;
        index = alt = Random.Range(0, 16);
        List<int> history = new List<int> { index };

        List<int>[] perms = new List<int>[16];
        List<int>[] yo = new List<int>[16];
        for (int i = 0; i < 16; ++i) {
            perms[i] = new int[] { -4, 1, 4, -1 }.OrderBy(elem => Random.Range(0f, 1f)).ToList();
            yo[i] = new int[] { -4, 1, 4, -1 }.OrderBy(elem => Random.Range(0f, 1f)).ToList();
        }

        int timer = 0;
        for (; timer < 100000 && history.Count < 16; ++timer) {
            print(timer.ToString() + ": " + history.Count.ToString());
            index += perms[index][0];
            int backup = !flip ? history.Last() : history.First();
            if (0 > index || index >= 16 || Abs(perms[backup][0] % 2) != Abs(backup % 4 - index % 4) || history.Contains(index)) {
                flip = !flip;
                (index, alt) = (alt, index);
                (perms, yo) = (yo, perms);

                index = flip ? history.First() : history.Last();
                perms[index].RemoveAt(0);
                while (perms[index].Count == 0) {
                    history.RemoveAt(flip ? 0 : history.Count - 1);
                    index = flip ? history.First() : history.Last();
                }
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
            Instantiate(prefabs[System.Array.IndexOf(table, hash(elem - history[i - 2], history[i] - elem)) / 2], new Vector3(pos[elem % 4], -pos[elem / 4], 0), Quaternion.identity);
        }
    }

    void Update() {
        
    }

    int hash(int x, int y) {
        return 10 * x + y;
    }
}
