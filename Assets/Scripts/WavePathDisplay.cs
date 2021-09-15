using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// README
// 세팅은 LindRenderer 컴포넌트와 같은 오브젝트에 두고 enemyManger 필드만 지정해주면 됩니다.
// DisplayPath()를 호출해주면 선을 그립니다.
// animationDuration 값은 HEAD 기준입니다.
// total Animation Duration은 animationDuration + timeBetweenHeadAndTail 입니다.

public class WavePathDisplay : MonoBehaviour
{
    [SerializeField] private float animationDuration = 5f;
    [SerializeField] private float timeBetweenHeadAndTail = 2f;

    private LineRenderer lineRenderer;
    private Vector3[] linePoints;
    private int pointsCount;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    public void DisplayPath(Vector3[] pathPoints)
    {
        Vector3[] linePoints = new Vector3[pathPoints.Length];
        Vector3 bias = new Vector3(0, 0.5f, 0); // 라인이 지형에 묻히는 것 방지
        for (int i = 0; i < pathPoints.Length; i++)
        {
            linePoints[i] = pathPoints[i] + bias;
        }
        lineRenderer.positionCount = pathPoints.Length;
        lineRenderer.SetPositions(linePoints);
        CopyPathData(linePoints);

        /*
        // 아래는 lineRenderer의 points를 그대로 사용할 경우
        // Store a copy of lineRenderer's points in linePoints array
        pointsCount = lineRenderer.positionCount;
        linePoints = new Vector3[pointsCount];
        for (int i = 0; i < pointsCount; i++)
        {
            linePoints[i] = lineRenderer.GetPosition(i);
        }
        */

        if (animateLineCoroutine != null)
            StopCoroutine(animateLineCoroutine);
        if (animateHeadCoroutine != null)
            StopCoroutine(animateHeadCoroutine);
        if (animateTailCoroutine != null)
            StopCoroutine(animateTailCoroutine);
        animateLineCoroutine = StartCoroutine(AnimateLine());
    }

    private void CopyPathData(Vector3[] pathPoints)
    {
        pointsCount = pathPoints.Length;
        linePoints = new Vector3[pointsCount];
        for (int i = 0; i < pointsCount; i++)
        {
            linePoints[i] = pathPoints[i];
        }
    }

    Coroutine animateLineCoroutine;
    private IEnumerator AnimateLine()
    {
        lineRenderer.enabled = true;
        if (animateHeadCoroutine != null)
            StopCoroutine(animateHeadCoroutine);
        if (animateTailCoroutine != null)
            StopCoroutine(animateTailCoroutine);
        animateHeadCoroutine = StartCoroutine(AnimateHead());
        yield return new WaitForSeconds(timeBetweenHeadAndTail);
        animateTailCoroutine = StartCoroutine(AnimateTail());
        animateLineCoroutine = null;
    }

    Coroutine animateHeadCoroutine;
    private IEnumerator AnimateHead()
    {
        float segmentDuration = 0;

        for (int i = 0; i < pointsCount - 1; i++)
        {
            segmentDuration = CalculateSegmentRatio(i) * animationDuration;
            float startTime = Time.time;

            Vector3 startPosition = linePoints[i];
            Vector3 endPosition = linePoints[i + 1];

            Vector3 pos = startPosition;
            while (pos != endPosition)
            {
                float t = (Time.time - startTime) / segmentDuration;
                pos = Vector3.Lerp(startPosition, endPosition, t);

                // animate all other points except point at index i
                for (int j = i + 1; j < pointsCount; j++)
                    lineRenderer.SetPosition(j, pos);

                yield return null;
            }
        }
        animateHeadCoroutine = null;
    }

    Coroutine animateTailCoroutine;
    private IEnumerator AnimateTail()
    {
        float segmentDuration = 0;

        for (int i = 0; i < pointsCount - 1; i++)
        {
            segmentDuration = CalculateSegmentRatio(i) * animationDuration;
            float startTime = Time.time;

            Vector3 startPosition = linePoints[i];
            Vector3 endPosition = linePoints[i + 1];

            Vector3 pos = startPosition;
            while (pos != endPosition)
            {
                float t = (Time.time - startTime) / segmentDuration;
                pos = Vector3.Lerp(startPosition, endPosition, t);

                // animate all other points except point at index i
                for (int j = 0; j <= i; j++)
                    lineRenderer.SetPosition(j, pos);

                yield return null;
            }
        }
        // 마지막엔 렌더러 꺼주기
        lineRenderer.enabled = false;

        animateTailCoroutine = null;
    }

    private float CalculateSegmentRatio(int segmentStartPointIndex)
    {
        float segmentLength = Vector3.Distance(linePoints[segmentStartPointIndex], linePoints[segmentStartPointIndex + 1]);
        return segmentLength / CalculateTotalLineLength();
    }

    private float CalculateTotalLineLength()
    {
        if (linePoints.Length <= 1)
            return 0;
        else
        {
            float totalLength = 0f;
            for (int i = 0; i < pointsCount - 1; i++)
            {
                totalLength += Vector3.Distance(linePoints[i], linePoints[i + 1]);
            }
            return totalLength;
        }
    }
}
