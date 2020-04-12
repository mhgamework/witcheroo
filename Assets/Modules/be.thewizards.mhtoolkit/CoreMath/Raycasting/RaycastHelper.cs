namespace Assets.Modules.MHGameWork.TheWizards.Mathematics.Raycasting
{
    public struct RaycastCalculator<T>
    {
        public float MinDistance;
        public T MinValue;
        public bool IsHit => MinValue != null;

        public static RaycastCalculator<T> Create()
        {
            return new RaycastCalculator<T>() { MinDistance = float.MaxValue };
        }

        public void Apply(float distance, T value)
        {
            if (distance > MinDistance) return;
            MinValue = value;
            MinDistance = distance;
        }
    }
}