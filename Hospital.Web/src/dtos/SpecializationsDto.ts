import { SpecializationDto } from "./SpecializationDto";

export interface SpecializationsDto {
    pageIndex: number,
    pageSize: number,
    pagesCount: number,
    totalCount: number,
    data: SpecializationDto[]
}