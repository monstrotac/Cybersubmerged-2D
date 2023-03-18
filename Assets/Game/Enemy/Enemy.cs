using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyState{
        PATROLLING,
        FOLLOWING,
        RETURNING,
        SEARCHING
    }

    public GameObject player;

    public List<Vector2> patrol;
    public int patrolstep = 0;
    public Vector3 lastPosition;
    public Vector3 returnPosition;

    public float timer = 0;
    public float speed = 1;
    public float dist = 1;
    public float viewDist = 1;

    public EnemyState state = EnemyState.PATROLLING;

    public Vector3 InverstigationPlace;

    // Start is called before the first frame update
    void Start()
    {
        //Initialisation de la patrouille
        if(patrol.Count > 1)
        {
            dist = calculateDist(patrol[1], patrol[0]);
        }

        //On recup le player
        player = FindObjectOfType<PlayerController>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //On passe le temps 
        timer += Time.deltaTime * speed;

        //On detecte si le joueur est dans les alentours
        if (calculateDist(transform.position, player.transform.position) < viewDist)
        {
            if (state != EnemyState.FOLLOWING)
            {
                lastPosition = transform.position;
                state = EnemyState.FOLLOWING;
            }
        }
        else if (state == EnemyState.FOLLOWING)
        {
            timer = 0;
            dist = 5;
            state = EnemyState.SEARCHING;
            InverstigationPlace = transform.position;
        }
        else if (state == EnemyState.SEARCHING)
        {
            if (timer >= dist)
            {
                timer = 0;
                returnPosition = transform.position;
                dist = calculateDist(returnPosition, patrol[calculateNextPoisitionIndex(patrolstep + 1)]);
                state = EnemyState.RETURNING;
            }

            /*if (lastPosition.x > )
            {
                lastPosition = new Vector3(InverstigationPlace.x + Random.Range(-5, 5), InverstigationPlace.y + Random.Range(-5, 5), InverstigationPlace.z);
            }
            */

            transform.position = InverstigationPlace + lastPosition.normalized * speed * Time.deltaTime;
        }

        //Notre boucle de patrouille
        if (state == EnemyState.PATROLLING && patrol.Count > 1)
        {
            if (timer >= dist)
            {
                timer = 0;
                patrolstep = calculateNextPoisitionIndex(patrolstep+1);
                dist = calculateDist(patrol[patrolstep], patrol[calculateNextPoisitionIndex(patrolstep + 1)]);
            }

            //Debug.Log("PATROL");
            transform.position = new Vector3(Mathf.Lerp(patrol[calculateNextPoisitionIndex(patrolstep)].x, patrol[calculateNextPoisitionIndex(patrolstep + 1)].x, timer/dist), Mathf.Lerp(patrol[calculateNextPoisitionIndex(patrolstep)].y, patrol[calculateNextPoisitionIndex(patrolstep + 1)].y, timer/dist), transform.position.z);
        }
        if (state == EnemyState.RETURNING)
        {
            if (timer >= dist)
            {
                state = EnemyState.PATROLLING;
            }
            else
            {
                //Debug.Log("RETURN");
                transform.position = new Vector3(Mathf.Lerp(returnPosition.x, patrol[calculateNextPoisitionIndex(patrolstep + 1)].x, timer / dist), Mathf.Lerp(returnPosition.y, patrol[calculateNextPoisitionIndex(patrolstep + 1)].y, timer / dist), transform.position.z);
            }
        }
        if (state == EnemyState.FOLLOWING)
        {
            Vector3 playPos = player.transform.position;
            Vector3 EnePos = transform.position;
            Vector3 dir = new Vector3(playPos.x - EnePos.x, playPos.y - EnePos.y, EnePos.z);
            
            transform.position = EnePos + dir.normalized * speed * Time.deltaTime;
        }
    }

    public int calculateNextPoisitionIndex(int index) {
        if (index >= patrol.Count) {
            return index-patrol.Count;
        }
        return index;
    }

    public float calculateDist(Vector3 p1, Vector3 p2)
    {
        return Mathf.Sqrt(Mathf.Pow(p2.x - p1.x, 2) + Mathf.Pow(p2.y - p1.y, 2));
    }
}
