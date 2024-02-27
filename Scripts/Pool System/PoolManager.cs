using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] Pool[] _playerBulletPools;

    static Dictionary<GameObject, Pool> _dictionary;

    private void Start()
    {
        _dictionary = new Dictionary<GameObject, Pool>();
        Initialize(_playerBulletPools);
    }
    void Initialize(Pool[] pools)
    {
        foreach (Pool pool in pools)
        {
            if (_dictionary.ContainsKey(pool.Prefab))
            {
                continue;
            }
            _dictionary.Add(pool.Prefab, pool);

            Transform poolParent = new GameObject("Pool: " + pool.Prefab.name).transform;
            poolParent.parent = transform;
            pool.Initialize(poolParent);
        }
    }

    /// <summary>
    /// 返回对象池中预备好的游戏对象
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab)
    {
        if (!_dictionary.ContainsKey(prefab)){
            return null;
        }
        return _dictionary[prefab].PreparedObject();
    }

    public static GameObject Release(GameObject prefab,Vector3 position)
    {
        if (!_dictionary.ContainsKey(prefab))
        {
            return null;
        }
        return _dictionary[prefab].PreparedObject(position);
    }
    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!_dictionary.ContainsKey(prefab))
        {
            return null;
        }
        return _dictionary[prefab].PreparedObject(position,rotation);
    }

    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 localScale)
    {
        if (!_dictionary.ContainsKey(prefab))
        {
            return null;
        }
        return _dictionary[prefab].PreparedObject(position,rotation,localScale);
    }

    public static void ReturnToPool(GameObject prefab, GameObject obj)
    {
        if (_dictionary.ContainsKey(prefab))
        {
            _dictionary[prefab].ReturnToPool(obj);
        }
    }

}
