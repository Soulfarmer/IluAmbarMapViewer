
export class IMapConfig{
  spawnControl!:LocationBase
}

export interface IWaypoint{
  waypoints: IMarker[];
}

export interface IMarker{
  Coords: LocationBase;
  Title: string;
  DateAdded:string;
  Layer:string;
  PlayerName:string;
}

export interface LocationBase {
  Latitude: number;
  Longitude: number;
}

export interface IRoute{
  Name:string;
  Coords:LocationBase[];
  DateAdded?:string;
  PlayerName?:string;
  Title?:string;
  config:IMapConfig
}