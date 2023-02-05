using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 10; // Make unity rendering 10 frames per seconds
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Debug.Log(horizontal);
        Debug.Log(vertical);

        Vector2 position = transform.position;
        position.x = position.x + 3f*horizontal*Time.deltaTime;
        position.y = position.y + 3f * vertical*Time.deltaTime;

        transform.position = position;

    }
}
