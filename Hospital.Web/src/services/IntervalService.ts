import "reflect-metadata";
import { inject, injectable } from "inversify";
import { ContentType, MethodType } from "./HttpService";
import type { HttpService } from "./HttpService";
import ownTypes from "../ioc/ownTypes";
import { IntervalsDto } from "../dtos/IntervalsDto";

export interface IntervalService {
    getFreeIntervals(doctorId: number, date: Date): Promise<IntervalsDto | null>;
}

@injectable()
export default class DefaultIntervalService implements IntervalService {
    public constructor(
        @inject(ownTypes.httpService) private readonly httpService: HttpService
    ) {
    }

    public async getFreeIntervals(doctorId: number, date: Date): Promise<IntervalsDto | null> {
        const headers = { contentType: ContentType.Json};
        const data = { doctorId, date };
        console.log(date);
        const result = await this.httpService.send<IntervalsDto>(`${process.env.BASE_API_URL}HospitalBff/GetFreeIntervalsByDoctorDate/`, MethodType.POST, headers, data);
        return result.data;
    }
}