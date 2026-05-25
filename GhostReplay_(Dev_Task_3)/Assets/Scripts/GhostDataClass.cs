using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostDataClass : MonoBehaviour
{
    // Fixed capitalization to match standard conventions and GhostManager
    public static GhostDataClass instance { get; private set; }
    
    private List<List<Vector3>> positionData;
    private float timePerPos = 0.1f;
    private float indicator = 0f;
    
    // curr_index points to the list currently RECORDING the player
    public int curr_index = 0; 
    
    // inner_index tracks the playback frame for the ghosts
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
        // Guard check: Make sure playerTransform isn't missing
        if (playerTransform == null) return;

        indicator += Time.deltaTime;

        if (indicator >= timePerPos)
        {
            positionData[curr_index].Add(playerTransform.position);
            // Using modulo to preserve overflow time for accurate recordings
            indicator %= timePerPos; 
        }
    }

    // Completely wiped data (hard reset back to level start/first attempt)
    public void Reset()
    {
        curr_index = 0;
        inner_index = 0;
        indicator = 0f;
        positionData = new List<List<Vector3>>();
        positionData.Add(new List<Vector3>());
    }

    // Called when the player dies/finishes and resets the run, creating a new ghost slot
    public void AddnewReplay()
    {
        curr_index++;
        positionData.Add(new List<Vector3>());
        
        // CRITICAL: Reset the playback head for the ghosts so they start from the beginning next run
        inner_index = 0; 
        indicator = 0f; 
    }

    public List<Vector3> getNextPosData()
    {
        List<Vector3> allGhostData = new List<Vector3>();
        
        // FIXED: Changed '<' to '<=' so that ALL completed runs are read.
        // If curr_index is 1, it means run 0 is finished (ghost) and run 1 is recording. 
        // We need to fetch data for run 0.
        for (int i = 0; i <= curr_index; i++)
        {
            // Don't read from the run currently being recorded! Ghosts only mimic PAST runs.
            if (i == curr_index) continue; 

            // Prevent OutOfBounds exception if a previous run was shorter than the current one
            if (inner_index >= positionData[i].Count)
            {
                // Ghost stays at its final recorded position
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