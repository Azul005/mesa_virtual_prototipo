using System;
using System.IO;

namespace MesaPartesDigital
{
    /// <summary>
    /// =========================================================================================
    /// MÓDULO ESTÁTICO: ModuloMesaPartes
    /// RÚBRICA: FUNDAMENTOS DE PROGRAMACIÓN - LEY N.° 31170 (MESA DE PARTES DIGITAL)
    /// =========================================================================================
    /// Arquitectura: Programación Estructurada y Modular (Sin Programación Orientada a Objetos).
    /// PROHIBIDO: Clases de datos (Expediente), List<T>, Dictionary, LINQ, Array.Sort.
    /// PERMITIDO Y EXIGIDO:
    ///   - Arreglos Paralelos Unidimensionales (1D).
    ///   - Arreglo Bidimensional (2D) para estadísticas de Áreas vs Estados.
    ///   - Algoritmos manuales: Búsqueda Secuencial O(n), Búsqueda Binaria O(log n), Bubble Sort O(n²).
    ///   - Paso de parámetros por valor y por referencia (ref / out).
    ///   - Persistencia en archivo de texto (.txt) mediante StreamReader / StreamWriter.
    /// =========================================================================================
    /// </summary>
    public static class ModuloMesaPartes
    {
        // --------------------------------------------------------------------------------------
        // CONSTANTES Y CATÁLOGOS DEL SISTEMA
        // --------------------------------------------------------------------------------------
        public const int CAPACIDAD_INICIAL = 20;

        // Catálogos oficiales para las dimensiones de la matriz 2D
        public static readonly string[] AREAS_VALIDAS = new string[]
        {
            "Alcaldía",
            "Rentas",
            "Obras",
            "Logística"
        };

        public static readonly string[] ESTADOS_VALIDOS = new string[]
        {
            "Registrado",
            "En Trámite",
            "Observado"
        };

        // Ruta del archivo de texto plano para persistencia
        public static readonly string RUTA_ARCHIVO = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "expedientes.txt");

        // --------------------------------------------------------------------------------------
        // 2. ESTRUCTURAS DE DATOS: ARREGLOS PARALELOS UNIDIMENSIONALES (1D)
        // Cada índice [i] representa los datos completos de un mismo trámite documentario.
        // --------------------------------------------------------------------------------------
        public static string[] codigos = new string[CAPACIDAD_INICIAL];      // Ej: EXP-0001
        public static string[] dnis = new string[CAPACIDAD_INICIAL];         // Ej: 71234567 (8 dígitos)
        public static string[] ciudadanos = new string[CAPACIDAD_INICIAL];   // Nombres y Apellidos
        public static string[] asuntos = new string[CAPACIDAD_INICIAL];      // Descripción del trámite
        public static string[] areas = new string[CAPACIDAD_INICIAL];        // Alcaldía, Rentas, Obras, Logística
        public static string[] estados = new string[CAPACIDAD_INICIAL];      // Registrado, En Trámite, Observado
        public static string[] fechas = new string[CAPACIDAD_INICIAL];       // Formato dd/MM/yyyy

        // Contador de expedientes registrados actualmente en memoria
        public static int cantidadRegistros = 0;

        // --------------------------------------------------------------------------------------
        // 2. ESTRUCTURA DE DATOS: ARREGLO BIDIMENSIONAL (2D)
        // Filas = Áreas (4), Columnas = Estados (3)
        // --------------------------------------------------------------------------------------
        public static int[,] matrizEstadisticas = new int[AREAS_VALIDAS.Length, ESTADOS_VALIDOS.Length];

        // Indicador si los arreglos se encuentran ordenados ascendentemente por código
        public static bool estaOrdenadoAscendente = false;

        // --------------------------------------------------------------------------------------
        // INICIALIZACIÓN Y REDIMENSIONAMIENTO MANUAL DE ARREGLOS
        // --------------------------------------------------------------------------------------

        /// <summary>
        /// Inicializa o reinicia las estructuras de datos en memoria.
        /// </summary>
        public static void InicializarEstructuras(int capacidad = CAPACIDAD_INICIAL)
        {
            codigos = new string[capacidad];
            dnis = new string[capacidad];
            ciudadanos = new string[capacidad];
            asuntos = new string[capacidad];
            areas = new string[capacidad];
            estados = new string[capacidad];
            fechas = new string[capacidad];
            cantidadRegistros = 0;
            estaOrdenadoAscendente = false;

            ReiniciarMatrizEstadisticas();
        }

        /// <summary>
        /// Redimensiona manualmente todos los arreglos paralelos duplicando su capacidad.
        /// Se realiza una copia elemento a elemento mediante un bucle for tradicional (sin LINQ ni Array.Resize).
        /// </summary>
        public static void RedimensionarArreglos()
        {
            int nuevaCapacidad = codigos.Length * 2;

            // Arreglos temporales con la nueva capacidad
            string[] nuevosCodigos = new string[nuevaCapacidad];
            string[] nuevosDnis = new string[nuevaCapacidad];
            string[] nuevosCiudadanos = new string[nuevaCapacidad];
            string[] nuevosAsuntos = new string[nuevaCapacidad];
            string[] nuevasAreas = new string[nuevaCapacidad];
            string[] nuevosEstados = new string[nuevaCapacidad];
            string[] nuevasFechas = new string[nuevaCapacidad];

            // Copia manual de los datos existentes
            for (int i = 0; i < cantidadRegistros; i++)
            {
                nuevosCodigos[i] = codigos[i];
                nuevosDnis[i] = dnis[i];
                nuevosCiudadanos[i] = ciudadanos[i];
                nuevosAsuntos[i] = asuntos[i];
                nuevasAreas[i] = areas[i];
                nuevosEstados[i] = estados[i];
                nuevasFechas[i] = fechas[i];
            }

            // Asignación de las nuevas referencias
            codigos = nuevosCodigos;
            dnis = nuevosDnis;
            ciudadanos = nuevosCiudadanos;
            asuntos = nuevosAsuntos;
            areas = nuevasAreas;
            estados = nuevosEstados;
            fechas = nuevasFechas;
        }

        // --------------------------------------------------------------------------------------
        // 3. FUNCIONES DE VALIDACIÓN Y MANIPULACIÓN DE CADENAS (string)
        // Emplean métodos nativos: Trim, ToUpper, Substring, Split, Contains, Length.
        // --------------------------------------------------------------------------------------

        /// <summary>
        /// Valida que el DNI contenga exactamente 8 caracteres numéricos.
        /// Demuestra paso de parámetro por valor (dni) y por referencia mediante 'out' (mensajeError).
        /// </summary>
        public static bool ValidarDni(string dni, out string mensajeError)
        {
            mensajeError = string.Empty;

            if (string.IsNullOrEmpty(dni))
            {
                mensajeError = "El número de DNI es obligatorio.";
                return false;
            }

            string dniLimpio = dni.Trim();

            if (dniLimpio.Length != 8)
            {
                mensajeError = $"El DNI debe tener exactamente 8 dígitos numéricos (actualmente tiene {dniLimpio.Length}).";
                return false;
            }

            // Validación manual de dígitos numéricos carácter por carácter
            for (int i = 0; i < dniLimpio.Length; i++)
            {
                char c = dniLimpio[i];
                if (c < '0' || c > '9')
                {
                    mensajeError = $"El DNI solo puede contener caracteres numéricos del 0 al 9 (carácter inválido: '{c}').";
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Valida todos los campos de entrada antes de proceder con el registro.
        /// </summary>
        public static bool ValidarCamposRegistro(
            string dni,
            string ciudadano,
            string asunto,
            string area,
            string estado,
            out string mensajeError)
        {
            mensajeError = string.Empty;

            // Validación de DNI
            if (!ValidarDni(dni, out mensajeError))
            {
                return false;
            }

            // Validación de Nombre del Ciudadano
            if (string.IsNullOrEmpty(ciudadano) || ciudadano.Trim().Length < 3)
            {
                mensajeError = "El nombre del ciudadano debe tener al menos 3 caracteres.";
                return false;
            }

            // Validación del Asunto
            if (string.IsNullOrEmpty(asunto) || asunto.Trim().Length < 5)
            {
                mensajeError = "El asunto o descripción del trámite debe tener al menos 5 caracteres.";
                return false;
            }

            // Validación del Área seleccionada
            if (ObtenerIndiceArea(area) == -1)
            {
                mensajeError = "Debe seleccionar un Área de destino válida del catálogo.";
                return false;
            }

            // Validación del Estado seleccionado
            if (ObtenerIndiceEstado(estado) == -1)
            {
                mensajeError = "Debe seleccionar un Estado de trámite válido del catálogo.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Autogenera el siguiente código correlativo de expediente (ej. EXP-0001).
        /// Utiliza Substring e int.TryParse sobre los códigos existentes para encontrar el correlativo más alto.
        /// </summary>
        public static string GenerarSiguienteCodigo()
        {
            int maximoNumero = 0;

            for (int i = 0; i < cantidadRegistros; i++)
            {
                string cod = codigos[i];
                if (!string.IsNullOrEmpty(cod) && cod.StartsWith("EXP-") && cod.Length >= 8)
                {
                    // Extraer los caracteres numéricos a partir del índice 4
                    string parteNumerica = cod.Substring(4);
                    if (int.TryParse(parteNumerica, out int num))
                    {
                        if (num > maximoNumero)
                        {
                            maximoNumero = num;
                        }
                    }
                }
            }

            int siguienteNumero = maximoNumero + 1;
            // Formatear a 4 dígitos correlativos: EXP-0001, EXP-0002...
            return $"EXP-{siguienteNumero.ToString("D4")}";
        }

        // --------------------------------------------------------------------------------------
        // 2. MATRIZ BIDIMENSIONAL (2D): GESTIÓN Y CÁLCULO ESTADÍSTICO
        // Filas = Áreas (4), Columnas = Estados (3)
        // --------------------------------------------------------------------------------------

        /// <summary>
        /// Obtiene la fila correspondiente a un Área dentro de la matriz 2D.
        /// Devuelve -1 si no se encuentra en el catálogo.
        /// </summary>
        public static int ObtenerIndiceArea(string areaBuscada)
        {
            if (string.IsNullOrEmpty(areaBuscada)) return -1;

            string buscada = areaBuscada.Trim().ToUpper();
            for (int i = 0; i < AREAS_VALIDAS.Length; i++)
            {
                if (AREAS_VALIDAS[i].ToUpper() == buscada)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Obtiene la columna correspondiente a un Estado dentro de la matriz 2D.
        /// Devuelve -1 si no se encuentra en el catálogo.
        /// </summary>
        public static int ObtenerIndiceEstado(string estadoBuscado)
        {
            if (string.IsNullOrEmpty(estadoBuscado)) return -1;

            string buscado = estadoBuscado.Trim().ToUpper();
            for (int i = 0; i < ESTADOS_VALIDOS.Length; i++)
            {
                if (ESTADOS_VALIDOS[i].ToUpper() == buscado)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Reinicia todos los contadores de la matriz 2D a cero mediante bucles for anidados.
        /// </summary>
        public static void ReiniciarMatrizEstadisticas()
        {
            for (int f = 0; f < AREAS_VALIDAS.Length; f++)
            {
                for (int c = 0; c < ESTADOS_VALIDOS.Length; c++)
                {
                    matrizEstadisticas[f, c] = 0;
                }
            }
        }

        /// <summary>
        /// Actualiza la matriz bidimensional recorriendo los arreglos paralelos y totalizando
        /// los trámites existentes por cada combinación de [Área, Estado] con bucles anidados.
        /// </summary>
        public static void ActualizarMatrizEstadisticas()
        {
            ReiniciarMatrizEstadisticas();

            // Recorrer los expedientes registrados y acumular en la matriz 2D
            for (int i = 0; i < cantidadRegistros; i++)
            {
                int filaArea = ObtenerIndiceArea(areas[i]);
                int colEstado = ObtenerIndiceEstado(estados[i]);

                if (filaArea >= 0 && filaArea < AREAS_VALIDAS.Length &&
                    colEstado >= 0 && colEstado < ESTADOS_VALIDOS.Length)
                {
                    matrizEstadisticas[filaArea, colEstado]++;
                }
            }
        }

        /// <summary>
        /// Calcula el total de expedientes de una fila específica (Área) sumando sus columnas.
        /// </summary>
        public static int CalcularTotalPorArea(int filaArea)
        {
            if (filaArea < 0 || filaArea >= AREAS_VALIDAS.Length) return 0;

            int total = 0;
            for (int c = 0; c < ESTADOS_VALIDOS.Length; c++)
            {
                total += matrizEstadisticas[filaArea, c];
            }
            return total;
        }

        /// <summary>
        /// Calcula el total de expedientes de una columna específica (Estado) sumando sus filas.
        /// </summary>
        public static int CalcularTotalPorEstado(int colEstado)
        {
            if (colEstado < 0 || colEstado >= ESTADOS_VALIDOS.Length) return 0;

            int total = 0;
            for (int f = 0; f < AREAS_VALIDAS.Length; f++)
            {
                total += matrizEstadisticas[f, colEstado];
            }
            return total;
        }

        /// <summary>
        /// Calcula el gran total de expedientes sumando toda la matriz 2D.
        /// </summary>
        public static int CalcularTotalGeneralMatriz()
        {
            int granTotal = 0;
            for (int f = 0; f < AREAS_VALIDAS.Length; f++)
            {
                for (int c = 0; c < ESTADOS_VALIDOS.Length; c++)
                {
                    granTotal += matrizEstadisticas[f, c];
                }
            }
            return granTotal;
        }

        // --------------------------------------------------------------------------------------
        // 3. INSERCIÓN DE NUEVO TRÁMITE
        // --------------------------------------------------------------------------------------

        /// <summary>
        /// Agrega un nuevo expediente a los arreglos paralelos, actualiza la matriz 2D y persiste en .txt.
        /// Demuestra paso de parámetros por valor y por referencia con 'out'.
        /// </summary>
        public static bool RegistrarNuevoTramite(
            string dni,
            string ciudadano,
            string asunto,
            string area,
            string estado,
            string fecha,
            out string codigoGenerado,
            out string mensajeResultado)
        {
            codigoGenerado = string.Empty;
            mensajeResultado = string.Empty;

            // Validación estricta previa
            if (!ValidarCamposRegistro(dni, ciudadano, asunto, area, estado, out string errorValidacion))
            {
                mensajeResultado = errorValidacion;
                return false;
            }

            // Redimensionar si se alcanzó el límite de los arreglos
            if (cantidadRegistros >= codigos.Length)
            {
                RedimensionarArreglos();
            }

            // Generar código único correlativo
            codigoGenerado = GenerarSiguienteCodigo();

            // Insertar datos en la posición 'cantidadRegistros' de todos los arreglos paralelos
            codigos[cantidadRegistros] = codigoGenerado;
            dnis[cantidadRegistros] = dni.Trim();
            ciudadanos[cantidadRegistros] = ciudadano.Trim().ToUpper();
            asuntos[cantidadRegistros] = asunto.Trim().ToUpper();
            areas[cantidadRegistros] = area.Trim();
            estados[cantidadRegistros] = estado.Trim();
            fechas[cantidadRegistros] = string.IsNullOrEmpty(fecha) ? DateTime.Now.ToString("dd/MM/yyyy") : fecha.Trim();

            // Incrementar contador de registros
            cantidadRegistros++;

            // Los nuevos registros al final pueden alterar el orden previo
            estaOrdenadoAscendente = false;

            // Actualizar la matriz bidimensional con el nuevo registro
            ActualizarMatrizEstadisticas();

            // Guardar inmediatamente en el archivo de texto
            bool guardadoExitoso = GuardarEnArchivo(out string errorArchivo);
            if (!guardadoExitoso)
            {
                mensajeResultado = $"Trámite registrado en memoria pero ocurrió un error al guardar en archivo: {errorArchivo}";
                return true; // Se registró en memoria
            }

            mensajeResultado = $"¡Expediente {codigoGenerado} registrado y guardado exitosamente!";
            return true;
        }

        // --------------------------------------------------------------------------------------
        // 3. ORDENAMIENTO MANUAL: BUBBLE SORT (BURBUJA) CON INTERCAMBIO EN ARREGLOS PARALELOS
        // --------------------------------------------------------------------------------------

        /// <summary>
        /// Función auxiliar que intercambia dos referencias de texto empleando 'ref'.
        /// Demuestra el paso de parámetros por referencia obligatorio en la rúbrica.
        /// </summary>
        private static void IntercambiarTexto(ref string textoA, ref string textoB)
        {
            string temporal = textoA;
            textoA = textoB;
            textoB = temporal;
        }

        /// <summary>
        /// Intercambia las posiciones i y j en TODOS los 7 arreglos paralelos simultáneamente.
        /// Esto garantiza que los datos de un mismo expediente nunca se desfasen ni se corrompan.
        /// </summary>
        public static void Intercambiar(int i, int j)
        {
            IntercambiarTexto(ref codigos[i], ref codigos[j]);
            IntercambiarTexto(ref dnis[i], ref dnis[j]);
            IntercambiarTexto(ref ciudadanos[i], ref ciudadanos[j]);
            IntercambiarTexto(ref asuntos[i], ref asuntos[j]);
            IntercambiarTexto(ref areas[i], ref areas[j]);
            IntercambiarTexto(ref estados[i], ref estados[j]);
            IntercambiarTexto(ref fechas[i], ref fechas[j]);
        }

        /// <summary>
        /// Ordena manualmente los expedientes por Código utilizando el algoritmo Bubble Sort (Burbuja).
        /// Complejidad temporal: O(n²).
        /// Permite ordenamiento ascendente (A-Z) o descendente (Z-A).
        /// Recibe contadores por referencia (ref) para medir comparaciones e intercambios.
        /// </summary>
        public static void OrdenarBurbujaPorCodigo(bool ascendente, ref int totalComparaciones, ref int totalIntercambios)
        {
            totalComparaciones = 0;
            totalIntercambios = 0;

            if (cantidadRegistros <= 1) return;

            for (int i = 0; i < cantidadRegistros - 1; i++)
            {
                bool huboIntercambioEnPasada = false;

                for (int j = 0; j < cantidadRegistros - 1 - i; j++)
                {
                    totalComparaciones++;

                    // Comparación lexicográfica entre códigos adyacentes
                    int comparacion = string.Compare(codigos[j], codigos[j + 1], StringComparison.OrdinalIgnoreCase);

                    bool debeIntercambiar = ascendente ? (comparacion > 0) : (comparacion < 0);

                    if (debeIntercambiar)
                    {
                        Intercambiar(j, j + 1);
                        totalIntercambios++;
                        huboIntercambioEnPasada = true;
                    }
                }

                // Optimización: si en una pasada no hubo intercambios, ya está completamente ordenado
                if (!huboIntercambioEnPasada)
                {
                    break;
                }
            }

            estaOrdenadoAscendente = ascendente;
        }

        /// <summary>
        /// Verifica si los arreglos paralelos ya se encuentran ordenados ascendentemente por código.
        /// </summary>
        public static bool VerificarEstaOrdenadoAscendente()
        {
            for (int i = 0; i < cantidadRegistros - 1; i++)
            {
                if (string.Compare(codigos[i], codigos[i + 1], StringComparison.OrdinalIgnoreCase) > 0)
                {
                    return false;
                }
            }
            return true;
        }

        // --------------------------------------------------------------------------------------
        // 3. ALGORITMOS DE BÚSQUEDA MANUAL (PRINCIPIOS DE INGENIERÍA)
        // --------------------------------------------------------------------------------------

        /// <summary>
        /// Búsqueda Secuencial (Lineal) por Código o por DNI.
        /// Complejidad temporal en el peor caso: O(n).
        /// Recorre elemento a elemento desde el índice 0 hasta cantidadRegistros - 1.
        /// Devuelve el índice encontrado (o -1 si no existe) y el número de pasos realizados.
        /// </summary>
        public static int BuscarSecuencial(string criterio, out int pasosRealizados)
        {
            pasosRealizados = 0;

            if (string.IsNullOrEmpty(criterio) || cantidadRegistros == 0)
            {
                return -1;
            }

            string criterioLimpio = criterio.Trim().ToUpper();

            for (int i = 0; i < cantidadRegistros; i++)
            {
                pasosRealizados++;

                // Búsqueda flexible: coincide si es igual al código o igual al DNI
                if (codigos[i].ToUpper() == criterioLimpio || dnis[i].Trim() == criterioLimpio)
                {
                    return i; // Índice del expediente encontrado
                }
            }

            return -1; // No encontrado
        }

        /// <summary>
        /// Búsqueda Binaria por Código de Expediente.
        /// Complejidad temporal en el peor caso: O(log n).
        /// Requisito de la rúbrica: Primero asegura el ordenamiento del arreglo y luego busca dividiendo
        /// el rango a la mitad, devolviendo el índice encontrado, pasos realizados y si fue necesario ordenar.
        /// </summary>
        public static int BuscarBinariaPorCodigo(
            string codigoBuscado,
            out int pasosRealizados,
            out bool seReordeno)
        {
            pasosRealizados = 0;
            seReordeno = false;

            if (string.IsNullOrEmpty(codigoBuscado) || cantidadRegistros == 0)
            {
                return -1;
            }

            string codigoLimpio = codigoBuscado.Trim().ToUpper();

            // Requisito: Garantizar que esté ordenado ascendentemente antes de aplicar búsqueda binaria
            if (!VerificarEstaOrdenadoAscendente())
            {
                int compDummy = 0;
                int swapDummy = 0;
                OrdenarBurbujaPorCodigo(true, ref compDummy, ref swapDummy);
                seReordeno = true;
            }

            int inicio = 0;
            int fin = cantidadRegistros - 1;

            while (inicio <= fin)
            {
                pasosRealizados++;

                // Cálculo seguro del punto medio para evitar desbordamiento aritmético
                int medio = inicio + (fin - inicio) / 2;

                int comparacion = string.Compare(codigos[medio], codigoLimpio, StringComparison.OrdinalIgnoreCase);

                if (comparacion == 0)
                {
                    return medio; // Elemento localizado en el punto medio
                }

                if (comparacion < 0)
                {
                    // El valor en el medio es menor al buscado -> descartar mitad izquierda
                    inicio = medio + 1;
                }
                else
                {
                    // El valor en el medio es mayor al buscado -> descartar mitad derecha
                    fin = medio - 1;
                }
            }

            return -1; // No encontrado
        }

        // --------------------------------------------------------------------------------------
        // 3. PERSISTENCIA EN ARCHIVO DE TEXTO (expedientes.txt)
        // Formato de línea: CODIGO|DNI|CIUDADANO|ASUNTO|AREA|ESTADO|FECHA
        // --------------------------------------------------------------------------------------

        /// <summary>
        /// Guarda todos los datos de los arreglos paralelos en expedientes.txt utilizando StreamWriter.
        /// Manejo robusto de errores mediante bloque try-catch.
        /// </summary>
        public static bool GuardarEnArchivo(out string mensajeError)
        {
            mensajeError = string.Empty;

            try
            {
                using (StreamWriter sw = new StreamWriter(RUTA_ARCHIVO, false, System.Text.Encoding.UTF8))
                {
                    for (int i = 0; i < cantidadRegistros; i++)
                    {
                        // Concatenación de los campos paralelos separados por '|'
                        string linea = $"{codigos[i]}|{dnis[i]}|{ciudadanos[i]}|{asuntos[i]}|{areas[i]}|{estados[i]}|{fechas[i]}";
                        sw.WriteLine(linea);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                mensajeError = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Carga los datos desde expedientes.txt hacia los arreglos paralelos utilizando StreamReader.
        /// Descompone cada línea usando Split('|') y redimensiona si es necesario.
        /// </summary>
        public static bool CargarDesdeArchivo(out string mensajeResultado)
        {
            mensajeResultado = string.Empty;

            if (!File.Exists(RUTA_ARCHIVO))
            {
                mensajeResultado = "El archivo expedientes.txt aún no existe. Se creará al registrar el primer trámite.";
                return false;
            }

            try
            {
                // Reiniciar estructuras en memoria
                InicializarEstructuras(CAPACIDAD_INICIAL);

                int lineasLeidas = 0;

                using (StreamReader sr = new StreamReader(RUTA_ARCHIVO, System.Text.Encoding.UTF8))
                {
                    string? linea;
                    while ((linea = sr.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(linea)) continue;

                        // Descomposición mediante método nativo Split
                        string[] partes = linea.Split('|');

                        if (partes.Length >= 7)
                        {
                            if (cantidadRegistros >= codigos.Length)
                            {
                                RedimensionarArreglos();
                            }

                            codigos[cantidadRegistros] = partes[0].Trim();
                            dnis[cantidadRegistros] = partes[1].Trim();
                            ciudadanos[cantidadRegistros] = partes[2].Trim();
                            asuntos[cantidadRegistros] = partes[3].Trim();
                            areas[cantidadRegistros] = partes[4].Trim();
                            estados[cantidadRegistros] = partes[5].Trim();
                            fechas[cantidadRegistros] = partes[6].Trim();

                            cantidadRegistros++;
                            lineasLeidas++;
                        }
                    }
                }

                // Actualizar matriz 2D con los registros leídos
                ActualizarMatrizEstadisticas();
                estaOrdenadoAscendente = VerificarEstaOrdenadoAscendente();

                mensajeResultado = $"Se cargaron con éxito {lineasLeidas} expedientes desde el archivo de texto.";
                return true;
            }
            catch (Exception ex)
            {
                mensajeResultado = $"Error al leer el archivo: {ex.Message}";
                return false;
            }
        }

        // --------------------------------------------------------------------------------------
        // CARGA DE DATOS DE PRUEBA REALES (8 EXPEDIENTES MUNICIPALES LEY 31170)
        // --------------------------------------------------------------------------------------

        /// <summary>
        /// Inserta automáticamente 8 trámites reales y variados de gestión pública local,
        /// cumpliendo con el requisito de demostración de la rúbrica.
        /// </summary>
        public static int CargarDatosDePruebaReales(out string mensaje)
        {
            // Catálogo de datos representativos (Gobierno Local / Municipalidad)
            string[] testDnis = new string[]
            {
                "71234567",
                "45892134",
                "10456789",
                "72901234",
                "09341256",
                "73456781",
                "42189065",
                "76543210"
            };

            string[] testCiudadanos = new string[]
            {
                "CARLOS EDUARDO MENDOZA QUISPE",
                "MARIA ELENA ROJAS BENITES",
                "JORGE ANTONIO ALVAREZ SOTELO",
                "PATRICIA GIOVANNA HUAMAN VEGA",
                "ROBERTO DANIEL PAREDES RUIZ",
                "ANA CECILIA FLORES MAMANI",
                "VICTOR HUGO ESPINOZA CHAVEZ",
                "LUCIA BEATRIZ CONDORI QUINTANA"
            };

            string[] testAsuntos = new string[]
            {
                "SOLICITUD DE LICENCIA DE FUNCIONAMIENTO DEFINITIVA PARA BODEGA",
                "AUTORIZACIÓN DE CONSTRUCCIÓN Y AMPLIACIÓN DE VIVIENDA FAMILIAR",
                "RECLAMO POR COBRO INDEBIDO EN DETERMINACIÓN DE ARBITRIOS 2026",
                "SOLICITUD DE AUDIENCIA PÚBLICA VECINAL CON EL SEÑOR ALCALDE",
                "PROPUESTA DE COTIZACIÓN PARA PROVISIÓN DE MATERIALES DE LIMPIEZA",
                "CERTIFICADO DE NUMERACIÓN Y JURISDICCIÓN DE PREDIO URBANO",
                "MANTENIMIENTO PREVENTIVO DE ALUMBRADO Y ÁREAS VERDES EN PARQUE",
                "ACTA DE CONFORMIDAD DE BIENES DE OFICINA - ORDEN DE SERVICIO 512"
            };

            string[] testAreas = new string[]
            {
                "Rentas",
                "Obras",
                "Rentas",
                "Alcaldía",
                "Logística",
                "Obras",
                "Alcaldía",
                "Logística"
            };

            string[] testEstados = new string[]
            {
                "En Trámite",
                "Registrado",
                "Observado",
                "Registrado",
                "En Trámite",
                "Observado",
                "En Trámite",
                "Registrado"
            };

            string[] testFechas = new string[]
            {
                "10/10/2026",
                "11/10/2026",
                "12/10/2026",
                "13/10/2026",
                "13/10/2026",
                "14/10/2026",
                "14/10/2026",
                "15/10/2026"
            };

            int cargados = 0;

            for (int i = 0; i < testDnis.Length; i++)
            {
                // Asegurar espacio en arreglos
                if (cantidadRegistros >= codigos.Length)
                {
                    RedimensionarArreglos();
                }

                string nuevoCod = GenerarSiguienteCodigo();

                codigos[cantidadRegistros] = nuevoCod;
                dnis[cantidadRegistros] = testDnis[i];
                ciudadanos[cantidadRegistros] = testCiudadanos[i];
                asuntos[cantidadRegistros] = testAsuntos[i];
                areas[cantidadRegistros] = testAreas[i];
                estados[cantidadRegistros] = testEstados[i];
                fechas[cantidadRegistros] = testFechas[i];

                cantidadRegistros++;
                cargados++;
            }

            estaOrdenadoAscendente = VerificarEstaOrdenadoAscendente();
            ActualizarMatrizEstadisticas();
            GuardarEnArchivo(out _);

            mensaje = $"Se insertaron exitosamente {cargados} trámites de prueba en los arreglos paralelos y se guardaron en 'expedientes.txt'.";
            return cargados;
        }
    }
}
