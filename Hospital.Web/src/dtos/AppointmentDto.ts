import { DoctorDto } from "./DoctorDto";
import { OfficeDto } from "./OfficeDto";

export interface AppointmentDto {
    id: number,
    doctorId: number,
    doctor: DoctorDto,
    officeId: number,
    office: OfficeDto,
    startDate: Date,
    endDate: Date,
    patientName: string
}