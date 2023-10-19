using UnityEngine;

public class Fruit : MonoBehaviour
{
    public GameObject whole;
    public GameObject sliced;

    private Rigidbody fruitRigidbody;
    private Collider fruitCollider;
    private ParticleSystem juiceEffect;

    public int points = 1;

    private void Awake()
    {
        fruitRigidbody = GetComponent<Rigidbody>();
        fruitCollider = GetComponent<Collider>();
        juiceEffect = GetComponentInChildren<ParticleSystem>();
    }

    private void Slice(Vector3 direction, Vector3 position, float force) //параметры: направление, в котором нарезан фрукт, позиция в которой столкнулись с фруктом
    {                                                                    //и сила которую надо добавить к фрукту при нарезке
        FindObjectOfType<GameManager>().IncreaseScore(points);

        fruitCollider.enabled = false;
        whole.SetActive(false); //disable prefab with whole fruit
        sliced.SetActive(true); //enabled prefab with sliced fruit
        juiceEffect.Play(); //enable effect ParticleSystem

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; //the angle by which to rotate the fruit //* Mathf.Rad2Deg переводит радианы в градусы
        sliced.transform.rotation = Quaternion.Euler(0f, 0f, angle); //вращать ломтик фрукта в том направлении, в котором нарезали

        Rigidbody[] slices = sliced.GetComponentsInChildren<Rigidbody>(); //массив ломтиков фруктов

        foreach (Rigidbody slice in slices)
        {
            slice.velocity = fruitRigidbody.velocity; //скорость каждого ломтика должна соответствовать скорости фрукта, чтобы он двигался по той же траектории
            slice.AddForceAtPosition(direction * force, position, ForceMode.Impulse); //сила пользователя в точке соприкосновения
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Blade blade = other.GetComponent<Blade>();
            Slice(blade.direction, blade.transform.position, blade.sliceForce);
        }
    }

}
