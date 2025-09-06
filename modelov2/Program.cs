using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using modelov2.Graphics;
using modelov2.Examples;
using modelov2.Serialization;

namespace modelov2
{
    class Program
    {
        static void Main(string[] args)
        {
            IExample example;
            
            // Si se proporciona un archivo como argumento, cargarlo
            if (args.Length > 0)
            {
                var archivo = args[0];
                Console.WriteLine($"Cargando escena desde: {archivo}");
                example = SceneLoader.LoadFromFile(archivo);
            }
            else
            {
                // Mostrar menú de ejemplos disponibles
                MostrarMenu();
                var seleccion = LeerSeleccion();
                example = CargarEjemplo(seleccion);
            }
            
            var gw = new DemoWindow(GameWindowSettings.Default,
                new NativeWindowSettings { ClientSize = new Vector2i(1000, 700), Title = $"OpenTK - {example.Name}" }, example);
            gw.Run();
        }

        static void MostrarMenu()
        {
            Console.WriteLine("=== Selecciona un ejemplo ===");
            Console.WriteLine("1. Cubos (código)");
            Console.WriteLine("2. Sistema Solar (código)");
            Console.WriteLine("3. Auto (código)");
            Console.WriteLine("4. Brazo Mecánico (código)");
            Console.WriteLine("5. Cubos (archivo)");
            Console.WriteLine("6. Sistema Solar (archivo)");
            Console.WriteLine("7. Cargar archivo personalizado");
            Console.WriteLine();
        }

        static int LeerSeleccion()
        {
            Console.Write("Ingresa tu selección (1-7): ");
            if (int.TryParse(Console.ReadLine(), out int seleccion) && seleccion >= 1 && seleccion <= 7)
            {
                return seleccion;
            }
            Console.WriteLine("Selección inválida, usando ejemplo por defecto.");
            return 1;
        }

        static IExample CargarEjemplo(int seleccion)
        {
            return seleccion switch
            {
                1 => new CuboExample(),
                2 => new SistemaSolarExample(),
                3 => new AutoExample(),
                4 => new BrazoMecanicoExample(),
                5 => SceneLoader.LoadFromFile("ejemplos/cubos.json"),
                6 => SceneLoader.LoadFromFile("ejemplos/sistema_solar.json"),
                7 => CargarArchivoPersonalizado(),
                _ => new CuboExample()
            };
        }

        static IExample CargarArchivoPersonalizado()
        {
            Console.Write("Ingresa la ruta del archivo: ");
            var ruta = Console.ReadLine();
            if (string.IsNullOrEmpty(ruta))
            {
                Console.WriteLine("Ruta vacía, usando ejemplo por defecto.");
                return new CuboExample();
            }
            
            try
            {
                return SceneLoader.LoadFromFile(ruta);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar archivo: {ex.Message}");
                Console.WriteLine("Usando ejemplo por defecto.");
                return new CuboExample();
            }
        }
    }
}