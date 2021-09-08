using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedZone : MonoBehaviour
{
    [Header("PARAMETERS")]
    public Vector3[] stagesScale;
    public float[] stagesTime;
    public Vector3 startRandomPos;
    public float minRange;
    public float maxRange;

    [Header("SETTINGS")]
    public GameObject zoneVisualizer;
    public int currentStage;
    public float firstTimer;
    public float timer;
    public bool isWaiting;
    public Vector3 currentZoneSize;
    float smoothTimer;
    public float speed;

    public Vector3 GetRandomPosition()
    {
        return startRandomPos = new Vector3(Random.Range(minRange, maxRange), 0.01f, Random.Range(minRange, maxRange));
    }
    public void SetStartPosition(Vector3 pos)
    {
        zoneVisualizer.transform.position = pos;
    }

    public void NextStage()
    {
        if (currentStage < stagesScale.Length-1)
        {
            currentStage++;
            timer = stagesTime[currentStage];
            isWaiting = true;
        }
    }

    private void FixedUpdate()
    {
        zoneVisualizer.transform.localScale = currentZoneSize;
        if(currentStage == -1)
        {
            firstTimer -= Time.deltaTime;
            if(firstTimer <= 0f)
            {
                NextStage();
                isWaiting = false;
            }
            else
            {
                isWaiting = true;
                smoothTimer = 0;
            }
        }
        else
        {
            if (isWaiting == false)
            {
                smoothTimer = Time.deltaTime * speed;
                if (currentZoneSize.x > stagesScale[currentStage].x)
                {
                    currentZoneSize = new Vector3(currentZoneSize.x - smoothTimer, currentZoneSize.y, currentZoneSize.z - smoothTimer);
                }
                else
                {
                    NextStage();
                }
            }
            else
            {
                timer -= Time.deltaTime;
                if(timer <= 0f)
                {
                    isWaiting = false;
                }
            }
        }
    }
}
