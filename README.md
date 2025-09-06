# Sistema de Gráficos 3D

Un sistema completo de gráficos 3D desarrollado en C# utilizando OpenTK4 para renderizado y manipulación de objetos tridimensionales.

## 🚀 Características

- **Renderizado 3D** con OpenTK4
- **Modelado de objetos** complejos con múltiples partes
- **Sistema de animación** para objetos dinámicos
- **Carga de escenas** desde archivos JSON
- **Múltiples ejemplos** incluidos (cubos, sistema solar, brazo mecánico)
- **Gestión de geometría** avanzada con vértices y polígonos
- **Sistema de colores** y materiales

## 📁 Estructura del Proyecto

```
modelov2/
├── Data/                    # Clases de datos para serialización
│   ├── AnimationData.cs     # Datos de animación
│   ├── ColorData.cs         # Información de colores
│   ├── GeometryData.cs      # Datos geométricos
│   ├── ParteData.cs         # Datos de partes de objetos
│   └── SceneData.cs         # Datos de escenas completas
├── Examples/                # Ejemplos de uso
│   ├── AutoExample.cs       # Ejemplo de automóvil
│   ├── BrazoMecanicoExample.cs # Ejemplo de brazo mecánico
│   ├── CuboExample.cs       # Ejemplo básico de cubo
│   ├── FileBasedExample.cs  # Ejemplo con archivos JSON
│   ├── IExample.cs          # Interfaz para ejemplos
│   └── SistemaSolarExample.cs # Ejemplo de sistema solar
├── Graphics/                # Sistema de gráficos
│   ├── DemoWindow.cs        # Ventana principal de demostración
│   ├── GeometryGenerator.cs # Generador de geometrías
│   └── ShaderManager.cs     # Gestor de shaders
├── Models/                  # Modelos de datos
│   ├── Objeto.cs            # Clase principal de objeto 3D
│   ├── Parte.cs             # Partes de objetos
│   ├── Poligono.cs          # Polígonos 3D
│   └── Vertice.cs           # Vértices 3D
├── Serialization/           # Serialización
│   └── SceneLoader.cs       # Cargador de escenas
├── ejemplos/                # Archivos JSON de ejemplo
│   ├── cubos.json           # Escena de cubos
│   └── sistema_solar.json   # Escena del sistema solar
└── Program.cs               # Punto de entrada principal
```

## 🛠️ Requisitos

- **.NET 8.0** o superior
- **OpenTK4** (incluido en las dependencias)
- **Visual Studio 2022** o **VS Code** (recomendado)

## 🚀 Instalación y Uso

### 1. Clonar el repositorio
```bash
git clone https://github.com/quantworld95/sistema-graficos-3d.git
cd sistema-graficos-3d
```

### 2. Restaurar dependencias
```bash
dotnet restore
```

### 3. Compilar el proyecto
```bash
dotnet build
```

### 4. Ejecutar
```bash
dotnet run --project modelov2
```

## 📖 Ejemplos Incluidos

### Cubo Básico
```csharp
var cuboExample = new CuboExample();
cuboExample.Ejecutar();
```

### Sistema Solar
```csharp
var sistemaSolar = new SistemaSolarExample();
sistemaSolar.Ejecutar();
```

### Brazo Mecánico
```csharp
var brazoMecanico = new BrazoMecanicoExample();
brazoMecanico.Ejecutar();
```

### Carga desde Archivo
```csharp
var fileExample = new FileBasedExample("ejemplos/sistema_solar.json");
fileExample.Ejecutar();
```

## 🎮 Controles

- **Mouse**: Rotación de la cámara
- **Teclado**: Navegación y controles específicos por ejemplo
- **ESC**: Salir de la aplicación

## 📝 Formato de Archivos JSON

El sistema soporta carga de escenas desde archivos JSON con la siguiente estructura:

```json
{
  "objetos": [
    {
      "nombre": "Cubo",
      "partes": [
        {
          "geometria": {
            "vertices": [...],
            "indices": [...]
          },
          "color": {
            "r": 1.0,
            "g": 0.0,
            "b": 0.0
          }
        }
      ],
      "animacion": {
        "rotacion": {
          "eje": [0, 1, 0],
          "velocidad": 1.0
        }
      }
    }
  ]
}
```

## 🔧 Desarrollo

### Agregar Nuevos Ejemplos
1. Implementar la interfaz `IExample`
2. Agregar la lógica de renderizado
3. Registrar en el sistema principal

### Crear Nuevas Geometrías
1. Extender `GeometryGenerator`
2. Implementar métodos de generación
3. Agregar a los ejemplos existentes

## 📚 Documentación

- [OpenTK4 Documentation](https://opentk.net/)
- [.NET 8.0 Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [OpenGL Reference](https://www.opengl.org/sdk/docs/)

## 🤝 Contribuciones

Las contribuciones son bienvenidas. Por favor:

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## 📄 Licencia

Este proyecto está bajo la Licencia MIT. Ver el archivo `LICENSE` para más detalles.

## 👨‍💻 Autor

**quantworld95**
- GitHub: [@quantworld95](https://github.com/quantworld95)

## 🙏 Agradecimientos

- OpenTK Team por la excelente librería de gráficos
- Microsoft por .NET y C#
- Comunidad de desarrolladores de gráficos 3D

---

⭐ ¡No olvides darle una estrella al proyecto si te resulta útil!
