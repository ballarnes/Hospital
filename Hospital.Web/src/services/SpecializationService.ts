import "reflect-metadata";
import { inject, injectable } from "inversify";
import { ContentType, MethodType } from "./HttpService";
import type { HttpService } from "./HttpService";
import ownTypes from "../ioc/ownTypes";
import { SpecializationsDto } from "../dtos/SpecializationsDto";

export interface SpecializationService {
    getByPage(pageSize: number, pageIndex: number): Promise<SpecializationsDto | null>;
}

@injectable()
export default class DefaultSpecializationService implements SpecializationService {
    public constructor(
        @inject(ownTypes.httpService) private readonly httpService: HttpService
    ) {
    }

    public async getByPage(pageIndex: number, pageSize: number): Promise<SpecializationsDto | null> {
        const headers = { contentType: ContentType.Json};
        const data = { pageIndex, pageSize };
        const result = await this.httpService.send<SpecializationsDto>(`${process.env.BASE_API_URL}HospitalBff/GetSpecializations/`, MethodType.POST, headers, data);
        return result.data;
    }
}