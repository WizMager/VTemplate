namespace Generator
{
    public class TypeElement
    {
        public readonly string TypeName;
        public readonly EExecutionPriority Priority;
        public readonly int Order;

        public TypeElement(
            string typeName, 
            EExecutionPriority priority, 
            int order
        )
        {
            TypeName = typeName;
            Priority = priority;
            Order = order;
        }
    }
}