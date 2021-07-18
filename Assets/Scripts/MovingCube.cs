using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovingCube : MonoBehaviour
{

    public static MovingCube CurrentCube { get; private set; }
    public static MovingCube LastCube { get; private set; }
    public MoveDirection moveDirection { get; internal set; }


    [SerializeField]
    private float moveSpeed = 1f;

    // Start is called before the first frame update
/*    void Start()
    {
        transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);
    }*/

    private void OnEnable()
    {
        if (LastCube == null)
            LastCube = GameObject.Find("Start").GetComponent<MovingCube>();
        if (this.gameObject != GameObject.Find("Start"))
            CurrentCube = this;
        GetComponent<Renderer>().material.color = GetRandomColor();
        transform.localScale = new Vector3(LastCube.transform.localScale.x, transform.localScale.y, LastCube.transform.localScale.z);
    }

    private Color GetRandomColor()
    {
        return new Color(UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f));
    }

    internal void Stop()
    {
        moveSpeed = 0;
        Vector3 distanceToCube = (transform.position - LastCube.transform.position);
        distanceToCube.y = 0;
        Vector3 direction = distanceToCube;
        direction.Normalize();
        float hangover = distanceToCube.magnitude;
        Vector3 directionMask = new Vector3(Mathf.Abs(direction.x), Mathf.Abs(direction.y), Mathf.Abs(direction.z));

        if (distanceToCube.magnitude >= Vector3.Dot(LastCube.transform.localScale, directionMask))
        {
            Debug.Log(LastCube.transform.localScale);
            LastCube = null;
            CurrentCube = null;
            SceneManager.LoadScene(0);
        }

        SplitCube(distanceToCube, direction, directionMask);
        LastCube = this;
    }

    private void SplitCube(Vector3 distanceToCube, Vector3 direction, Vector3 directionMask)
    {

        float newSize = Vector3.Dot(LastCube.transform.localScale, directionMask) - distanceToCube.magnitude;
        float fallingBlockSize = Vector3.Dot(transform.localScale, directionMask) - newSize;

        float newPosition = Vector3.Dot(transform.position, directionMask) + ((distanceToCube.x + distanceToCube.z) * (-1) / 2);
        transform.localScale = new Vector3(direction.x != 0 ? newSize : transform.localScale.x, transform.localScale.y, direction.z != 0 ? newSize : transform.localScale.z);
        transform.position = new Vector3(direction.x != 0 ? newPosition : transform.position.x, transform.position.y, direction.z != 0 ? newPosition : transform.position.z);

        float cubeEdge = (direction.z != 0 ? transform.position.z : transform.position.x) + (newSize / 2f * Mathf.Sign((distanceToCube.x + distanceToCube.z)));
        float fallingBlockZPosition = cubeEdge + fallingBlockSize / 2f * Mathf.Sign((distanceToCube.x + distanceToCube.z));

        // for debuging the edges
/*        var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = new Vector3(transform.position.x, transform.position.y, cubeEdge);*/

        SpawnDropCube(fallingBlockZPosition, fallingBlockSize, direction);
    }

    private void SpawnDropCube(float fallingBlockPosition, float fallingBlockSize, Vector3 direction)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localScale = new Vector3(direction.x != 0 ? fallingBlockSize : transform.localScale.x, transform.localScale.y, direction.z != 0 ? fallingBlockSize : transform.localScale.z);
        cube.transform.position = new Vector3(direction.x != 0 ? fallingBlockPosition : transform.position.x, transform.position.y, direction.z != 0 ? fallingBlockPosition : transform.position.z);
        cube.AddComponent<Rigidbody>();
        cube.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;
        Destroy(cube.gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (moveDirection == MoveDirection.Z) 
        {
            transform.position += transform.forward * Time.deltaTime * moveSpeed;
        }
        else
        {
            transform.position += transform.right * Time.deltaTime * moveSpeed;
        }
    }
}
