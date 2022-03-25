import { IntervalDto } from "./IntervalDto";

export interface IntervalsDto {
    pageIndex: number,
    pageSize: number,
    pagesCount: number,
    totalCount: number,
    data: IntervalDto[]
}