using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;


[System.Serializable]
public class Pool
{
    public GameObject Prefab => _prefab;
    [SerializeField] GameObject _prefab;
    [SerializeField] int _size = 1;

    Queue<GameObject> _queue;

    Transform _parent;

    public void Initialize(Transform _parent)
    {
        _queue = new Queue<GameObject>();
        this._parent = _parent;
        for(var i = 0; i < _size; i++)
        {
            _queue.Enqueue(Copy());
        }
    }

    GameObject Copy()
    {
        var copy = GameObject.Instantiate(_prefab, _parent);
        copy.SetActive(false);
        return copy;
    }
    GameObject AvailableObject()
    {
        GameObject availableObject = null;  
        if(_queue.Count > 0 && !_queue.Peek().activeSelf)
        {
            availableObject = _queue.Dequeue();
        }
        else
        {
            availableObject = Copy();
        }
        _queue.Enqueue(availableObject);
        return availableObject;
    }

    public GameObject PreparedObject()
    {
        GameObject preparedObject = AvailableObject();
        if (preparedObject != null)
        {
            preparedObject.SetActive(true);
        }
        return preparedObject;
    }

    public GameObject PreparedObject(Vector3 position)
    {
        GameObject preparedObject = AvailableObject();
        if (preparedObject != null)
        {
            preparedObject.SetActive(true);
            preparedObject.transform.position = position;
        }
        return preparedObject;
    }

    public GameObject PreparedObject(Vector3 position, Quaternion rotation)
    {
        GameObject preparedObject = AvailableObject();
        if (preparedObject != null)
        {
            preparedObject.SetActive(true);
            preparedObject.transform.position = position;
            preparedObject.transform.rotation = rotation;
        }
        return preparedObject;
    }

    public GameObject PreparedObject(Vector3 position, Quaternion rotation,Vector3 localScale)
    {
        GameObject preparedObject = AvailableObject();
        if (preparedObject != null)
        {
            preparedObject.SetActive(true);
            preparedObject.transform.position = position;
            preparedObject.transform.rotation = rotation;
            preparedObject.transform.localScale = localScale;
        }
        return preparedObject;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        _queue.Enqueue(obj);
    }
}
