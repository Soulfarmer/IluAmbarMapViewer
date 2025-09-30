import { fRoute } from "@/models/dbModels";
import type {IRoute, LocationBase}  from '@/models/locationInfo'
import {QueryDocumentSnapshot, type FirestoreDataConverter,type WithFieldValue}  from 'firebase/firestore'

export class RouteConverter implements FirestoreDataConverter<IRoute,fRoute>{
    toFirestore(modelObject: IRoute): WithFieldValue<fRoute> {
       return {
            Coords: modelObject.Coords.map(m=> m.Latitude+":"+m.Longitude),
            DateAdded: modelObject.DateAdded,
            PlayerName: modelObject.PlayerName,
            Title: modelObject.Name
       } as fRoute
    }
    fromFirestore(snapshot: QueryDocumentSnapshot<fRoute>): IRoute {
        const data = snapshot.data()
        return {
            Coords: data.Coords.map(_=>this.toLocationBase(_)),
            DateAdded: data.DateAdded,
            PlayerName: data.PlayerName,
            Title: data.Title
        } as IRoute
    }
    toLocationBase(coords: string) : LocationBase{
        const xy = coords.split(":");
        return {
            Latitude:parseInt(xy[0]),
            Longitude:parseInt(xy[1])
        } as LocationBase
    }
}