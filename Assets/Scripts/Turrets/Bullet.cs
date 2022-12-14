using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{


    public float speed = 50f;

    private Transform target;

    public float explosion_radius = 0f;

    public GameObject impact_effect;

    private float damage = Difficulty.bullet_dmg;

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
        if (gameObject.tag == "Arrow")
        {
            transform.transform.Rotate(0, 90, 0);
        }




    }


    // on hit check if there is explosion radius or not, then check type of enemy and return a form of take damage or explode
    void hit_target()
    {
        GameObject effect_instance = (GameObject)Instantiate(impact_effect, transform.position, transform.rotation);
        AudioManager.instance.PlaySoundEffect("Impact");

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
                Damage_enemy(target);
            }
            if (target.tag == "Tower")
            {
                Damage_tower(target);
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

                Damage_enemy(collider.transform);


            }

            //if (collider.tag == "Shield")
            //{
            //    Damage_shield(collider.transform);
            //}

            if (collider.tag == "Tower")
            {
                Damage_tower(collider.transform);
            }
        }
    }

    // private void Damage_shield(Transform Shield)
    //{
    //    Shield shield_component = Shield.GetComponent<Shield>();
    //    Debug.Log("stuff");
    //    if (shield_component != null)
    //    {
    //        
    //        shield_component.TakeDamage(damage);
    //    }
    //}

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

    // updates tower hp with dmg
    void Damage_tower(Transform Tower)
    {
        // retrieves script aspect of enemy
        Turret turret_component = Tower.GetComponent<Turret>();

        if (turret_component != null)
        {
            turret_component.TakeDamage(damage);
        }
    }

    // specific type dmg
    void Magic_damage(Transform Enemy)
    {
        // retrieves script aspect of enemy
        Enemies enemy_component = Enemy.GetComponent<Enemies>();



        if (enemy_component != null)
        {

            enemy_component.TakeDamage(damage * 3);
        }
    }
    // specific type dmg
    void Physical_damage(Transform Enemy)
    {
        // retrieves script aspect of enemy
        Enemies enemy_component = Enemy.GetComponent<Enemies>();



        if (enemy_component != null)
        {

            enemy_component.TakeDamage(damage * 3);

        }
    }
    // specific type dmg
    void Imaginary_damage(Transform Enemy)
    {
        // retrieves script aspect of enemy
        Enemies enemy_component = Enemy.GetComponent<Enemies>();



        if (enemy_component != null)
        {

            enemy_component.TakeDamage(damage * 3);
        }
    }
    // specific type dmg
    void Mechanical_damage(Transform Enemy)
    {
        // retrieves script aspect of enemy
        Enemies enemy_component = Enemy.GetComponent<Enemies>();



        if (enemy_component != null)
        {

            enemy_component.TakeDamage(damage * 2);
            //enemy_component.mechanical_lift(1);
        }
    }
    //visual explosion range
    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, explosion_radius);
    }
}
