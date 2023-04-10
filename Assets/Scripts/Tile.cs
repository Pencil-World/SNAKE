using System.Collections.Generic;
using UnityEngine;
using static System.Math;

public class Tile : MonoBehaviour {
    private Dictionary<int, int> table = new Dictionary<int, int>();

    public void init(int hash) {
        var (backward, forward) = ReadHash(hash);
        table[-backward] = 0;
        table[forward] = 0;
        if (backward == 0) {
            table[100] = forward;
            backward = forward;
        } else if (forward == 0) {
            table[backward] = -100;
            forward = backward;
        } else {
            table[backward] = forward;
            table[-forward] = -backward;
        }
        
        if (backward == forward) {
            table[-backward + Sign(backward) * 5] = 0;
            table[forward - Sign(forward) * 5] = 0;
        }
    }

    public void move(float x, float y) {
        x *= 2.5f; y *= 2.5f;
        transform.Translate(new Vector2(x, y));
        if (Abs(transform.position.x) == 6.25)
            transform.Translate(new Vector2(-4 * x, 0));
        else if (Abs(transform.position.y) == 6.25)
            transform.Translate(new Vector2(0, -4 * y));
    }

    private (int, int) ReadHash(int hash) {
        return (hash / 10 - 5, hash % 10 - 5);
    }

    public int next(int delta) {
        return table[delta];
    }
}