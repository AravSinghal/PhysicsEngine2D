﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace PhysicsEngine2D
{
    public class PhysicsWorld
    {
        public static Vector2 gravity = new Vector2(0, -9.8f);

        public static bool bruteForce = false;

        public List<Body> bodies = new List<Body>();

        internal HashSet<Manifold> manifolds = new HashSet<Manifold>();

        private CollisionSystemSap collisionSystem;

        public int ManifoldCount
        {
            get
            {
                return manifolds.Count;
            }
        }

        private int maxIterations = 10;

        public PhysicsWorld()
        {
            collisionSystem = new CollisionSystemSap(manifolds);
        }

        public void AddBody(Body b)
        {
            bodies.Add(b);
        }

        public void RemoveBody(Body b)
        {
            bodies.Remove(b);
        }

        public void Update(float dt)
        {
            foreach (Body b in bodies)
                b.Update();

            if (bruteForce)
            {
                //Obsolete Brute Force Collisions
                for (int i = 0; i < bodies.Count - 1; i++)
                {
                    for (int j = i + 1; j < bodies.Count; j++)
                    {
                        Manifold key = new Manifold(bodies[i], bodies[j]);
                        if (!manifolds.Contains(key))
                            manifolds.Add(key);
                    }
                }
            }
            else
            {
                //Broad phase
                collisionSystem.BroadPhase(bodies);
            }

            //Narrow phase
            foreach (Manifold m in manifolds)
                m.Collide();

            foreach (Body b in bodies)
                b.IntegrateForces(dt);

            foreach (Manifold m in manifolds)
                m.PreStep(1 / dt);

            //Process maxIterations times for more stability
            for (int i = 0; i < maxIterations; i++)
                foreach (Manifold m in manifolds)
                    m.ApplyImpulse();

            //Integrate positions
            foreach (Body b in bodies)
                b.IntegrateVelocity(dt);
        }
    }
}