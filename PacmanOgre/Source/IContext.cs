
namespace PacmanOgre
{
    public interface IContext
    {
        IRenderer GetRenderer();

        ITickManager GetITickManager();
    }
}
