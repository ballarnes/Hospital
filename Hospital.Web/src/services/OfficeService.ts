import "reflect-metadata";
import { inject, injectable } from "inversify";
import { ContentType, MethodType } from "./HttpService";
import type { HttpService } from "./HttpService";
import ownTypes from "../ioc/ownTypes";
import { OfficesArrayDto } from "../dtos/OfficesArrayDto";

export interface OfficeService {
    getFreeOfficesByDate(date: string): Promise<OfficesArrayDto | null>;
}

@injectable()
export default class DefaultOfficeService implements OfficeService {
    public constructor(
        @inject(ownTypes.httpService) private readonly httpService: HttpService
    ) {
    }

    public async getFreeOfficesByDate(date: string): Promise<OfficesArrayDto | null> {
        const headers = { contentType: ContentType.Json};
        const data = { date };
        const result = await this.httpService.send<OfficesArrayDto>(`${process.env.BASE_API_URL}HospitalBff/GetFreeOfficesByDate/`, MethodType.POST, headers, data);
        return result.data;
    }
}