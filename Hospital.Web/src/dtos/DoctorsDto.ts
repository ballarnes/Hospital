import { DoctorDto } from "./DoctorDto";

export interface DoctorsDto {
    pageIndex: number,
    pageSize: number,
    pagesCount: number,
    totalCount: number,
    data: DoctorDto[]
}