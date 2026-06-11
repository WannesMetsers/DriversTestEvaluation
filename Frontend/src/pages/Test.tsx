import { useRef, useState } from "react";
import { useNavigate } from "react-router-dom";
import { api } from "@/lib/api";
import type { CoordinatesResponseDto, DrivingEventResponseDto } from "@/types";
import DrivingEventComponent from "@/components/DrivingEventComponent";
import RouteMap from "@/components/MapComponent";
import NavBar from "@/components/NavBar";




export default function Test() {
  const [events, setEvents] = useState<DrivingEventResponseDto[]>([]);
  const [coordinates, setCoordinates] = useState<CoordinatesResponseDto[]>([]);
  const continueUpdate = useRef(false);
  const [status, setStatus] = useState<"idle" | "running" | "stopped">("idle");
  const navigate = useNavigate();



  const startUpdateTest = async () => {
  const sessionId = localStorage.getItem("session_id");

  while (continueUpdate.current) {
    try {
      const update = await api.GetUpdate(sessionId);

      setEvents(prev => {
        const existing = new Set(prev.map(e => e.id));
        const filtered = update.events.filter(e => !existing.has(e.id));
        return [...prev, ...filtered];
      });

      setCoordinates(prev => {
        const existing = new Set(prev.map(c => `${c.x}-${c.y}`));
        const filtered = update.coordinates.filter(
          c => !existing.has(`${c.x}-${c.y}`)
        );
        return [...prev, ...filtered];
      });

      await new Promise(r => setTimeout(r, 1000));
    } catch (error) {
      console.error(error);
      break;
    }
  }
};

  const handleStartTest = async () => {
    const id = await api.StartTest();
    setStatus("running");
    console.log("StartTest raw result:", id);
    localStorage.setItem("session_id", id);
    continueUpdate.current = true;
    console.log("sessionId =" + localStorage.getItem("session_id"));
    startUpdateTest();
  };

  const handleStopTest = async () => {
    continueUpdate.current = false;
    setStatus("stopped");

    const sessionId = localStorage.getItem("session_id");

    await api.StopTest(sessionId);
    console.log("Stopped Test");
    const goToResults = window.confirm(
      `Test stopped.\n\nView results for session ${sessionId}?`
    );

    if (goToResults) {
      navigate(`/results/${sessionId}`);
    }
  };

  const getStatusStyle = (status: "idle" | "running" | "stopped") => ({
  margin: "20px",
  fontSize: "40px",
  padding: "24px 16px",
  borderRadius: "8px",
  textAlign: "center",
  border: "1px solid var(--border)",
  color: "var(--text-h)",
  background:
    status === "running"
      ? "green"
      : status === "stopped"
      ? "red"
      : "var(--accent-bg)",
});

  return (
    
    <div>
      <div>
        <NavBar />
      </div>
<div
  style={{
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    gap: "20px",
  }}
>
  <button onClick={handleStartTest} style={buttonStyle}>
    Start
  </button>

  <div style={getStatusStyle(status)}>
    {status === "idle" && "Press start to begin"}
    {status === "running" && "Test is running"}
    {status === "stopped" && "Test stopped"}
  </div>

  <button onClick={handleStopTest} style={buttonStyle}>
    Stop
  </button>
</div>
<div
  style={{
    display: "flex",
    gap: "20px",
    alignItems: "flex-start",
  }}
>
  {/* LEFT: events */}
  <div style={{ flex: 1,padding: "4em", border: "1px solid var(--border)" }}>
    <h1 style={{textAlign: "center", marginTop: 0}}>Events</h1>
    {events.map((event, index) => (
      <DrivingEventComponent
        key={index}
        drivingEvent={event}
      />
    ))}
  </div>

  {/* RIGHT: map */}
  <div style={{ flex: 1,  border: "1px solid var(--border)" }}>
    <h1>map</h1>
    <RouteMap coordinates={coordinates} />
  </div>
</div>
    </div>
  );
}

const buttonStyle: React.CSSProperties = {

  textDecoration: "none",
  margin: "20px",
  marginLeft: "20px",
  marginRight: "20px",
  padding: "8px 12px",
  fontSize: "50px",
  borderRadius: "8px",
  cursor: "pointer",
  border: "1px solid var(--border)",
  background: "var(--accent-bg)",
  color: "var(--text-h)",
};

