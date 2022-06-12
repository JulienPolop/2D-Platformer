using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    class AngleHelper
    {
        static public Vector2 DirFromAngle(float angleInDegree)
        {
            return new Vector2(Mathf.Cos(angleInDegree * Mathf.Deg2Rad), Mathf.Sin(angleInDegree * Mathf.Deg2Rad));
        }
        static public float AngleFromDir(Vector2 direction)
        {
            return (float)Math.Atan2(direction.y, direction.x)*180 / (float)Math.PI;
        }
    }
}
