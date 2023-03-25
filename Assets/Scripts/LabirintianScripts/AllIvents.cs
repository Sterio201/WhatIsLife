using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class AllIvents 
{
    public static UnityEvent generateMaze = new UnityEvent();
    public static UnityEvent clearMaze = new UnityEvent();
    public static UnityEvent<Transform> shiftPositionPlayer = new UnityEvent<Transform>();
    public static UnityEvent<Color> shiftColorMaze = new UnityEvent<Color>();

    public static UnityEvent pointerExitShow = new UnityEvent();
    public static GameObject pointerExit;
}