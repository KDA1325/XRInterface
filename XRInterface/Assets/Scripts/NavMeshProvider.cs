using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

/// <summary>
/// TeleportationProvider�� Ȯ���ؼ� NavMeshAgent�� ���� �ڿ������� ���������� �̵��ϰ� ��.
/// �����̵� ��� ���������� ��θ� ���� �̵�.
/// </summary>
[AddComponentMenu("XR/Locomotion/NavMesh Teleportation Provider")]
public class NavMeshProvider : TeleportationProvider
{
    [SerializeField]
    [Tooltip("�̵��� ����� NavMeshAgent")]
    private NavMeshAgent navMeshAgent;

    [SerializeField]
    [Tooltip("�ڷ���Ʈ ��û �� �̵� ���۱��� ���� �ð� (��)")]
    private float customDelayTime = 0.3f;

    private float m_DelayStart;
    private bool isDelaying = false;

    public override bool QueueTeleportRequest(TeleportRequest teleportRequest)
    {
        Debug.Log("[NavMeshProvider] QueueTeleportRequest ȣ���");

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
            Debug.Log($"[NavMeshProvider] ��û�� ������: {target}");

            if (NavMesh.SamplePosition(target, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                navMeshAgent.SetDestination(hit.position);
                Debug.Log($"[NavMeshTeleportationProvider] ������ ������: {hit.position}");
            }
            else
            {
                Debug.LogWarning("[NavMeshTeleportationProvider] ��ȿ�� NavMesh ��ġ�� ã�� ����");
            }

            TryEndLocomotion();
            validRequest = false;
            isDelaying = false;
        }
    }
}

    //[SerializeField]
    //[Tooltip("�̵��� ����� NavMeshAgent")]
    //private NavMeshAgent navMeshAgent;

    //private void Update()
    //{
    //    if()
    //}
    //protected virtual void Update()
    //{

    //}


    //[SerializeField]
    //[Tooltip("�ڷ���Ʈ ���� �ð� (��)")]
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
    //            Debug.Log($"[NavMeshProvider] NavMeshAgent ������ ������: {navHit.position}");
    //        }
    //        else
    //        {
    //            Debug.LogWarning("[NavMeshProvider] NavMesh���� ��ȿ�� ��ġ�� ã�� ����.");
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
//    [Tooltip("�̵��� ����� NavMeshAgent")]
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
//            // ����: XR Origin�� �̵���Ŵ �� ���� ����
//            Vector3 navTarget = currentRequest.destinationPosition;

//            if (NavMesh.SamplePosition(navTarget, out NavMeshHit navHit, 1.0f, NavMesh.AllAreas))
//            {
//                navMeshAgent.SetDestination(navHit.position);
//                Debug.Log($"NavMeshAgent ������ ������: {navHit.position}");
//            }
//            else
//            {
//                Debug.LogWarning("NavMesh���� ��ȿ�� ��ġ�� ã�� ����.");
//            }

//            TryEndLocomotion();
//            validRequest = false;
//        }
//    }
//}
