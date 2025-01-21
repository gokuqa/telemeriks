namespace SensorAPI.Models
{
    public class Sensor
    {
        public int Id { get; set; }
        public string DispositivoID { get; set; }
        public DateTime FechaHora { get; set; }
        public double Valor { get; set; }
    }
}
