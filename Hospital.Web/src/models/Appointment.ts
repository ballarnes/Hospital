import { Doctor } from "./Doctor";
import { Office } from "./Office";
import { Interval } from "./Interval";

export interface Appointment {
    id: number,
    doctorId: number,
    doctor: Doctor,
    intervalId: number,
    interval: Interval,
    officeId: number,
    office: Office,
    date: Date,
    patientName: string
}