using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class NavAgentHUDUpdater : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI batteryText;

    [Header("배터리 설정")]
    [SerializeField] private float maxBattery = 100f;
    [SerializeField] private float batteryDrainPerSecond = 1f;

    private float currentBattery;

    private void Start()
    {
        currentBattery = maxBattery;
    }

    private void Update()
    {
        if (agent != null && speedText != null)
        {
            speedText.text = $"{agent.velocity.magnitude:F2} m/s";
        }

        if (batteryText != null)
        {
            currentBattery -= batteryDrainPerSecond * Time.deltaTime;
            currentBattery = Mathf.Max(0, currentBattery);
            batteryText.text = $"Battery: {currentBattery:F0}%";
        }
    }
}
