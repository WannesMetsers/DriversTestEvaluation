import { useTheme } from "@/hooks/useTheme";

export default function Navbar() {
  const { theme, toggleTheme } = useTheme();

  return (
    <nav
      style={{
        width: "100%",
        padding: "12px 20px",
        display: "flex",
        alignItems: "center",
        justifyContent: "space-between",
        borderBottom: "1px solid var(--border)",
        background: "var(--bg)",
        color: "var(--text-h)",
        boxSizing: "border-box",
      }}
    >
      {/* Left side - logo / title */}
      <div style={{ fontWeight: 600, fontSize: "18px" }}>
        Driving Test 
      </div>

      {/* Center links */}
      <div
        style={{
          display: "flex",
          gap: "16px",
        }}
      >
        <a href="/" style={linkStyle}>Home</a>
        <a href="/test" style={linkStyle}>Test</a>
        <a href="/results" style={linkStyle}>Results</a>
      </div>

      {/* Right side - theme toggle */}
      <button
        onClick={toggleTheme}
        style={{
          padding: "8px 12px",
          borderRadius: "8px",
          cursor: "pointer",
          border: "1px solid var(--border)",
          background: "var(--accent-bg)",
          color: "var(--text-h)",
        }}
      >
        {theme === "dark" ? "Dark" : "Light"}
      </button>
    </nav>
  );
}

const linkStyle: React.CSSProperties = {
  textDecoration: "none",
  color: "var(--text)",
  fontWeight: 500,
};