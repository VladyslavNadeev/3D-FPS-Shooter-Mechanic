using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKinematics : MonoBehaviour
{
    [Header("Settings Arm Left")]
    [SerializeField] private Transform _armLeftTarget;

    [Range(0.0f, 1.0f)] 
    [SerializeField] private float _armLeftWeightPosition = 1.0f;
        
    [Range(0.0f, 1.0f)]
    [SerializeField] private float _armLeftWeightRotation = 1.0f;

    [SerializeField] private Transform[] _armLeftHierarchy;
        
    [Header("Settings Arm Right")]
    [SerializeField] private Transform _armRightTarget;
    
    [Range(0.0f, 1.0f)]
    [SerializeField] private float _armRightWeightPosition = 1.0f;
    
    [Range(0.0f, 1.0f)]
    [SerializeField] private float _armRightWeightRotation = 1.0f;
    
    [SerializeField] private Transform[] _armRightHierarchy;

    [Header("Generic")]
    [SerializeField] private Transform _hint;
        
    [Range(0.0f, 1.0f)]
    [SerializeField] private float _weightHint;
    
    private const float KSqrEpsilon = 1e-8f;
    
    private bool _maintainTargetPositionOffset;
    private bool _maintainTargetRotationOffset;

    public void Compute(float weightLeft = 1.0f, float weightRight = 1.0f)
    {
        ComputeOnce(_armLeftHierarchy, _armLeftTarget,
            _armLeftWeightPosition * weightLeft,
            _armLeftWeightRotation * weightLeft);

        ComputeOnce(_armRightHierarchy, _armRightTarget,
            _armRightWeightPosition * weightRight,
            _armRightWeightRotation * weightRight);
    }

    private void ComputeOnce(IReadOnlyList<Transform> hierarchy, Transform target, float weightPosition = 1.0f,
        float weightRotation = 1.0f)
    {
        Vector3 targetOffsetPosition = Vector3.zero;
        Quaternion targetOffsetRotation = Quaternion.identity;

        if (_maintainTargetPositionOffset)
            targetOffsetPosition = hierarchy[2].position - target.position;
        if (_maintainTargetRotationOffset)
            targetOffsetRotation = Quaternion.Inverse(target.rotation) * hierarchy[2].rotation;

        Vector3 aPosition = hierarchy[0].position;
        Vector3 bPosition = hierarchy[1].position;
        Vector3 cPosition = hierarchy[2].position;
        Vector3 targetPos = target.position;
        Quaternion targetRot = target.rotation;
        Vector3 tPosition = Vector3.Lerp(cPosition, targetPos + targetOffsetPosition, weightPosition);
        Quaternion tRotation = Quaternion.Lerp(hierarchy[2].rotation, targetRot * targetOffsetRotation, weightRotation);
        bool hasHint = _hint != null && _weightHint > 0f;

        Vector3 ab = bPosition - aPosition;
        Vector3 bc = cPosition - bPosition;
        Vector3 ac = cPosition - aPosition;
        Vector3 at = tPosition - aPosition;

        float abLen = ab.magnitude;
        float bcLen = bc.magnitude;
        float acLen = ac.magnitude;
        float atLen = at.magnitude;

        float oldAbcAngle = TriangleAngle(acLen, abLen, bcLen);
        float newAbcAngle = TriangleAngle(atLen, abLen, bcLen);

        Vector3 axis = Vector3.Cross(ab, bc);
        if (axis.sqrMagnitude < KSqrEpsilon)
        {
            axis = hasHint ? Vector3.Cross(_hint.position - aPosition, bc) : Vector3.zero;

            if (axis.sqrMagnitude < KSqrEpsilon)
                axis = Vector3.Cross(at, bc);

            if (axis.sqrMagnitude < KSqrEpsilon)
                axis = Vector3.up;
        }

        axis = Vector3.Normalize(axis);

        float a = 0.5f * (oldAbcAngle - newAbcAngle);
        float sin = Mathf.Sin(a);
        float cos = Mathf.Cos(a);
        Quaternion deltaR = new Quaternion(axis.x * sin, axis.y * sin, axis.z * sin, cos);
        hierarchy[1].rotation = deltaR * hierarchy[1].rotation;

        cPosition = hierarchy[2].position;
        ac = cPosition - aPosition;
        hierarchy[0].rotation = Quaternion.FromToRotation(ac, at) * hierarchy[0].rotation;

        if (hasHint)
        {
            float acSqrMag = ac.sqrMagnitude;
            if (acSqrMag > 0f)
            {
                bPosition = hierarchy[1].position;
                cPosition = hierarchy[2].position;
                ab = bPosition - aPosition;
                ac = cPosition - aPosition;

                Vector3 acNorm = ac / Mathf.Sqrt(acSqrMag);
                Vector3 ah = _hint.position - aPosition;
                Vector3 abProj = ab - acNorm * Vector3.Dot(ab, acNorm);
                Vector3 ahProj = ah - acNorm * Vector3.Dot(ah, acNorm);

                float maxReach = abLen + bcLen;
                if (abProj.sqrMagnitude > (maxReach * maxReach * 0.001f) && ahProj.sqrMagnitude > 0f)
                {
                    Quaternion hintR = Quaternion.FromToRotation(abProj, ahProj);
                    hintR.x *= _weightHint;
                    hintR.y *= _weightHint;
                    hintR.z *= _weightHint;
                    hintR = Quaternion.Normalize(hintR);
                    hierarchy[0].rotation = hintR * hierarchy[0].rotation;
                }
            }
        }

        hierarchy[2].rotation = tRotation;
    }

    private static float TriangleAngle(float aLen, float aLen1, float aLen2)
    {
        float c = Mathf.Clamp((aLen1 * aLen1 + aLen2 * aLen2 - aLen * aLen) / (aLen1 * aLen2) / 2.0f, -1.0f, 1.0f);
        return Mathf.Acos(c);
    }
}
