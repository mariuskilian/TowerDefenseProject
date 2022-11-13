using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalBullet : MonoBehaviour
{
    public float speed = 50f;

    private Transform target;

    public float explosion_radius = 0f;

    public GameObject impact_effect;

    public int damage = 50;

    public GameObject target_null_effect;

    public GameObject identity_of_shooter;
    public void Chase(Transform _target)
    {
        target = _target;
    }

    // Update is called once per frame
    void Update()
    {   // bullet destroyed if target dies, MAYBE CHANGE TO BULLET DROP?
        if (target == null)
        {
            GameObject effect_instance = (GameObject)Instantiate(target_null_effect, transform.position, transform.rotation);

            Destroy(effect_instance, 5f);

            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.position - transform.position;
        float distance_per_frame = speed * Time.deltaTime;

        // if the bullet slows, it calls that it has hit target
        if (direction.magnitude <= distance_per_frame)
        {
            hit_target();
            return;
        }
        //bullet follows target
        transform.Translate(direction.normalized * distance_per_frame, Space.World);
        transform.LookAt(target);




    }


    // on hit check if there is explosion radius or not, then check type of enemy and return a form of take damage or explode
    void hit_target()
    {
        GameObject effect_instance = (GameObject)Instantiate(impact_effect, transform.position, transform.rotation);

        Destroy(effect_instance, 5f);

        // for cannonball etc
        if (explosion_radius > 0f)
        {
            Explode();
        }
        else
        {
            if (target.tag == "Enemy")
            {
               

                if (target.GetComponent<Tags>().HasTag("Physical Enemy"))
                {

                    if (identity_of_shooter.tag == "Physical Mage")
                    {
                        Physical_damage(target);
                    }
                }

                else
                {
                    Damage_enemy(target);
                }
                    
            }
            
        }


        Destroy(gameObject);

    }

    // checks tags in a collider list in a sphere around explosion point, then deals dmg to all injured enemies
    void Explode()
    {
        Collider[] collided_objects = Physics.OverlapSphere(transform.position, explosion_radius);
        foreach (Collider collider in collided_objects)
        {
            if (collider.tag == "Enemy")
            {

                    if (target.GetComponent<Tags>().HasTag("Physical Enemy"))
                    {
                        if (identity_of_shooter.tag == "Physical Mage")
                        {
                                Physical_damage(target);
                        }
                    }
                    

                    else
                    {
                        Damage_enemy(collider.transform);
                    }

            }



            
        }

    }
    // updates enemy hp with dmg
    void Damage_enemy(Transform Enemy)
    {
        // retrieves script aspect of enemy
        Enemies enemy_component = Enemy.GetComponent<Enemies>();



        if (enemy_component != null)
        {

            enemy_component.TakeDamage(damage);
        }
    }

    

    
    void Physical_damage(Transform Enemy)
    {
        // retrieves script aspect of enemy
        Enemies enemy_component = Enemy.GetComponent<Enemies>();



        if (enemy_component != null)
        {

            enemy_component.TakeDamage(damage * 3);

        }
    }

    //visual explosion range
    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, explosion_radius);
    }
}

