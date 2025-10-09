<script lang="ts">
// https://github.com/jhkluiver/Vue3Leaflet/blob/main/vue3-quasar-leaflet/src/components/VueLeafletMap.vue
import "leaflet/dist/leaflet.css";
import { LControlLayers, LMap, LTileLayer,LFeatureGroup, LPolyline } from "@vue-leaflet/vue-leaflet";
import { type PointTuple } from "leaflet";
import { type IRoute, type LocationBase } from "@/models/locationInfo";
import { ref } from 'vue'
import { storeToRefs } from "pinia";
import {useMarkerStore} from "@/stores/markers"
import {useRoutesStore} from "@/stores/routeStore"
import {useMapConfigStore} from "@/stores/mapConfig"
import FeatureGroup from "@/components/FeatureGroup.vue"
import { Utility } from "@/composables/Utility";

export default{
  created() {
    useMapConfigStore().fetch()
    useRoutesStore().fetch()
    useMarkerStore().fetch()
  },
  mounted() {
    this.mapRef = (this.$refs.mapRef as typeof LMap)?.leafletObject;
    console.log(this.locations.Markers)
  },
  components:{
    LMap,
    LTileLayer,
    LControlLayers,
    LFeatureGroup,
    LPolyline,
    FeatureGroup
  },
  data(){
    return{
      mapRef : ref<typeof LMap>(),
      controlLayers : ref<typeof LControlLayers>(),
      fg : ref<typeof LFeatureGroup | null>(null),
      defaultZoom : ref(0),
      center : ref<L.PointExpression>([-74, -53]),
      mapConfig: storeToRefs(useMapConfigStore()),
      locations : storeToRefs(useMarkerStore()),
      routes: storeToRefs(useRoutesStore()),
      mapSize:ref([4000,3000] as [number,number]),
      bounds : ref()
    }
  },
  methods:{
    init(){
      this.controlLayers = (this.$refs.controlLayers as typeof LControlLayers)?.leafletObject
      // hack to fix map not loading properly
      setTimeout(() => {
        this.mapRef?.invalidateSize();
      }, 100);
      this.controlLayers?.addOverlay((this.$refs.fgtp as typeof LFeatureGroup )?.leafletObject,"Translocators")
      this.bounds = [[0, 0], [this.mapSize[0], this.mapSize[1]]];
    },
    initLayer(Layer:typeof LFeatureGroup , name:string){
       this.controlLayers?.addOverlay(Layer.leafletObject,name)
    },
    getLatLon(item: LocationBase): PointTuple{
      return [item.Latitude+ this.mapConfig.spawnControl.Latitude, item.Longitude+this.mapConfig.spawnControl.Longitude]
    },
    getRoute(route: IRoute):PointTuple[]{
      return route.Coords.map(m=>[m.Latitude + this.mapConfig.spawnControl.Latitude, m.Longitude+this.mapConfig.spawnControl.Longitude ])
    },
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    leafletToGame(e:any){
      const util = new Utility(this.mapSize)
      const g = util.leafletToGame(e.latlng)
      console.log(g, e.latlng)
    }
    
  }
}
</script>

<template>
     <div class="inmap">
    <l-map ref="mapRef" 
    v-model:zoom="defaultZoom" 
    :min-zoom="0" 
    :max-zoom="6" 
    :center="center"
    :bounds="bounds"
    style="z-index: 0;"
    :use-global-leaflet="true"
    :cursor="true"
    crs="Base"
    @update:zoom="(e)=>{/*console.log(e)*/}"
    @click="(e:any)=> leafletToGame(e)"
    @ready="()=>init()"
    >
    <l-tile-layer
    url="/maps/iluambar/{z}/{x}/{y}.png"
    name="Overworld"
    :no-wrap="true"
    />
    <l-control-layers ref="controlLayers" :options="{collapsed:false}"/>
      <l-feature-group ref="fgtp" name="Translocators">
        <l-polyline v-for="(r) in routes.routes" :lat-lngs="getRoute(r)" color="blue"  dashArray="10, 10" dashOffset="30" :key="r.Name"/>
      </l-feature-group>
      <suspense>
        <FeatureGroup v-for="(doc,k) in locations.Markers" :-control="mapConfig.spawnControl" :-name="k" :-markers="doc" :key="k" @init-layer="initLayer"/>
      </suspense>
    </l-map>
  </div>
</template>
<style scoped>
.inmap{
  width: 98vw;
  height: 95vh;
}
</style>