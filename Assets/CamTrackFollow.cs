using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTrackFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private float smoothness = 1f;

    [SerializeField] private Vector3 offset;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null) return;

        var currentY = transform.position.y;
        var targetY = target.position.y - offset.y;

        var travelAmount = Mathf.Lerp(currentY, targetY, Time.deltaTime * smoothness);

        transform.position = new Vector3(transform.position.x, travelAmount, transform.position.z);
    }
}
