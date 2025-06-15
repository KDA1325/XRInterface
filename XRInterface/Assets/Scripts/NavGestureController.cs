using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class NavGestureController : MonoBehaviour
{
//    [SerializeField]
//    [Tooltip("�̵��� ����� NavMeshAgent")]
//    private NavMeshProvider navMeshProvider;


    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float slowDownSpeed = 1.0f;
    [SerializeField] private float minSpeed = 0.1f;

    private Coroutine slowingCoroutine;
    private float originalSpeed;

    private void Start()
    {
        // ���� �� ���� �ӵ��� ����
        originalSpeed = agent.speed;
    }

    public void SlowDown()
    {
        if (slowingCoroutine != null)
            StopCoroutine(slowingCoroutine);

        // ��� ����
        if (agent.hasPath)
            agent.ResetPath();  // ������ ����


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

    // �ӵ� ȸ��
    public void RestoreSpeed()
    {
        agent.speed = originalSpeed;
    }


    ///// <summary>
    ///// ���ο� �������� �̵� �� ȣ���ؼ� �ӵ� ȸ��
    ///// </summary>
    //public void MoveTo(Vector3 destination)
    //{
    //    //// ���� ��û �ʱ�ȭ
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
