import { AppointmentDto } from "./AppointmentDto";

export interface AppointmentsArrayDto {
    totalCount: number,
    data: AppointmentDto[]
}