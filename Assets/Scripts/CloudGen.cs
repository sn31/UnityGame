using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGen : MonoBehaviour
{

    public GameObject cloud;
    public float width = 10;
    public float height = 10;
    public float cloudSize = 5;

    void Start()
    {
        for (int x = 0; x < width;x++)
        {
            for (int y = 0; y < height;y++)
            {
                GameObject cloudCube = Instantiate(cloud, new Vector3(transform.position.x + x * cloudSize, transform.position.y, transform.position.z + y * cloudSize),Quaternion.identity);

                cloudCube.name = "Cloud_" + x + "_" + y;
                cloudCube.transform.SetParent(transform);
            }
        }
    }

}
