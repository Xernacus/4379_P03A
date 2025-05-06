using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

[ExecuteInEditMode]
public class AISensor : MonoBehaviour
{
    public float distance = 10;
    public float angle = 30;
    public float height = 1.0f;
    public Color meshColor = Color.blue;
    public int scanFrequency = 30;
    public LayerMask layers;
    public LayerMask occlusionLayers;
    public List<GameObject> Objects
    {
        get {
            objects.RemoveAll(obj => !obj);
            return objects; 
        }
    }
    private List<GameObject> objects = new List<GameObject>();

    Collider[] colliders = new Collider[50];
    Mesh mesh;
    int count;
    float scanInterval;
    float scanTimer;

    // Start is called before the first frame update
    void Start()
    {
        scanInterval = 1.0f / scanFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        /**scanTimer -= Time.deltaTime;
        if (scanTimer < 0)
        {
            scanTimer += scanInterval;
            Scan();
        }
        **/
    }

    public void Scan()
    {
        count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders, layers, QueryTriggerInteraction.Collide);
        objects.Clear();
        for (int i = 0; i < count; ++i)
        {
            GameObject obj = colliders[i].gameObject;
            if (IsInSight(obj))
            {
                objects.Add(obj);
            }
        }
        
    }

    public bool IsInSight(GameObject obj)
    {
        Vector3 origin = transform.position;
        Vector3 dest = obj.transform.position;
        Vector3 direction = dest - origin;
        if (direction.y < -height || direction.y > height)
        {
            return false;
        }

        float deltaAngle = Vector3.Angle(direction, transform.forward);
        
        if (deltaAngle > angle)
        {
            return false;
        }

        origin.y = height / 2;
        dest.y = origin.y;

        if (Physics.Linecast(origin, dest, occlusionLayers))
        {
            return false;
        }
        return true;
    }

    public GameObject[] Filter(GameObject[] buffer, string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        int count = 0;
        foreach (var obj in Objects)
        {
            if (obj.layer == layer)
            {
                buffer[count++] = obj;
            }

            if (buffer.Length == count)
            {
                break;
            }

        }
        
        return buffer;
    }

    private void OnValidate()
    {
        scanInterval = 1.0f / scanFrequency;
    }
    
}
