using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class WorkerTableUI : MonoBehaviour
{
    //[System.Serializable]
    //public class Worker
    //{
    //    public bool checkbox;
    //    public string name;
    //    public string team;
    //    public string position;
    //    public string worktime; //datatime�� string������
    //    public bool on_offline;
    //}

    [SerializeField]
    WorkersManager workersManager;
    public RectTransform contentParent; // Content ������Ʈ ����
    public GameObject rowPrefab;        // ������: �� ��(Row)

    private List<WorkerInfo> workers = new List<WorkerInfo>();

    void Start()
    {
        // ���ÿ� �۾��� ���
        workers.Add(new WorkerInfo { checkbox = true, name = "��ö��", team = "ȫ����", rank = "����", position = "(100, 200)", worktime = "5:30", on_offline = true });
        //x: 100, y: 200
        //5�ð� 30��
        workers.Add(new WorkerInfo { checkbox = false, name = "�ȿ���", team = "������", rank = "����", position = "(0, 0)", worktime = "0:00", on_offline = false });
        workers.Add(new WorkerInfo { checkbox = false, name = "�ڼ���", team = "������", rank = "����", position = "(10, 50)", worktime = "1:00", on_offline = true });
        PopulateTable();
    }

    public void PopulateTable()
    {
        foreach (Transform child in contentParent)
        {
            Debug.Log("Destroy");
            //Destroy(child.gameObject); // ���� �� ����
        }

        foreach (var worker in workers)
        {
            GameObject row = Instantiate(rowPrefab, contentParent);
            TextMeshProUGUI[] texts = row.GetComponentsInChildren<TextMeshProUGUI>();

            texts[0].text = worker.name;
            texts[1].text = worker.team;
            texts[2].text = worker.position;
            texts[3].text = worker.worktime;
            //texts[4].text = worker.on_offline;
        }
    }

    //public void AddWorker(string name, string team, string position, string worktime, bool on_offline)
    //{
    //    workers.Add(new WorkerInfo { name = name, team = team, position = position, worktime = worktime, on_offline = on_offline });
    //    PopulateTable();
    //}

    // worker ������Ʈ�� ����Ʈ�� �߰�
    public void AddWorker(GameObject worker)
	{
        workers.Add(worker.GetComponent<WorkerController>().workerInfo);
		PopulateTable();
	}
}
