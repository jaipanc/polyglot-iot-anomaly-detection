namespace SensorApi
{
    public record SensorReading(string MachineId, double Temperature, DateTime Datetime);
    public record AnomalyResult(bool Anomaly);
}
