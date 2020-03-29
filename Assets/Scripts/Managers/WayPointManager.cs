﻿using UnityEngine;

public class WayPointManager : MonoBehaviour
{
    public static WayPointManager Get { get; set; } = null;

    public Transform[] waypoints;

    private void Awake()
    {
        Get = this;
    }

    public Transform GetWaypoint(int index)
    {
        if (index != waypoints.Length) return waypoints[index];
        return null;

    }
}