using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : IManager
{    
    private Dictionary<string, Object> _pool = new Dictionary<string, Object>();
    public void Init()
    {
    }

    public void Clear()
    {
    }

    public T Load<T>(string path) where T : Object
    {
        if (_pool.TryGetValue(path, out Object resource))
            return resource as T;

        _pool[path] = Resources.Load<T>(path);
        return _pool[path] as T;
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"{path}");
        if (original == null)
        {
            Debug.LogError($"Failed to load prefab : {path}"); 
            return null;
        }

        // Poolable 컴포넌트가 있으면 풀링 처리
        if (original.GetComponent<IPoolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;

        // 풀링 처리가 안되어 있으면 일반 인스턴스화
        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;
        return go; 
    }

}
