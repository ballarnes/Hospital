import "reflect-metadata";
import { inject, injectable } from "inversify";
import { ContentType, MethodType } from "./HttpService";
import type { HttpService } from "./HttpService";
import ownTypes from "../ioc/ownTypes";
import { CreateResponse } from "../dtos/CreateResponse";
import { AppointmentDto } from "../dtos/AppointmentDto";

export interface AppointmentService {
    makeAppointment(doctorId: number, intervalId: number, officeId: number, date: Date, patientName: string): Promise<CreateResponse>;
    getAppointment(id: number): Promise<AppointmentDto>;
}

@injectable()
export default class DefaultAppointmentService implements AppointmentService {
    public constructor(
        @inject(ownTypes.httpService) private readonly httpService: HttpService
    ) {
    }

    public async makeAppointment(doctorId: number, intervalId: number, officeId: number, date: Date, patientName: string): Promise<CreateResponse> {
        const headers = { contentType: ContentType.Json};
        const data = { doctorId, intervalId, officeId, date, patientName };
        const result = await this.httpService.send<CreateResponse>(`Appointment/AddAppointment/`, MethodType.POST, headers, data);
        return result.data;
    }

    public async getAppointment(id: number): Promise<AppointmentDto> {
        const headers = { contentType: ContentType.Json};
        const data = { id };
        const result = await this.httpService.send<AppointmentDto>(`HospitalBff/GetAppointmentById/`, MethodType.POST, headers, data);
        return result.data;
    }
}