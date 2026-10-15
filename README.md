# Sistema de Mesa de Partes Digital (Ley N.º 31170)
## Prototipo Académico para el Curso de Fundamentos de Programación

Este proyecto implementa una solución completa y funcional en **C# con Windows Forms (.NET)** para una **Mesa de Partes Digital** orientada a la gestión pública y gobiernos locales, en estricto cumplimiento del marco legal de la **Ley N.º 31170** (Ley que dispone la digitalización y modernización del Estado y la implementación de la Mesa de Partes Digital).

El software ha sido diseñado con un enfoque 100% didáctico y académico, cumpliendo de forma rigurosa con las restricciones de la rúbrica de **Programación Estructurada y Modular (Sin POO)**.

---

### 1. Cumplimiento de Restricciones Arquitectónicas
* **PROHIBICIÓN DE POO PARA MODELAR DATOS:** No existe una clase o `struct` `Expediente`. Toda la información se mantiene en memoria a través de **Arreglos Paralelos Unidimensionales (1D)** indexados por la posición `i`.
* **PROHIBICIÓN DE COLECCIONES DINÁMICAS Y LINQ:** No se utiliza `List<T>`, `Dictionary`, `ArrayList`, `Queue`, `Stack`, ni operadores LINQ (`OrderBy`, `Where`, `Array.Sort`, `Array.Resize`). Todos los algoritmos de ordenamiento, búsqueda, inserción, redimensionamiento y actualización son manuales e iterativos.
* **ESTRUCTURA DE ARCHIVOS SIMPLE Y DESACOPLADA:**
  - [`Program.cs`](file:///C:/Users/DAVID/Desktop/0.1.0-Alfa/Program.cs): Punto de entrada `Main` y configuración gráfica de la aplicación.
  - [`FrmPrincipal.cs`](file:///C:/Users/DAVID/Desktop/0.1.0-Alfa/FrmPrincipal.cs): Interfaz gráfica construida 100% por código en Windows Forms (sin dependencia de diseñador visual ni archivos `.Designer.cs` / `.resx`).
  - [`ModuloMesaPartes.cs`](file:///C:/Users/DAVID/Desktop/0.1.0-Alfa/ModuloMesaPartes.cs): Módulo estático que contiene los arreglos paralelos, la matriz bidimensional y todas las subrutinas lógicas y algorítmicas reutilizables.
  - [`MesaPartesDigital.csproj`](file:///C:/Users/DAVID/Desktop/0.1.0-Alfa/MesaPartesDigital.csproj): Archivo de configuración del proyecto .NET.

---

### 2. Estructuras de Datos en Memoria

#### Arreglos Paralelos Unidimensionales (1D)
En [`ModuloMesaPartes.cs`](file:///C:/Users/DAVID/Desktop/0.1.0-Alfa/ModuloMesaPartes.cs), un expediente está determinado por el mismo índice `i` a lo largo de 7 arreglos:
```csharp
public static string[] codigos;      // Ej. "EXP-0001"
public static string[] dnis;         // 8 dígitos numéricos
public static string[] ciudadanos;   // Nombres y Apellidos
public static string[] asuntos;      // Descripción del petitorio
public static string[] areas;        // Alcaldía, Rentas, Obras, Logística
public static string[] estados;      // Registrado, En Trámite, Observado
public static string[] fechas;       // Formato dd/MM/yyyy
public static int cantidadRegistros; // Cantidad actual de elementos
```
* **Redimensionamiento Manual:** Si `cantidadRegistros == codigos.Length`, se invoca `RedimensionarArreglos()`, la cual duplica la capacidad creando nuevos arreglos y copiando elemento a elemento con un bucle `for` tradicional.

#### Arreglo Bidimensional (2D)
* `public static int[,] matrizEstadisticas = new int[4, 3];`
* Representa: **Filas = Áreas de Destino (4)** y **Columnas = Estados del Trámite (3)**.
* Se recalcula mediante bucles anidados en `ActualizarMatrizEstadisticas()`, acumulando las ocurrencias para proveer métricas en tiempo real en la pestaña de registro.

---

### 3. Algoritmos y Principios de Ingeniería

| Algoritmo | Función en Código | Complejidad Teórica | Descripción y Demostración en Sustentación |
| :--- | :--- | :--- | :--- |
| **Búsqueda Secuencial (Lineal)** | `BuscarSecuencial()` | $\mathcal{O}(n)$ | Recorre linealmente los arreglos desde el índice `0` hasta `cantidadRegistros - 1`. Permite buscar por **Código** o por **DNI**. Retorna el índice y la cantidad exacta de comparaciones realizadas. |
| **Búsqueda Binaria** | `BuscarBinariaPorCodigo()` | $\mathcal{O}(\log n)$ | Divide el espacio de búsqueda a la mitad en cada iteración. **Garantía algorítmica:** Verifica previamente si el arreglo está ordenado; si no lo está, ejecuta Bubble Sort automáticamente antes de buscar. |
| **Ordenamiento Burbuja (Bubble Sort)** | `OrdenarBurbujaPorCodigo()` | $\mathcal{O}(n^2)$ | Permite ordenar ascendente (A-Z) o descendente (Z-A). **Intercambio Sincronizado:** Cada intercambio invoca `Intercambiar(j, j + 1)`, el cual intercambia los valores en **los 7 arreglos paralelos simultáneamente** mediante paso por referencia (`ref`) para evitar desfases de datos. |

---

### 4. Persistencia en Archivo de Texto (`expedientes.txt`)
* **Guardado:** La función `GuardarEnArchivo()` recorre los arreglos con un bucle `for` y escribe línea por línea utilizando `StreamWriter` y codificación UTF-8. Cada línea utiliza el separador delimitador `|`:
  ```text
  EXP-0001|71234567|CARLOS EDUARDO MENDOZA QUISPE|SOLICITUD DE LICENCIA DE FUNCIONAMIENTO|RENTAS|EN TRÁMITE|10/10/2026
  ```
* **Carga:** La función `CargarDesdeArchivo()` utiliza `StreamReader`, descarta líneas vacías, utiliza el método nativo `Split('|')` y puebla los arreglos paralelos, actualizando la matriz 2D.
* **Manejo de Errores:** Bloques `try-catch` para capturar cualquier excepción de I/O.

---

### 5. Interfaz Gráfica (Windows Forms)
Construida en [`FrmPrincipal.cs`](file:///C:/Users/DAVID/Desktop/0.1.0-Alfa/FrmPrincipal.cs) organizada en un `TabControl` de dos pestañas:

1. **Apartado 1: "1. Registro de Expedientes"**
   - Formulario de captura con validación de DNI (8 dígitos numéricos), nombre, asunto y combos de catálogo.
   - Código correlativo autogenerado (`EXP-0001`, `EXP-0002`...) y fecha automática del sistema.
   - Botón **"Registrar Expediente"**: Valida entradas, almacena en arreglos paralelos, persiste en `expedientes.txt` y notifica al usuario.
   - Botón **"Limpiar Campos"**: Restablece el formulario.
   - Botón **"Cargar 8 Datos de Prueba Reales"**: Carga 8 expedientes típicos de gestión municipal (licencias de obra, arbitrios, audiencias vecinales, adquisiciones logísticas) para demostración inmediata.
   - **Matriz 2D en Vivo:** Grid interactivo que muestra las cantidades cruzadas entre Áreas y Estados, con sumatorias por área y fila de Total General calculadas por bucles.

2. **Apartado 2: "2. Búsqueda y Listado"**
   - Barra de búsqueda por Código o DNI.
   - Botón **"Buscar (Secuencial)"**: Muestra pasos ejecutados bajo complejidad $\mathcal{O}(n)$.
   - Botón **"Buscar (Binaria)"**: Muestra iteraciones bajo complejidad $\mathcal{O}(\log n)$ y si requirió pre-ordenamiento.
   - Botones de ordenamiento manual **"▲ Ordenar A-Z"** y **"▼ Ordenar Z-A"**.
   - **Panel de Diagnóstico Algorítmico:** Detalla pasos, comparaciones y complejidad para la sustentación.
   - **DataGridView General:** Cargado manualmente desde los 7 arreglos paralelos con un bucle `for`. Resalta con color distintivo la fila del expediente encontrado.

---

### 6. Instrucciones para Compilar y Ejecutar

#### Desde la Terminal (PowerShell / CMD)
```powershell
# Ubicarse en el directorio del proyecto
cd "c:\Users\usuario\Desktop\0.1.0-Alfa"

# Compilar el proyecto
dotnet build

# Ejecutar el aplicativo
dotnet run
```

#### Desde Visual Studio o Visual Studio Code
1. Abrir la carpeta del proyecto en Visual Studio / VS Code.
2. Seleccionar el archivo `MesaPartesDigital.csproj`.
3. Presionar **F5** o el botón **Iniciar / Depurar**.
