
namespace MushROMs
{
    public interface ISelection
    {
        Data GetData(Data src);
        void SetData(Data value, Data dest);
    }
}
