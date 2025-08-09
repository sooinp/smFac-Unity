using System.Data;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonHandler : MonoBehaviour
{
    //UIPageCanvasControllers���� ��������(Show~() �Լ���)
    public UIPageCanvasController pageController;

    //ĵ����
    public GameObject mainCanvas;
    public GameObject listCanvas;
    public GameObject dashboardCanvas;
    public GameObject buttonCanvas;

    // �г� �� ��ư ���
    public GameObject Panel;
    public GameObject addList;
    public GameObject searchInput;
    public GameObject alertlog;

    public Button alertButton;
    public Button listButton;
    public Button gotoFirst;
    public Button dashboard;
    public Button exit;

    public Button add;
    public Button edit;
    public Button delete;

    public TMP_InputField searchField;
    public List<GameObject> workerItems;

    // �˾� ����
    public GameObject popupWindow;
    public TMP_Text popupText;
    public Button popupConfirmButton;

    void Start()
    {
        // ��ư �̺�Ʈ ���� (����)
        add.onClick.AddListener(() => OnAddClicked());
        edit.onClick.AddListener(() => OnEditClicked());
        delete.onClick.AddListener(() => OnDeleteClicked());

        alertButton.onClick.AddListener(() => OnAlertButtonClicked());
        listButton.onClick.AddListener(() => OnListButtonClicked());
        gotoFirst.onClick.AddListener(() => OnGotoFirstClicked());
        dashboard.onClick.AddListener(() => OnDashboardClicked());
        exit.onClick.AddListener(() => OnExitClicked());
        //popupConfirmButton.onClick.AddListener(() => popupWindow.SetActive(false));
    }

    //���� ȭ��
    public void OnGotoFirstClicked()
    {
        Debug.Log("go to main");
        mainCanvas.SetActive(true);
        listCanvas.SetActive(false);
        dashboardCanvas.SetActive(false);
        buttonCanvas.SetActive(false);
        Panel?.SetActive(false);
        addList?.SetActive(false);
        searchInput?.SetActive(false);
        alertlog?.SetActive(false);
    }

    public void OnListButtonClicked()
    {
        Debug.Log("WorkerList click");
        //ĵ���� �̵�: mainCanvas -> listCanvas
        pageController.ShowWorkerlist();
        /*mainCanvas.SetActive(false);
        listCanvas.SetActive(true);*/
    }

    public void OnDashboardClicked()
    {
        Debug.Log("Dashboard click");
        //ĵ���� �̵�: mainCanvas -> dashboardCanvas
        pageController.Showdashboard();
        /*mainCanvas.SetActive(false);
        dashboardCanvas.SetActive(true);*/
    }

    public void OnExitClicked()
    {
        Debug.Log("Exit click");
        ShowPopup("�����Ͻðڽ��ϱ�?", () =>
        {
            gameObject.SetActive(false);
            // �α��� ȭ������ ��ȯ (����)
            // SceneManager.LoadScene("LoginScene"); �� ���� ��� ��� ����
        });
    }

    public int count = 0;

    public void OnAlertButtonClicked()
    {
        count++;
        Debug.Log("�˸� ���� Ŭ����");
        alertlog?.SetActive(true);
    }

    public void alertlogOpen()
    {
        Debug.Log("�˶� �α� ��µ�");
        if (count == 1 && alertlog == true)
        {
            //�˸� ���� �� �� �� Ŭ���ϸ� �α� ����
            alertlog.SetActive(false);
            Debug.Log("count�� �ʱ�ȭ�մϴ�.");
            count = 0;
        }
        //�˸� ���� �α� ��µ�
        //���� �ֽ� �˶� �ֻ�ܿ� ���� �� �ȵ�
    }

    //�۾��� ����Ʈ
    public void editing()
    {
        Debug.Log("editing click");
        Panel?.SetActive(true);
    }

    private void OnAddClicked()
    {
        Debug.Log("Add click");

        pageController.ShowAddlist();
        //addList?.SetActive(true);

        listCanvas?.SetActive(false);
    }

    private void OnEditClicked()
    {
        Debug.Log("���� �׸� ����");

        pageController.ShowAddlist();
        //addList?.SetActive(true);

        listCanvas?.SetActive(false);
    }

    private void OnDeleteClicked()
    {
        Debug.Log("�����Ͻðڽ��ϱ�?");
        ShowPopup("�����Ͻðڽ��ϱ�?", () =>
        {
            // ���� ����
            foreach (var item in workerItems)
            {
                Debug.Log("DeleteClick");
                //Destroy(item);
            }

            ShowPopup("�����Ǿ����ϴ�.");
            listCanvas?.SetActive(true);
        });
    }

    public void search( )
    {
        Debug.Log("search click");
        searchInput?.SetActive(true);
        string keyword = searchField?.text;

        foreach (var item in workerItems)
        {
            bool contains = item.name.Contains(keyword);
            item.SetActive(contains);
        }
    }

    public void filter()
    {
        //�� ���� ���� ����(�̸�, �Ҽ���, ������ġ, �ٹ��ð�, �����Ȳ, ���赵 ��) ��
        //���ϴ� �ڷḸ�� (���) �����Ͽ�
        //���õ� �ڷḸ ����Ʈ�� ���
        // ���� ������ ����� ���� UI (���, üũ�ڽ� ��)�� ���� ���� ���� ����
        Debug.Log("���� ��� ���� - ���ǿ� �´� �׸� ���");
    }

    //��ú���
    public void csvsave()
    {
        Debug.Log("CSV ����");
        ShowPopup("CSV ���� ��θ� �����ϼ���", () =>
        {
            string path = "C:\\Users\\sooin\\Desktop\\peaceeyeinstance\\dashboard.csv"; // �׽�Ʈ�� ��� (�����δ� ���� ������ ������ �޴� �� �̻���)

            List<string> lines = new List<string>
            {
                "�̸�,�Ҽ���,��ġ,�ٹ��ð�,�����Ȳ,���赵"
                // ���� ������ �߰� ����
            };

            File.WriteAllLines(path, lines);
            ShowPopup("����Ǿ����ϴ�.");
        });
    }

    // �˾� ��ƿ��Ƽ
    private void ShowPopup(string message, System.Action onConfirm = null)
    {
        Debug.Log("showPopup");
        popupWindow?.SetActive(true);
        popupText.text = message;

        popupConfirmButton.onClick.RemoveAllListeners();
        popupConfirmButton.onClick.AddListener(() =>
        {
            popupWindow.SetActive(false);
            onConfirm?.Invoke();
        });
    }
}
