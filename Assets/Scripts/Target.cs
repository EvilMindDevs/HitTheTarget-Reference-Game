using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float minSpeed;
    public float maxSpeed;
    float speed;
    Spawner spawnerScript;
    int spawnPointSide;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        speed = Random.Range(minSpeed, maxSpeed);
        spawnerScript = GameObject.Find("Spawner").GetComponent<Spawner>();
        spawnPointSide = spawnerScript.getSpawnPointSide();
    }

    // Update is called once per frame
    void Update()
    {
        //spawnPointSide = spawnerScript.getSpawnPointSide();
        
        if (spawnPointSide == 0) 
        { 
            transform.Translate(Vector2.down * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D hitObject)
    {
        /*if (hitObject.tag == "Player")
        {
            //Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }*/

        if (hitObject.tag == "Ground")
        {
            //Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        else if (hitObject.tag == "Rock")
        {
            anim.SetBool("shouldYouPop", true);
            //anim.SetBool("shouldYouPop", false);
            //Instantiate(explosion, transform.position, Quaternion.identity);
            //print("HIT BY THE ROCK!");
            //Destroy(gameObject);
        }
    }
}
