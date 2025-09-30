
export class fRoute {
    Coords!:string[];
    DateAdded!: string;
    PlayerName!: string;
    Title!: string;
}

export class fMarker{
    Coords!:string;
    DateAdded!:string;
    PlayerName!:string;
    Title!:string
    Layer!:string
}

export interface fMapConfig{
    spawncontrol:string
}