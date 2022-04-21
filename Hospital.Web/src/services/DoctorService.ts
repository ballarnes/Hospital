import "reflect-metadata";
import { inject, injectable } from "inversify";
import { ContentType, MethodType } from "./HttpService";
import type { HttpService } from "./HttpService";
import ownTypes from "../ioc/ownTypes";
import { DoctorsDto } from "../dtos/DoctorsDto";

export interface DoctorService {
    getBySpecId(id: number): Promise<DoctorsDto | null>;
}

@injectable()
export default class DefaultDoctorService implements DoctorService {
    public constructor(
        @inject(ownTypes.httpService) private readonly httpService: HttpService
    ) {
    }

    public async getBySpecId(id: number): Promise<DoctorsDto | null> {
        const headers = { contentType: ContentType.Json};
        const data = { id };
        const result = await this.httpService.send<DoctorsDto>(`${process.env.BASE_API_URL}HospitalBff/GetDoctorsBySpecializationId/`, MethodType.POST, headers, data);
        return result.data;
    }
}