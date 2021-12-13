using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    [SerializeField] private AnimationCurve speedCurve;
    [SerializeField] private float speed;
    Vector3 prevPos;
    private int i = 0;
    private float t;

    // Start is called before the first frame update
    void Start()
    {
        t = 0f;
        Vector3 p0 = points[(i - 1 + points.Length) % points.Length].position;
        Vector3 p1 = points[i].position;
        Vector3 p2 = points[(i + 1 + points.Length) % points.Length].position;
        Vector3 p3 = points[(i + 2 + points.Length) % points.Length].position;
        Vector3 nextDir = GetSplinePos(p0, p1, p2, p3, t + 0.1f);
        transform.LookAt(nextDir);
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime * (speedCurve.Evaluate(t) * (1/speed));

        if(t >= 1)
        {
            t = 0;
            i = (i + 1) % points.Length;
        }
        Vector3 p0 = points[(i - 1 + points.Length) % points.Length].position;
        Vector3 p1 = points[i].position;
        Vector3 p2 = points[(i + 1 + points.Length) % points.Length].position;
        Vector3 p3 = points[(i + 2 + points.Length) % points.Length].position;

 
        Vector3 nextPos = GetSplinePos(p0, p1, p2, p3, t);
        Vector3 nextDir = GetSplinePos(p0, p1, p2, p3, t + 0.1f);
        prevPos = transform.position;
        transform.LookAt(nextDir);
        transform.position = nextPos;

    }

    Vector3 GetSplinePos(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        //solve for the catmul-rom polynomial
        Vector3 a = 2f * p1;
        Vector3 b = p2 - p0;
        Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
        Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;

        return 0.5f * (a + (b * t) + (c * t * t) + (d * t * t * t));
    }

    Vector3 GetSplineDir(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        Vector3 a = -p0 + p2;
        Vector3 b = 2f * p0 - 5f * p1 + 4f * p2 - p3;
        Vector3 c = -p0 + 3f * p1 - 3f * p2 + p3;

        return 0.5f * a * t + b * t + 1.5f * c * t * t;

    }
}
