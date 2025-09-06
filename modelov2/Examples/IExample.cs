using modelov2.Models;
using OpenTK.Mathematics;

namespace modelov2.Examples
{
    public interface IExample
    {
        string Name { get; }
        void Setup(Objeto objeto);
        void Update(Objeto objeto, float tiempo);
        void SetColors(Parte parte);
    }
}
