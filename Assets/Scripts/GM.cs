using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static System.Math;

public class GM : MonoBehaviour {
    public GameObject player;
    public GameObject[] prefabs;
    private Tile[] board = new Tile[16];
    private int agent;

    void Start() {
        init();
    }

    void Update() {
        //if (Input.GetKeyDown(KeyCode.Space) || IsSolved())
        //    NewGame();
        if (Input.GetKeyDown(KeyCode.Space))
            print(IsSolved());
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            rotate(-4);
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            rotate(1);
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            rotate(4);
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            rotate(-1);
    }

    private int Hash(int backward, int forward) {
        return 10 * (backward + 5) + (forward + 5);
    }

    // clockwise or never eat soggy wheat
    private void init() {
        foreach (Tile elem in board)
            if (elem != null)
                Destroy(elem.gameObject);

        bool flip = true;
        int index = Random.Range(0, 16);
        List<int> history = new List<int> { index };
        List<int>[] perms = new List<int>[16];
        perms[index] = new int[] { -4, 1, 4, -1 }.OrderBy(elem => Random.Range(0f, 1f)).ToList();

        while (history.Count < 16) {
            int backup = index;
            index += perms[index][0];
            if (index < 0 || 16 <= index || Abs(perms[backup][0] % 2) != Abs(backup % 4 - index % 4) || history.Contains(index)) {
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
                perms[index] = new int[] { -4, 1, 4, -1 }.OrderBy(elem => Random.Range(0f, 1f)).ToList();
            }
        }

        history.Insert(0, history[0]);
        history.Add(history.Last());
        float[] pos = new float[] { -3.75f, -1.25f, 1.25f, 3.75f };
        int[] table = new int[] {   Hash(0, -4), Hash(4, 0), Hash(0, 1), Hash(-1, 0), Hash(0, 4), Hash(-4, 0), Hash(0, -1), Hash(1, 0),
                                    Hash(4, 4), Hash(-4, -4), Hash(1, 1), Hash(-1, -1),
                                    Hash(4, 1), Hash(-1, -4), Hash(-1, 4), Hash(-4, 1), Hash(-4, -1), Hash(1, 4), Hash(1, -4), Hash(4, -1) };
        for (int i = 2; i < 18; ++i) {
            int elem = history[i - 1];
            int hash = Hash(elem - history[i - 2], history[i] - elem);
            board[elem] = Instantiate(prefabs[System.Array.IndexOf(table, hash) / 2], new Vector3(pos[elem % 4], -pos[elem / 4], 0), Quaternion.identity).GetComponent<Tile>();
            board[elem].init(hash);
        }

        agent = history[0];
        Instantiate(player, board[agent].transform);
    }

    private void rotate(int delta) {
        int initial = Abs(delta) == 1 ? agent / 4 * 4 : agent % 4;
        List<int> range = Enumerable.Range(0, 4).Select(elem => Abs(elem * Abs(delta) + initial)).ToList();
        if (0 < delta) range.Reverse();
        agent = agent == range.First() ? range.Last() : agent + delta;

        range.Add(range[0]);
        Tile temp = board[range[0]];
        int prev = range[0];
        foreach (int index in range.Skip(1)) {
            board[prev].move(Abs(delta) == 1 ? delta : 0, -delta / 4); // board[index].move() causes unity errors
            board[prev] = index == range.Last() ? temp : board[index];
            prev = index;
        }
    }

    private bool IsSolved() {
        int delta = 100, index = agent;
        for (int i = 0; i < 16; ++i) {
            delta = board[index].next(delta);
            index += delta;
            if (delta != -100 && (delta == 0 || index < 0 || 16 <= index || Abs(delta) != Abs((index - delta) % 4 - index % 4)))
                return false;
        }

        return true;
    }

    private IEnumerator NewGame() {
        yield return new WaitForSeconds(3);
        init();
    }
}