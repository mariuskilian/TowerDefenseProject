using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemies
{
    public float turn_speed = 5;
    public float explosion_radius = 3f;
    public float attack_speed_in_sec = 3f;
    public GameObject explosion_effect;

    new void Start()
    {

        // Enemy Attributes
        base.Start();
        _maxHealth = 100000;
        _health = _maxHealth;
        _attack = 0;
        _range = 0;


        InvokeRepeating("Explode", 1.0f, attack_speed_in_sec);

    }

    new void Update()
    {
        base.Update();
        transform.LookAt(Vector3.zero * Time.deltaTime * turn_speed);
        transform.transform.Rotate(0, 90, 0);
        speed = max_speed;

    }

    public void Explode()
    {
        GameObject effect_instance = (GameObject)Instantiate(explosion_effect, transform.position, transform.rotation);
        Destroy(effect_instance, 1f);

        Collider[] collided_objects = Physics.OverlapSphere(transform.position, explosion_radius);
        foreach (Collider collider in collided_objects)
        {

            if (collider.tag == "Tower")
            {
                Destroy(collider.gameObject);

            }




        }

    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, explosion_radius);
    }


}
