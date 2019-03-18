namespace Brainiac.Contract
{
    /// <summary>
    /// A data item.
    /// </summary>
    public interface IDataItem<T> 
    {
        T Id { get; set; }
    }
}
