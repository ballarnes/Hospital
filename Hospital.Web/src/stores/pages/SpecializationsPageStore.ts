import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import ownTypes from "../../ioc/ownTypes";
import { Specialization } from "../../models/Specialization";
import SpecializationService from "../../services/SpecializationService";

@injectable()
export default class SpecializationsPageStore  {

    specializations : Specialization[] = [];
    isLoading = false;
    pageSize = 10;
    pageIndex = 0;

    constructor(   
        @inject(ownTypes.specializationService) private readonly specializationService: SpecializationService
   ) {
       makeAutoObservable(this);
   }

    
    public init = async () => {
        try {
            this.isLoading = true;
            const result = await this.specializationService.getByPage(this.pageIndex, this.pageSize);
            this.specializations = result.data;
            
          } catch (e) {
            if (e instanceof Error) {
                console.error(e.message);
            }
          }
        this.isLoading = false;
    }
}