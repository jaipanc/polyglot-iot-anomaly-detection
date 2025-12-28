# Polyglot Industrial Sensor System 🏭

## 📚 Purpose

This is a **learning and revision project** focused on:

- Revisiting and strengthening **C# / .NET fundamentals**
- Designing async and concurrent systems using modern .NET features
- Gradually learning **Python for AI/ML use cases**
- Exploring common Python libraries such as:
  - **Pydantic** (data validation & schemas)
  - **NumPy** (numerical processing)
  - FastAPI and related ecosystem tools

The project intentionally evolves over time as new concepts are learned and applied.


## 🏗 Architecture

This project demonstrates a classic **Producer-Consumer** pattern combined with a **Microservices** approach:

1.  **Ingestion (C#):** A .NET 8 Web API receives high-velocity sensor data.
2.  **Buffering (System.Threading.Channels):** Data is immediately offloaded to an unbounded in-memory channel to ensure the API remains responsive.
3.  **Processing (C# BackgroundService):** A hosted background worker reads from the channel.
4.  **Analysis (Python/FastAPI):** The worker sends data to a Python microservice for anomaly detection.
5.  **Alerting:** The C# service processes the Python analysis result and alerts on anomalies.

## 🚀 Getting Started

### Prerequisites
* .NET 10  SDK
* Python 3.10+

### Step 1: Start the Python "Brain" 🧠
Navigate to the `ai_service` folder and run:
```bash
pip install -r requirements.txt
uvicorn main:app --reload
```

The Analysis Service will listen on http://127.0.0.1:8000

### Step 2: Start the .NET "Nervous System" ⚡
Navigate to the SensorApi folder and run:

```bash
dotnet run
The Ingestion API will listen on http://localhost:5xxx
```

### Step 3: Test It
Send a POST request to your .NET API (via Swagger or Postman):

```bash
JSON

{
  "machineId": "Press-01",
  "temperature": 105.5,
  "time": "2023-10-27T10:00:00Z"
}
```
Check the .NET console output for the anomaly alert!