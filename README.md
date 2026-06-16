# Drivers Test Evaluator

## Wat?

Deze applicatie wordt gebruikt om de rijvaardigheid van neurologische patiënten te beoordelen. De applicatie verzamelt gegevens uit een rijsimulator, analyseert deze en presenteert de resultaten via een webinterface.

## Hoe opstarten

1. Voer het bestand `start.bat` uit.
2. Open de frontend via:

```text
http://localhost:5173/
```

3. De backend draait standaard op:

```text
http://localhost:5294/
```

## Hoe een simulator toevoegen

### 1. Simulator-URL configureren

Pas de URL van de simulator aan in `appsettings.json`:

```json
{
  "SimulatorApi": {
    "BaseUrl": "http://localhost:5000/api/simulator"
  }
}
```

### 2. Overschakelen naar de externe simulator

In `DriversTestEvaluation.Business.Services.SessionService` wordt standaard een AI-gegenereerde simulatie gebruikt.

Zet onderstaande methode in commentaar:

```csharp
private async Task AnalyzeJsonLoop(DrivingSession session)
{
    JsonEntry lastJsonEntry = null;

    while (true)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DriversTestEvaluationDbContext>();

        var latestSession = await db.DrivingSession
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == session.Id);

        if (latestSession == null || !latestSession.SessionActive)
            break;

        var service = scope.ServiceProvider.GetRequiredService<IDrivingEventService>();

        try
        {
            JsonEntry result = vision.AnalyzeJsonEntry(lastJsonEntry).Result;

            lastJsonEntry = result;

            await service.CreateJsonEvent(result, session.Id);
            await CreateCoordinates(result, session.Id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex}");
        }

        await Task.Delay(2000);
    }
}
```

Vervang deze door de versie die gegevens ophaalt van de externe simulator:

```csharp
private async Task AnalyzeJsonLoop(DrivingSession session)
{
    while (true)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DriversTestEvaluationDbContext>();

        var latestSession = await db.DrivingSession
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == session.Id);

        if (latestSession == null || !latestSession.SessionActive)
            break;

        var service = scope.ServiceProvider.GetRequiredService<IDrivingEventService>();

        try
        {
            var result = await _analyzer.AnalyzeJsonEntry();

            await service.CreateJsonEvent(result, session.Id);
            await CreateCoordinates(result, session.Id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex}");
        }

        await Task.Delay(2000);
    }
}
```

### 3. Vereiste JSON-structuur

De simulator moet een JSON-object teruggeven met volgende structuur:

```json
{
  "position": [0, 0],
  "speed_kmh": 50,
  "spaceCarInFront": 25,
  "colorTrafficLight": "Green",
  "inFrontOfTrafficLight": false,
  "speedLimit": 50
}
```

Deze gegevens worden vervolgens automatisch verwerkt en opgeslagen in de evaluatiesessie.
