using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealityData : MonoBehaviour
{
    private static RealityData instance;
    public float _stress;
    public List<string> _itemList;

    public static RealityData Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<RealityData>();
            return instance;
        }
    }
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("RealityData");

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
