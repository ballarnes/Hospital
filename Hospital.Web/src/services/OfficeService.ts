import "reflect-metadata";
import { inject, injectable } from "inversify";
import { ContentType, MethodType } from "./HttpService";
import type { HttpService } from "./HttpService";
import ownTypes from "../ioc/ownTypes";
import { OfficesDto } from "../dtos/OfficesDto";

export interface OfficeService {
    getFreeOffices(intervalId: number, date: Date): Promise<OfficesDto | null>;
}

@injectable()
export default class DefaultOfficeService implements OfficeService {
    public constructor(
        @inject(ownTypes.httpService) private readonly httpService: HttpService
    ) {
    }

    public async getFreeOffices(intervalId: number, date: Date): Promise<OfficesDto | null> {
        const headers = { contentType: ContentType.Json};
        const data = { intervalId, date };
        const result = await this.httpService.send<OfficesDto>(`${process.env.BASE_API_URL}HospitalBff/GetFreeOfficesByIntervalDate/`, MethodType.POST, headers, data);
        return result.data;
    }
}