using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    Repeater _hor = new Repeater("Mouse X");
    Repeater _ver = new Repeater("Mouse Y");
    string[] _buttons = new string[] { "Fire1", "Fire2", "Fire3" };

    public static event EventHandler<InfoEventArgs<Point>> moveEvent;
    public static event EventHandler<InfoEventArgs<int>> fireEvent;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        int x = _hor.Update();
        int y = _ver.Update();

        if (x != 0 || y != 0)
        {
            if (moveEvent != null)
                moveEvent(this, new InfoEventArgs<Point>(new Point(x, y)));
        }

        for (int i = 0; i < 3; ++i)
        {
            if (Input.GetButtonUp(_buttons[i]))
            {
                if (fireEvent != null)
                    fireEvent(this, new InfoEventArgs<int>(i));
            }
        }
    }
}

class Repeater
{
    const float threshold = 0.1f;
    const float rate = 0.1f;
    float _next;
    bool _hold;
    string _axis;

    public Repeater(string axisName)
    {
        _axis = axisName;
    }

    public int Update()
    {
        int returnValue = 0;
        int value = Mathf.RoundToInt(Input.GetAxis(_axis)); //wartość współrzędnej która będzie przesuwana
        if (value != 0) //
        {
            if (Time.time > _next)
            {
                returnValue = value;
                _next = Time.time + (_hold ? rate : threshold);
                _hold = true;
            }
        }
        else
        {
            _hold = false;
            _next = 0;
        }
        return returnValue;
    }
}
