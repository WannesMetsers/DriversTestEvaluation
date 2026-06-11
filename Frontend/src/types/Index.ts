export interface DrivingEventResponseDto{
    id: number
    eventType: string;
    penalty: number;
}

export interface ResultsResponseDto{
    passed: boolean;
    score: number;
    numberOfEvents: number;
    events: DrivingEventResponseDto[];
    drivingStraight: boolean;
    regularSpeed: boolean;
    speeds: number[];
    differencesInPlace: number[];
}

export interface UpdateDto{
    events: DrivingEventResponseDto[];
    coordinates: CoordinatesResponseDto[];
}

export interface CoordinatesResponseDto {
    id: string;
    sessionId: string;
    x: number;
    y: number;
}