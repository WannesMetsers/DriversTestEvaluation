import type { DrivingEventResponseDto } from "@/types"

type DrivingEventProps = {
    drivingEvent: DrivingEventResponseDto
}

function DrivingEventComponent({ drivingEvent } : DrivingEventProps){
    return (
        <div>
          <p>{drivingEvent.eventType} | {drivingEvent.penalty}</p>
        </div>
    )
}

export default DrivingEventComponent