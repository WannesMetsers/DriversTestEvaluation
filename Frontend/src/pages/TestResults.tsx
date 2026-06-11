import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { api } from "@/lib/api";
import type { DrivingEventResponseDto, ResultsResponseDto } from "@/types";
import TestResultsComponent from "@/components/TestResultsComponent";
import GraphComponent from "@/components/GraphComponent";
import Navbar from "@/components/NavBar";
import DrivingEventComponent from "@/components/DrivingEventComponent";

export default function TestResults() {
  const { sessionId: routeSessionId } = useParams();

  const [sessionId, setSessionId] = useState(routeSessionId ?? "");
  const [results, setResults] = useState<ResultsResponseDto | null>(null);
  const [events, setEvents] = useState<DrivingEventResponseDto[]>([]);

  useEffect(() => {
    if (routeSessionId) {
      handleGetTestResults(routeSessionId);
    }
  }, [routeSessionId]);

  const handleGetTestResults = async (id: string) => {
    const result = await api.GetTestResult(id);
    setEvents(result.events)
    console.log("Result is: ", result);
    setResults(result);
  };

  return (
    <div>
      <Navbar />
      <h1>Results</h1>

      <input
        type="text"
        value={sessionId}
        onChange={(e) => setSessionId(e.target.value)}
      />

      <button onClick={() => handleGetTestResults(sessionId)}>
        Get test results
      </button>

      {results && (
  <>
    <TestResultsComponent results={results} />

    <GraphComponent
      name="Swerving"
      values={results.differencesInPlace}
    />

    <GraphComponent
      name="Speeds"
      values={results.speeds}
    />
  </>
)}

      <h2>Events ({events.length})</h2>

      {events.length === 0 ? (
        <p>No events recorded</p>
        ) : (
        events.map((event) => (
        <DrivingEventComponent
            key={event.id}
            drivingEvent={event}
          />
        ))
)}
    </div>
  );
}