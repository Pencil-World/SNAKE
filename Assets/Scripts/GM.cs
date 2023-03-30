using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static System.Math;

public class GM : MonoBehaviour {
    //private enum NESW { Up, Right, Down, Left } 
    public GameObject[] prefabs;

    void Start() {
        print("start");
        int index = Random.Range(0, 16);
        int main, alt;
        main = alt = Random.Range(0, 16);
        bool face = true;
        List<int> history = new List<int> { index };
        int lim = 100000;
        for (List<int>[] permuations = new List<int>[16]; lim > 0 && history.Count < 16; --lim) {
            print(lim.ToString() + ": " + history.Count.ToString());
            if (permuations[index] == null || permuations[index].Count == 0)
                permuations[index] = new int[] { -4, 1, 4, -1 }.OrderBy(elem => Random.Range(0f, 1f)).ToList(); // never eat soggy wheat

            index += permuations[index][0];
            if (0 > index || index >= 16 || Abs(permuations[history.Last()][0] % 2) != Abs(history.Last() % 4 - index % 4) || history.Contains(index)) {
                (main, alt) = (alt, main);
                //print(string.Join(", ", history));
                index = history[face ? 0 : history.Count - 1];
                permuations[index].RemoveAt(face ? 0 : history.Count - 1);
                while (permuations[index].Count == 0) {
                    history.RemoveAt(face ? 0 : history.Count - 1);
                    index = history[face ? 0 : history.Count - 1];
                }
            } else
                history.Insert(face ? 0 : history.Count - 1, index);
        }

        print("done randomizing");
        history.Insert(0, history[0]);
        history.Add(history.Last());
        float[] positions = new float[] { -3.75f, -1.25f, 1.25f, 3.75f };
        int[] table = new int[] {   hash(0, -4), hash(4, 0), hash(0, 1), hash(-1, 0), hash(0, 4), hash(-4, 0), hash(0, -1), hash(1, 0),
                                    hash(4, 4), hash(-4, -4), hash(1, 1), hash(-1, -1),
                                    hash(4, 1), hash(-1, -4), hash(-1, 4), hash(-4, 1), hash(-4, -1), hash(1, 4), hash(1, -4), hash(4, -1) };
        for (int i = 2; i < 18; ++i) {
            int elem = history[i - 1];
            Instantiate(prefabs[table[hash(elem - history[i - 2], history[i] - elem)]], new Vector3(positions[elem % 4], positions[elem / 4], 0), Quaternion.identity);
        }
    }

    void Update() {
        
    }

    int hash(int x, int y) {
        return 10 * x + y;
    }
}
