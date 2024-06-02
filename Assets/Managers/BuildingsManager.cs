using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// - Controls tower-related functionalities, including construction and defense calculations.
/// Construction Logic of towers:
/// Manages tower construction in specified territories.
/// Collaborates with the UIManager for visual feedback.
/// - Construction of barraks and other buildings
/// </summary>
public class BuildingsManager : MonoBehaviour
{
    public static BuildingsManager Instance { get; private set; }

    void Awake()
    {
        // Ensure only one instance exists.
        if (Instance == null)
        {
            Instance = this;
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