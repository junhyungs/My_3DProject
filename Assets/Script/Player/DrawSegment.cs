using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class DrawSegment : MonoBehaviour
{
    //public Material trailMaterial;
    private GameObject m_player;
    public float segmentWidth = 1f;
    public float segmentHeight = 1f;
    public float segmentDistance = 1f;
    private bool isDraw;
    public int maxSegment = 50;
    public float m_maxDraw = 10f;

    private List<GameObject> segments = new List<GameObject>();
    private Vector3 lastPosition;
    
    void Start()
    {
        lastPosition = transform.position;
    }

    private void OnEnable()
    {
        m_player = GameManager.Instance.Player;
        isDraw = true;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(lastPosition, transform.position);

        Draw();

        if (distance >= segmentDistance)
        {
            AddSegment(transform.position, transform.rotation);
            lastPosition = transform.position;
        }
    }

    private void Draw()
    {
        if (Vector3.Distance(transform.position, m_player.transform.position) >= m_maxDraw)
            isDraw = false;
    }

    void AddSegment(Vector3 position, Quaternion rotation)
    {
        if (isDraw)
        {
            GameObject segment = PoolManager.Instance.GetPlayerSegment(position, rotation);
            segments.Add(segment);
        }
        

        if (segments.Count > maxSegment)
        {
        
            PoolManager.Instance.ReturnPlayerSegment(segments[0]);
            segments.RemoveAt(0);
        }

        //GameObject newSegment = new GameObject("TrailSegment");
        //newSegment.transform.position = position;

        //Mesh mesh = new Mesh();

        //Vector3[] vertices = new Vector3[4]
        //{
        //    new Vector3(-segmentWidth / 2, -segmentHeight / 2, 0),
        //    new Vector3(segmentWidth / 2, -segmentHeight / 2, 0),
        //    new Vector3(-segmentWidth / 2, segmentHeight / 2, 0),
        //    new Vector3(segmentWidth / 2, segmentHeight / 2, 0)
        //};

        //int[] triangles = new int[6] { 0, 2, 1, 2, 3, 1 };

        //Vector2[] uv = new Vector2[4]
        // {
        //    new Vector2(0, 0),
        //    new Vector2(1, 0),
        //    new Vector2(0, 1),
        //    new Vector2(1, 1)
        // };

        //mesh.vertices = vertices;
        //mesh.triangles = triangles;
        //mesh.uv = uv;  
        //mesh.RecalculateNormals();

        //MeshFilter meshFilter = newSegment.AddComponent<MeshFilter>();
        //meshFilter.mesh = mesh;

        //MeshRenderer meshRenderer = newSegment.AddComponent<MeshRenderer>();
        //meshRenderer.material = trailMaterial;

        //newSegment.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        //newSegment.transform.rotation = Quaternion.Euler(90f, 0f, -90f);

        //segments.Add(newSegment);

        //if(segments.Count > maxSegment)
        //{
        //    Destroy(segments[0]);
        //    segments.RemoveAt(0);
        //}

    }
}
