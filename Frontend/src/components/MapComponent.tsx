type Coordinate = {
  x: number;
  y: number;
};

type RouteMapProps = {
  coordinates: Coordinate[];
};

export default function RouteMap({ coordinates }: RouteMapProps) {
  if (coordinates.length < 2) {
    return <p>Not enough points</p>;
  }

  const width = 800;
  const height = 600;
  const padding = 20;

  const minX = Math.min(...coordinates.map(p => p.x));
  const maxX = Math.max(...coordinates.map(p => p.x));

  const minY = Math.min(...coordinates.map(p => p.y));
  const maxY = Math.max(...coordinates.map(p => p.y));

  const rangeX = Math.max(maxX - minX, 1);
  const rangeY = Math.max(maxY - minY, 1);

  const scaledPoints = coordinates.map(p => ({
    x:
      ((p.x - minX) / rangeX) *
        (width - padding * 2) +
      padding,

    y:
      height -
      (((p.y - minY) / rangeY) *
        (height - padding * 2) +
        padding),
  }));

  const polylinePoints = scaledPoints
    .map(p => `${p.x},${p.y}`)
    .join(" ");

  return (
    <svg
      width={width}
      height={height}
      style={{
     
      }}
    >
      <polyline
        points={polylinePoints}
        fill="none"
        stroke="orange"
        strokeWidth={2}
      />

    
    </svg>
  );
}