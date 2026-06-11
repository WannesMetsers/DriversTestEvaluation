import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
} from "recharts";

type GraphProps = {
  name: string;
  values: number[];
};

export default function GraphComponent({
  name,
  values,
}: GraphProps) {
  const data = values.map((value, index) => ({
    index: index + 1,
    value,
  }));

  return (
    <div
      style={{
        border: "1px solid var(--border)",
        borderRadius: "8px",
        padding: "16px",
        background: "var(--bg)",
      }}
    >
      <h2 style={{ textAlign: "center", marginBottom: "20px" }}>
        {name}
      </h2>

      <ResponsiveContainer width="100%" height={300}>
        <LineChart data={data}>
          <CartesianGrid strokeDasharray="3 3" />

          <XAxis
            dataKey="index"
            label={{
              value: "Time",
              position: "insideBottom",
              offset: -5,
            }}
          />

          <YAxis />

          <Tooltip />

          <Line
            type="monotone"
            dataKey="value"
            stroke="orange"
            strokeWidth={3}
          />
        </LineChart>
      </ResponsiveContainer>
    </div>
  );
}