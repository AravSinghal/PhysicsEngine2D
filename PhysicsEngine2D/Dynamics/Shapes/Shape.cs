﻿namespace PhysicsEngine2D
{
    public abstract class Shape
    {
        public enum ShapeType
        {
            Polygon, Circle,
            Count //Count is just so that the number of shapes can be referenced easily
        };

        public Body body;
        public Transform transform;

        public ShapeType type;
        
        public abstract Shape Clone();
        public abstract Bounds GetBoundingBox();
        public abstract void ComputeMass(float density);
        public abstract bool Raycast(Ray2 ray, float distance, out RaycastResult result);
    }
}
