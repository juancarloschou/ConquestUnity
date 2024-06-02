using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// - Orchestrates the real-time battles.
/// - Calculates fight outcomes based on troops involved.
/// Battle Execution:
/// Orchestrates the real-time battles.
/// Calculates and communicates results to the UIManager for display.
/// </summary>
public class BattleManager : MonoBehaviour
{
    private static BattleManager instance;

    void Awake()
    {
        // Ensure only one instance exists.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // This ensures the GameObject persists between scenes.
        }
        else
        {
            Destroy(gameObject);
        }

        // Additional initialization code...
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}