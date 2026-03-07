using UnityEngine;

/// <summary>Vector3 유틸리티 확장 메서드</summary>
public static class VectorExtensions
{
    /// <summary>Y값만 변경한 새 Vector3를 반환합니다.</summary>
    public static Vector3 SetY(this Vector3 v, float y) => new Vector3(v.x, y, v.z);

    /// <summary>X값만 변경한 새 Vector3를 반환합니다.</summary>
    public static Vector3 SetX(this Vector3 v, float x) => new Vector3(x, v.y, v.z);

    /// <summary>Z값만 변경한 새 Vector3를 반환합니다.</summary>
    public static Vector3 SetZ(this Vector3 v, float z) => new Vector3(v.x, v.y, z);

    /// <summary>Y=0으로 평탄화한 Vector3를 반환합니다. (수평 방향 계산에 유용)</summary>
    public static Vector3 Flat(this Vector3 v) => new Vector3(v.x, 0f, v.z);

    /// <summary>XZ 성분만 추출한 Vector2를 반환합니다.</summary>
    public static Vector2 ToVector2XZ(this Vector3 v) => new Vector2(v.x, v.z);

    /// <summary>Vector2(x,z)를 Vector3(x,0,z)로 변환합니다.</summary>
    public static Vector3 ToVector3XZ(this Vector2 v) => new Vector3(v.x, 0f, v.y);

    /// <summary>벡터를 원뿔 안에서 최대 maxAngle도 회전시킵니다.</summary>
    public static Vector3 RandomDeviation(this Vector3 v, float maxAngle)
    {
        float angle = Random.Range(0f, maxAngle);
        Vector3 randomAxis = Random.insideUnitSphere;
        randomAxis = Vector3.Cross(v, randomAxis).normalized;
        return Quaternion.AngleAxis(angle, randomAxis) * v;
    }

    /// <summary>두 벡터가 거의 같은지 확인합니다 (부동소수점 오차 허용).</summary>
    public static bool Approximately(this Vector3 a, Vector3 b, float tolerance = 0.001f)
        => Vector3.Distance(a, b) < tolerance;
}
