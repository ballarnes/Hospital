import { AppointmentDto } from "./AppointmentDto";

export interface AppointmentsDto {
    pageIndex: number,
    pageSize: number,
    pagesCount: number,
    totalCount: number,
    data: AppointmentDto[]
}