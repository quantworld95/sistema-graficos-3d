# Pipeline del Proyecto ModeloV2

## 🔄 Pipeline Completo del Sistema de Gráficos 3D

### 1. 🚀 INICIALIZACIÓN
```
Program.cs
    │
    ▼
┌─────────────────────────────────────┐
│        Main()                       │
│  ┌─────────────────────────────────┐│
│  │ 1. Crear IExample              ││
│  │ 2. Instanciar DemoWindow       ││
│  │ 3. Ejecutar gw.Run()           ││
│  └─────────────────────────────────┘│
└─────────────────────────────────────┘
```

### 2. 🏗️ CONFIGURACIÓN (OnLoad)
```
DemoWindow.OnLoad()
    │
    ▼
┌─────────────────────────────────────┐
│        Setup del Sistema            │
│  ┌─────────────────────────────────┐│
│  │ 1. GL.Enable(DepthTest)         ││
│  │ 2. Compilar Shaders             ││
│  │ 3. Configurar Cámara            ││
│  │ 4. example.Setup(objeto)        ││
│  └─────────────────────────────────┘│
└─────────────────────────────────────┘
    │
    ▼
┌─────────────────────────────────────┐
│        IExample.Setup()             │
│  ┌─────────────────────────────────┐│
│  │ 1. Crear geometrías             ││
│  │ 2. Calcular centros de masa     ││
│  │ 3. Subir a GPU                  ││
│  └─────────────────────────────────┘│
└─────────────────────────────────────┘
```

### 3. 🎮 BUCLE PRINCIPAL (Game Loop)
```
┌─────────────────────────────────────┐
│         DemoWindow.Run()            │
│                                     │
│  ┌─────────────────────────────────┐│
│  │     OnUpdateFrame()             ││
│  │  ┌─────────────────────────────┐││
│  │  │ 1. _t += args.Time          │││
│  │  │ 2. example.Update(objeto, t)│││
│  │  └─────────────────────────────┘││
│  └─────────────────────────────────┘│
│                                     │
│  ┌─────────────────────────────────┐│
│  │     OnRenderFrame()             ││
│  │  ┌─────────────────────────────┐││
│  │  │ 1. Limpiar buffers          │││
│  │  │ 2. Configurar shaders       │││
│  │  │ 3. Renderizar partes        │││
│  │  │ 4. SwapBuffers()            │││
│  │  └─────────────────────────────┘││
│  └─────────────────────────────────┘│
└─────────────────────────────────────┘
```

### 4. 🎨 PIPELINE DE RENDERIZADO

#### A. Preparación de Geometría:
```
┌─────────────────────────────────────┐
│        GeometryGenerator            │
│  ┌─────────────────────────────────┐│
│  │ 1. Crear vértices               ││
│  │ 2. Calcular normales            ││
│  │ 3. Generar índices              ││
│  │ 4. Triangulación                ││
│  └─────────────────────────────────┘│
└─────────────────────────────────────┘
    │
    ▼
┌─────────────────────────────────────┐
│        ShaderManager                │
│  ┌─────────────────────────────────┐│
│  │ 1. Crear VAO, VBO, EBO         ││
│  │ 2. Subir datos a GPU            ││
│  │ 3. Configurar atributos         ││
│  └─────────────────────────────────┘│
└─────────────────────────────────────┘
```

#### B. Pipeline de Transformaciones:
```
┌─────────────────────────────────────┐
│        Matrices de Transformación   │
│                                     │
│  Local (Parte) ──┐                  │
│                  │                  │
│  Global (Objeto) ─┼── Model Matrix  │
│                  │                  │
│  View (Cámara) ───┼── View Matrix   │
│                  │                  │
│  Projection ──────┘  Projection     │
│                        Matrix       │
└─────────────────────────────────────┘
    │
    ▼
┌─────────────────────────────────────┐
│        MVP = P * V * M              │
└─────────────────────────────────────┘
```

#### C. Pipeline de Shaders:
```
┌─────────────────────────────────────┐
│           Vertex Shader             │
│  ┌─────────────────────────────────┐│
│  │ 1. Aplicar MVP                  ││
│  │ 2. Transformar posiciones       ││
│  │ 3. Pasar normales               ││
│  └─────────────────────────────────┘│
└─────────────────────────────────────┘
    │
    ▼
┌─────────────────────────────────────┐
│        Rasterización                │
│  ┌─────────────────────────────────┐│
│  │ 1. Interpolación                ││
│  │ 2. Depth testing                ││
│  │ 3. Fragment generation          ││
│  └─────────────────────────────────┘│
└─────────────────────────────────────┘
    │
    ▼
┌─────────────────────────────────────┐
│          Fragment Shader            │
│  ┌─────────────────────────────────┐│
│  │ 1. Iluminación                  ││
│  │ 2. Aplicar colores              ││
│  │ 3. Output color                 ││
│  └─────────────────────────────────┘│
└─────────────────────────────────────┘
```

### 5. 🎬 PIPELINE DE ANIMACIÓN

#### A. Actualización de Tiempo:
```
┌─────────────────────────────────────┐
│        FrameEventArgs.Time          │
│                │                    │
│                ▼                    │
│  ┌─────────────────────────────────┐│
│  │        _t += time               ││
│  └─────────────────────────────────┘│
└─────────────────────────────────────┘
```

#### B. Aplicación de Animaciones:
```
┌─────────────────────────────────────┐
│        IExample.Update()            │
│  ┌─────────────────────────────────┐│
│  │ 1. Animación Global             ││
│  │    - Traslación del objeto      ││
│  │    - Rotación del objeto        ││
│  │                                 ││
│  │ 2. Animaciones Individuales     ││
│  │    - Rotación de partes         ││
│  │    - Órbitas                    ││
│  │    - Posicionamiento            ││
│  └─────────────────────────────────┘│
└─────────────────────────────────────┘
```

### 6. 🎨 PIPELINE DE COLORES

```
┌─────────────────────────────────────┐
│        IExample.SetColors()         │
│  ┌─────────────────────────────────┐│
│  │ 1. Obtener color de parte       ││
│  │ 2. Configurar uniform uBaseColor││
│  │ 3. Aplicar en fragment shader   ││
│  └─────────────────────────────────┘│
└─────────────────────────────────────┘
```

### 7. 📺 SALIDA (Output)
```
┌─────────────────────────────────────┐
│           Pantalla                  │
│  ┌─────────────────────────────────┐│
│  │ 1. Buffer de color              ││
│  │ 2. Buffer de profundidad        ││
│  │ 3. SwapBuffers()                ││
│  └─────────────────────────────────┘│
└─────────────────────────────────────┘
```

## 🔄 Flujo Completo por Frame

```
1. ⏰ Tiempo → 2. 🎬 Animación → 3. 🎨 Renderizado → 4. 📺 Pantalla
     │                    │                    │
     ▼                    ▼                    ▼
┌─────────┐         ┌─────────┐         ┌─────────┐
│ Update  │   →     │ Transform│   →    │ Render  │
│ Logic   │         │ Matrices │        │ Pipeline│
└─────────┘         └─────────┘         └─────────┘
```

## 📊 Características del Pipeline

- **🎯 60 FPS**: Bucle de renderizado continuo
- **⚡ GPU**: Procesamiento paralelo en shaders
- **🎯 Modular**: Separación clara de responsabilidades
- **📁 Extensible**: Fácil agregar nuevas geometrías/animaciones
- **💾 Persistente**: Carga desde archivos JSON

## 🏗️ Arquitectura del Sistema

### Componentes Principales:
1. **Program.cs**: Punto de entrada y menú de selección
2. **DemoWindow**: Ventana principal y bucle de renderizado
3. **IExample**: Interfaz para diferentes ejemplos
4. **GeometryGenerator**: Generación de geometrías 3D
5. **ShaderManager**: Manejo de shaders y GPU
6. **SceneLoader**: Carga de escenas desde archivos JSON
7. **FileBasedExample**: Ejemplo que carga desde archivos

### Flujo de Datos:
```
JSON/Code → SceneData → IExample → DemoWindow → GPU → Pantalla
```

## 🎮 Tipos de Ejemplos Soportados

1. **CuboExample**: Cubos con animación
2. **SistemaSolarExample**: Sistema solar con órbitas
3. **AutoExample**: Auto con ruedas giratorias
4. **BrazoMecanicoExample**: Brazo articulado
5. **FileBasedExample**: Carga desde archivos JSON

## 🎨 Tipos de Geometría

- **Cubo**: Parámetro `lado`
- **Rectángulo**: Parámetros `ancho`, `alto`, `profundidad`
- **Cilindro**: Parámetros `radio`, `altura`, `segmentos`
- **Esfera**: Parámetros `radio`, `segmentos`

## 🎬 Tipos de Animación

- **Rotación**: Sobre ejes X, Y, Z
- **Traslación**: Movimiento lineal
- **Órbita**: Alrededor de otra parte

## 📁 Estructura de Archivos

```
modelov2/
├── Examples/           # Ejemplos de código
├── ejemplos/          # Archivos JSON
├── Data/              # Clases de datos
├── Graphics/          # Sistema de renderizado
├── Models/            # Modelos de datos
└── Serialization/     # Sistema de carga/guardado
```

---

*Documento generado automáticamente - Pipeline del Proyecto ModeloV2*
