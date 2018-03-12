using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    
    [HideInInspector] public int distance;
    public const float stepHeight = 0.25f;
    public int height;

    [HideInInspector] public Tile prev;
    public Point pos;
    public GameObject content;


    public Vector3 center { get { return new Vector3(pos.x, height * stepHeight, pos.y); } }

    void MatchCurrentPosition ()
    {
        transform.localPosition = new Vector3(pos.x, height * stepHeight / 2f, pos.y);
        transform.localScale = new Vector3(1, height * stepHeight, 1);
    }

    public void Grow ()
    {
        height++;
        MatchCurrentPosition();
    }

    public void Shrink ()
    {
        height--;
        MatchCurrentPosition();
    }

    public void Load(Point p, int h)
    {
        pos = p;
        height = h;
        MatchCurrentPosition();
    }

    public void Load(Vector3 v)
    {
        Load(new Point((int)v.x, (int)v.z), (int)v.y);
    }
}
