import { SpecializationDto } from "./SpecializationDto";

export interface DoctorDto {
    id: number,
    name: string,
    surname: string,
    specializationId: number,
    specialization: SpecializationDto
}