using Newtonsoft.Json.Linq;
using UnityEngine;

// Worker�� ������ �����ϱ� ���� Ŭ����.
public class WorkerInfo
{
	public bool checkbox;				// ?

	public string id = "";				// �۾��� ID
	public string name = "���� ����";		// �̸�
	public string team = "";			// �μ�
	public string rank = "";			// ����
	public string worktime = "";		// �ٹ��ð�
	public bool on_offline = false;     // �¿���, ���� �Ⱦ�
	public string position = "(0, 0)";

	// JSON ���ڿ��� ���� ����.
	public void SetInfoFromJSONString(string jsonString)
	{
		JObject jsonObject = JObject.Parse(jsonString);

		id = jsonObject.GetValue("workerID")?.Value<string>();
		name = jsonObject.GetValue("�̸�")?.Value<string>();
		team = jsonObject.GetValue("�μ�")?.Value<string>();
		rank = jsonObject.GetValue("����")?.Value<string>();
		worktime = jsonObject.GetValue("�ٹ��ð�")?.Value<string>();
	}

	// ���� ��ġ�� �ؽ�Ʈ �������� ���. WorkerController�� Update �Լ����� ȣ���. (�� ������ ������Ʈ)
	public void SetPosition(float x, float y)
	{
		position = $"({x:F1}, {y:F1})";
	}
}
