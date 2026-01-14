using UnityEngine;
using System.Collections.Generic;

public class HandTracking : MonoBehaviour
{
    [Header("UDP Receiver")]
    public UDPReceiver uDPReceiver;

    [Header("Settings")]
    public GameObject pointPrefab;
    public float scale = 5f;
    public Vector3 baseOffset = new Vector3(0, 1.5f, 2f);
    [Range(0f, 1f)]
    public float smoothFactor = 0.5f;

    private GameObject[] leftHandPoints;
    private GameObject[] rightHandPoints;
    private Vector3[] leftPrevPositions;
    private Vector3[] rightPrevPositions;
    private List<LineRenderer> leftLines = new List<LineRenderer>();
    private List<LineRenderer> rightLines = new List<LineRenderer>();

    private int[,] connections = new int[,]
    {
        {0,1},{1,2},{2,3},{3,4},
        {0,5},{5,6},{6,7},{7,8},
        {0,9},{9,10},{10,11},{11,12},
        {0,13},{13,14},{14,15},{15,16},
        {0,17},{17,18},{18,19},{19,20}
    };

    [System.Serializable]
    public class HandLandmark { public float x, y, z; }
    [System.Serializable]
    public class HandFrame { public HandLandmark[] left; public HandLandmark[] right; }

    void Start()
    {
        leftHandPoints = new GameObject[21];
        leftPrevPositions = new Vector3[21];
        rightHandPoints = new GameObject[21];
        rightPrevPositions = new Vector3[21];

        GameObject leftParent = new GameObject("LeftHandPoints");
        leftParent.transform.parent = transform;
        GameObject rightParent = new GameObject("RightHandPoints");
        rightParent.transform.parent = transform;

        for (int i = 0; i < 21; i++)
        {
            GameObject lp = Instantiate(pointPrefab, leftParent.transform);
            lp.name = "LeftJoint" + i;
            
            leftHandPoints[i] = lp;
            leftPrevPositions[i] = lp.transform.localPosition;

            GameObject rp = Instantiate(pointPrefab, rightParent.transform);
            rp.name = "RightJoint" + i;
            
            rightHandPoints[i] = rp;
            rightPrevPositions[i] = rp.transform.localPosition;
        }

        for (int i = 0; i < connections.GetLength(0); i++)
        {
            // Left
            GameObject lLineObj = new GameObject("LeftLine" + i);
            lLineObj.transform.parent = leftParent.transform;
            LineRenderer lr = lLineObj.AddComponent<LineRenderer>();
            lr.positionCount = 2;
            lr.startWidth = lr.endWidth = 0.02f;
            lr.material = new Material(Shader.Find("Sprites/Default"));
            lr.startColor = lr.endColor = Color.green;
            lr.useWorldSpace = false;
            leftLines.Add(lr);

            // Right
            GameObject rLineObj = new GameObject("RightLine" + i);
            rLineObj.transform.parent = rightParent.transform;
            LineRenderer rlr = rLineObj.AddComponent<LineRenderer>();
            rlr.positionCount = 2;
            rlr.startWidth = rlr.endWidth = 0.02f;
            rlr.material = new Material(Shader.Find("Sprites/Default"));
            rlr.startColor = rlr.endColor = Color.blue;
            rlr.useWorldSpace = false;
            rightLines.Add(rlr);
        }
    }

    void Update()
    {
        if (uDPReceiver == null) return;

        string json = uDPReceiver.data;
        if (string.IsNullOrEmpty(json)) return;

        HandFrame frame = JsonUtility.FromJson<HandFrame>(json);
        if (frame == null) return;

        if (frame.left != null && frame.left.Length == 21)
            UpdateHand(frame.left, leftHandPoints, leftPrevPositions, leftLines);

        if (frame.right != null && frame.right.Length == 21)
            UpdateHand(frame.right, rightHandPoints, rightPrevPositions, rightLines);
    }


    void UpdateHand(HandLandmark[] landmarks, GameObject[] points, Vector3[] prevPositions, List<LineRenderer> lines)
    {
        for (int i = 0; i < landmarks.Length; i++)
        {
            float x = (landmarks[i].x - 0.5f) * scale;
            float y = (0.5f - landmarks[i].y) * scale;
            float z = -landmarks[i].z * scale;

            Vector3 target = new Vector3(x, y, z) + baseOffset;
            Vector3 smooth = prevPositions[i] * (1f - smoothFactor) + target * smoothFactor;
            points[i].transform.localPosition = smooth;
            prevPositions[i] = smooth;
        }

        for (int i = 0; i < connections.GetLength(0); i++)
        {
            lines[i].SetPosition(0, points[connections[i, 0]].transform.localPosition);
            lines[i].SetPosition(1, points[connections[i, 1]].transform.localPosition);
        }
    }
}
