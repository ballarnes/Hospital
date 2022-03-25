import { Specialization } from "./Specialization";

export interface Doctor {
    id: number,
    name: string,
    surname: string,
    specializationId: number,
    specialization: Specialization
}