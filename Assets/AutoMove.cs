using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class AutoMoveWorker : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float rotationSpeed = 5f;
    public float arriveThreshold = 0.1f;

    private Rigidbody rb;
    private Vector3? targetPosition = null;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate()
    {
        if (targetPosition == null)
            return;

        Vector3 target = targetPosition.Value;
        Vector3 direction = (target - transform.position);
        Vector3 directionFlat = new Vector3(direction.x, 0, direction.z);

        if (directionFlat.magnitude < arriveThreshold)
            return;

        // ȸ��
        Quaternion targetRotation = Quaternion.LookRotation(directionFlat);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

        // �̵�
        Vector3 forward = transform.forward;
        rb.MovePosition(rb.position + forward * moveSpeed * Time.fixedDeltaTime);
    }

    public void SetTarget(Vector3 newTarget)
    {
        targetPosition = newTarget;
    }
}
