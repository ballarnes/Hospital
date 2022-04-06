import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import ownTypes from "../../ioc/ownTypes";
import { Appointment } from "../../models/Appointment";
import AppointmentService from "../../services/AppointmentService";

@injectable()
export default class InformationStore {

    appointment: Appointment | null = null;
    isLoading = false;
    id = 1;
    error = false;

    constructor(
        @inject(ownTypes.appointmentService) private readonly appointmentService: AppointmentService
   ) {
       makeAutoObservable(this);
   }

    
    public init = async () => {
        this.isLoading = false;
    }

    public changeId = async (id: string) => {
        this.id = parseInt(id);
    }

    public changeAppointment = async () => {
        this.appointment = null;
    }

    public getAppointment = async () => {
        try {
            this.error = false;
            this.isLoading = true;
            const result = await this.appointmentService.getAppointment(this.id);
            this.appointment = result;
          } catch (e) {
            if (e instanceof Error) {
                console.error(e.message);
                this.error = true;
            }
          }
        this.isLoading = false;
    }
}