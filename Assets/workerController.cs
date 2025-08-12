using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class WorkerController : MonoBehaviour
{
	// worker�� ���� ���. JSON���� �Ľ��ؼ� ���� �� ����.
	public WorkerInfo workerInfo = new WorkerInfo();

	// �ڵ����� �����̱� ���� ������.
	public float moveSpeed = 1f;
	public float rotationSpeed = 5f;
	public float arriveThreshold = 0.1f;

	// ������ �����ٺ��� ī�޶�
	GameObject workerCamera;


	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		// �ӽ� ������. �����ؾ� ��.
		// ���⿡�� JSON ���ڿ��� �����͸� ���� �ۼ��ϰų� ���޹��� ���� ������
		//string aiJsonString = @"{
		//          'cameraID': 'CAM_01',
		//          'workerID': 'W123',
		//          'position_x': 10.5,
		//          'position_y': 8.3
		//      }";

		string dbJsonString = @"{
            'workerID': 'W123',
            '�̸�': 'ȫ�浿',
            '�μ�': '����1��',
            '����': '�븮',
            '�ٹ��ð�': '5�ð� 40��'
        }";


		//// JObject�� �Ľ� �� ���ڿ��� �ٽ� ����
		//JObject aiJson = JObject.Parse(aiJsonString);
		//JObject dbJson = JObject.Parse(dbJsonString);
		//Debug.Log(aiJson);
		//Debug.Log(dbJson);


		// JSON ���ڿ��� worker�� ���� ���� (�ӽ�, �Ŀ� �������� �޾ƿ� JSON ���ڿ��� �ְ� ��ü�ؾ���.)
		workerInfo.SetInfoFromJSONString(dbJsonString);
        //StartCoroutine(GetWorkerDataFromServer());

        // ���� object �߿� workerCamera�� ã��.
        workerCamera = transform.Find("workerCamera")?.gameObject;
	}

	// worker camera�� Ȱ��ȭ / ��Ȱ��ȭ
	public void SetActiveTopviewCamera(bool enable)
	{
		// ī�޶� ������ ���� ����
		if(!workerCamera)
		{
			Debug.LogWarning("���� ������Ʈ �߿� ī�޶� �����ϴ�.");
			return;
		}

		// Ȱ��ȭ / ��Ȱ��ȭ
		workerCamera.SetActive(enable);
	}

    // Update is called once per frame
    void Update()
    {
		// ���� ��ġ�� workerInfo�� �ؽ�Ʈ �������� ��� ������Ʈ
		workerInfo.SetPosition(transform.position.x, transform.position.z);
    }
}
