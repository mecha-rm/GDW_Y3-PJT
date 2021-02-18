using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Source: https://github.com/eppz/Unity.Library.eppz_easing
// This source was provided in the Computer Animation course.
public class Interpolation : MonoBehaviour
{
    // interpolation types for switching modes.
    public enum interType
    {
        lerp,
        easeIn1,
        easeIn2,
        easeIn3,
        easeOut1,
        easeOut2,
        easeOut3,
        easeInOut1,
        easeInOut2,
        easeInOut3,
        easeInCircular,
        easeOutCircular,
        easeInOutCircular,
        easeInBounce1,
        easeInBounce2,
        easeInBounce3,
        easeOutBounce1,
        easeOutBounce2,
        easeOutBounce3,
        easeInOutBounce1,
        easeInOutBounce2,
        easeInOutBounce3
    }

    // SELF DEFINED
    // 0. LERP - linear interpolation (standard)
    public static Vector3 Lerp(Vector3 v1, Vector3 v2, float t) 
    { 
        return ((1.0F - t) * v1 + t * v2); 
    }



    // EaseIn Operation
    public static Vector3 EaseIn(Vector3 v1, Vector3 v2, float t, float pow)
    {
        return Vector3.Lerp(v1, v2, Mathf.Pow(t, pow));
    }

    // 1. EaseIn1 - Slow In, Fast Out (Quadratic)
    public static Vector3 EaseIn1(Vector3 v1, Vector3 v2, float t) 
    { 
        return Lerp(v1, v2, Mathf.Pow(t, 2)); 
    }

    // 2. EaseIn2 - Slow In, Fast Out (Cubic)
    public static Vector3 EaseIn2(Vector3 v1, Vector3 v2, float t) 
    { 
        return Lerp(v1, v2, Mathf.Pow(t, 3)); 
    }

    // 3. EaseIn3 - Slow In, Fast Out (Optic)
    public static Vector3 EaseIn3(Vector3 v1, Vector3 v2, float t) 
    { 
        return Lerp(v1, v2, Mathf.Pow(t, 8)); 
    }




    // EaseOut Operation
    public static Vector3 EaseOut(Vector3 v1, Vector3 v2, float t, float pow)
    {
        return Vector3.Lerp(v1, v2, 1.0F - Mathf.Pow(1.0F - t, pow));
    }

    // 4. EaseOut1 Operation - Fast In, Slow Out
    public static Vector3 EaseOut1(Vector3 v1, Vector3 v2, float t) 
    { 
        return Lerp(v1, v2, 1.0F - Mathf.Pow(1.0F - t, 2)); 
    }

    // 5. EaseOut2 Operation - Fast In, Slow Out (Inverse Cubic)
    public static Vector3 EaseOut2(Vector3 v1, Vector3 v2, float t) 
    { 
        return Lerp(v1, v2, 1.0F - Mathf.Pow(1.0F - t, 3)); 
    }

    // 6. EaseOut3 Operation - Fast In, Slow Out (Inverse Octic)
    public static Vector3 EaseOut3(Vector3 v1, Vector3 v2, float t) 
    { 
        return Lerp(v1, v2, 1.0F - Mathf.Pow(1.0F - t, 8)); 
    }




    // 7. EaseInOut1 - Shrink, Offset, Simplify In / Out
    public static Vector3 EaseInOut1(Vector3 v1, Vector3 v2, float t)
    {
        t = (t < 0.5F) ? 2 * Mathf.Pow(t, 2) : -2 * Mathf.Pow(t, 2) + 4 * t - 1;

        return Lerp(v1, v2, t);
    }

    // 8. EaseInOut2 - Shrink, Offset, Simplify In / Out
    // Equation: y = (x < 0.5) ? 4x ^ 3 : 4x ^ 3-12x ^ 2 + 12x - 4
    public static Vector3 EaseInOut2(Vector3 v1, Vector3 v2, float t)
    {
        t = (t < 0.5F) ? 4 * Mathf.Pow(t, 3) : 4 * Mathf.Pow(t, 3) - 12 * Mathf.Pow(t, 2) + 12 * t - 3;

        return Lerp(v1, v2, t);
    }

    // 9. EaseInOut3 - Shrink, Offset, Simplify In / Out
    public static Vector3 EaseInOut3(Vector3 v1, Vector3 v2, float t)
    {
        t = (t < 0.5F) ? 128 * Mathf.Pow(t, 8) : 0.5F + (1 - Mathf.Pow(2 * (1 - t), 8)) / 2.0F;

        return Lerp(v1, v2, t);
    }




    // 10. EaseInCircular - Inwards (Valley) Curve
    public static Vector3 EaseInCircular(Vector3 v1, Vector3 v2, float t) 
    { 
        return Lerp(v1, v2, 1.0F - Mathf.Sqrt(1 - Mathf.Pow(t, 2))); 
    }

    // 11. EaseOutCircular - Outwards (Hill) Curve
    public static Vector3 EaseOutCircular(Vector3 v1, Vector3 v2, float t) 
    { 
        return Lerp(v1, v2, Mathf.Sqrt(-(t - 2) * t)); 
    }

    // 12. EaseInOutCircular - Curve Inward, Then Outwards (Valley -> Hill)
    public static Vector3 EaseInOutCircular(Vector3 v1, Vector3 v2, float t)
    {
        // changing the value of 't'.
        t = (t < 0.5F) ?
            0.5F * (1 - Mathf.Sqrt(1 - 4 * Mathf.Pow(t, 2))) :
            0.5F * (Mathf.Sqrt(-4 * (t - 2) * t - 3) + 1);

        return Lerp(v1, v2, t);
    }




    // EaseInBounce Operation
    public static Vector3 EaseInBounce(Vector3 v1, Vector3 v2, float t, float pow)
    {
        return Vector3.Lerp(v1, v2, Mathf.Pow(t, 2) * (pow * t - (pow - 1.0F)));
    }

    // 13. EASE_IN_BOUNCE_1 - Offset Power Composition
    public static Vector3 EaseInBounce1(Vector3 v1, Vector3 v2, float t) 
    { 
        return Lerp(v1, v2, Mathf.Pow(t, 2) * (2 * t - 1)); 
    }

    // 14. EASE_IN_BOUNCE_2 - Offset Power Composition
    public static Vector3 EaseInBounce2(Vector3 v1, Vector3 v2, float t) 
    { 
        return Lerp(v1, v2, Mathf.Pow(t, 2) * (3 * t - 2)); 
    }

    // 15. EASE_IN_BOUNCE_3 - Offset Power Composition
    public static Vector3 EaseInBounce3(Vector3 v1, Vector3 v2, float t) 
    { 
        return Lerp(v1, v2, Mathf.Pow(t, 2) * (4 * t - 3)); 
    }




    // EaseOutBounce operation
    public static Vector3 EaseOutBounce(Vector3 v1, Vector3 v2, float t, float pow)
    {
        // pow + 2 + pow - 1 -> pow * 2 + 1
        return Vector3.Lerp(v1, v2, t * (t * (pow * t - (pow * 2 + 1) + (pow + 2))));
    }

    // 16. EASE_OUT_BOUNCE_1 - Inverse offset, power composition
    public static Vector3 EaseOutBounce1(Vector3 v1, Vector3 v2, float t) 
    { 
        return Lerp(v1, v2, t * (t * (2 * t - 5) + 4)); 
    }

    // 17. EASE_OUT_BOUNCE_2 - Inverse offset, power composition
    public static Vector3 EaseOutBounce2(Vector3 v1, Vector3 v2, float t) 
    { 
        return Lerp(v1, v2, t * (t * (3 * t - 7) + 5)); 
    }

    // 18. EASE_OUT_BOUNCE_3 - Inverse offset, power composition
    public static Vector3 EaseOutBounce3(Vector3 v1, Vector3 v2, float t) 
    { 
        return Lerp(v1, v2, t * (t * (4 * t - 9) + 6)); 
    }





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

    // Interpolate by Type
    public Vector3 InterpolateByType(interType type, Vector3 v1, Vector3 v2, float t)
    {
        Vector3 result; // result of operation

        // goes based on type
        switch (type)
        {
            case interType.lerp:
                result = Lerp(v1, v2, t);
                break;
            
            case interType.easeIn1:
                result = EaseIn1(v1, v2, t);
                break;

            case interType.easeIn2:
                result = EaseIn2(v1, v2, t);
                break;

            case interType.easeIn3:
                result = EaseIn3(v1, v2, t);
                break;

            case interType.easeOut1:
                result = EaseOut1(v1, v2, t);
                break;

            case interType.easeOut2:
                result = EaseOut2(v1, v2, t);
                break;

            case interType.easeOut3:
                result = EaseOut3(v1, v2, t);
                break;

            case interType.easeInOut1:
                result = EaseInOut1(v1, v2, t);
                break;

            case interType.easeInOut2:
                result = EaseInOut2(v1, v2, t);
                break;

            case interType.easeInOut3:
                result = EaseInOut3(v1, v2, t);
                break;

            case interType.easeInCircular:
                result = EaseInCircular(v1, v2, t);
                break;

            case interType.easeOutCircular:
                result = EaseOutCircular(v1, v2, t);
                break;

            case interType.easeInOutCircular:
                result = EaseInOutCircular(v1, v2, t);
                break;

            case interType.easeInBounce1:
                result = EaseInBounce1(v1, v2, t);
                break;

            case interType.easeInBounce2:
                result = EaseInBounce2(v1, v2, t);
                break;

            case interType.easeInBounce3:
                result = EaseInBounce3(v1, v2, t);
                break;

            case interType.easeOutBounce1:
                result = EaseOutBounce1(v1, v2, t);
                break;

            case interType.easeOutBounce2:
                result = EaseOutBounce2(v1, v2, t);
                break;

            case interType.easeOutBounce3:
                result = EaseOutBounce3(v1, v2, t);
                break;

            case interType.easeInOutBounce1:
                result = EaseInOutBounce1(v1, v2, t);
                break;

            case interType.easeInOutBounce2:
                result = EaseInOutBounce2(v1, v2, t);
                break;

            case interType.easeInOutBounce3:
                result = EaseInOutBounce3(v1, v2, t);
                break;

            default: // unity lerp
                result = Vector3.Lerp(v1, v2, t);
                break;
        }

        return result;
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
