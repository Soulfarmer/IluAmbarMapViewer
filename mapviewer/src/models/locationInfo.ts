export interface LocationInfo extends LocationBase{
  Title: string;
}

export interface LocationBase {
  Latitude: number;
  Longitude: number;
}

export interface IRoute{
  Name:string;
  Waypoints:LocationBase[];
}