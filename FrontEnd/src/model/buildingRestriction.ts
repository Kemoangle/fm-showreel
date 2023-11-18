import { Building } from "./building";
import { Restriction } from "./restriction";

export interface BuildingRestriction{
    buildingId: Building
    restrictionId: Restriction
    isAcctive: boolean
}
