using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashboardUI : MonoBehaviour
{
    [SerializeField] private DashboardClient client;

    [Header("UI References")]
    [SerializeField] private Text tasksCountText;         // UnityEngine.UI.Text
    [SerializeField] private Transform tasksContainer;    // �������� ������ �θ� Transform
    [SerializeField] private GameObject taskItemPrefab;   // Unity UI ������ (Text ����)

    private void OnEnable()
    {
        if (client != null)
            client.OnSummary += RenderSummary;
    }

    private void OnDisable()
    {
        if (client != null)
            client.OnSummary -= RenderSummary;
    }

    private void RenderSummary(SummaryV1 data)
    {
        // �� ���� ǥ��
        if (tasksCountText != null)
        {
            tasksCountText.text = $"Tasks: {data.ongoing_tasks.Length}";
        }

        // ���� ������ ���� ����
        foreach (Transform child in tasksContainer)
        {
            Destroy(child.gameObject);
        }

        // �� ������ ����
        foreach (var t in data.ongoing_tasks)
        {
            var go = Instantiate(taskItemPrefab, tasksContainer);
            var text = go.GetComponentInChildren<Text>();
            if (text != null)
            {
                text.text = $"{t.task_name} ({t.part}) - {t.progress}%";
            }
        }
    }
}
