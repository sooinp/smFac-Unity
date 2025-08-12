using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ButtonHandler : MonoBehaviour
{
    //UIPageCanvasControllers���� ��������(Show~() �Լ���)
    public UIPageCanvasController pageController;

    [Header("ĵ����")]
    public GameObject mainCanvas;
    public GameObject listCanvas;
    public GameObject dashboardCanvas;

    [Header("�г� ���")]
    //����ȭ��
    public GameObject alertlog;
    public GameObject LogoutPanel;

    //�۾��� ����Ʈ
    public GameObject editPanel;
    public GameObject addList;
    public GameObject searchInput;
    public GameObject filterPanel;

    public TMP_InputField searchField;
    public List<GameObject> workerItems;

    [Header("��ư ���")]
    //����ȭ��
    public Button alertButton;
    public Button listButton;
    public Button gotoFirst;
    public Button dashboard;
    public Button exit;
    public Button accept;  //�α׾ƿ� Ȯ��
    public Button cancel;  //�α׾ƿ� ���

    //�۾��� ����Ʈ
    public Button add;
    public Button edit;
    public Button delete;
    public Button filter;
    public Button upperlower;  //��������, �ٽ� ������ ��������
    
    //��ú���
    public GameObject saveFile;
    public Button acceptSave;
    public Button cancelSave;

    // �˾� ����
    public GameObject popupWindow;
    public TMP_Text popupText;
    public Button popupConfirmButton;

    // --- ���� ��ɿ� �ʿ��� ������ (���� �߰�) ---
    private string currentSortColumn;   // ���� ���� ������ �Ǵ� �� �̸�
    private bool isSortAscending = true; // ���� ���� ���� (true: ��������, false: ��������)

    // --- �ڹٽ�ũ��Ʈ �÷����� �Լ� ����Ʈ --- WebGL ���忡���� �� �Լ��� �ν��մϴ�.
    [DllImport("__Internal")]
    private static extern void DownloadFile(string fileName, string fileContent);

    void Start()
    {
        // ��ư �̺�Ʈ ���� (����)
        add.onClick.AddListener(() => OnAddClicked());
        edit.onClick.AddListener(() => OnEditClicked());
        delete.onClick.AddListener(() => OnDeleteClicked());
        acceptSave.onClick.AddListener(() => OnAcceptSaveClicked());
        cancelSave.onClick.AddListener(() => OnCancelSaveClicked());

        //alertButton.onClick.AddListener(() => OnAlertButtonClicked());
        listButton.onClick.AddListener(() => OnListButtonClicked());
        //gotoFirst.onClick.AddListener(() => OnGotoFirstClicked());
        //dashboard.onClick.AddListener(() => OnDashboardClicked());
        //exit.onClick.AddListener(() => OnExitClicked());

        //popupConfirmButton.onClick.AddListener(() => popupWindow.SetActive(false));
        
        // --- ���� ��ư �̺�Ʈ ������ ���� (���� �߰�) ---
        upperlower.onClick.AddListener(OnUpperLowerClicked);

        // --- ���� ���� �ʱ�ȭ (���� �߰�) ---
        if (columnNames.Count > 0)
        {
            currentSortColumn = columnNames[0]; // �⺻ ���� ������ '�̸�'���� ����
        }
    }

    //���� ȭ��
    public void OnGotoFirstClicked()
    {
        Debug.Log("go to main");
        mainCanvas.SetActive(true);
        listCanvas.SetActive(false);
        dashboardCanvas.SetActive(false);
        saveFile.SetActive(false);
        editPanel?.SetActive(false);
        addList?.SetActive(false);
        searchInput?.SetActive(false);
        //alertlog?.SetActive(false);
        LogoutPanel.SetActive(false);
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
        LogoutPanel.SetActive(true);
    }

    public void CancelClick()
    {
        // �α׾ƿ� ���: �г� �ݱ�
        LogoutPanel.SetActive(false);
        Debug.Log("�α׾ƿ� �����");
    }

    public void AcceptClick()
    {
        // �α׾ƿ� Ȯ��: �α׾ƿ� ���� ����
        OnLogoutConfirmed();
        LogoutPanel.SetActive(false);
        Debug.Log("�α׾ƿ� ������");
    }

    public void OnLogoutConfirmed()
    {
#if UNITY_WEBGL == true && !UNITY_EDITOR
        // WebGL ���� ȯ�濡���� JavaScript �Լ��� ȣ���մϴ�.
        //Application.ExternalCall("handleUnityLogout");
#else
        // ������ ȯ�濡���� �α׸� ����Ͽ� �׽�Ʈ�մϴ�.
        Debug.Log("������ ���: �α׾ƿ� ��ȣ�� ���½��ϴ�.");
#endif
    }

    public void OnAlertButtonClicked()
    {
        Debug.Log("�˸� ��ư Ŭ��");
        // alertlog�� ���� Ȱ��ȭ�Ǿ� �ִ���(.activeSelf) Ȯ���մϴ�.
        bool isCurrentlyActive = alertlog.activeSelf;
        // ���� ������ -> !true -> false (����)
        // ���� ������ -> !false -> true (����)
        alertlog.SetActive(!isCurrentlyActive);
    }

    //�۾��� ����Ʈ
    public void editing()
    {
        Debug.Log("editing click");
        editPanel?.SetActive(true);
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
        // 1. �˻�â(searchInput)�� ���� Ȱ��ȭ ���¸� Ȯ���մϴ�.
        bool isSearchActive = searchInput.activeSelf;
        // 2. ���� ������ �ݴ� ������ �����Ͽ� ����� �����մϴ�.
        searchInput.SetActive(!isSearchActive);
        // 3. ���� �˻�â�� �� ���̶��(isSearchActive�� true���ٸ�),
        //    �˻�� �ʱ�ȭ�ϰ� ����Ʈ�� ������� �ǵ����ϴ�.
        if (isSearchActive)
        {
            Debug.Log("�˻�â�� �ݰ� ����Ʈ�� �ʱ�ȭ�մϴ�.");
            searchField.text = ""; // �˻� �ʵ� �ʱ�ȭ
            // ��� �������� �ٽ� �����ֱ�
            foreach (var item in workerItems)
            {
                item.SetActive(true);
            }
        }
    }

    // �� �Լ��� Input Field�� OnValueChanged �̺�Ʈ�� �����ϸ� �ǽð� �˻��� ���������ϴ�.
    public void UpdateSearchByKeyword()
    {
        string keyword = searchField.text.ToLower();
        // ��ҹ��� ���� ���� �˻��Ϸ��� ToLower()�� ����մϴ�.
       foreach (var item in workerItems)
        {
            // workerItems�� �� �׸񿡼� '�̸�' �ڽ� ������Ʈ�� TMP_Text ���� ������ ��
            string itemName = GetColumnValue(item, "�̸�").ToLower();
            bool contains = itemName.Contains(keyword);
            item.SetActive(contains);
        }
    }
    
    // ���� ������ �� �̸��� (UI Text ������Ʈ �̸��� ����)
    public List<string> columnNames = new List<string>
    {
        "�̸�", "�Ҽ�", "������ġ", "�ٹ��ð�", "�����Ȳ", "���赵"
    };

    public void Filter()
    {
        //�� ���� ���� ����(�̸�, �Ҽ���, ������ġ, �ٹ��ð�, �����Ȳ, ���赵 ��) ��
        //���ϴ� �ڷḸ�� (���) �����Ͽ�
        //���õ� �ڷḸ ����Ʈ�� ���
        // ���� ������ ����� ���� UI (���, üũ�ڽ� ��)�� ���� ���� ���� ����
        Debug.Log("���� ��� ���� - ���ǿ� �´� �׸� ���");
        filterPanel.SetActive(!filterPanel.activeSelf);

    }

    // --- ���� ������ �����ϴ� �Լ� (���� �߰�) ---
    private void SortByColumn(string columnName, bool ascending)
    {
        // 1. workerItems ����Ʈ�� �����մϴ�.
        // ���� ����(columnName)�� �ش��ϴ� �ڽ� ������Ʈ�� TMP_Text ���� �����ͼ� ���մϴ�.
        workerItems = ascending
            ? workerItems.OrderBy(w => GetColumnValue(w, columnName)).ToList()
            : workerItems.OrderByDescending(w => GetColumnValue(w, columnName)).ToList();

        // 2. ���ĵ� ������� UI ���� ������ ���ġ�մϴ�.
        // SetSiblingIndex�� �θ� Transform ������ ������ �����Ͽ� ȭ�鿡 ���̴� ������ �ٲߴϴ�.
        for (int i = 0; i < workerItems.Count; i++)
        {
            workerItems[i].transform.SetSiblingIndex(i);
        }
    }

    // --- ���� ���� ���� �������� ���� �Լ� (���� �߰�) ---
    private string GetColumnValue(GameObject workerItem, string columnName)
    {
        // workerItem(�� ��)�� �ڽ� �� columnName�� �̸��� ���� ������Ʈ�� ã���ϴ�.
        Transform columnTransform = workerItem.transform.Find(columnName);
        if (columnTransform != null)
        {
            // ã�� ������Ʈ���� TMP_Text ������Ʈ�� �ؽ�Ʈ�� ��ȯ�մϴ�.
            TMP_Text textComp = columnTransform.GetComponent<TMP_Text>();
            if (textComp != null)
            {
                return textComp.text;
            }
        }
        return ""; // ���� ã�� ���ϸ� �� ���ڿ��� ��ȯ�մϴ�.
    }

    // --- ��������/�������� ��ư Ŭ�� �� ȣ��� �Լ� (���� �߰�) ---
    private void OnUpperLowerClicked()
    {
        // ���� ���� �Լ��� ȣ���մϴ�.
        SortByColumn(currentSortColumn, isSortAscending);

        Debug.Log($"{currentSortColumn} �������� {(isSortAscending ? "��������" : "��������")} ���� �Ϸ�");

        // ���� Ŭ���� ���� ���� ������ ������ŵ�ϴ�.
        isSortAscending = !isSortAscending;
    }

    //��ú��� --- csvsave() �Լ� ���� ---
    public void csvsave()
    {
        Debug.Log("CSV ���� �õ�");
        saveFile?.SetActive(true);
        if (saveFile)
        {
            OnAcceptSaveClicked();
        } else
        {
            OnCancelSaveClicked();
        }
    }
        
    public void OnAcceptSaveClicked() {
        Debug.Log("svcSaved");
        // 1. CSV�� ���� ������ ����
        // ���(ù ��)�� ������ ���ø� ����ϴ�. ���� �����ͷ� ��ü�ؾ� �մϴ�.
        List<string> lines = new List<string>
        {
            "�̸�,�Ҽ���,��ġ,�ٹ��ð�,�����Ȳ,���赵", // CSV ���
            "ȫ�浿,A��,�۾���1,8�ð�,���,����",      // ���� ������ 1
            "��ö��,B��,�۾���2,5�ð�,���,����"       // ���� ������ 2
        };
        string csvContent = string.Join("\n", lines); // �� ���� �ٹٲ� ���ڷ� ����

        // 2. �÷����� ���� �ٸ� ���� ��� ����
#if UNITY_WEBGL && !UNITY_EDITOR
        // WebGL ���� ȯ���� ���:
        // ������ ���� �ڹٽ�ũ��Ʈ �÷������� DownloadFile �Լ��� ȣ���մϴ�.
        // ù ��° ���ڴ� ���� �̸�, �� ��° ���ڴ� ���� �����Դϴ�.
        DownloadFile("dashboard.csv", csvContent);
        ShowPopup("���������� �ٿ�ε尡 ���۵˴ϴ�.");
#else
        // Unity ������ �Ǵ� PC ���� ȯ���� ���:
        // ����ó�� ���� ���� ��ο� �����մϴ�. (�׽�Ʈ��)
        // ���� ���� �ÿ��� ���� Ž���⸦ ���� �ڵ�� �ٲٴ� ���� �����ϴ�.
        string path = "C:\\Users\\sooin\\Desktop\\peaceeyeinstance\\dashboard.csv";
        try
        {
            File.WriteAllText(path, csvContent);
            ShowPopup($"����Ǿ����ϴ�.\n���: {path}");
        }
        catch (System.Exception e)
        {
            ShowPopup($"���忡 �����߽��ϴ�: {e.Message}");
            Debug.LogError($"���� ���� ����: {e.Message}");
        }
#endif
    }

    public void OnCancelSaveClicked()
    {
        saveFile?.SetActive(false);
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
