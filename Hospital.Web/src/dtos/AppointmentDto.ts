import { DoctorDto } from "./DoctorDto";
import { IntervalDto } from "./IntervalDto";
import { OfficeDto } from "./OfficeDto";

export interface AppointmentDto {
    id: number,
    doctorId: number,
    doctor: DoctorDto,
    intervalId: number,
    interval: IntervalDto,
    officeId: number,
    office: OfficeDto,
    date: Date,
    patientName: string
}