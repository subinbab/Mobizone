namespace UILayer.Models
{
    public interface IRequestHandler<T>
    {
        T content { get; set; }
        string url { get; set; }

        ResponseModel<T> Delete(int id);
        ResponseModel<T> Edit(T entity);
        ResponseModel<T> Get();
        ResponseModel<T> Post(T entity);
    }
}