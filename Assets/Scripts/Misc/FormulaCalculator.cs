using UnityEngine;
using System.Collections;
using static UnityEngine.Mathf;

public static class FormulaCalculator
{
    public static float Wave(float x, float t)
    {
        return Sin(PI * (x + t));
    }

    public static float CosWave(float x, float t)
    {
        return Cos(PI * (x + t));
    }

    public static float MultiWave(float x, float y, float t)
    {
        float z = Sin(PI * (x + 0.5f * t));
        z += 0.5f * Sin(2f * PI * (y + t));
        return z * (2f / 3f);
    }
    public static float AnimatedRipple(float d, float t)
    {
        float y = Sin(PI * (4f * d - t));
        return y / (1f + 10f * d);
    }

    public static float MorphWave(float x, float t)
    {
        float y = Sin(PI * (x + 0.5f * t));
        y += 0.5f * Sin(2f * PI * (x + t));
        return y;
    }

    public static float Sphere(float u, float v, float t)
    {
        float r = 0.9f + 0.1f * Sin(8f * PI * v);
        float s = r * Cos(0.5f * PI * v);
        Vector3 p;
        p.x = s * Sin(PI * u);
        p.y = r * Sin(0.5f * PI * v);
        p.z = s * Cos(PI * u);
        return r;
    }

    public static Vector2 Direction(Vector2 a, Vector2 b)
    {
        return (a - b).normalized;
    }

    public static float AngleFromPos(Vector2 a, Vector2 b)
    {
        Vector2 Temp = (a - b).normalized;
        float Ang = Atan2(Temp.x, Temp.y);
        return Ang;
    }

    public static bool DirectionR(Vector2 b)
    {
        Vector2 Pos = (new Vector2(0f, 50f) - b).normalized;

        if (Pos.x > 0f)
        {
            return true;
        }
        else if (Pos.x < 0f)
        {
            return false;
        }
        else
        {
            return false;
        }
    }
    public static bool DirectionU(Vector2 a,Vector2 b)
    {
        Vector2 Pos = (b - a).normalized;

        if (Pos.y > 0f)
        {
            return true;
        }
        else if (Pos.y < 0f)
        {
            return false;
        }
        else
        {
            return false;
        }
    }

    public static bool DirectionR(Vector2 a, Vector2 b)
    {
        Vector2 Pos =  (b - a).normalized;

        if(Pos.x > 0f)
        {
            return true;
        }
        else if (Pos.x < 0f)
        {
            return false;
        }
        else
        {
            return false;
        }
    }
    public static bool DirectionU(Vector2 b)
    {
        Vector2 Pos = (new Vector2(0f, 50f) - b).normalized;

        if (Pos.y > 0f)
        {
            return true;
        }
        else if (Pos.y < 0f)
        {
            return false;
        }
        else
        {
            return false;
        }
    }
}
