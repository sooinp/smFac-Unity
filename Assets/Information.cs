using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;

public class WorkerInfoUI : MonoBehaviour
{
    [Header("UI Components")]
    public TextMeshProUGUI workerID;
    public TextMeshProUGUI workerName;
    public TextMeshProUGUI workerTeam;
    public TextMeshProUGUI workerRank;
    public TextMeshProUGUI workTimeText;
    public TextMeshProUGUI positionText;

   [Header("Cameras")]
    public Camera workerCamera;
    public Camera overviewCamera;

    private string currentWorkerID;     // �߰� ���� ��û��

    void Start()
    {
        gameObject.SetActive(false);
    }

    // JSON���� �г� ���� ���� ����
    public void SetWorkerInfoFromJson(string aiJson, string dbJson)
    {
        Debug.Log("SetWorkerInfoFromJson");
        // �г� ǥ��
        gameObject.SetActive(true);

        if (string.IsNullOrEmpty(aiJson) || string.IsNullOrEmpty(dbJson))
        {
            Debug.LogWarning("JSON �Է��� ��� �ֽ��ϴ�.");
            return;
        }

        JObject aiData = JObject.Parse(aiJson);
        JObject dbData = JObject.Parse(dbJson);

        string aiWorkerID = aiData["workerID"]?.ToString();
        string dbWorkerID = dbData["workerID"]?.ToString();

        if (aiWorkerID != dbWorkerID)
        {
            Debug.LogWarning("AI �����Ϳ� DB �������� workerID�� ��ġ���� �ʽ��ϴ�.");
            return;
        }

        currentWorkerID = aiWorkerID;

        // UI ������Ʈ
        workerID.text = $"{aiWorkerID}";
        workerName.text = $"{dbData["�̸�"]}";
        workerTeam.text = $"{dbData["�μ�"]}";
        workerRank.text = $"{dbData["����"]}";
        workTimeText.text = $"{dbData["�ٹ��ð�"]}";

        float x = aiData["position_x"]?.ToObject<float>() ?? 0f;
        float y = aiData["position_y"]?.ToObject<float>() ?? 0f;
        positionText.text = $"({x:F1}, {y:F1})";

        // ���� ī�޶� �ѱ�, �⺻ ī�޶� ����
        if (overviewCamera != null) overviewCamera.gameObject.SetActive(false);
        if (workerCamera != null) workerCamera.gameObject.SetActive(true);
    }

   // X ��ư ������ �ݱ� ��û
    public void HidePanel()
    {
        gameObject.SetActive(false);

        // ���� ī�޶� ����
        if (workerCamera != null) overviewCamera.gameObject.SetActive(true);
        if (overviewCamera != null) workerCamera.gameObject.SetActive(false);
    }

    // �߰� ���� ��û (��ư ����)
    public void RequestMoreInfo()
    {
        Debug.Log("�߰� ���� ��û: " + currentWorkerID);

        // JS �Ǵ� ���� ��û ȣ�� ����
#if UNITY_WEBGL && !UNITY_EDITOR
        Application.ExternalCall("RequestWorkerDetail", currentWorkerID);
#else
        // �Ǵ� ����Ƽ ���ο��� ���� ȣ���ϴ� ���
        // StartCoroutine(RequestInfoFromServer(currentWorkerID));
#endif
    }
}