import { Doctor } from "./Doctor";
import { Office } from "./Office";

export interface Appointment {
    id: number,
    doctorId: number,
    doctor: Doctor,
    officeId: number,
    office: Office,
    startDate: Date,
    endDate: Date,
    patientName: string
}