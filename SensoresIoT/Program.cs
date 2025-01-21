namespace SensoresIoT
{
    // Clase base abstracta Sensor
    public abstract class Sensor
    {
        // Propiedad común
        public string ID { get; private set; }

        // Constructor con validación
        public Sensor(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("El ID del sensor no puede estar vacío o ser nulo.");
            }

            ID = id;
        }

        // Método abstracto para procesar datos
        public abstract void ProcesarDatos();
    }

    // Clase derivada SensorTemperatura
    public class SensorTemperatura : Sensor
    {
        public SensorTemperatura(string id) : base(id) { }

        // Implementación del método abstracto
        public override void ProcesarDatos()
        {
            Console.WriteLine($"Procesando datos del sensor de temperatura con ID {ID}");
        }
    }

    // Clase derivada SensorHumedad
    public class SensorHumedad : Sensor
    {
        public SensorHumedad(string id) : base(id) { }

        // Implementación del método abstracto
        public override void ProcesarDatos()
        {
            Console.WriteLine($"Procesando datos del sensor de humedad con ID {ID}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Crear instancias de sensores
            Sensor sensorTemp = new SensorTemperatura("T-1001");
            Sensor sensorHum = new SensorHumedad("H-2002");

            // Procesar datos de cada sensor
            sensorTemp.ProcesarDatos();
            sensorHum.ProcesarDatos();
        }
    }
}
