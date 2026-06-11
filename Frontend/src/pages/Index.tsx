import React from "react";
import type { ResultsResponseDto } from "@/types";
import NavBar from "@/components/NavBar";
const Index: React.FC = () => {


  return (
    
    <div>
      <div>
        <NavBar />
      </div>

      <h1>Driving test</h1>
      <p style={pStyle}>Choose what you want to do? </p>

      <a href="/test" style={buttonStyle}>Test</a>
        <a href="/results" style={buttonStyle}>Results</a>
    </div>
  );
};

const pStyle: React.CSSProperties ={
  margin: "15px",
  color: "var(--text-h)",
};

const buttonStyle: React.CSSProperties = {
  textDecoration: "none",
  margin: "5px",
  padding: "8px 12px",
  borderRadius: "8px",
  cursor: "pointer",
  border: "1px solid var(--border)",
  background: "var(--accent-bg)",
  color: "var(--text-h)",
};

export default Index;