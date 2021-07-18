using System;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool activated = false;
    
    [SerializeField] private BoxCollider[] boxCollider;
    private Transform door;

    private float min = 0;
    private float max = 2.5f;
    private float t = 0.0f;
    
    void Start() => door = transform.GetChild(0);
    void Update()
    {
        if (activated)
        {
            var _doorPos = door.localPosition;
            _doorPos.y = Mathf.Lerp(min, max, t);

            t += 1 * Time.deltaTime;
            door.localPosition = _doorPos;
            
            if (t > 2.5f)
            {
                var temp = max;
                max = min;
                   min = temp;
        
                foreach (var _t in boxCollider)
                    _t.enabled = true;

                t = 0.0f;
                activated = false;
            }
        }                
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var _t in boxCollider)
                _t.enabled = false;
            
            activated = true;
        }
    }
}
