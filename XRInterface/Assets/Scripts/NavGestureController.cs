using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class NavGestureController : MonoBehaviour
{
//    [SerializeField]
//    [Tooltip("이동에 사용할 NavMeshAgent")]
//    private NavMeshProvider navMeshProvider;


    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float slowDownSpeed = 1.0f;
    [SerializeField] private float minSpeed = 0.1f;

    private Coroutine slowingCoroutine;
    private float originalSpeed;

    private void Start()
    {
        // 시작 시 현재 속도를 저장
        originalSpeed = agent.speed;
    }

    public void SlowDown()
    {
        if (slowingCoroutine != null)
            StopCoroutine(slowingCoroutine);

        // 경로 멈춤
        if (agent.hasPath)
            agent.ResetPath();  // 목적지 해제


        slowingCoroutine = StartCoroutine(SlowDownAgent());
    }

    public void StopAgent()
    {
        if (slowingCoroutine != null)
            StopCoroutine(slowingCoroutine);

        agent.isStopped = true;
    }


    private IEnumerator SlowDownAgent()
    {
        agent.isStopped = false;

        while (agent.speed > minSpeed)
        {
            agent.speed -= slowDownSpeed * Time.deltaTime;
            yield return null;
        }

        agent.speed = minSpeed;
    }

    // 속도 회복
    public void RestoreSpeed()
    {
        agent.speed = originalSpeed;
    }


    ///// <summary>
    ///// 새로운 목적지로 이동 시 호출해서 속도 회복
    ///// </summary>
    //public void MoveTo(Vector3 destination)
    //{
    //    //// 기존 요청 초기화
    //    //navMeshProvider.validRequest = false;
    //    //navMeshProvider.isDelaying = false;

    //    TeleportRequest request = new TeleportRequest
    //    {
    //        destinationPosition = destination,
    //        matchOrientation = MatchOrientation.None,
    //    };

    //    navMeshProvider.QueueTeleportRequest(request);
    //}

}
