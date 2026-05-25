using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostDataClass : MonoBehaviour
{
    public static GhostDataClass instance { get; private set; }
    
    private List<List<Vector3>> positionData;
    private float timePerPos = 0.05f;
    private float indicator = 0f;
    
    public int curr_index = 0; 
    
    private int inner_index = 0;
    public Transform playerTransform;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }

        positionData = new List<List<Vector3>>();
        positionData.Add(new List<Vector3>());
    }

    private void LateUpdate()
    {
        if (playerTransform == null) return;

        indicator += Time.deltaTime;

        if (indicator >= timePerPos)
        {
            positionData[curr_index].Add(playerTransform.position);
            indicator %= timePerPos; 
        }
    }

    public void Reset()
    {
        curr_index = 0;
        inner_index = 0;
        indicator = 0f;
        positionData = new List<List<Vector3>>();
        positionData.Add(new List<Vector3>());
    }
    public void AddnewReplay()
    {
        curr_index++;
        positionData.Add(new List<Vector3>());
        inner_index = 0; 
        indicator = 0f; 
    }

    public List<Vector3> getNextPosData()
    {
        List<Vector3> allGhostData = new List<Vector3>();
        for (int i = 0; i < curr_index; i++)
        {
            if (inner_index >= positionData[i].Count)
            {
                allGhostData.Add(positionData[i][positionData[i].Count - 1]);
            }
            else
            {
                allGhostData.Add(positionData[i][inner_index]);
            }
        }
        inner_index++;
        return allGhostData;
    }
}