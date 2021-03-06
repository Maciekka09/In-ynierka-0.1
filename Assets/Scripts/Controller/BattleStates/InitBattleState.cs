﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitBattleState : BattleState
{
    IEnumerator Init()
    {
        board.Load(levelData);
        Point p = new Point((int)levelData.tiles[0].x, (int)levelData.tiles[0].z);
        SelectTile(p);
        SpawnTestUnits(); // This is new
        yield return null;
        owner.ChangeState<SelectUnitState>(); // This is changed
    }
    void SpawnTestUnits()
    {
        System.Type[] components = new System.Type[] { typeof(Walk), typeof(Walk), typeof(Walk) };
        for (int i = 0; i < 3; ++i)
        {
            GameObject instance = Instantiate(owner.heroPrefab) as GameObject;
            Point p = new Point((int)levelData.tiles[i].x, (int)levelData.tiles[i].z);
            Unit unit = instance.GetComponent<Unit>();
            unit.Place(board.GetTile(p));
            unit.Match();
            Movement m = instance.AddComponent(components[i]) as Movement;
            m.range = 5;
            m.jumpHeight = 1;
        }
    }

    public override void Enter()
    {
        base.Enter();
        StartCoroutine(Init());
    }
}
