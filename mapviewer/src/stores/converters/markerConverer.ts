import type { fWaypoint } from "@/models/dbModels";
import type { IWaypoint, LocationBase } from "@/models/locationInfo";
import type { FirestoreDataConverter, QueryDocumentSnapshot, WithFieldValue } from "firebase/firestore";

export class MarkerConverter implements FirestoreDataConverter<IWaypoint, fWaypoint>{

    toFirestore(modelObject: IWaypoint): WithFieldValue<fWaypoint> {
        return {
            waypoints: modelObject.waypoints.map(m=>({
                Coords:m.Coords.Latitude+":"+m.Coords.Longitude,
                DateAdded:m.DateAdded,
                PlayerName:m.PlayerName,
                Title:m.Title,
                Layer:m.Layer
            }))};
    }
    fromFirestore(snapshot: QueryDocumentSnapshot<fWaypoint>) {
        const data = snapshot.data()
        return {
            waypoints: data.waypoints.map(m=>({
                Coords: this.toLocationBase(m.Coords[0]),
                Title: m.Title,
                DateAdded: m.DateAdded,
                Layer: m.Layer,
                PlayerName: m.PlayerName,
            })),
        } as IWaypoint
    }
    toLocationBase(coords :string):LocationBase{
        const l = coords.split(":")
        return {Latitude:parseFloat(l[0]), Longitude:parseFloat(l[1])} as LocationBase
    }
}