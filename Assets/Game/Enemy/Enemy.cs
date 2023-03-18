using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public List<Vector2> patrol;
    public int patrolstep = 0;

    public float timer = 0;
    public float speed = 1;
    public float dist = 1;

    public bool isPatroling = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (isPatroling && patrol.count > 0)
        {
            transform.position = new Vector3(Mathf.Lerp(patrol[calculateNextPoisitionIndex(step)].x, patrol[calculateNextPoisitionIndex(step+1)].x, timer/dist), Mathf.Lerp(), transform.position.z);
        }
    }

    public int calculateNextPoisitionIndex(int index) {
        if (index >= patrol.count) {
            return index-patrol.count;
        }
        return index;
    }
}
