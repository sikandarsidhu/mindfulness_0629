
// =================================	
// Namespaces.
// =================================

using UnityEngine;

// =================================	
// Define namespace.
// =================================

namespace MirzaBeig
{

    namespace ParticleSystems
    {

        namespace Demos
        {

            // =================================	
            // Classes.
            // =================================
            
            public class LoopingParticleSystemsManager : ParticleManager
            {
                // =================================	
                // Nested classes and structures.
                // =================================

                // ...

                // =================================	
                // Variables.
                // =================================

                // ...

                // =================================	
                // Functions.
                // =================================

                // ...

                protected override void Awake()
                {
                    base.Awake();
                }

                // ...

                protected override void Start()
                {
                    base.Start();

                    // ...
                    
                    particlePrefabs[currentParticlePrefabIndex].gameObject.SetActive(true);
                }

                // ...

                public override void next()
                {
                    particlePrefabs[currentParticlePrefabIndex].gameObject.SetActive(false);

                    base.next();
                    particlePrefabs[currentParticlePrefabIndex].gameObject.SetActive(true);
                }
                public override void previous()
                {
                    particlePrefabs[currentParticlePrefabIndex].gameObject.SetActive(false);

                    base.previous();
                    particlePrefabs[currentParticlePrefabIndex].gameObject.SetActive(true);
                }

                // ...

                protected override void Update()
                {
                    base.Update();
                }

                // ...

                public override int getParticleCount()
                {
                    // Return particle count from active prefab.

                    return particlePrefabs[currentParticlePrefabIndex].getParticleCount();
                }

                // =================================	
                // End functions.
                // =================================

            }

            // =================================	
            // End namespace.
            // =================================

        }

    }

}

// =================================

// =================================	
// --END-- //
// =================================
