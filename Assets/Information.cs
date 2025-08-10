using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using static WorkerTableUI;

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
	//public Camera workerCamera;
	public Camera overviewCamera;

	private string currentWorkerID;     // �߰� ���� ��û��
	private GameObject currentWorker;   // ���� ���õ� worker

	[SerializeField]
	UIPageCanvasController canvasController;


	void Start()
	{
		gameObject.SetActive(false);
	}

	void Update()
	{
		// �ǽð����� worker�� ��ġ�� ����.
		UpdateWorkerPosition();
	}


	// worker�� �Է¹ް�, worker�� ������ ȭ�鿡 ǥ��
	public void SetWorkerInfo(GameObject worker)
	{
		// ����â Ȱ��ȭ
		//gameObject.SetActive(true);
		canvasController.ShowWorkerInfo();

		// worker�� �ִ��� Ȯ��
		if (!worker)
		{
			Debug.LogWarning("�ش� worker�� �����ϴ�.");
			return;
		}

		// ������ worker�� �Էµ� worker ����.
		currentWorker = worker;

		// �Էµ� worker�� ������ UI�� ä��.
		WorkerController workerController = worker.GetComponent<WorkerController>();
		//Debug.Log(workerController);
		workerID.text = workerController.workerInfo.id;
		workerName.text = workerController.workerInfo.name;
		workerTeam.text = workerController.workerInfo.team;
		workerRank.text = workerController.workerInfo.rank;
		workTimeText.text = workerController.workerInfo.worktime;

		// ���� worker�� ID ���. �߰� ���� ��û�� ���.
		currentWorkerID = workerController.workerInfo.id;

		// worker�� ��ġ�� �ǽð����� ����Ǵ� ���� �ݿ��ϱ� ���� Update���� ���.
		//// worker�� ��ġ ����.
		//positionText.text = $"({x:F1}, {y:F1})";

		// ���� ī�޶� �ѱ�, �⺻ ī�޶� ����
		if (overviewCamera != null) overviewCamera.gameObject.SetActive(false);
		if (currentWorker != null) currentWorker.GetComponent<WorkerController>().SetActiveTopviewCamera(true);
	}

	
	// worker�� ��ġ�� positionText�� ������Ʈ. �ǽð� �ݿ��� ���� Update �Լ������� ȣ��.
	void UpdateWorkerPosition()
	{
		// ���� ���õ� worker�� ������ ������Ʈ ����.
		if(!currentWorker)
		{
			return;
		}

		positionText.text = currentWorker.GetComponent<WorkerController>().workerInfo.position;
		//float x = currentWorker.transform.position.x;
		//float z = currentWorker.transform.position.z;
		//positionText.text = $"({x:F1}, {z:F1})";
	}

	//// JSON���� �г� ���� ���� ����
	//public void SetWorkerInfoFromJson(string aiJson, string dbJson)
	//{
	//	Debug.Log("SetWorkerInfoFromJson");
	//	// �г� ǥ��
	//	gameObject.SetActive(true);

	//	if (string.IsNullOrEmpty(aiJson) || string.IsNullOrEmpty(dbJson))
	//	{
	//		Debug.LogWarning("JSON �Է��� ��� �ֽ��ϴ�.");
	//		return;
	//	}

	//	JObject aiData = JObject.Parse(aiJson);
	//	JObject dbData = JObject.Parse(dbJson);

	//	string aiWorkerID = aiData["workerID"]?.ToString();
	//	string dbWorkerID = dbData["workerID"]?.ToString();

	//	if (aiWorkerID != dbWorkerID)
	//	{
	//		Debug.LogWarning("AI �����Ϳ� DB �������� workerID�� ��ġ���� �ʽ��ϴ�.");
	//		return;
	//	}

	//	currentWorkerID = aiWorkerID;

	//	// UI ������Ʈ
	//	workerID.text = $"{aiWorkerID}";
	//	workerName.text = $"{dbData["�̸�"]}";
	//	workerTeam.text = $"{dbData["�μ�"]}";
	//	workerRank.text = $"{dbData["����"]}";
	//	workTimeText.text = $"{dbData["�ٹ��ð�"]}";

	//	float x = aiData["position_x"]?.ToObject<float>() ?? 0f;
	//	float y = aiData["position_y"]?.ToObject<float>() ?? 0f;
	//	positionText.text = $"({x:F1}, {y:F1})";

	//	// ���� ī�޶� �ѱ�, �⺻ ī�޶� ����
	//	if (overviewCamera != null) overviewCamera.gameObject.SetActive(false);
	//	if (workerCamera != null) workerCamera.gameObject.SetActive(true);
	//}

	// X ��ư ������ �ݱ� ��û
	public void HidePanel()
	{
		//gameObject.SetActive(false);

		// ���� ī�޶� ����
		if (overviewCamera != null) overviewCamera.gameObject.SetActive(true);
		if (currentWorker != null) currentWorker.GetComponent<WorkerController>().SetActiveTopviewCamera(false);
	}

	// �߰� ���� ��û (��ư ����)
	public void RequestMoreInfo()
	{
		Debug.Log("�߰� ���� ��û: " + currentWorkerID);

		// JS �Ǵ� ���� ��û ȣ�� ����
#if UNITY_WEBGL && !UNITY_EDITOR
        //Application.ExternalCall("RequestWorkerDetail", currentWorkerID);
#else
		// �Ǵ� ����Ƽ ���ο��� ���� ȣ���ϴ� ���
		// StartCoroutine(RequestInfoFromServer(currentWorkerID));
#endif
	}
}