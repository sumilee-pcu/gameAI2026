using UnityEngine;

/// <summary>
/// 공통 유틸리티: Gizmos/Debug 시각화 헬퍼.
/// </summary>
public static class DebugDrawer
{
    /// <summary>화살표를 Gizmos로 그립니다.</summary>
    public static void DrawArrow(
        Vector3 start, Vector3 end, Color color,
        float arrowHeadLength = 0.25f, float arrowHeadAngle = 20f)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(start, end);

        Vector3 dir = (end - start).normalized;
        Vector3 right = Quaternion.LookRotation(dir) * Quaternion.Euler(0, arrowHeadAngle, 0) * Vector3.back;
        Vector3 left  = Quaternion.LookRotation(dir) * Quaternion.Euler(0, -arrowHeadAngle, 0) * Vector3.back;

        Gizmos.DrawLine(end, end + right * arrowHeadLength);
        Gizmos.DrawLine(end, end + left  * arrowHeadLength);
    }

    /// <summary>Debug.DrawLine으로 화살표를 그립니다. (런타임)</summary>
    public static void DrawArrowDebug(
        Vector3 start, Vector3 end, Color color,
        float duration = 0f, float arrowHeadLength = 0.25f)
    {
        Debug.DrawLine(start, end, color, duration);
        Vector3 dir   = (end - start).normalized;
        Vector3 right = Quaternion.LookRotation(dir) * Quaternion.Euler(0, 20f, 0) * Vector3.back;
        Vector3 left  = Quaternion.LookRotation(dir) * Quaternion.Euler(0, -20f, 0) * Vector3.back;
        Debug.DrawLine(end, end + right * arrowHeadLength, color, duration);
        Debug.DrawLine(end, end + left  * arrowHeadLength, color, duration);
    }

    /// <summary>XZ 평면에 원을 Gizmos로 그립니다.</summary>
    public static void DrawCircle(Vector3 center, float radius, Color color, int segments = 32)
    {
        Gizmos.color = color;
        float angleStep = 360f / segments;
        Vector3 prev = center + new Vector3(radius, 0f, 0f);

        for (int i = 1; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 next = center + new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * radius;
            Gizmos.DrawLine(prev, next);
            prev = next;
        }
    }

    /// <summary>3개의 수직 원으로 구를 Gizmos로 그립니다.</summary>
    public static void DrawWireSphere(Vector3 center, float radius, Color color, int segments = 24)
    {
        DrawCircleWithNormal(center, radius, color, Vector3.up,      segments);
        DrawCircleWithNormal(center, radius, color, Vector3.right,   segments);
        DrawCircleWithNormal(center, radius, color, Vector3.forward, segments);
    }

    private static void DrawCircleWithNormal(Vector3 center, float radius, Color color, Vector3 normal, int segments)
    {
        Gizmos.color = color;
        Vector3 tangent  = Vector3.Cross(normal, Vector3.up).normalized;
        if (tangent == Vector3.zero) tangent = Vector3.Cross(normal, Vector3.right).normalized;
        Vector3 biTangent = Vector3.Cross(normal, tangent);

        float angleStep = 360f / segments;
        Vector3 prev = center + tangent * radius;

        for (int i = 1; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 next = center + (tangent * Mathf.Cos(angle) + biTangent * Mathf.Sin(angle)) * radius;
            Gizmos.DrawLine(prev, next);
            prev = next;
        }
    }

    /// <summary>FOV(시야각) 부채꼴을 Gizmos로 그립니다.</summary>
    public static void DrawFOV(Vector3 origin, Vector3 forward, float angle, float radius, Color color, int segments = 20)
    {
        Gizmos.color = color;
        float halfAngle = angle * 0.5f;
        float step = angle / segments;

        Vector3 prevPoint = origin + Quaternion.AngleAxis(-halfAngle, Vector3.up) * forward * radius;

        for (int i = 1; i <= segments; i++)
        {
            float   a    = -halfAngle + step * i;
            Vector3 dir  = Quaternion.AngleAxis(a, Vector3.up) * forward;
            Vector3 next = origin + dir * radius;
            Gizmos.DrawLine(prevPoint, next);
            prevPoint = next;
        }

        // 경계선
        Gizmos.DrawLine(origin, origin + Quaternion.AngleAxis(-halfAngle, Vector3.up) * forward * radius);
        Gizmos.DrawLine(origin, origin + Quaternion.AngleAxis( halfAngle, Vector3.up) * forward * radius);
    }
}
