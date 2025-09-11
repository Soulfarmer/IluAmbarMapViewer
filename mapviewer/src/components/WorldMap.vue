<script lang="ts">
// https://github.com/jhkluiver/Vue3Leaflet/blob/main/vue3-quasar-leaflet/src/components/VueLeafletMap.vue
import "leaflet/dist/leaflet.css";
import { LControlLayers, LMap, LTileLayer,LFeatureGroup } from "@vue-leaflet/vue-leaflet";
import { ref } from 'vue'

export default{
  components:{
    LMap,
    LTileLayer,
    LControlLayers,
    LFeatureGroup
  },
  data(){
    return{
      mapRef : ref<typeof LMap>(),
      controlLayers : ref<typeof LControlLayers>(),
      fg : ref<typeof LFeatureGroup | null>(null),
      defaultZoom : ref(0),
      center : ref<L.PointExpression>([100, 100])
    }
  },
  mounted(){
  },
  methods:{
    init(){
      this.mapRef = (this.$refs.mapRef as typeof LMap)?.leafletObject;
      console.log(this.mapRef)
      setTimeout(() => {
            console.log("ran invalidateSize()")
            this.mapRef?.invalidateSize();
          }, 100);
      },
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
    @update:zoom="(e)=>{/*console.log(e)*/}"
    @ready="()=>init()"
    >
      <l-control-layers ref="controlLayers"/>
      <suspense>
      <l-tile-layer
        url="/maps/iluambar/{z}/{x}/{y}.png"
        name="Overworld"
        :no-wrap="true"
      />
      </suspense>
      <l-feature-group ref="fg" name="Translocators">
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