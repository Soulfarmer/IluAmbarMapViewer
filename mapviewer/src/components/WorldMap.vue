<script lang="ts">
// https://github.com/jhkluiver/Vue3Leaflet/blob/main/vue3-quasar-leaflet/src/components/VueLeafletMap.vue
import "leaflet/dist/leaflet.css";
import { LControlLayers, LMap, LTileLayer,LFeatureGroup, LMarker, LTooltip, LPolyline } from "@vue-leaflet/vue-leaflet";
import { type PointTuple } from "leaflet";
import { type IRoute, type LocationBase } from "@/models/locationInfo";
import { ref } from 'vue'
import { storeToRefs } from "pinia";
import {useMarkerStore} from "@/stores/markers"
import {useRoutesStore} from "@/stores/routeStore"
import {useMapConfigStore} from "@/stores/mapConfig"

export default{
  components:{
    LMap,
    LTileLayer,
    LControlLayers,
    LFeatureGroup,
    LMarker,
    LTooltip,
    LPolyline
  },
  data(){
    return{
      mapRef : ref<typeof LMap>(),
      controlLayers : ref<typeof LControlLayers>(),
      fg : ref<typeof LFeatureGroup | null>(null),
      defaultZoom : ref(0),
      center : ref<L.PointExpression>([-74, -53]),
      locations : storeToRefs(useMarkerStore()),
      routes: storeToRefs(useRoutesStore()),
      mapConfig:storeToRefs(useMapConfigStore())
    }
  },
  beforeMount() {
    useMapConfigStore().fetch()
  },
  mounted(){
    useRoutesStore().fetch()
    useMarkerStore().fetch()
  },
  methods:{
    init(){
      this.mapRef = (this.$refs.mapRef as typeof LMap)?.leafletObject;
      this.controlLayers = (this.$refs.controlLayers as typeof LControlLayers)?.leafletObject
      this.controlLayers?.addOverlay((this.$refs.fgtp as typeof LFeatureGroup )?.leafletObject,"Translocators")
      this.controlLayers?.addOverlay((this.$refs.fgpoi as typeof LFeatureGroup)?.leafletObject,"POIs")
      setTimeout(() => {
            this.mapRef?.invalidateSize();
          }, 100);
      },
      getLatLon(item: LocationBase): PointTuple{
        return [item.Latitude+(-74.31), item.Longitude+(-53.56)]
      },
      getRoute(route: IRoute):PointTuple[]{
        return route.Coords.map(m=>[m.Latitude+(-74.31), m.Longitude+(-53.56)])
      }
  }
}
</script>

<template>
     <div class="inmap">
    <l-map ref="mapRef" 
    v-model:zoom="defaultZoom" 
    :min-zoom="0" 
    :max-zoom="5" 
    :center="center"
    style="z-index: 0;"
    :use-global-leaflet="true"
    :cursor="true"
    crs="Base"
    @update:zoom="(e)=>{/*console.log(e)*/}"
    @click="(e:any)=>console.log(e.latlng)"
    @ready="()=>init()"
    >
       <l-control-layers ref="controlLayers" :options="{collapsed:false}"/>
      <l-tile-layer
        url="/maps/iluambar/{z}/{x}/{y}.png"
        name="Overworld"
        :no-wrap="true"
      />
      <l-feature-group ref="fgtp" name="Translocators">
        <l-polyline v-for="(r) in routes.routes" :lat-lngs="getRoute(r)" color="blue"  dashArray="10, 10" dashOffset="30" :key="r.Name"/>
      </l-feature-group>
      <l-feature-group ref="fgpoi" name="POIs">
        <l-marker v-for="loc in  locations.Markers" :lat-lng="getLatLon(loc.Coords)" :key="loc.Title">
          <l-tooltip>{{ loc.Title }}</l-tooltip>
        </l-marker>
      </l-feature-group>
    </l-map>
  </div>
</template>
<style scoped>
.inmap{
  width: 100vw;
  height: 95vh;
}
</style>