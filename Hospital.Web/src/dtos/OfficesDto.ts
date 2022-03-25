import { OfficeDto } from "./OfficeDto";

export interface OfficesDto {
    pageIndex: number,
    pageSize: number,
    pagesCount: number,
    totalCount: number,
    data: OfficeDto[]
}