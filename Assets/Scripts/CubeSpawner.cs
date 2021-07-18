using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField]
    private MovingCube cubePrefab;
    [SerializeField]
    private MoveDirection moveDirection;


    public void SpawnCube()
    {
        var cube = Instantiate(cubePrefab);
        if (MovingCube.LastCube != null && MovingCube.LastCube.gameObject != GameObject.Find("Start"))
        {
            cube.transform.position = new Vector3(moveDirection == MoveDirection.Z ? MovingCube.LastCube.transform.position.x : transform.position.x, MovingCube.LastCube.transform.position.y + cubePrefab.transform.localScale.y, moveDirection == MoveDirection.Z ? transform.position.z : MovingCube.LastCube.transform.position.z);
        }
        else 
        {
            cube.transform.position = transform.position;
        }

        cube.moveDirection = moveDirection;
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, cubePrefab.transform.localScale);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum MoveDirection
{
    X,
    Z
}
