

export class Utility {
    mapWidth! : number
    mapHeight!:number
    constructor(mapSize:[number,number]){
        this.mapWidth = mapSize[0]
        this.mapHeight = mapSize[1]
    }

    leafletToGame(latlng :{lat:number,lng:number}) {
        const { lat, lng } = latlng;
        const gameX = (lng / this.mapWidth) * 10000;
        const gameY = ((this.mapHeight - lat) / this.mapHeight) * 10000;
        return { x: gameX, y: gameY };
    }

    gameToLeaflet(x:number, y:number) {
        // Flip Y
        const leafletX = (x / 10000) * this.mapWidth;
        const leafletY = this.mapHeight - (y / 10000) * this.mapHeight;
        return [leafletY, leafletX]; // Leaflet uses [y, x]
    }
}