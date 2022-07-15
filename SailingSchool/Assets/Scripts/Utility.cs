using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility {
    public static float BiLerp(float v00, float v01, float v10, float v11, float t) {
        return (1 - t) * ((t - 1) * v00 + t * v01) + t * ((t - 1) * v10 + t * v11);
    }

    public static Vector2 BiLerp(Vector2 v00, Vector2 v01, Vector2 v10, Vector2 v11, float t) {
        return (1 - t) * ((t - 1) * v00 + t * v01) + t * ((t - 1) * v10 + t * v11);
    }

    public static void DrawHorizontalArrow(Vector2 start, Vector2 direction) {
        DrawArrow(FromHorizontalVector(start), FromHorizontalVector(direction));
    }

    public static void DrawArrow(Vector3 start, Vector3 direction) {
        DrawArrow(start, direction, Color.white);
    }

    public static void DrawArrow(Vector3 start, Vector3 direction, Color color) {
        Vector3 end = start + direction;
        Vector3 partway = start + direction * 0.9f;
        Vector3 perpendicular = RotateVectorCW(direction) * 0.1f;

        Debug.DrawLine(start, end);
        Debug.DrawLine(end, partway + perpendicular);
        Debug.DrawLine(end, partway - perpendicular);
    }

    public static Vector3 FromHorizontalVector(Vector2 v) {
        return new Vector3(v.x, 0.0f, v.y);
    }

    public static Vector2 RotateVectorCW(Vector2 v) {
        return new Vector2(v.y, -v.x);
    }

    public static Vector2 ToHorizontalVector(Vector3 v) {
        return new Vector2(v.x, v.z);
    }
}
