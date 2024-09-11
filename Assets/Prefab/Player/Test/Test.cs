using UnityEngine;

public class Test : MonoBehaviour
{
    public MeshRenderer meshRenderer; // �Ž� ������ ����
    public int materialIndex = 0; // ����� ���׸����� �ε���
    public float speed = 0.5f; // ������ ��ȭ �ӵ�

    [Header("Y")]
    public bool YPlus; // Y �� �̵� ���� �÷��� (����)
    public bool y; // Y �� �̵� Ȱ��ȭ �÷���
    public bool yLimit; // Y �� ���� �÷���
    public float startYOffset = 0.0f; // ���� Y ������
    public float endYOffset = -0.0f; // �����ؾ� �� Y ������

    [Header("X")]
    public bool XPlus; // X �� �̵� ���� �÷��� (������)
    public bool x; // X �� �̵� Ȱ��ȭ �÷���
    public bool xLimit; // X �� ���� �÷���
    public float startXOffset = 0.0f; // ���� X ������
    public float endXOffset = -0.0f; // �����ؾ� �� X ������

    private Vector2 currentOffset;
    private Material originalMaterial;

    private void Start()
    {
        // MeshRenderer�� ������Ʈ���� ��������
        meshRenderer = GetComponent<MeshRenderer>();

        // ���׸����� �ε����� ��ȿ���� Ȯ��
        if (meshRenderer.sharedMaterials.Length > materialIndex)
        {
            originalMaterial = meshRenderer.sharedMaterials[materialIndex];
            originalMaterial.mainTextureOffset = new Vector2(0f, 0);
        }
    }

    void Update()
    {
        // �Ž� �������� ���׸����� ����� �����Ǿ����� Ȯ��
        if (meshRenderer == null || originalMaterial == null)
        {
            return;
        }

        // ���� ������
        currentOffset = originalMaterial.mainTextureOffset;

        // Y �� �̵� ������Ʈ
        if (y)
        {
            currentOffset.y = UpdateOffset(currentOffset.y, startYOffset, endYOffset, YPlus, yLimit);
        }

        // X �� �̵� ������Ʈ
        if (x)
        {
            currentOffset.x = UpdateOffset(currentOffset.x, startXOffset, endXOffset, XPlus, xLimit);
        }

        // ����� �������� ���׸��� ����
        originalMaterial.mainTextureOffset = currentOffset;
    }

    private void OnDisable()
    {
        // ��ũ��Ʈ�� ��Ȱ��ȭ�� �� ������ �ʱ�ȭ
        if (originalMaterial != null)
        {
            originalMaterial.mainTextureOffset = new Vector2(0f, 0f);
        }
    }

    // �������� ������Ʈ�ϴ� �޼���
    private float UpdateOffset(float current, float start, float end, bool isPositive, bool isLimited)
    {
        // �̵� ���⿡ ���� ������ ����
        current += (isPositive ? 1 : -1) * speed * Time.deltaTime;

        // ������ �ִ� ���, �������� �Ѱ踦 �Ѿ�� �ٽ� ���� ��ġ�� ����
        if (isLimited)
        {
            if ((isPositive && current >= end) || (!isPositive && current <= end))
            {
                return start;
            }
        }

        return current;
    }
}