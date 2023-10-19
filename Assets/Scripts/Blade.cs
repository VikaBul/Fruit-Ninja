using UnityEngine;

public class Blade : MonoBehaviour
{
    public Vector3 direction { get; private set; }

    private Camera mainCamera;

    private Collider sliceCollider; // collider on player
    private TrailRenderer sliceTrail; //children of blade, след от лезвия

    public float sliceForce = 5f;
    public float minSliceVelocity = 0.01f;

    private bool slicing;

    private void Awake()
    {
        mainCamera = Camera.main;
        sliceCollider = GetComponent<Collider>();
        sliceTrail = GetComponentInChildren<TrailRenderer>();
    }

    private void OnEnable()
    {
        StopSlice();
    }

    private void OnDisable()
    {
        StopSlice();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartSlice();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopSlice();
        }
        else if (slicing)
        {
            ContinueSlice();
        }
    }

    private void StartSlice()
    {
        Vector3 position = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0f;
        transform.position = position;

        slicing = true;
        sliceCollider.enabled = true;
        sliceTrail.enabled = true;
        sliceTrail.Clear();
    }

    private void StopSlice()
    {
        slicing = false;
        sliceCollider.enabled = false;
        sliceTrail.enabled = false;
    }

    private void ContinueSlice() //update blade position relative to mouse
    {
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition); //converting from screen space to world space
        newPosition.z = 0f; //all fruits at zero depth

        direction = newPosition - transform.position; //blade direction 

        float velocity = direction.magnitude / Time.deltaTime; //blade speed, т.е сколько он переместился за последний кадр
        sliceCollider.enabled = velocity > minSliceVelocity; //коллайдер включен, если скорость лезвия больше 0,01

        transform.position = newPosition; //update blade position
    }

}