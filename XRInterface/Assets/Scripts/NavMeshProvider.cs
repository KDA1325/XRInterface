using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

/// <summary>
/// TeleportationProvider를 확장해서 NavMeshAgent를 통해 자연스럽게 목적지까지 이동하게 함.
/// 순간이동 대신 목적지까지 경로를 따라 이동.
/// </summary>
[AddComponentMenu("XR/Locomotion/NavMesh Teleportation Provider")]
public class NavMeshProvider : TeleportationProvider
{
    [SerializeField]
    [Tooltip("이동에 사용할 NavMeshAgent")]
    private NavMeshAgent navMeshAgent;

    [SerializeField]
    [Tooltip("텔레포트 요청 후 이동 시작까지 지연 시간 (초)")]
    private float customDelayTime = 0.3f;

    private float m_DelayStart;
    private bool isDelaying = false;

    public override bool QueueTeleportRequest(TeleportRequest teleportRequest)
    {
        Debug.Log("[NavMeshProvider] QueueTeleportRequest 호출됨");

        currentRequest = teleportRequest;
        validRequest = true;
        return true;
    }

    protected override void Update()
    {
        if (!validRequest)
            return;

        if (locomotionState == LocomotionState.Idle)
        {
            if (customDelayTime > 0f)
            {
                if (!isDelaying && TryPrepareLocomotion())
                {
                    m_DelayStart = Time.time;
                    isDelaying = true;
                }

                if (isDelaying && Time.time - m_DelayStart >= customDelayTime)
                {
                    TryStartLocomotionImmediately();
                }
            }
            else
            {
                TryStartLocomotionImmediately();
            }
        }

        if (locomotionState == LocomotionState.Moving)
        {
            Vector3 target = currentRequest.destinationPosition;
            Debug.Log($"[NavMeshProvider] 요청된 목적지: {target}");

            if (NavMesh.SamplePosition(target, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                navMeshAgent.SetDestination(hit.position);
                Debug.Log($"[NavMeshTeleportationProvider] 목적지 설정됨: {hit.position}");
            }
            else
            {
                Debug.LogWarning("[NavMeshTeleportationProvider] 유효한 NavMesh 위치를 찾지 못함");
            }

            TryEndLocomotion();
            validRequest = false;
            isDelaying = false;
        }
    }
}

    //[SerializeField]
    //[Tooltip("이동에 사용할 NavMeshAgent")]
    //private NavMeshAgent navMeshAgent;

    //private void Update()
    //{
    //    if()
    //}
    //protected virtual void Update()
    //{

    //}


    //[SerializeField]
    //[Tooltip("텔레포트 지연 시간 (초)")]
    //private float customDelayTime = 0.3f;

    //private float m_DelayStartTime = 0f;
    //private bool isDelaying = false;

    //protected override void Update()
    //{
    //    if (!validRequest)
    //        return;

    //    if (locomotionState == LocomotionState.Idle)
    //    {
    //        if (customDelayTime > 0f)
    //        {
    //            if (!isDelaying && TryPrepareLocomotion())
    //            {
    //                m_DelayStartTime = Time.time;
    //                isDelaying = true;
    //            }

    //            if (isDelaying && Time.time - m_DelayStartTime >= customDelayTime)
    //            {
    //                TryStartLocomotionImmediately();
    //            }
    //        }
    //        else
    //        {
    //            TryStartLocomotionImmediately();
    //        }
    //    }

    //    if (locomotionState == LocomotionState.Moving)
    //    {
    //        Vector3 navTarget = currentRequest.destinationPosition;

    //        if (NavMesh.SamplePosition(navTarget, out NavMeshHit navHit, 1.0f, NavMesh.AllAreas))
    //        {
    //            navMeshAgent.SetDestination(navHit.position);
    //            Debug.Log($"[NavMeshProvider] NavMeshAgent 목적지 설정됨: {navHit.position}");
    //        }
    //        else
    //        {
    //            Debug.LogWarning("[NavMeshProvider] NavMesh에서 유효한 위치를 찾지 못함.");
    //        }

    //        TryEndLocomotion();
    //        validRequest = false;
    //        isDelaying = false;
    //    }
    //}



//using UnityEngine;
//using UnityEngine.AI;
//using UnityEngine.Assertions;
//using UnityEngine.XR.Interaction.Toolkit;
//using UnityEngine.XR.Interaction.Toolkit.Locomotion;
//using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

//[AddComponentMenu("XR/Locomotion/NavMesh Teleportation Provider")]

//public class NavMeshProvider : TeleportationProvider
//{
//    [SerializeField]
//    [Tooltip("이동에 사용할 NavMeshAgent")]
//    private NavMeshAgent navMeshAgent;

//    float m_DelayStart;

//    protected override void Update()
//    {
//        if (!validRequest)
//            return;

//        if (locomotionState == LocomotionState.Idle)
//        {
//            if (m_DelayStart > 0f)
//            {
//                if (TryPrepareLocomotion())
//                    m_DelayStart = Time.time;
//            }
//            else
//            {
//                TryStartLocomotionImmediately();
//            }
//        }

//        if (locomotionState == LocomotionState.Moving)
//        {
//            // 기존: XR Origin을 이동시킴 → 여기 수정
//            Vector3 navTarget = currentRequest.destinationPosition;

//            if (NavMesh.SamplePosition(navTarget, out NavMeshHit navHit, 1.0f, NavMesh.AllAreas))
//            {
//                navMeshAgent.SetDestination(navHit.position);
//                Debug.Log($"NavMeshAgent 목적지 설정됨: {navHit.position}");
//            }
//            else
//            {
//                Debug.LogWarning("NavMesh에서 유효한 위치를 찾지 못함.");
//            }

//            TryEndLocomotion();
//            validRequest = false;
//        }
//    }
//}
