using System;
using UnityEngine;


namespace Model
{
    [Serializable]
    public struct AIConfig
    {
        public float Speed;
        public float MinDistanceToTarget;
        public float MinSqrDistanceToTarget;
        public Transform[] Waypoints;
    }
}