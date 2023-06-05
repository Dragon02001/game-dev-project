//
// Rain Maker (c) 2016 Digital Ruby, LLC
// http://www.digitalruby.com
//

using UnityEngine;
using System.Collections.Generic;

namespace DigitalRuby.RainMaker
{
    public class RainCollision : MonoBehaviour
    {
        private static readonly Color32 color = new Color32(255, 255, 255, 255);
        private readonly List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

        public ParticleSystem RainExplosion;
        public ParticleSystem RainParticleSystem;
        private List<Collider> caveRoofColliders = new List<Collider>();


        private void Start()
        {
            // Find all cave roof colliders and add them to the list
            Collider[] colliders = GameObject.FindObjectsOfType<Collider>();
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("CaveRoof"))
                {
                    caveRoofColliders.Add(collider);
                }
            }
        }

        private void Update()
        {

        }

        private void Emit(ParticleSystem p, ref Vector3 pos)
        {         // Check if the position collides with any cave roof collider
            foreach (Collider caveRoofCollider in caveRoofColliders)
            {
                if (caveRoofCollider.bounds.Contains(pos))
                    return; // Don't emit rain particles if colliding with a cave roof
            }
            int count = UnityEngine.Random.Range(2, 5);
            while (count != 0)
            {
                float yVelocity = UnityEngine.Random.Range(1.0f, 3.0f);
                float zVelocity = UnityEngine.Random.Range(-2.0f, 2.0f);
                float xVelocity = UnityEngine.Random.Range(-2.0f, 2.0f);
                const float lifetime = 0.75f;// UnityEngine.Random.Range(0.25f, 0.75f);
                float size = UnityEngine.Random.Range(0.05f, 0.1f);
                ParticleSystem.EmitParams param = new ParticleSystem.EmitParams();
                param.position = pos;
                param.velocity = new Vector3(xVelocity, yVelocity, zVelocity);
                param.startLifetime = lifetime;
                param.startSize = size;
                param.startColor = color;
                p.Emit(param, 1);
                count--;
            }
        }

        private void OnParticleCollision(GameObject obj)
        {
            if (RainExplosion != null && RainParticleSystem != null)
            {
                int count = RainParticleSystem.GetCollisionEvents(obj, collisionEvents);
                for (int i = 0; i < count; i++)
                {
                    ParticleCollisionEvent evt = collisionEvents[i];
                    Vector3 pos = evt.intersection;
                    Emit(RainExplosion, ref pos);
                }
            }
        }
    }
}