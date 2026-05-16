using System;
using System.Collections.Generic;

namespace NeuroHealth
{
    internal class Program
    {
        /*
         * ============================================================
         * PROYECTO: NeuroHealth - Sistema de Triaje de Emergencias
         * ============================================================
         * Integrantes:
         * -
         * -
         * -
         * -
         *
         * Explicación general del programa:
         * TODO: explicar brevemente qué hace el sistema.
         *
         * Organización de datos:
         * TODO: explicar cómo organizaron pacientes, cola de espera,
         * pacientes admitidos y observaciones.
         *
         * Justificación de estructuras:
         * TODO: explicar por qué usaron List<T>, Queue<T> y Stack<T>.
         *
         * Algoritmo de triaje:
         * TODO: explicar cómo se asignan los niveles Verde, Amarillo y Rojo.
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




            // TODO: cargar algunas observaciones.
            // Esta función es opcional, pero recomendada para probar el sistema.
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
        }

        static NivelUrgencia ClasificarTriaje(SignosVitales signos)
        {
            // TODO: aplicar reglas de triaje.
            // Rojo: Saturación < 90, Pulso > 120, Temperatura >= 39, Dolor >= 9.
            // Amarillo: si no es rojo y cumple reglas intermedias.
            // Verde: si no cumple condiciones anteriores.

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
        }

        static void MostrarObservaciones()
        {
            // TODO: pedir DNI del paciente.
            // TODO: permitir -1 para volver.
            // TODO: mostrar observaciones desde la más reciente a la más antigua.
        }

        #endregion

        #region LISTADOS Y FILTROS

        static void ListarPacientesAdmitidos()
        {
            // TODO: mostrar DNI, nombre, edad, motivo y nivel de urgencia.
        }

        static void MostrarDatosPaciente(Paciente paciente)
        {
            // TODO: mostrar los datos de un paciente de manera clara.
        }

        static void FiltrarPorUrgencia()
        {
            // TODO: pedir nivel de urgencia.
            // TODO: permitir -1 para volver.
            // TODO: mostrar pacientes admitidos que coincidan con el nivel seleccionado.
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
        }

        #endregion

        #region FUNCIONES DE LECTURA Y VALIDACIÓN

        static int LeerEntero(string mensaje)
        {
            // TODO: implementar lectura segura de enteros con TryParse.
            Console.Write(mensaje);
            return int.Parse(Console.ReadLine());
        }

        static long LeerLong(string mensaje)
        {
            // TODO: implementar lectura segura de long con TryParse.
            Console.Write(mensaje);
            return long.Parse(Console.ReadLine());
        }

        static double LeerDouble(string mensaje)
        {
            // TODO: implementar lectura segura de double con TryParse.
            Console.Write(mensaje);
            return double.Parse(Console.ReadLine());
        }

        static string LeerTextoObligatorio(string mensaje)
        {
            // TODO: impedir que el texto quede vacío.
            Console.Write(mensaje);
            return Console.ReadLine();
        }

        static int LeerEnteroEnRango(string mensaje, int minimo, int maximo)
        {
            // TODO: validar que el valor esté entre mínimo y máximo.
            return LeerEntero(mensaje);
        }

        static double LeerDoubleEnRango(string mensaje, double minimo, double maximo)
        {
            // TODO: validar que el valor esté entre mínimo y máximo.
            return LeerDouble(mensaje);
        }

        static long LeerDniOCancelar(string mensaje)
        {
            // TODO: permitir DNI positivo o -1 para volver.
            return LeerLong(mensaje);
        }

        static int LeerEnteroEnRangoOCancelar(string mensaje, int minimo, int maximo)
        {
            // TODO: permitir un valor entre mínimo y máximo o -1 para volver.
            return LeerEntero(mensaje);
        }

        static MotivoConsulta LeerMotivoConsulta()
        {
            // TODO: mostrar menú de motivos de consulta.
            // TODO: validar opción entre 1 y 8.
            return MotivoConsulta.ControlGeneral;
        }

        static NivelUrgencia LeerNivelUrgencia()
        {
            // TODO: mostrar niveles Verde, Amarillo, Rojo y opción -1 para volver.
            return NivelUrgencia.SinEvaluar;
        }

        #endregion
    }
}
