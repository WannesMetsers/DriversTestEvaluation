import type { ResultsResponseDto } from "@/types";

type TestResultsProps = {
  results: ResultsResponseDto;
};

export default function TestResultsComponent({
  results,
}: TestResultsProps) {
  return (
    <div
      style={{
        border: "1px solid #ccc",
        borderRadius: "8px",
        padding: "16px",
        marginTop: "16px",
        minWidth: "300px",
      }}
    >
      <h2>Test Results</h2>

      <p>
        <strong>Score:</strong> {results.score}
      </p>

     <p>
  <strong>Swerving:</strong> {results && !results.drivingStraight ? "Yes" : "No"}
</p>

<p>
  <strong>Irregular Speeds:</strong> {results && !results.regularSpeed ? "Yes" : "No"}
</p>


      <p>
        <strong>Status:</strong>{" "}
        {results.passed ? "Passed" : "Failed"}
      </p>
    </div>
  );
}