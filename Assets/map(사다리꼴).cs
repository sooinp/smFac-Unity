using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TrapezoidFloor : MonoBehaviour
{
    void Start()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        // ��ٸ��� �ٴ��� ������ 4�� (2D ��ٸ��� ����, Y�� 0���� ���)
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(-1, 0, -1), // ����
            new Vector3(1, 0, -1),  // ����
            new Vector3(-0.5f, 0, 1), // �»�
            new Vector3(0.5f, 0, 1)   // ���
        };

        int[] triangles = new int[]
        {
            0, 2, 1,  // ù �ﰢ��
            1, 2, 3   // �� ��° �ﰢ��
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
