using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Source: https://github.com/eppz/Unity.Library.eppz_easing
// This source was provided in the Computer Animation course.
// TODO: edit functiosn to be customizable.

public class Interpolation : MonoBehaviour
{
    // SELF DEFINED
    // 0. LERP - linear interpolation (standard)
    public static Vector3 Lerp(Vector3 v1, Vector3 v2, float t) 
    { 
        return ((1.0F - t) * v1 + t * v2); 
    }

    // 1. EASE_IN - Slow In, Fast Out (Quadratic)
    public static Vector3 EaseIn1(Vector3 v1, Vector3 v2, float t) 
    { 
        return Lerp(v1, v2, Mathf.Pow(t, 2)); 
    }

    // 2. EASE_IN_2 - Slow In, Fast Out (Cubic)
    public static Vector3 EaseIn2(Vector3 v1, Vector3 v2, float t) 
    { 
        return Lerp(v1, v2, Mathf.Pow(t, 3)); 
    }

    // 3. EASE_IN_3 - Slow In, Fast Out (Optic)
    public static Vector3 EaseIn3(Vector3 v1, Vector3 v2, float t) 
    { 
        return Lerp(v1, v2, Mathf.Pow(t, 8)); 
    }

    // 4. EASE_OUT_1 - Fast In, Slow Out
    public static Vector3 EaseOut1(Vector3 v1, Vector3 v2, float t) 
    { 
        return Lerp(v1, v2, 1.0F - Mathf.Pow(1.0F - t, 2)); 
    }

    // 5. EASE_OUT_2 - Fast In, Slow Out (Inverse Cubic)
    public static Vector3 EaseOut2(Vector3 v1, Vector3 v2, float t) 
    { 
        return Lerp(v1, v2, 1.0F - Mathf.Pow(1.0F - t, 3)); 
    }

    // 6. EASE_OUT_3 - Fast In, Slow Out (Inverse Octic)
    public static Vector3 EaseOut3(Vector3 v1, Vector3 v2, float t) 
    { 
        return Lerp(v1, v2, 1.0F - Mathf.Pow(1.0F - t, 8)); 
    }

    // 7. EASE_IN_OUT_1 - Shrink, Offset, Simplify In / Out
    public static Vector3 EaseInOut1(Vector3 v1, Vector3 v2, float t)
    {
        t = (t < 0.5F) ? 2 * Mathf.Pow(t, 2) : -2 * Mathf.Pow(t, 2) + 4 * t - 1;

        return Lerp(v1, v2, t);
    }

    // 8. EASE_IN_OUT_2 - Shrink, Offset, Simplify In / Out
    // Equation: y = (x < 0.5) ? 4x ^ 3 : 4x ^ 3-12x ^ 2 + 12x - 4
    public static Vector3 EaseInOut2(Vector3 v1, Vector3 v2, float t)
    {
        t = (t < 0.5F) ? 4 * Mathf.Pow(t, 3) : 4 * Mathf.Pow(t, 3) - 12 * Mathf.Pow(t, 2) + 12 * t - 3;

        return Lerp(v1, v2, t);
    }

    // 9. EASE_IN_OUT_3 - Shrink, Offset, Simplify In / Out
    public static Vector3 EaseInOut3(Vector3 v1, Vector3 v2, float t)
    {
        t = (t < 0.5F) ? 128 * Mathf.Pow(t, 8) : 0.5F + (1 - Mathf.Pow(2 * (1 - t), 8)) / 2.0F;

        return Lerp(v1, v2, t);
    }

    // 10. EASE_IN_CIRCULAR - Inwards (Valley) Curve
    public static Vector3 EaseInCircular(Vector3 v1, Vector3 v2, float t) { return Lerp(v1, v2, 1.0F - Mathf.Sqrt(1 - Mathf.Pow(t, 2))); }

    // 11. EASE_OUT_CIRCULAR - Outwards (Hill) Curve
    public static Vector3 EaseOutCircular(Vector3 v1, Vector3 v2, float t) { return Lerp(v1, v2, Mathf.Sqrt(-(t - 2) * t)); }

    // 12. EASE_IN_OUT_CIRCULAR - Curve Inward, Then Outwards (Valley -> Hill)
    public static Vector3 EaseInOutCircular(Vector3 v1, Vector3 v2, float t)
    {
        // changing the value of 't'.
        t = (t < 0.5F) ?
            0.5F * (1 - Mathf.Sqrt(1 - 4 * Mathf.Pow(t, 2))) :
            0.5F * (Mathf.Sqrt(-4 * (t - 2) * t - 3) + 1);

        return Lerp(v1, v2, t);
    }

    // 13. EASE_IN_BOUNCE_1 - Offset Power Composition
    public static Vector3 EaseInBounce1(Vector3 v1, Vector3 v2, float t) { return Lerp(v1, v2, Mathf.Pow(t, 2) * (2 * t - 1)); }

    // 14. EASE_IN_BOUNCE_2 - Offset Power Composition
    public static Vector3 EaseInBounce2(Vector3 v1, Vector3 v2, float t) { return Lerp(v1, v2, Mathf.Pow(t, 2) * (3 * t - 2)); }

    // 15. EASE_IN_BOUNCE_3 - Offset Power Composition
    public static Vector3 EaseInBounce3(Vector3 v1, Vector3 v2, float t) { return Lerp(v1, v2, Mathf.Pow(t, 2) * (4 * t - 3)); }

    // 16. EASE_OUT_BOUNCE_1 - Inverse offset, power composition
    public static Vector3 EaseOutBounce1(Vector3 v1, Vector3 v2, float t) { return Lerp(v1, v2, t * (t * (2 * t - 5) + 4)); }

    // 17. EASE_OUT_BOUNCE_2 - Inverse offset, power composition
    public static Vector3 EaseOutBounce2(Vector3 v1, Vector3 v2, float t) { return Lerp(v1, v2, t * (t * (3 * t - 7) + 5)); }

    // 18. EASE_OUT_BOUNCE_3 - Inverse offset, power composition
    public static Vector3 EaseOutBounce3(Vector3 v1, Vector3 v2, float t) { return Lerp(v1, v2, t * (t * (4 * t - 9) + 6)); }

    // 19. EASE_IN_OUT_BOUNCE_1 - Shrink, offset, simplify In / Out
    public static Vector3 EaseInOutBounce1(Vector3 v1, Vector3 v2, float t)
    {
        t = (t < 0.5F) ?
            8 * Mathf.Pow(t, 3) - 2 * Mathf.Pow(t, 2) :
            8 * Mathf.Pow(t, 3) - 22 * Mathf.Pow(t, 2) + 20 * t - 5;

        return Lerp(v1, v2, t);
    }

    // 20. EASE_IN_OUT_BOUNCE_2 - Shrink, offset, simplify In / Out
    public static Vector3 EaseInOutBounce2(Vector3 v1, Vector3 v2, float t)
    {
        t = (t < 0.5F) ?
            12 * Mathf.Pow(t, 3) - 4 * Mathf.Pow(t, 2) :
            12 * Mathf.Pow(t, 3) - 32 * Mathf.Pow(t, 2) + 28 * t - 7;

        return Lerp(v1, v2, t);
    }

    // 21. EASE_IN_OUT_BOUNCE_3 - Shrink, offset, simplify In / Out
    public static Vector3 EaseInOutBounce3(Vector3 v1, Vector3 v2, float t)
    {
        t = (t < 0.5F) ?
            16 * Mathf.Pow(t, 3) - 6 * Mathf.Pow(t, 2) :
            16 * Mathf.Pow(t, 3) - 42 * Mathf.Pow(t, 2) + 36 * t - 9;

        return Lerp(v1, v2, t);
    }
    
    // Start is called before the first frame update
    // void Start()
    // {
    //     
    // }
    // 
    // // Update is called once per frame
    // void Update()
    // {
    //     
    // }
}
