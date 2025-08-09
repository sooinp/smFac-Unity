using UnityEngine;
using UnityEngine.InputSystem;

public class WorkerRaycaster : MonoBehaviour
{
    public Camera mainCamera;             // ���� ī�޶� ����
    public WorkerController controller;   // WorkerController ����
    void Start()
    {
        // �ڵ����� mainCamera ���� (null�� ��츸)
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    void Update() 
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                //Debug.Log(hit.collider.name);
                controller.ShowWorkerInfo(hit.collider.name);
                //controller.ShowWorkerInfo()
                //var handler = hit.collider.GetComponent<WorkerClickHandler>();
                //if (handler != null)
                //{
                //    Debug.Log("Worker Ŭ���� (Raycast): " + handler.workerID);
                //    controller.ShowWorkerInfo(handler.workerID);
                //}
            }
        }
    }
}