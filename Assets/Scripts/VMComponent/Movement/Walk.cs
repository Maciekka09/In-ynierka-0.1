using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : Movement
{
    protected override bool ExpandSearch(Tile from, Tile to)
    {
        // pominac za wysokie do wskoczenia na nie
        if ((Mathf.Abs(from.height - to.height) > jumpHeight))
            return false;
        // pominac zajete
        if (to.content != null)
            return false;
        return base.ExpandSearch(from, to);
    }

    public override IEnumerator Traverse(Tile tile)
    {
        unit.Place(tile);
        // Lista punktow miedzy startem a celem
        List<Tile> targets = new List<Tile>();
        while (tile != null)
        {
            targets.Insert(0, tile);
            tile = tile.prev;
        }
        // poruszanie miedzy kolejnymi pkt
        for (int i = 1; i < targets.Count; ++i)
        {
            Tile from = targets[i - 1];
            Tile to = targets[i];
            Directions dir = from.GetDirection(to);
            if (unit.direction != dir)
                yield return StartCoroutine(Turn(dir));
            if (from.height == to.height)
                yield return StartCoroutine(Walking(to));
            else
                yield return StartCoroutine(Jumping(to));
        }
        yield return null;
    }

    IEnumerator Walking(Tile target)
    {
        Tweener tweener = transform.MoveTo(target.center, 0.5f, EasingEquations.Linear);
        while (tweener != null)
            yield return null;
    }

    IEnumerator Jumping(Tile to)
    {
        Tweener tweener = transform.MoveTo(to.center, 0.5f, EasingEquations.Linear);
        Tweener t2 = jumper.MoveToLocal(new Vector3(0, Tile.stepHeight * 2f, 0), tweener.easingControl.duration / 2f, EasingEquations.EaseOutQuad);
        t2.easingControl.loopCount = 1;
        t2.easingControl.loopType = EasingControl.LoopType.PingPong;
        while (tweener != null)
            yield return null;
    }
}
