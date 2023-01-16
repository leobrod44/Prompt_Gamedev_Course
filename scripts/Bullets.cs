using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    public ParticleSystem impactSplash;
    
    //Créer des particules lors du contact avec un monstre et détruire le projectile
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == ("monster"))
        {
            ParticleSystem parts = Instantiate(impactSplash, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
    //Méthode pour détruire les particules apres un délais avec une Coroutine
    public IEnumerator DestroyParticles(ParticleSystem p, float t)
    {
        yield return new WaitForSeconds(t);
        Destroy(p);
    }
}
