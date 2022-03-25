import "reflect-metadata";
import { inject, injectable } from "inversify";
import { ContentType, MethodType } from "./HttpService";
import type { HttpService } from "./HttpService";
import ownTypes from "../ioc/ownTypes";
import { DoctorsDto } from "../dtos/DoctorsDto";

export interface DoctorService {
    getBySpecId(id: number): Promise<DoctorsDto>;
}

@injectable()
export default class DefaultDoctorService implements DoctorService {
    public constructor(
        @inject(ownTypes.httpService) private readonly httpService: HttpService
    ) {
    }

    public async getBySpecId(id: number): Promise<DoctorsDto> {
        const headers = { contentType: ContentType.Json};
        const data = { id };
        const result = await this.httpService.send<DoctorsDto>(`HospitalBff/GetDoctorsBySpecializationId/`, MethodType.POST, headers, data);
        return result.data;
    }
}