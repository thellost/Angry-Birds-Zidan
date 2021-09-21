using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwlScript : Bird
{
    //untuk instantiate ledakannya , ledakan menggunakan point effector 2D
    public GameObject Explosion;
    bool exploded = false;
    public override void OnCollisionEnter2D(Collision2D collision)
    {
        
        Debug.Log("Hit something");
        if ((State == BirdState.HitSomething || State == BirdState.Thrown) && !exploded && (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Obstacle")) {
           explode();
        }
        State = BirdState.HitSomething;
    }

    private void explode()
    {
        GameObject x = Instantiate(Explosion, transform.position, Quaternion.identity);
        exploded = true;

        //menghapus eexplosion setelah 0.1 detik
        StartCoroutine(deleteAfter(x));
    }

    IEnumerator deleteAfter(GameObject explosion)
    {
        yield return new WaitForSeconds(.1f);
        Destroy(explosion);
    }
}
