using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    public float timeFlow;
    public GameManager gameManager;
    public float projectileSpeed;
    float rotation;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        timeFlow = gameManager.timeFlow;
        transform.Translate(Vector2.up * projectileSpeed * timeFlow * Time.deltaTime);
    }

    public void RandomRotate(Vector3 randomAngle)
    {
        transform.up = transform.up + randomAngle;
    }
}
