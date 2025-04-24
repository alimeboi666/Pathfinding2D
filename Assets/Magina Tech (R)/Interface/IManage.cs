
public interface IManage<T> where T: IManageable
{
    T Managee { get; set; }
}

public interface IManageable
{
    void OnStart();
}