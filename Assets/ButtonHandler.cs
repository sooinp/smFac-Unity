using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    //UIPageCanvasControllers���� ��������(Show~() �Լ���)
    public UIPageCanvasController pageController;

    //ĵ����
    public GameObject mainCanvas;
    public GameObject listCanvas;
    public GameObject dashboardCanvas;

    // �г� �� ��ư ���
    public GameObject editPanel;
    public GameObject addList;
    public GameObject searchInput;
    public GameObject alertlog;
    public GameObject LogoutPanel;

    public Button alertButton;
    public Button listButton;
    public Button gotoFirst;
    public Button dashboard;
    public Button exit;
    public Button accept;
    public Button cancel;

    public Button add;
    public Button edit;
    public Button delete;

    public GameObject saveFile;
    public Button acceptSave;
    public Button cancelSave;

    public TMP_InputField searchField;
    public List<GameObject> workerItems;

    // �˾� ����
    public GameObject popupWindow;
    public TMP_Text popupText;
    public Button popupConfirmButton;

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

        alertButton.onClick.AddListener(() => OnAlertButtonClicked());
        listButton.onClick.AddListener(() => OnListButtonClicked());
        //gotoFirst.onClick.AddListener(() => OnGotoFirstClicked());
        //dashboard.onClick.AddListener(() => OnDashboardClicked());
        //exit.onClick.AddListener(() => OnExitClicked());

        //popupConfirmButton.onClick.AddListener(() => popupWindow.SetActive(false));
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
        alertlog?.SetActive(false);
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
