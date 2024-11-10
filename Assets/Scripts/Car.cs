using UnityEngine;

public abstract class Car : MonoBehaviour
{
    [Header("Nodos")]
    [SerializeField] protected Corner previousCorner = null;
    [SerializeField] protected Corner actualCorner = null;
    [SerializeField] protected Corner nextCorner = null;
    
    [Header("Velocidad")]
    [SerializeField] protected float maxVelocity = 10;
    protected float velocity = 10;
    
    protected Controller controller;

    public Corner ActualCorner => actualCorner;

    void Start()
    {
        velocity = maxVelocity;
    }

    protected virtual void Update()
    {
        //moverse al siguiente nodo.
        if (nextCorner != null)
        {
            //rotar el coche hacia el siguiente nodo.
            Quaternion targetRotation = Quaternion.LookRotation(nextCorner.transform.position - transform.position);
            targetRotation.z = transform.rotation.z; // Mantener la rotación de z siempre igual
            targetRotation.x = transform.rotation.x; // Mantener la rotación de x siempre igual
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * velocity);
            //mover el coche hacia el siguiente nodo.
            Vector3 targetPosition = nextCorner.transform.position;
            targetPosition.y = 0; // Mantener la posición de y siempre en 0
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, velocity * Time.deltaTime);
        }
    }

    public void SetStartCorner(Corner corner)
    {
        this.previousCorner = corner;
    }

    public void SetNextCorner(Corner corner)
    {
        this.nextCorner = corner;
    }

    protected void setVelocity(int weight)
    {
        float k = 0.3f;
        // v = 100*e^(-k*w) || k = 0.3 || k = constante de decaimiento || w = peso de la calle
        velocity = maxVelocity * Mathf.Exp(-(k * weight)); // formula de decaimiento exponencial
    }
}
