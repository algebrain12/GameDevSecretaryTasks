using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    public static GhostManager Instance { get; private set; }
    
    public GameObject ghostPrefab;

    private List<GameObject> allGhosts = new List<GameObject>();
    
    private float timePerPos = 0.05f;
    private float indicator = 0f;
    
    public Vector3 initialPos;
    private int noOfGhosts;
    
    private List<Vector3> prevData = new List<Vector3>();
    private List<Vector3> currData = new List<Vector3>();

    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    public void Reset()
    {
        noOfGhosts += 1;
        
        foreach (var ghost in allGhosts)
        {
            if (ghost != null) Destroy(ghost);
        }
        allGhosts.Clear();

        for (int i = 0; i < noOfGhosts; i++)
        {
            GameObject thisGhost = Instantiate(ghostPrefab, initialPos, Quaternion.identity);
            allGhosts.Add(thisGhost);
        }

        prevData.Clear();
        currData.Clear();
        indicator = 0f;
    } 

    void Update()
    {
        if (allGhosts.Count == 0) return;

        indicator += Time.deltaTime;

        if (indicator >= timePerPos)
        {
            prevData = new List<Vector3>(currData);
            
            currData = GhostDataClass.instance.getNextPosData(); 
            
            indicator %= timePerPos; 

            if (prevData.Count == 0)
            {
                prevData = new List<Vector3>(currData);
            }
        }

        int ghostsToUpdate = Mathf.Min(allGhosts.Count, prevData.Count, currData.Count);
        
        float lerpRatio = indicator / timePerPos;

        for (int i = 0; i < ghostsToUpdate; i++)
        {
            if (allGhosts[i] != null)
            {
                allGhosts[i].transform.position = Vector3.Lerp(prevData[i], currData[i], lerpRatio);
            }
        }
    }
}