using System;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            print("hit" + collision.gameObject.name + ":)");

            BulletEffectImpact(collision);

            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            print("Hit wall");

            BulletEffectImpact(collision);

            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Bob"))
        {
            print("Hit Bob");

            BulletEffectImpact(collision);

            EnemyAi EnemyAi = collision.gameObject.GetComponent<EnemyAi>();

            if (EnemyAi != null)
            {
                EnemyAi.BobLife -= 25;
                print("Bob Life: " + EnemyAi.BobLife);
            }

            BulletEffectImpact(collision);
            Destroy(gameObject); // La bala desaparece

        }
    }


    void BulletEffectImpact(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];

        GameObject hole = Instantiate(
            GlobalReferences.Instance.bulletEffectPrefab,
            contact.point,
            Quaternion.LookRotation(contact.normal)
            );

        hole.transform.SetParent(collision.transform);

        Destroy(hole, 100f);
    }

}
