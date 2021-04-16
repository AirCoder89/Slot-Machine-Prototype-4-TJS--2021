namespace Core
{
    /// <summary>
    /// Represents 2 approach to generate slots:
    /// Asynchronous : Load and instantiate all slots instance (async) with Unity Addressable.
    /// Synchronous  : Load the slot prefab (async) with Unity Addressable then Instantiate (sync) slots.
    /// </summary>
    public enum GenerateType 
    {
        Asynchronous , Synchronous
    }
}