import type { fMarker } from "@/models/dbModels";
import type { IMarker, LocationBase } from "@/models/locationInfo";
import type { FirestoreDataConverter, QueryDocumentSnapshot, WithFieldValue } from "firebase/firestore";

export class MarkerConverter implements FirestoreDataConverter<IMarker, fMarker>{

    toFirestore(modelObject: IMarker): WithFieldValue<fMarker> {
        return {
            Coords:modelObject.Coords.Latitude+":"+modelObject.Coords.Longitude,
            DateAdded:modelObject.DateAdded,
            PlayerName:modelObject.PlayerName,
            Title:modelObject.Title,
            Layer:modelObject.Layer
        } as fMarker
    }
    fromFirestore(snapshot: QueryDocumentSnapshot<fMarker>) {
        const data = snapshot.data()
        return {
            Coords: this.toLocationBase(data.Coords),
            Title: data.Title,
            DateAdded: data.DateAdded,
            Layer: data.Layer,
            PlayerName: data.PlayerName,
        }as IMarker
    }
    toLocationBase(coords :string):LocationBase{
        const l = coords.split(":")
        return {Latitude:parseInt(l[0]), Longitude:parseInt(l[1])} as LocationBase
    }
}