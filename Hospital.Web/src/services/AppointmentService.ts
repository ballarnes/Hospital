import "reflect-metadata";
import { inject, injectable } from "inversify";
import { ContentType, MethodType } from "./HttpService";
import type { HttpService } from "./HttpService";
import ownTypes from "../ioc/ownTypes";
import { CreateResponse } from "../dtos/CreateResponse";
import { AppointmentDto } from "../dtos/AppointmentDto";
import { AppointmentsDto } from "../dtos/AppointmentsDto";
import { AppointmentsArrayDto } from "../dtos/AppointmentsArrayDto";

export interface AppointmentService {
    makeAppointment(doctorId: number, officeId: number, startDate: string, endDate: string, patientName: string, authorization: string): Promise<CreateResponse | null>;
    getAppointment(id: number): Promise<AppointmentDto | null>;
    getUpcomingAppointments(pageIndex: number, pageSize: number, name: string): Promise<AppointmentsDto | null>;
    getAppointmentsByDoctorDate(doctorId: number, date: Date): Promise<AppointmentsArrayDto | null>;
    updateAppointment(id: number, doctorId: number, officeId: number, startDate: string, endDate: string, patientName: string, authorization: string): Promise<number>;
    deleteAppointment(id: number, authorization: string): Promise<number>;
}

@injectable()
export default class DefaultAppointmentService implements AppointmentService {
    public constructor(
        @inject(ownTypes.httpService) private readonly httpService: HttpService
    ) {
    }

    public async makeAppointment(doctorId: number, officeId: number, startDate: string, endDate: string, patientName: string, authorization: string): Promise<CreateResponse | null> {
        const headers = { contentType: ContentType.Json, authorization: authorization };
        const data = { doctorId, officeId, startDate, endDate, patientName };
        const result = await this.httpService.send<CreateResponse>(`${process.env.BASE_API_URL}Appointment/AddAppointment/`, MethodType.POST, headers, data);
        return result.data;
    }

    public async getAppointment(id: number): Promise<AppointmentDto | null> {
        const headers = { contentType: ContentType.Json};
        const data = { id };
        const result = await this.httpService.send<AppointmentDto>(`${process.env.BASE_API_URL}HospitalBff/GetAppointmentById/`, MethodType.POST, headers, data);
        return result.data;
    }

    public async getUpcomingAppointments(pageIndex: number, pageSize: number, name: string): Promise<AppointmentsDto | null> {
        const headers = { contentType: ContentType.Json};
        const data = { pageIndex, pageSize, name };
        const result = await this.httpService.send<AppointmentsDto>(`${process.env.BASE_API_URL}HospitalBff/GetUpcomingAppointments/`, MethodType.POST, headers, data);
        return result.data;
    }

    public async getAppointmentsByDoctorDate(doctorId: number, date: Date): Promise<AppointmentsArrayDto | null> {
        const headers = { contentType: ContentType.Json };
        const data = { doctorId, date };
        const result = await this.httpService.send<AppointmentsArrayDto>(`${process.env.BASE_API_URL}HospitalBff/GetAppointmentsByDoctorDate/`, MethodType.POST, headers, data);
        return result.data;
    }

    public async updateAppointment(id: number, doctorId: number, officeId: number, startDate: string, endDate: string, patientName: string, authorization: string): Promise<number> {
        const headers = { contentType: ContentType.Json, authorization: authorization };
        const data = { id, doctorId, officeId, startDate, endDate, patientName };
        const result = await this.httpService.send<unknown>(`${process.env.BASE_API_URL}Appointment/UpdateAppointment/`, MethodType.POST, headers, data);
        return result.status;
    }
    
    public async deleteAppointment(id: number, authorization: string): Promise<number> {
        const headers = { contentType: ContentType.Json, authorization: authorization };
        const data = { id };
        const result = await this.httpService.send<unknown>(`${process.env.BASE_API_URL}Appointment/DeleteAppointment/`, MethodType.POST, headers, data);
        return result.status;
    }
}