using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace NeuroHealth
{
    internal class Program
    {
        /*
         * ============================================================
         * PROYECTO: NeuroHealth - Sistema de Triaje de Emergencias
         * ============================================================
         * Integrantes:
         * - Franco Perrone Rey
         * - Francisco Aguilar
         * - Demaria Leonel
         * 
         *
         * Explicación general del programa:
         * TODO: explicar brevemente qué hace el sistema.
         * 
         * El sistema es un breve gestor de pacientes, el cual clasifica el estado de estos segun sintomas
         * registra observaciones medicas, identifica a los mismos, pondera estos segun criterio y tiene un 
         * breve analisis estadistico
         * 
         *
         * Organización de datos:
         * TODO: explicar cómo organizaron pacientes, cola de espera,
         * pacientes admitidos y observaciones.
         * 
         * 
         * 
         *
         * Justificación de estructuras:
         * TODO: explicar por qué usaron List<T>, Queue<T> y Stack<T>.
         *
         * Algoritmo de triaje:
         * TODO: explicar cómo se asignan los niveles Verde, Amarillo y Rojo.
         * 
         * Los niveles estaan previamente documentados en la consigna y en resumen son los siguientes:
         * Reglas de Triaje
         *   El sistema clasificará automáticamente a cada paciente según:
         *   Nivel Rojo (Crítico)
         *   • Saturación < 90
         *   • Pulso > 120
         *   • Temperatura ≥ 39.0
         *   • Dolor ≥ 9
         *    Nivel Amarillo
         *   (Solo si no fue Rojo)
         *   • Saturación 90–94
         *   • Pulso 100–120
         *   • Temperatura 38–38.9
         *   • Dolor 6–8
         *    Nivel Verde
         *   Si no cumple condiciones anteriores.
         *
         * Recursividad:
         * TODO: explicar qué función recursiva implementaron.
         *
         * Búsquedas:
         * TODO: explicar la búsqueda lineal y la búsqueda binaria recursiva.
         */

        #region TIPOS DEL SISTEMA

        // Los motivos de consulta son valores cerrados definidos por la consigna.
        enum MotivoConsulta
        {
            DolorToracico = 1,
            DificultadRespiratoria = 2,
            Fiebre = 3,
            DolorAbdominal = 4,
            Traumatismo = 5,
            PerdidaConocimiento = 6,
            Cefalea = 7,
            ControlGeneral = 8
        }

        // Los niveles de urgencia son valores cerrados definidos por la consigna.
        enum NivelUrgencia
        {
            SinEvaluar = 0,
            Verde = 1,
            Amarillo = 2,
            Rojo = 3
        }

        // Sugerencia de modelado: agrupar signos vitales.
        // El grupo puede adaptar este modelo si lo justifica correctamente.
        struct SignosVitales
        {
            public int Pulso;
            public double Temperatura;
            public string Presion;
            public int Saturacion;
            public int Dolor;
        }

        // Sugerencia de modelado: representar al paciente como un registro de datos.
        // El grupo puede modificar esta representación si lo justifica correctamente.s
        record Paciente(
            long Dni,
            string NombreApellido,
            int Edad,
            MotivoConsulta Motivo,
            SignosVitales Signos,
            DateTime FechaIngreso,
            NivelUrgencia Nivel
        );

        // Sugerencia de modelado: observación asociada a un DNI.
        struct Observacion
        {
            public long DniPaciente;
            public string Texto;
            public DateTime Fecha;
        }

        #endregion

        #region ESTRUCTURAS PRINCIPALES

        // TODO: declarar las estructuras principales del sistema.
        // Sugerencias según la consigna:
        // - Cola de espera: Queue<Paciente>
        // - Lista de pacientes admitidos: List<Paciente>
        // - Pila de observaciones: Stack<Observacion>

        // Ejemplo de declaración posible:
        static Queue<Paciente> colaEspera = new Queue<Paciente>();
        static List<Paciente> pacientesAdmitidos = new List<Paciente>();
        static Stack<Observacion> observaciones = new Stack<Observacion>();

        #endregion

        #region PROGRAMA PRINCIPAL

        static void Main(string[] args)
        {
            // TODO: inicializar estructuras si corresponde.
            // TODO: cargar casos de prueba si el grupo decide incluirlos.
            CargarCasosDePrueba();

            bool salir = false;

            while (!salir)
            {
                MostrarMenu();
               
                int opcion = LeerEntero("Seleccione una opción: ");

                switch (opcion)
                {
                    case 1:
                        RegistrarPaciente();
                        break;
                    case 2:
                        MostrarColaEspera();
                        break;
                    case 3:
                        EvaluarPaciente();
                        break;
                    case 4:
                        RegistrarObservacion();
                        break;
                    case 5:
                        MostrarObservaciones();
                        break;
                    case 6:
                        BuscarPacientePorDni();
                        break;
                    case 7:
                        CalcularPuntajeRiesgo();
                        break;
                    case 8:
                        ListarPacientesAdmitidos();
                        break;
                    case 9:
                        FiltrarPorUrgencia();
                        break;
                    case 10:
                        MostrarEstadisticas();
                        break;
                    case 0:
                        salir = true;
                        Console.WriteLine("Gracias por usar NeuroHealth.");
                        break;
                    default:
                        Console.WriteLine("Opción inválida.");
                        break;
                }

                if (!salir)
                {
                    Console.WriteLine("\nPresione una tecla para continuar...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        #endregion

        #region MENÚ

        static void MostrarMenu()
        {
            Console.WriteLine("=======================================");
            Console.WriteLine("     NEUROHEALTH - SISTEMA DE TRIAJE   ");
            Console.WriteLine("=======================================");
            Console.WriteLine("1. Registrar paciente");
            Console.WriteLine("2. Mostrar cola de espera");
            Console.WriteLine("3. Evaluar paciente (triaje automático)");
            Console.WriteLine("4. Registrar observación médica");
            Console.WriteLine("5. Mostrar observaciones de un paciente");
            Console.WriteLine("6. Buscar paciente por DNI");
            Console.WriteLine("7. Calcular puntaje de riesgo recursivo");
            Console.WriteLine("8. Listar pacientes admitidos");
            Console.WriteLine("9. Filtrar pacientes por nivel de urgencia");
            Console.WriteLine("10. Mostrar estadísticas generales");
            Console.WriteLine("0. Salir");
            Console.WriteLine("=======================================");
        }

        #endregion

        #region CARGA DE DATOS DE PRUEBA

        static void CargarCasosDePrueba()
        {
            // TODO: cargar algunos pacientes en la cola de espera.

            // Ejemplo de signos vitales
            SignosVitales sv1 = new SignosVitales { Pulso = 80, Temperatura = 36.7, Presion = "120/80", Saturacion = 98, Dolor = 2 };
            SignosVitales sv2 = new SignosVitales { Pulso = 95, Temperatura = 38.2, Presion = "130/85", Saturacion = 96, Dolor = 5 };

            // Creación de pacientes
            Paciente paciente1 = new Paciente(
                12345678,
                "Juan Pérez",
                45,
                MotivoConsulta.DolorAbdominal,
                sv1,
                DateTime.Now,
                NivelUrgencia.SinEvaluar
            );

            Paciente paciente2 = new Paciente(
                87654321,
                "María Gómez",
                30,
                MotivoConsulta.Fiebre,
                sv2,
                DateTime.Now,
                NivelUrgencia.SinEvaluar
            );

            // Encolar pacientes
            colaEspera.Enqueue(paciente1);
            colaEspera.Enqueue(paciente2);



            // TODO: cargar algunos pacientes admitidos.

            // Pacientes admitidos de prueba (sin Random)
            SignosVitales svA = new SignosVitales { Pulso = 120, Temperatura = 39.0, Presion = "140/90", Saturacion = 90, Dolor = 8 };
            SignosVitales svB = new SignosVitales { Pulso = 75, Temperatura = 36.5, Presion = "118/78", Saturacion = 99, Dolor = 1 };
            SignosVitales svC = new SignosVitales { Pulso = 95, Temperatura = 37.8, Presion = "125/80", Saturacion = 95, Dolor = 4 };

            // Crear pacientes con SinEvaluar
            Paciente pacienteA = new Paciente(
                33445566,
                "Pedro Ramírez",
                60,
                MotivoConsulta.Fiebre,
                svA,
                DateTime.Now.AddMinutes(-50),
                NivelUrgencia.SinEvaluar
            );

            Paciente pacienteB = new Paciente(
                77889900,
                "Lucía Fernández",
                25,
                MotivoConsulta.DificultadRespiratoria,
                svB,
                DateTime.Now.AddMinutes(-30),
                NivelUrgencia.SinEvaluar
            );

            Paciente pacienteC = new Paciente(
                99112233,
                "Roberto Díaz",
                40,
                MotivoConsulta.PerdidaConocimiento,
                svC,
                DateTime.Now.AddMinutes(-20),
                NivelUrgencia.SinEvaluar
            );

            // Clasificar con tu función de triaje
            pacienteA = pacienteA with { Nivel = ClasificarTriaje(svA) };
            pacienteB = pacienteB with { Nivel = ClasificarTriaje(svB) };
            pacienteC = pacienteC with { Nivel = ClasificarTriaje(svC) };

            // Agregar a la lista de admitidos
            pacientesAdmitidos.Add(pacienteA);
            pacientesAdmitidos.Add(pacienteB);
            pacientesAdmitidos.Add(pacienteC);

            Console.WriteLine("Pacientes admitidos de prueba cargados correctamente.");



            // TODO: cargar algunas observaciones.
            // Esta función es opcional, pero recomendada para probar el sistema.

            observaciones.Push(new Observacion
            {
                DniPaciente = 33445566,
                Texto = "Paciente con dificultad respiratoria, se indica oxígeno suplementario.",
                Fecha = DateTime.Now.AddMinutes(-40)
            });

            observaciones.Push(new Observacion
            {
                DniPaciente = 77889900,
                Texto = "Paciente en control rutinario, signos vitales normales.",
                Fecha = DateTime.Now.AddMinutes(-30)
            });

            observaciones.Push(new Observacion
            {
                DniPaciente = 99112233,
                Texto = "Paciente con fiebre persistente, se solicita análisis de laboratorio.",
                Fecha = DateTime.Now.AddMinutes(-20)
            });

        }

        #endregion

        #region REGISTRO DE PACIENTES

        static void RegistrarPaciente()
        {
            // TODO: pedir DNI.
            // TODO: validar que sea positivo y no esté repetido.
            // TODO: pedir apellido y nombre.
            // TODO: pedir edad.
            // TODO: pedir motivo de consulta.
            // TODO: pedir signos vitales.
            // TODO: crear el paciente con NivelUrgencia.SinEvaluar.
            // TODO: agregarlo a la cola de espera.

            // 1. Pedir DNI
            Console.Write("Ingrese DNI: ");
            long dni = long.Parse(Console.ReadLine());

            // 2. Validar que sea positivo y no esté repetido
            if (dni <= 0)
            {
                Console.WriteLine("El DNI debe ser positivo.");
                return;
            }
            if (ExisteDniEnSistema(dni))
            {
                Console.WriteLine("El DNI ya existe en el sistema.");
                return;
            }

            // 3. Pedir apellido y nombre
            Console.Write("Ingrese Apellido y Nombre: ");
            string nombreApellido = Console.ReadLine();

            // 4. Pedir edad
            Console.Write("Ingrese Edad: ");
            int edad = int.Parse(Console.ReadLine());

            // 5. Pedir motivo de consulta (ejemplo simple)
            Console.Write("Ingrese Motivo de consulta (DolorAbdominal/Fiebre/etc): ");
            MotivoConsulta motivo = Enum.Parse<MotivoConsulta>(Console.ReadLine());

            // 6. Pedir signos vitales
            Console.Write("Pulso: ");
            int pulso = int.Parse(Console.ReadLine());
            Console.Write("Temperatura: ");
            double temperatura = double.Parse(Console.ReadLine());
            Console.Write("Presión: ");
            string presion = Console.ReadLine();
            Console.Write("Saturación: ");
            int saturacion = int.Parse(Console.ReadLine());
            Console.Write("Dolor (0-10): ");
            int dolor = int.Parse(Console.ReadLine());

            SignosVitales signos = new SignosVitales
            {
                Pulso = pulso,
                Temperatura = temperatura,
                Presion = presion,
                Saturacion = saturacion,
                Dolor = dolor
            };

            // 7. Crear el paciente con NivelUrgencia.SinEvaluar
            Paciente nuevo = new Paciente(
                dni,
                nombreApellido,
                edad,
                motivo,
                signos,
                DateTime.Now,
                NivelUrgencia.SinEvaluar
            );

            // 8. Agregarlo a la cola de espera
            colaEspera.Enqueue(nuevo);

            Console.WriteLine($"Paciente {nombreApellido} agregado a la cola de espera.");


        }

        static bool ExisteDniEnSistema(long dni)
        {
            // TODO: verificar si el DNI existe en la cola o en la lista de admitidos
            if (colaEspera.Any(p => p.Dni == dni)) { return true; }
            if (pacientesAdmitidos.Any(p => p.Dni == dni)) { return true; }
            return false;
        }

        #endregion

        #region COLA DE ESPERA Y TRIAJE

        static void MostrarColaEspera()
        {
            // TODO: mostrar los pacientes que están esperando evaluación.
            // Debe respetar el orden de llegada.
            Console.WriteLine("Pacientes en cola de espera:");

            foreach (var paciente in colaEspera)
            {
                Console.WriteLine($"DNI: {paciente.Dni}, Nombre: {paciente.NombreApellido}, Edad: {paciente.Edad}, Motivo: {paciente.Motivo}, Nivel: {paciente.Nivel}");
            }

        }

        static void EvaluarPaciente()
        {
            // TODO: verificar si hay pacientes en espera.
            // TODO: quitar el primer paciente de la cola.
            // TODO: clasificarlo con las reglas de triaje.
            // TODO: agregarlo a la lista de pacientes admitidos.

            // 1. Verificamos si hay paciente en espera
            if (colaEspera.Count == 0)
            {
                Console.WriteLine("No hay pacientes en la cola de espera.");
                return;
            }

            // 2. Quitar el primer paciente de la cola
            Paciente paciente = colaEspera.Dequeue();

            // 3. Clasificarlo con las reglas de triaje
            NivelUrgencia nivel = ClasificarTriaje(paciente.Signos);

            // Crear una nueva instancia con el nivel asignado
            Paciente pacienteClasificado = paciente with { Nivel = nivel };

            // 4. Agregarlo a la lista de pacientes admitidos
            pacientesAdmitidos.Add(pacienteClasificado);

            Console.WriteLine($"Paciente {pacienteClasificado.NombreApellido} admitido con nivel {pacienteClasificado.Nivel}.");

        }

        static NivelUrgencia ClasificarTriaje(SignosVitales signos)
        {
            // TODO: aplicar reglas de triaje.
            // Rojo: Saturación < 90, Pulso > 120, Temperatura >= 39, Dolor >= 9.
            // Amarillo: si no es rojo y cumple reglas intermedias.
            // Verde: si no cumple condiciones anteriores.

            // Reglas Reglas de Triaje
            //  El sistema clasificará automáticamente a cada paciente según:
            //  Nivel Rojo(Crítico)
            //• Saturación < 90
            //• Pulso > 120
            //• Temperatura ≥ 39.0
            //• Dolor ≥ 9
            // Nivel Amarillo
            // (Solo si no fue Rojo)
            //• Saturación 90–94
            //• Pulso 100–120
            //• Temperatura 38–38.9
            //• Dolor 6–8
            // Nivel Verde
            //Si no cumple condiciones anteriores.

            // Nivel Rojo (estricto: todas las condiciones críticas deben cumplirse)
            if (signos.Saturacion < 90 &&
                signos.Pulso > 120 &&
                signos.Temperatura >= 39.0 &&
                signos.Dolor >= 9)
            {
                return NivelUrgencia.Rojo;
            }

            // Nivel Amarillo (todas las condiciones intermedias deben cumplirse)
            if ((signos.Saturacion >= 90 && signos.Saturacion <= 94) &&
                (signos.Pulso >= 100 && signos.Pulso <= 120) &&
                (signos.Temperatura >= 38.0 && signos.Temperatura <= 38.9) &&
                (signos.Dolor >= 6 && signos.Dolor <= 8))
            {
                return NivelUrgencia.Amarillo;
            }

            // Nivel Verde (si no cumple las anteriores)
            return NivelUrgencia.Verde;


            return NivelUrgencia.SinEvaluar;
        }

        #endregion

        #region OBSERVACIONES MÉDICAS

        static void RegistrarObservacion()
        {
            // TODO: pedir DNI del paciente admitido.
            // TODO: permitir -1 para volver.
            // TODO: validar que el paciente exista en admitidos.
            // TODO: pedir texto de observación.
            // TODO: agregar observación a la pila.

            long dni = LeerDniOCancelar("Ingrese DNI: ");
       
            // Validar que el paciente exista en admitidos
            Paciente paciente = pacientesAdmitidos.FirstOrDefault(p => p.Dni == dni);
            if (paciente == null)
            {
                Console.WriteLine("No se encontró un paciente admitido con ese DNI.");
                return;
            }

            // Pedir texto de observación - utilizamos la funcion creada para obligatoriedad.
            string texto = LeerTextoObligatorio("Ingrese observacion: ");

            // Crear observación y agregarla a la pila
            Observacion obs = new Observacion
            {
                DniPaciente = dni,
                Texto = texto,
                Fecha = DateTime.Now
            };

            observaciones.Push(obs);

            Console.WriteLine("Observación registrada correctamente.");
        }

        static void MostrarObservaciones()
        {
            // TODO: pedir DNI del paciente.
            // TODO: permitir -1 para volver.
            // TODO: mostrar observaciones desde la más reciente a la más antigua.


            long dni = LeerDniOCancelar("Ingrese dni: ");

            // Validar que el paciente exista en admitidos
            Paciente paciente = pacientesAdmitidos.FirstOrDefault(p => p.Dni == dni);
            if (paciente == null)
            {
                Console.WriteLine("No se encontró un paciente admitido con ese DNI.");
                return;
            }

            // Mostrar observaciones asociadas al paciente
            Console.WriteLine($"Observaciones para {paciente.NombreApellido}:");

            foreach (var obs in observaciones)
            {
                if (obs.DniPaciente == dni)
                {
                    Console.WriteLine($"[{obs.Fecha}] {obs.Texto}");
                }
            }
        }

        #endregion

        #region LISTADOS Y FILTROS

        static void ListarPacientesAdmitidos()
        {
            // TODO: mostrar DNI, nombre, edad, motivo y nivel de urgencia.

            if (pacientesAdmitidos.Count == 0)
            {
                Console.WriteLine("No hay pacientes admitidos.");
                return;
            }

            Console.WriteLine("Listado de pacientes admitidos:");
            Console.WriteLine("---------------------------------------------------");

            foreach (var paciente in pacientesAdmitidos)
            {
                // utilizamos metodo definido para mostrar pacientes
                MostrarDatosPaciente(paciente);
            }

        }

        static void MostrarDatosPaciente(Paciente paciente)
        {
            // TODO: mostrar los datos de un paciente de manera clara.
            Console.WriteLine("=======================================");
            Console.WriteLine($"DNI: {paciente.Dni}");
            Console.WriteLine($"Nombre: {paciente.NombreApellido}");
            Console.WriteLine($"Edad: {paciente.Edad}");
            Console.WriteLine($"Motivo de consulta: {paciente.Motivo}");
            Console.WriteLine($"Fecha de ingreso: {paciente.FechaIngreso}");
            Console.WriteLine($"Nivel de urgencia: {paciente.Nivel}");
            Console.WriteLine("---- Signos Vitales ----");
            Console.WriteLine($"Pulso: {paciente.Signos.Pulso}");
            Console.WriteLine($"Temperatura: {paciente.Signos.Temperatura}");
            Console.WriteLine($"Presión: {paciente.Signos.Presion}");
            Console.WriteLine($"Saturación: {paciente.Signos.Saturacion}");
            Console.WriteLine($"Dolor: {paciente.Signos.Dolor}");
            Console.WriteLine("=======================================");


        }

        static void FiltrarPorUrgencia()
        {
            // TODO: pedir nivel de urgencia.
            // TODO: permitir -1 para volver.
            // TODO: mostrar pacientes admitidos que coincidan con el nivel seleccionado.

            // utilizamos funciones pre definidas - en este caso de lectura de nivelurgencia           
            NivelUrgencia nivelSeleccionado = LeerNivelUrgencia();

            // Filtrar pacientes admitidos
            var filtrados = pacientesAdmitidos.Where(p => p.Nivel == nivelSeleccionado).ToList();

            if (filtrados.Count == 0)
            {
                Console.WriteLine($"No hay pacientes admitidos con nivel {nivelSeleccionado}.");
                return;
            }

            Console.WriteLine($"Pacientes admitidos con nivel {nivelSeleccionado}:");
            Console.WriteLine("---------------------------------------------------");

            foreach (var paciente in filtrados)
            {
                MostrarDatosPaciente(paciente);
            }

        }

        #endregion

        #region BÚSQUEDAS

        static void BuscarPacientePorDni()
        {
            // TODO: pedir DNI a buscar.
            // TODO: permitir -1 para volver.
            // TODO: buscar en pacientes admitidos con búsqueda lineal.
            // TODO: ordenar una copia por DNI.
            // TODO: buscar con búsqueda binaria recursiva.
            // TODO: mostrar cantidad de pasos de cada búsqueda.
        }

        static int BuscarLineal(long dniBuscado, ref int pasos)
        {
            // TODO: implementar búsqueda lineal en la lista de pacientes admitidos.
            return -1;
        }

        static int BuscarBinariaRecursiva(List<Paciente> listaOrdenada, long dniBuscado, int inicio, int fin, ref int pasos)
        {
            // TODO: implementar búsqueda binaria recursiva.
            return -1;
        }

        static List<Paciente> CopiarListaPacientes()
        {
            // TODO: copiar manualmente la lista de pacientes admitidos.
            return new List<Paciente>();
        }

        static void OrdenarPacientesPorDni(List<Paciente> lista)
        {
            // TODO: ordenar por DNI.
            // Puede utilizarse un algoritmo simple visto en clase.
        }

        #endregion

        #region RECURSIVIDAD

        static void CalcularPuntajeRiesgo()
        {
            // TODO: cargar un arreglo de 4 puntajes entre 0 y 10.
            // Posiciones sugeridas:
            // 0 = temperatura
            // 1 = pulso
            // 2 = saturación
            // 3 = dolor
            // TODO: llamar a la función recursiva.
            // TODO: mostrar puntaje total e interpretación.
        }

        static int SumarPuntajesRecursivo(int[] puntajes, int indice)
        {
            // TODO: implementar suma recursiva del arreglo.
            return 0;
        }

        #endregion

        #region ESTADÍSTICAS

        static void MostrarEstadisticas()
        {
            // TODO: mostrar cantidad de pacientes en espera.
            // TODO: mostrar cantidad de pacientes admitidos.
            // TODO: mostrar cantidad por nivel de urgencia.
            // TODO: calcular edad promedio.
            // TODO: calcular porcentaje de pacientes críticos.
         
                Console.WriteLine("=== Estadísticas del sistema ===");

                // Cantidad de pacientes en espera
                Console.WriteLine($"Pacientes en espera: {colaEspera.Count}");

                // Cantidad de pacientes admitidos
                Console.WriteLine($"Pacientes admitidos: {pacientesAdmitidos.Count}");

                // Cantidad por nivel de urgencia
                int verdes = pacientesAdmitidos.Count(p => p.Nivel == NivelUrgencia.Verde);
                int amarillos = pacientesAdmitidos.Count(p => p.Nivel == NivelUrgencia.Amarillo);
                int rojos = pacientesAdmitidos.Count(p => p.Nivel == NivelUrgencia.Rojo);

                Console.WriteLine($"Verde: {verdes}");
                Console.WriteLine($"Amarillo: {amarillos}");
                Console.WriteLine($"Rojo: {rojos}");

                // Edad promedio
                if (pacientesAdmitidos.Any())
                {
                    double edadPromedio = pacientesAdmitidos.Average(p => p.Edad);
                    Console.WriteLine($"Edad promedio: {edadPromedio:F1} años");
                }
                else
                {
                    Console.WriteLine("Edad promedio: N/A");
                }

                // Porcentaje de pacientes críticos
                if (pacientesAdmitidos.Any())
                {
                    double porcentajeCriticos = (double)rojos / pacientesAdmitidos.Count * 100;
                    Console.WriteLine($"Porcentaje críticos: {porcentajeCriticos:F1}%");
                }
                else
                {
                    Console.WriteLine("Porcentaje críticos: N/A");
                }

                Console.WriteLine("================================");
        }

            #endregion

        #region FUNCIONES DE LECTURA Y VALIDACIÓN


       static int LeerEntero(string mensaje)
            {
                // TODO: implementar lectura segura de enteros con TryParse.
                int valor;
                bool valido = false;

                do
                {
                    Console.Write(mensaje);
                    string entrada = Console.ReadLine();

                    if (int.TryParse(entrada, out valor))
                    {
                        valido = true; // conversión exitosa
                    }
                    else
                    {
                        Console.WriteLine("Entrada inválida. Ingrese un número entero válido.");
                    }

                } while (!valido);

                return valor;
            }

       static long LeerLong(string mensaje)
            {
                // TODO: implementar lectura segura de long con TryParse.
                long valor;
                bool valido = false;

                do
                {
                    Console.Write(mensaje);
                    string entrada = Console.ReadLine();

                    if (long.TryParse(entrada, out valor))
                    {
                        valido = true; // conversión exitosa
                    }
                    else
                    {
                        Console.WriteLine("Entrada inválida. Ingrese un número entero largo válido.");
                    }

                } while (!valido);

                return valor;
            }

       static double LeerDouble(string mensaje)
            {
                // TODO: implementar lectura segura de double con TryParse.
                double valor;
                bool valido = false;

                do
                {
                    Console.Write(mensaje);
                    string entrada = Console.ReadLine();

                    if (double.TryParse(entrada, out valor))
                    {
                        valido = true; // conversión exitosa
                    }
                    else
                    {
                        Console.WriteLine("Entrada inválida. Ingrese un número válido.");
                    }

                } while (!valido);

                return valor;
            }

       static string LeerTextoObligatorio(string mensaje)
            {
                // TODO: impedir que el texto quede vacío.
                string texto;
                do
                {
                    Console.Write(mensaje);
                    texto = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(texto))
                    {
                        Console.WriteLine("El texto no puede quedar vacío. Intente nuevamente.");
                    }

                } while (string.IsNullOrWhiteSpace(texto));

                return texto;
            }

       static int LeerEnteroEnRango(string mensaje, int minimo, int maximo)
            {
                // TODO: validar que el valor esté entre mínimo y máximo.
                int valor;
                bool valido = false;

                do
                {
                    valor = LeerEntero(mensaje); // usa tu método seguro con TryParse

                    if (valor < minimo || valor > maximo)
                    {
                        Console.WriteLine($"El valor debe estar entre {minimo} y {maximo}. Intente nuevamente.");
                    }
                    else
                    {
                        valido = true;
                    }

                } while (!valido);

                return valor;
            }

       static double LeerDoubleEnRango(string mensaje, double minimo, double maximo)
            {
                // TODO: validar que el valor esté entre mínimo y máximo.
                double valor;
                bool valido = false;

                do
                {
                    valor = LeerDouble(mensaje); // usa tu método seguro con TryParse

                    if (valor < minimo || valor > maximo)
                    {
                        Console.WriteLine($"El valor debe estar entre {minimo} y {maximo}. Intente nuevamente.");
                    }
                    else
                    {
                        valido = true;
                    }

                } while (!valido);

                return valor;
            }

       static long LeerDniOCancelar(string mensaje)
            {
                // TODO: permitir DNI positivo o -1 para volver.
                long valor;
                bool valido = false;

                do
                {
                    Console.Write(mensaje);
                    string entrada = Console.ReadLine();

                    if (long.TryParse(entrada, out valor))
                    {
                        if (valor > 0 || valor == -1)
                        {
                            valido = true; // aceptamos DNI positivo o -1
                        }
                        else
                        {
                            Console.WriteLine("El DNI debe ser positivo o -1 para volver.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Entrada inválida. Ingrese un número válido.");
                    }

                } while (!valido);

                return valor;
            }

       static int LeerEnteroEnRangoOCancelar(string mensaje, int minimo, int maximo)
            {
                // TODO: permitir un valor entre mínimo y máximo o -1 para volver.
                int valor;
                bool valido = false;

                do
                {
                    valor = LeerEntero(mensaje); // usa tu método seguro con TryParse

                    if ((valor >= minimo && valor <= maximo) || valor == -1)
                    {
                        valido = true; // aceptamos dentro del rango o -1
                    }
                    else
                    {
                        Console.WriteLine($"El valor debe estar entre {minimo} y {maximo}, o -1 para volver.");
                    }

                } while (!valido);

                return valor;
            }

       static MotivoConsulta LeerMotivoConsulta()
            {
                // TODO: mostrar menú de motivos de consulta.
                // TODO: validar opción entre 1 y 8.
                Console.WriteLine("Seleccione motivo de consulta:");
                Console.WriteLine("1. Dolor torácico");
                Console.WriteLine("2. Dificultad respiratoria");
                Console.WriteLine("3. Fiebre");
                Console.WriteLine("4. Dolor abdominal");
                Console.WriteLine("5. Traumatismo");
                Console.WriteLine("6. Pérdida de conocimiento");
                Console.WriteLine("7. Cefalea");
                Console.WriteLine("8. Control general");
                Console.WriteLine("(-1 para volver)");

                int opcion = LeerEnteroEnRangoOCancelar("Opción: ", 1, 8);

                if (opcion == -1)
                {

                    throw new OperationCanceledException("El usuario eligió volver.");
                }

                return (MotivoConsulta)opcion;
            }

       static NivelUrgencia LeerNivelUrgencia()
            {
                // TODO: mostrar niveles Verde, Amarillo, Rojo y opción -1 para volver.
                Console.WriteLine("Seleccione nivel de urgencia:");
                Console.WriteLine("1. Verde");
                Console.WriteLine("2. Amarillo");
                Console.WriteLine("3. Rojo");
                Console.WriteLine("(-1 para volver)");

                int opcion = LeerEnteroEnRangoOCancelar("Opción: ", 1, 3);

                if (opcion == -1)
                {
                    throw new OperationCanceledException("El usuario eligio volver");
                }

                return (NivelUrgencia)opcion;
            }

       #endregion
    }
}

